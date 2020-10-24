using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Iso810.Entities;

namespace Iso810.Services.Contracts
{
    public interface IXmlService
    {
        Task<byte[]> SaveFile();
        Task<ICollection<StudentsView>> ImportFile(Stream stream);
    }
}