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
        Action<Account> _saveUserToSession;
        Func<Account> _getFromSession;
        IRepoAccount _repoAccount;

        public ServiceAccount(IRepoAccount repoAccount , Action<Account> saveUserToSession, Func<Account> getFromSession)
        {
            if (saveUserToSession == null) throw new ArgumentNullException("saveUserToSession cannot be null");
            if (getFromSession == null) throw new ArgumentNullException("getFromSession cannot be null");
            if (repoAccount == null) throw new ArgumentNullException("repoAccount cannot be null");

            _repoAccount = repoAccount;
            _saveUserToSession = saveUserToSession;
            _getFromSession = getFromSession;
        }

        public bool authenticate(string username, string password)
        {
            var account = _repoAccount.Get(username);
            if (account != null && account.Password == password)
            {
                _saveUserToSession(account);
                return true;
            }
            return false;
        }

        public Account getCurrentAccount()
        {
            return _getFromSession();
        }

        public async Task<IEnumerable<Character>> GetCharactersAsync()
        {
            var currentSession = _getFromSession();

            return await _repoAccount.GetCharactersAsync(currentSession.Id);
        }

        public async Task<bool> CreateCharacterAsync(Guid id, string name, int level, Race race, Faction faction, Class @class)
        {
            var currentSession = _getFromSession();

            var account = _repoAccount.GetById(currentSession.Id);

            if (account.AddNewCharacter(id,name, level, race, faction, @class))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> RemoveCharacterAsync(Guid idCharacter)
        {
            var currentSession = _getFromSession();

            var account = _repoAccount.GetById(currentSession.Id);

            if (account.RemoveCharacter(idCharacter))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> RetrieveCharacterAsync(Guid idCharacter)
        {
            var currentSession = _getFromSession();

            var account = _repoAccount.GetById(currentSession.Id);

            if (account.RetrieveCharacter(idCharacter))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }
    }
}
