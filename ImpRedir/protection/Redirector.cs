using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportRedir.Interfaces;
using AsmResolver;
using System.IO;

namespace ImportRedir.protection
{
    public class Redirector
    {
        private string filename;
        private byte[] bytes;
        private WindowsAssembly PE;
        private Exception _exception;

        public Exception Exception { get => _exception;}

        public static Redirector Load(string filename)
        {
            try
            {
                return new Redirector(filename);
            }
            catch
            {
                return null;
            }
        }

        private Redirector(string fileName)
        {
            this.filename = fileName;
            this.bytes = File.ReadAllBytes(filename);
            this.PE = WindowsAssembly.FromFile(filename);

        }
        public bool Protect(ProtectionOptions options)
        {
            return Protect(options,
                           new Stub((uint)(PE.NtHeaders.OptionalHeader.AddressOfEntrypoint +
                                            PE.NtHeaders.OptionalHeader.ImageBase),
                                    100000 // need a huge memory for FASM to work
                                   )
                          );
        }

        public bool Protect(ProtectionOptions options, IStub stub)
        {
            try
            {
                if (options.AddDllLoader)
                {
                    stub.AddDllsToLoad(PE); // copy (encrypted) names of imported dlls
                    stub.GenerateLoaderCode();
                }
                stub.AddAPIs(PE); // copy (encrypted / hash of) imported apis
                stub.GenerateRedirectionCode(PE); // generate code of every function that handle a redirected API
                stub.GenerateStub(PE, options); // compile stub
                stub.ImplementRedirection(PE,bytes); // add a section containing bytes of generated stub, modify IAT to redirect APIs

                //stub.DestroyImportDirectory(PE); // empty the import directory and patch the header
                //stub.MoveEP(PE);
                return true;
            }
            catch (Exception ex)
            {
                this._exception = ex;
                return false;
            }
        }
    }


    public class ProtectionOptions
    {
        public bool AddDllLoader;
        public ProtectionOptions()
        {

        }
    }
}
