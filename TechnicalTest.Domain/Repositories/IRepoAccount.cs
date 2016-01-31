using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public interface IRepoAccount
    {
        Account Get(string username);
        Task<IEnumerable<Character>> GetCharactersAsync(Account account);
        Account GetById(Guid id);
    }
}
