using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iso810.Entities;

namespace Iso810.Services.Contracts
{
    public interface ICsvService
    {
        Task<ICollection<StudentsView>> UploadData(string text);
        Task<string> DownloadData();
        string GetHeaders();
    }
}