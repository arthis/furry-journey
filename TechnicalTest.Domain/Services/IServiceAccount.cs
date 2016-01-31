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
        bool authenticate(string username, string password);
        Account getCurrentAccount();
        Task<IEnumerable<Character>> GetCharactersAsync();
        Task<ValidationResult> CreateCharacterAsync(string name, int level, Race race, Faction faction, Class @class);
    }
}