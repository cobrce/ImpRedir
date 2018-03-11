using AsmResolver;
using ImportRedir.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportRedir.protection
{
    class Stub : AbstractStub
    {
        #region fields        
        Random random = new Random();
        List<byte[]> _cryptedDllNames = new List<byte[]>();
        Dictionary<string, Tuple<uint, uint>> _ApiNameApiHashModuleHash = new Dictionary<string, Tuple<uint, uint>>();
        Dictionary<string, int> _ApiNameRedirectionAddress = new Dictionary<string, int>();
        byte _key;
        byte Key
        {
            get
            {
                return (_key == 0 ? _key = (byte)random.Next(1, 256) : _key); // a non-null byte
            }
        }
        #endregion

        #region .ctor
        public Stub(uint OEP, int memorySize) : base(OEP, memorySize)
        {
            EPoffset = 0;
            Code = "use32\r\n"; // use 32bit compiler 
        }
        #endregion

        #region overrides
        public override void AddDllsToLoad(WindowsAssembly PE)
        {
            foreach (ImageModuleImport mi in PE.ImportDirectory.ModuleImports)
            {
                _cryptedDllNames.Add(xorCrypt(mi.Name));
            }
        }
        public override void GenerateLoaderCode()
        {
            foreach (byte[] a in _cryptedDllNames)
                foreach (byte b in a)
                {
                    Code += (string.Format("db 0x{0:X}\r\n", b));
                    EPoffset++;
                }
            Code += (DllLoader);
            // done
        }
        public override void AddAPIs(WindowsAssembly pE)
        {

            foreach (ImageModuleImport mi in pE.ImportDirectory.ModuleImports)
            {
                foreach (ImageSymbolImport si in mi.SymbolImports)
                {
                    _ApiNameApiHashModuleHash[si.HintName.Name] = new Tuple<uint, uint>(Hash(si.HintName.Name), Hash(si.Module.Name));
                }
            }
        }
        public override void GenerateRedirectionCode(WindowsAssembly pE)
        {

            foreach (string apiName in _ApiNameApiHashModuleHash.Keys)
            {
                Code += string.Format(RedirectionCodeSnippet,
                                                                 apiName,
                                                                 _ApiNameApiHashModuleHash[apiName].Item1,
                                                                 _ApiNameApiHashModuleHash[apiName].Item2);
            }
        }


        public override int GenerateStub(WindowsAssembly pE, ProtectionOptions options)
        {

            try
            {
                // add Hash, GMH and GPA
                AddCommonCode();
                if (base.GenerateStub(pE, options) != -1)
                {
                    byte[] temp = generatedStub; // save stub

                    AddPushAddressesCode(); // a trick to retrieve the addresses of redirected APIs 

                    if (base.GenerateStub(pE, options) != -1)
                    {
                        ReadRedirectedFunctionsAddresses(temp.Length);
                        //System.IO.File.WriteAllBytes("stub.bin", generatedStub); // for debug
                        generatedStub = temp;
                        return generatedStub.Length;
                    }
                }
            }
            catch { }
            return -1;
        }


       
        #endregion //overrides

        #region assembly code
        private string DllLoader
        {
            get
            {
                return
                    "push ebp\r\n" +
                    "call @f\r\n" +
                    "@@:\r\n" +
                    "pop ebp\r\n" +
                    "sub ebp,6\r\n" +
                    string.Format("push 0x{0:X}\r\n", Hash("kernel32.dll")) +
                    "call GMH\r\n" +
                    string.Format("push 0x{0:X}\r\n", Hash("LoadLibraryA")) +
                    "push eax\r\n" +
                    "call GPA\r\n" +
                    "mov ebx,eax\r\n" +
                    "call @f\r\n" +
                    "@@:\r\n" +
                    "pop esi\r\n" +
                    "and si,0xF000\r\n" +
                    "sub esp,100\r\n" +
                    "mov edi,esp\r\n" +
                    "\r\n" +
                    "jmp @isEP\r\n" +
                    "\r\n" +
                    "@notEPyet:\r\n" +
                    "push edi\r\n" +
                    "push edi\r\n" +
                    "\r\n" +
                    "@decrypt:\r\n" +
                    "lodsb\r\n" +
                    string.Format("xor al,0x{0:X}\r\n", Key) +
                    "stosb\r\n" +
                    "cmp byte[edi-1],0\r\n" +
                    "jne @decrypt\r\n" +
                    "\r\n" +
                    "call ebx\r\n" +
                    "pop edi\r\n" +
                    "@isEP:\r\n" +
                    "cmp esi,ebp\r\n" +
                    "jne @notEPyet\r\n" +
                    "add esp,100\r\n" +
                    "pop ebp \r\n" +
                    string.Format("push 0x{0:X}\r\n", oep) +
                    "ret\r\n";
            }
        }
        private string RedirectionCodeSnippet
        {
            get
            {
                return
                    "@{0}:\r\n" +
                    "push {1}\r\n" +
                    "call GMH\r\n" +
                    "push {2}\r\n" +
                    "push eax\r\n" +
                    "call GPA\r\n" +
                    "jmp eax\r\n\r\n";
            }
        }
        private string HashCodeW
        {
            get
            {
                return
                    "@HashW:\r\n" +
                    "push ebp\r\n" +
                    "mov ebp, esp\r\n" +
                    "mov edx, dword [ebp+0x8]\r\n" +
                    "xor eax, eax\r\n" +
                    "Jmp @L66F2\r\n" +

                    "@L66D5:\r\n" +
                    "mov cl, byte [edx]\r\n" +
                    "cmp cl, 0x61\r\n" +
                    "Jl @L66EC\r\n" +
                    "cmp cl, 0x7A\r\n" +
                    "Jg @L66EC\r\n" +
                    "sub cl, 0x20\r\n" +

                    "@L66EC:\r\n" +
                    "xor al, cl\r\n" +
                    "rol eax, cl\r\n" +
                    "inc edx\r\n" +
                    "inc edx\r\n" +

                    "@L66F2:\r\n" +
                    "cmp word [edx], 0x0\r\n" +
                    "Jne @L66D5\r\n" +
                    "leave\r\n" +
                    "retn 0x4\r\n";
            }
        }
        private string HashCodeA
        {
            get
            {
                return
                    "@HashA:\r\n" +
                    "push ebp\r\n" +
                    "mov ebp, esp\r\n" +
                    "mov edx, dword [ebp+0x8]\r\n" +
                    "xor eax, eax\r\n" +
                    "Jmp @L66F2A\r\n" +

                    "@L66D5A:\r\n" +
                    "mov cl, byte [edx]\r\n" +
                    "cmp cl, 0x61\r\n" +
                    "Jl @L66ECA\r\n" +
                    "cmp cl, 0x7A\r\n" +
                    "Jg @L66ECA\r\n" +
                    "sub cl, 0x20\r\n" +

                    "@L66ECA:\r\n" +
                    "xor al, cl\r\n" +
                    "rol eax, cl\r\n" +
                    "inc edx\r\n" +
                    "@L66F2A:\r\n" +
                    "cmp byte [edx], 0x0\r\n" +
                    "Jne @L66D5A\r\n" +
                    "leave\r\n" +
                    "retn 0x4\r\n";
            }
        }
        private string GMH
        {
            get
            {
                return
                    "GMH:\r\n" +
                    "push ebp\r\n" +
                    "mov ebp, esp\r\n" +
                    "sub esp, 8\r\n" +
                    "mov eax, dword [fs:0x30]\r\n" +
                    "mov eax, dword [eax+0xC]\r\n" +
                    "mov eax, dword [eax+0xC]\r\n" +
                    "mov dword [ebp-0x8], eax\r\n" +
                    "push dword [eax+0x4]\r\n" +
                    "pop dword [ebp-0x4]\r\n" +
                    "Jmp @L66BD\r\n" +

                    "@L669D:\r\n" +
                    "mov eax, dword [eax+0x30]\r\n" +
                    "push eax\r\n" +
                    "Call @HashW\r\n" +
                    "cmp eax, dword [ebp+0x8]\r\n" +
                    "Jne @L66B5\r\n" +
                    "mov eax, dword [ebp-0x8]\r\n" +
                    "mov eax, dword [eax+0x18]\r\n" +
                    "leave\r\n" +
                    "retn 0x4\r\n" +

                    "@L66B5:\r\n" +
                    "mov eax, dword [ebp-0x8]\r\n" +
                    "mov eax, dword [eax]\r\n" +
                    "mov dword [ebp-0x8], eax\r\n" +

                    "@L66BD:\r\n" +
                    "cmp eax, dword [ebp-0x4]\r\n" +
                    "Jne @L669D\r\n" +
                    "mov eax, 0xFFFFFFFF\r\n" +
                    "leave\r\n" +
                    "retn 0x4\r\n";
            }
        }
        private string GPA
        {
            get
            {
                return
                    "GPA:\r\n" +
                    "pushad\r\n" +
                    "mov ebx, dword [esp+0x24]\r\n" +
                    "mov ecx, ebx\r\n" +
                    "add ebx, dword [ebx+0x3C]\r\n" +
                    "mov ebx, dword [ebx+0x78]\r\n" +
                    "add ebx, ecx\r\n" +
                    "mov edi, dword [ebx+0x20]\r\n" +
                    "add edi, ecx\r\n" +
                    "xor esi, esi\r\n" +

                    "@L6712:\r\n" +
                    "lea edx, dword [edi+esi*4]\r\n" +
                    "mov edx, dword [edx]\r\n" +
                    "add edx, ecx\r\n" +

                    "push ecx\r\n" +
                    "push edx\r\n" +
                    "call @HashA\r\n" +
                    "pop ecx\r\n" +

                    "cmp eax, dword [esp+0x28]\r\n" +
                    "Je @L6736\r\n" +
                    "inc esi\r\n" +
                    "cmp esi, dword [ebx+0x18]\r\n" +
                    "Jb @L6712\r\n" +
                    "xor eax, eax\r\n" +
                    "Jmp @L6749\r\n" +

                    "@L6736:\r\n" +
                    "mov edx, dword[ebx+0x24]\r\n" +
                    "add edx, ecx\r\n" +
                    "movzx edx,word [edx+esi*2]\r\n" +
                    "mov eax, dword [ebx+0x1C]\r\n" +
                    "add eax, ecx\r\n" +
                    "mov eax, dword [eax+edx*4]\r\n" +
                    "add eax, ecx\r\n" +

                    "@L6749:\r\n" +
                    "mov dword [esp+0x1C], eax\r\n" +
                    "popad\r\n" +
                    "retn 0x8\r\n";
            }


        }
        #endregion

        #region private
        private void AddCommonCode()
        {
            Code += HashCodeW;
            Code += HashCodeA;
            Code += GMH;
            Code += GPA;
        }
        private void AddPushAddressesCode()
        {
            foreach (string apiName in _ApiNameApiHashModuleHash.Keys)
                Code += string.Format("push @{0}\r\n", apiName);
        }

        private void ReadRedirectedFunctionsAddresses(int offset)
        {
            int i = 0;
            foreach (string api in _ApiNameApiHashModuleHash.Keys)
                _ApiNameRedirectionAddress[api] = ReadRedirectedFunctionAddress(offset, i++);
        }

        private int ReadRedirectedFunctionAddress(int offset, int index)
        {
            return BitConverter.ToInt32(generatedStub, offset + (index * 5) + 1);
        }

        private uint Hash(string name)
        {
            byte[] data = Encoding.ASCII.GetBytes(name.ToUpper());
            uint hash = 0;
            foreach (byte b in data)
                hash = RotateLeft(hash ^ b, b);

            return hash;
        }
        private byte[] xorCrypt(string name)
        {
            byte[] crypted = Encoding.ASCII.GetBytes(name + "\0");
            for (int i = 0; i < crypted.Length; i++)
                crypted[i] ^= Key;
            return crypted;
        }
        #region https://stackoverflow.com/a/812035
        public static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
        //public static uint RotateRight(uint value, int count)
        //{
        //    return (value >> count) | (value << (32 - count));
        //}
        #endregion
        #endregion
    }
}
