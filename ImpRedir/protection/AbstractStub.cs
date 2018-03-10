using AsmResolver;
using AsmResolver.X86;
using Binarysharp.Assemblers.Fasm;
using ImportRedir.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportRedir.protection
{
    abstract class AbstractStub : IStub
    {
        protected uint oep;
        protected int memorySize;
        protected byte[] generatedStub;
        public AbstractStub(uint OEP, int memorySize)
        {
            this.oep = OEP;
            this.memorySize = memorySize;
        }

        public abstract void AddAPIs(WindowsAssembly pE);

        public abstract void AddDllsToLoad(WindowsAssembly PE);

        public virtual void DestroyImportDirectory(WindowsAssembly pE)
        {
            foreach (ImageModuleImport im in pE.ImportDirectory.ModuleImports)
            {
                im.Name = "";
                // to complete
            }
        }

        public abstract void GenerateLoaderCode();

        public abstract void GenerateRedirectionCode(WindowsAssembly pE);

        public virtual int GenerateStub(WindowsAssembly pE, ProtectionOptions options)
        {
            try
            {
                FasmNet fasmNet = new FasmNet(memorySize, 10); // if compilcation fails increase memorySize parameter
                fasmNet.AddLine(Code);
                generatedStub = fasmNet.Assemble(NextSectionRVA(pE.NtHeaders.OptionalHeader.SectionAlignment,
                                                pE.SectionHeaders.Last()) +
                                                (uint)pE.NtHeaders.OptionalHeader.ImageBase);
                return generatedStub.Length;
            }
            catch { }
            return -1;
        }

        protected uint NextSectionRVA(uint sectionAlignment, ImageSectionHeader imageSectionHeader)
        {
            return AlignTo(sectionAlignment, imageSectionHeader.VirtualAddress + imageSectionHeader.VirtualSize);
        }
        protected uint AlignTo(uint alignment, uint value)
        {
            uint aligned = alignment;
            while (aligned < value)
                aligned += alignment;
            return aligned;
        }

        public virtual void ImplementRedirection(WindowsAssembly pE, byte[] peData)
        {
            this.dataStream = new MemoryStream();
            int EOF = EndOfFileOffset(pE.SectionHeaders.Last());

            WriteSetions(peData, EOF);
            WriteStubSection(pE.NtHeaders.OptionalHeader.FileAlignment);
            WriteEndOfFile(peData, EOF);
            WriteHeaders(pE);

            //File.WriteAllBytes("test.exe", dataStream.ToArray());
        }

        private void WriteHeaders(WindowsAssembly pE)
        {
            WriteNtHeaders(pE, WriteSectionHeader(pE));
        }

        private void WriteNtHeaders(WindowsAssembly pE, ImageSectionHeader section)
        {
            pE.NtHeaders.OptionalHeader.AddressOfEntrypoint = section.VirtualAddress + (uint)EPoffset;
            pE.NtHeaders.FileHeader.NumberOfSections++;
            pE.NtHeaders.OptionalHeader.SizeOfImage =  GetSizeOfImage(pE);

            dataStream.Seek(pE.NtHeaders.StartOffset, SeekOrigin.Begin);
            pE.NtHeaders.Write(new WritingContext(pE, new BinaryStreamWriter(dataStream), null));
        }

        private uint GetSizeOfImage(WindowsAssembly pE)
        {
            ImageSectionHeader last = pE.SectionHeaders.Last();
            return AlignTo(pE.NtHeaders.OptionalHeader.SectionAlignment, last.VirtualAddress+ last.VirtualSize );
        }

        private ImageSectionHeader WriteSectionHeader(WindowsAssembly pE)
        {
            int newSectionHeaderOffset = (int)pE.SectionHeaders.Last().StartOffset + 0x28;

            if (newSectionHeaderOffset + 0x28 > pE.SectionHeaders.First().PointerToRawData)
                throw new Exception("Not ehought space in header to add a new section");

            byte[] stubSectionHeader;
            ImageSectionHeader section = PrepareNewSection(pE, out stubSectionHeader);
            pE.SectionHeaders.Add(section);

            dataStream.Seek(newSectionHeaderOffset, SeekOrigin.Begin);
            if (stubSectionHeader != null)
                dataStream.Write(stubSectionHeader, 0, stubSectionHeader.Length);
            else
                throw new Exception("Can't generate IMAGE_SECTION_HEADER");

            return section;
        }

        private void WriteEndOfFile(byte[] peData, int EOF)
        {
            if (EOF < peData.Length)
                dataStream.Write(peData, EOF, peData.Length - EOF);
        }

        private void WriteStubSection(uint fileAlignment)
        {
            dataStream.Write(generatedStub, 0, generatedStub.Length);
            byte[] empty = new byte[AlignTo(fileAlignment, ((uint)dataStream.Length)) - dataStream.Length];
            dataStream.Write(empty, 0, empty.Length);
        }

        private void WriteSetions(byte[] peData, int EOF)
        {
            dataStream.Write(peData, 0, EOF < peData.Length ? EOF : peData.Length);
        }

        private int EndOfFileOffset(ImageSectionHeader lastImageSectionHeader)
        {
            return (int)(lastImageSectionHeader.PointerToRawData + lastImageSectionHeader.SizeOfRawData);
        }

        protected ImageSectionHeader PrepareNewSection(WindowsAssembly pE, out byte[] data)
        {
            ImageSectionHeader newSection = new ImageSectionHeader();
            ImageSectionHeader previous = pE.SectionHeaders.Last();

            newSection.PointerToRawData = previous.PointerToRawData + previous.SizeOfRawData;
            newSection.VirtualAddress = NextSectionRVA(pE.NtHeaders.OptionalHeader.SectionAlignment, previous);
            newSection.VirtualSize = AlignTo(pE.NtHeaders.OptionalHeader.SectionAlignment, (uint)generatedStub.Length);
            newSection.SizeOfRawData = AlignTo(pE.NtHeaders.OptionalHeader.FileAlignment, (uint)generatedStub.Length);
            newSection.Name = ".CIR";
            newSection.Attributes = ImageSectionAttributes.MemoryExecute | ImageSectionAttributes.MemoryRead | ImageSectionAttributes.ContentCode;

            data = new byte[0x28];
            newSection.Write(new WritingContext(null, new BinaryStreamWriter(new System.IO.MemoryStream(data)), null));
            return newSection;
        }


        public string Code { get; set; }

        public int EPoffset; // offset from loader RVA to EntryPoint
        protected MemoryStream dataStream;
    }
}
