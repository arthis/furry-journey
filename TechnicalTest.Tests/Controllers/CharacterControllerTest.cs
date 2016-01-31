using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TechnicalTest.Domain.Model;
using TechnicalTest.Domain.Services;
using TechnicalTest.Domain.Repositories;
using TechnicalTest.Mvc.Controllers;
using TechnicalTest.Mvc.Models.Commands;

namespace TechnicalTest.Tests
{
    [TestFixture]
    public class CharacterReadControllerTest
    {

        public IRepoAccount createFakeRepo()
        {
            var orgrimm = new Character() { Id = Guid.NewGuid(), Name = "Orgrim Doomhammer", Class = Class.Warrior, Faction = Faction.Horde, Level = 99, Race = Race.Orc, IsActive = true };
            var cairne = new Character() { Id = Guid.NewGuid(), Name = "Cairne Bloodhoof", Class = Class.Druid, Faction = Faction.Horde, Level = 99, Race = Race.Tauren, IsActive = true };
            var Lorthemar = new Character() { Id = Guid.NewGuid(), Name = "Lor'themar Theron", Class = Class.Mage, Faction = Faction.Horde, Level = 99, Race = Race.BloodElf, IsActive = true };

            var wowAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() { orgrimm, Lorthemar } };
            var wow2Account = new Account() { Id = Guid.NewGuid(), Name = "wow2", Password = "wow2", Characters = new List<Character>() { cairne } };

            var ds = new Dictionary<string, Account>() { { "wow", wowAccount }, { "wow2", wow2Account } };

             return new SimpleRepoAccount(ds);
        }


        [Test]
        public async Task Get_will_fetch_only_data_for_the_Account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            CharacterController controller = new CharacterController(serviceAccount);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(result.ElementAt(1).Name, "Lor'themar Theron");
        }

        [Test]
        public async Task Add_Correct_Character_To_Account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            CharacterController controller = new CharacterController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = (int)Class.Warrior,
                Faction = (int)Faction.Horde,
                Level = 99,
                Race = (int)Race.Orc
            };

            // Act
            var result = await controller.CreateCharacterAsync(cmd);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        [Test]
        public async Task Add_Validated_Character_To_Account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            CharacterController controller = new CharacterController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = (int)Class.Warrior,
                Faction = (int)Faction.Horde,
                Level = 99,
                Race = (int)Race.Human
            };

            // Act
            var result = await controller.CreateCharacterAsync(cmd);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }
    }
}