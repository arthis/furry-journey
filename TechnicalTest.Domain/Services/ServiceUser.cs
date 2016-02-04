using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;
using TechnicalTest.Domain.Repositories;

namespace TechnicalTest.Domain.Services
{
    public class ServiceUser : IServiceUser
    {
        Action<User> _saveUserToSession;
        Func<User> _getFromSession;
        IRepoUser _repoUser;

        public ServiceUser(IRepoUser repoUser, Action<User> saveUserToSession, Func<User> getFromSession)
        {
            if (saveUserToSession == null) throw new ArgumentNullException("saveUserToSession cannot be null");
            if (getFromSession == null) throw new ArgumentNullException("getFromSession cannot be null");
            if (repoUser == null) throw new ArgumentNullException("repoUser cannot be null");

            _repoUser = repoUser;
            _saveUserToSession = saveUserToSession;
            _getFromSession = getFromSession;
        }

        public bool authenticate(string username, string password)
        {
            var account = _repoUser.Get(username);
            if (account != null && account.Password == password)
            {
                _saveUserToSession(account);
                return true;
            }
            return false;
        }

        public User getCurrentUser()
        {
            return _getFromSession();
        }
    }
}
