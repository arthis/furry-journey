using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;
using TechnicalTest.Domain.Repositories;

namespace TechnicalTest.Domain.Services
{
    public class ServiceAccount : IServiceAccount
    {
        IRepoAccount _repoAccount;

        public ServiceAccount(IRepoAccount repoAccount)
        {
            if (repoAccount == null) throw new ArgumentNullException("repoAccount cannot be null");

            _repoAccount = repoAccount;
        }


        public async Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId)
        {
            return await _repoAccount.GetCharactersAsync(accountId);
        }

        public async Task<bool> CreateCharacterAsync(Guid accountId, Guid id, string name, int level, Race race, Faction faction, Class @class)
        {

            var account = await _repoAccount.GetByIdAsync(accountId);

            if (account.AddNewCharacter(id,name, level, race, faction, @class))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> RemoveCharacterAsync(Guid accountId, Guid idCharacter)
        {
            var account = await _repoAccount.GetByIdAsync(accountId);

            if (account.RemoveCharacter(idCharacter))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> RetrieveCharacterAsync(Guid accountId, Guid idCharacter)
        {
            var account = await _repoAccount.GetByIdAsync(accountId);

            if (account.RetrieveCharacter(idCharacter))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }
    }
}
