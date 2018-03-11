using AsmResolver;
using ImportRedir.protection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportRedir.Interfaces
{
    public interface IStub
    {
        void AddDllsToLoad(WindowsAssembly PE);
        void AddAPIs(WindowsAssembly pE);
        void GenerateRedirectionCode(WindowsAssembly pE);
        int GenerateStub(WindowsAssembly pE, ProtectionOptions options);
        void ImplementRedirection(WindowsAssembly pE,byte[] peData,ProtectionOptions options);
        void DestroyImportDirectory(WindowsAssembly pE);
        void GenerateLoaderCode();
    }
}
