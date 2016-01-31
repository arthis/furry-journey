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
            if (saveUserToSession == null) throw new Exception("saveUserToSession cannot be null");
            if (getFromSession == null) throw new Exception("getFromSession cannot be null");
            if (repoAccount == null) throw new Exception("repoAccount cannot be null");

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
            var account = _repoAccount.GetById(currentSession.Id);

            return await _repoAccount.GetCharactersAsync(account);
        }

        public async Task<bool> CreateCharacterAsync(string name, int level, Race race, Faction faction, Class @class)
        {
            var currentSession = _getFromSession();
            var account = _repoAccount.GetById(currentSession.Id);

            if (account.CreateCharacter(name, level, race, faction, @class))
                return await _repoAccount.SaveAsync(account);
            else
                return await Task.FromResult(false);
        }

        
    }
}
