using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Services
{
    public interface IServiceAccount
    {
        Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId);
        Task<bool> CreateCharacterAsync(Guid accountId, Guid id, string name, int level, Race race, Faction faction, Class @class);
        Task<bool> RemoveCharacterAsync(Guid accountId, Guid idCharacter);
        Task<bool> RetrieveCharacterAsync(Guid accountId, Guid idCharacter);
        
    }
}