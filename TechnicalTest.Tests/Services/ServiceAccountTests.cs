using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain;
using TechnicalTest.Domain.Model;
using TechnicalTest.Domain.Repositories;
using TechnicalTest.Domain.Services;

namespace TechnicalTest.Tests.Services
{
    [TestFixture]
    public class ServiceAccountTests
    {
        [Test]
        public void Authenticate()
        {
            var mutableBoolean = false;
            var ds = new Dictionary<string, Account>() { { "wow", new Account() { Name = "wow", Password = "wow" } } };
            IRepoAccount repo = new SimpleRepoAccount(ds);
            Action<Account> saveUserToSession = (account) => mutableBoolean = true;
            Func<Account> getFromSession = () => null;
            IServiceAccount serviceAccount = new ServiceAccount(repo, saveUserToSession,getFromSession);

            var result = serviceAccount.authenticate("wow", "wow");

            Assert.IsTrue(result);
            Assert.IsTrue(mutableBoolean);

        }


        [Test]
        public void getCurrentAccount()
        {
            Account mutableAccount = null;
            var ds = new Dictionary<string, Account>() { { "wow", new Account() { Name = "wow", Password = "wow" } } };
            IRepoAccount repo = new SimpleRepoAccount(ds);
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repo, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            var result = serviceAccount.getCurrentAccount();

            Assert.AreEqual(mutableAccount,result);

        }
        
    }
}
