using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TechnicalTest.Domain.Repositories;
using TechnicalTest.Domain;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Tests.Repositories
{
    [TestFixture]
    public class RepoAccountTests
    {
        [Test]
        public void Authenticate()
        {
            var expecteUser = new Account() { Name = "wow", Password = "wow" };
            var ds = new Dictionary<string, Account>() { { "wow", expecteUser } };
            IRepoAccount repo = new MemoryRepoAccount(ds);

            var result = repo.Get("wow");

            Assert.AreEqual(expecteUser, result);

        }
    }
}
