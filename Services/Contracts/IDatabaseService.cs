using System.Collections.Generic;
using System.Threading.Tasks;
using Iso810.Entities;

namespace Iso810.Services.Contracts
{
    public interface IDatabaseService
    {
        Task SaveData(List<StudentsView> souce);
    }
}