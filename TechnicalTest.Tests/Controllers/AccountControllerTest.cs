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
    public class AccountControllerTest
    {
        public IRepoAccount createFakeRepo()
        {
            var orgrimm = new Character() { Id = Guid.NewGuid(), Name = "Orgrim Doomhammer", Class = Class.Warrior, Faction = Faction.Horde, Level = 100, Race = Race.Orc };
            var cairne = new Character() { Id = Guid.NewGuid(), Name = "Cairne Bloodhoof", Class = Class.Druid, Faction = Faction.Horde, Level = 100, Race = Race.Tauren  };
            var Lorthemar = new Character() { Id = Guid.NewGuid(), Name = "Lor'themar Theron", Class = Class.Mage, Faction = Faction.Horde, Level = 100, Race = Race.BloodElf };

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
            AccountController controller = new AccountController(serviceAccount);

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
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = Class.Warrior.ToString(),
                Faction = Faction.Horde.ToString(),
                Level = 100,
                Race = Race.Orc.ToString()
            };

            // Act
            var result = await controller.CreateCharacterAsync(cmd);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        static object[] send_invalid_command_to_create_Cases =
        {
            new object[] { Guid.NewGuid(), "",  Class.Warrior.ToString(),  Faction.Horde.ToString(), 100 ,Race.Orc.ToString()  },
            new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  "Wawa",  Faction.Horde.ToString(), 100 ,Race.Orc.ToString()  },
            new object[] { Guid.NewGuid(), "Orgrim Doomhammer", Class.Warrior.ToString(),  "Ordos", 100 ,Race.Orc.ToString()  },
            //new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  Class.Warrior.ToString(),  Faction.Horde.ToString(), 0 ,Race.Orc.ToString()  },
            //new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  Class.Warrior.ToString(),  Faction.Horde.ToString(), 101 ,Race.Orc.ToString()  },
            new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  Class.Warrior.ToString(),  Faction.Horde.ToString(), 100 , "ordos"  }

        };
        [Test, TestCaseSource("send_invalid_command_to_create_Cases")]
        public async Task send_invalid_command_to_create_character(Guid id, string name, string @class, string faction, int level, string race)
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = id,
                Name = name,
                Class = @class,
                Faction = faction,
                Level = level,
                Race = race
            };

            // Act
            var result = await controller.CreateCharacterAsync(cmd);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

        [Test]
        public async Task Add_valid_character_to_account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = Class.Warrior.ToString(),
                Faction = Faction.Horde.ToString(),
                Level = 100,
                Race = Race.Orc.ToString()
            };

            // Act
            var result = await controller.CreateCharacterAsync(cmd);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        [Test]
        public async Task remove_active_character_from_account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter() {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = Class.Warrior.ToString(),
                Faction = Faction.Horde.ToString(),
                Level = 100,
                Race = Race.Orc.ToString()
            };
            await controller.CreateCharacterAsync(cmd);
            var currentAccount = serviceAccount.getCurrentAccount();
            var idCharacter = currentAccount.Characters[0].Id;

            // Act
            var result = await controller.RemoveCharacterAsync(idCharacter);  

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        [Test]
        public async Task remove_empty_guid_character_from_account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = Class.Warrior.ToString(),
                Faction = Faction.Horde.ToString(),
                Level = 100,
                Race = Race.Orc.ToString()
            };
            await controller.CreateCharacterAsync(cmd);
            var idCharacter = Guid.Empty;

            // Act
            var result = await controller.RemoveCharacterAsync(idCharacter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }


        [Test]
        public async Task retrieve_inactive_character_from_account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = Class.Warrior.ToString(),
                Faction = Faction.Horde.ToString(),
                Level = 100,
                Race = Race.Orc.ToString()
            };
            await controller.CreateCharacterAsync(cmd);
            var currentAccount = serviceAccount.getCurrentAccount();
            var idCharacter = currentAccount.Characters[0].Id;
            await controller.RemoveCharacterAsync(idCharacter);

            // Act
            var result = await controller.RetrieveCharacterAsync(idCharacter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        [Test]
        public async Task retrieve_empty_guid_character_from_account()
        {
            // Arrange
            IRepoAccount repoAccount = createFakeRepo();
            Account mutableAccount = null;
            Action<Account> saveUserToSession = (account) => mutableAccount = account;
            Func<Account> getFromSession = () => mutableAccount;
            IServiceAccount serviceAccount = new ServiceAccount(repoAccount, saveUserToSession, getFromSession);

            serviceAccount.authenticate("wow", "wow");
            AccountController controller = new AccountController(serviceAccount);
            var cmd = new CreateCharacter()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = Class.Warrior.ToString(),
                Faction = Faction.Horde.ToString(),
                Level = 100,
                Race = Race.Orc.ToString()
            };
            await controller.CreateCharacterAsync(cmd);
            var currentAccount = serviceAccount.getCurrentAccount();
            var idCharacter = currentAccount.Characters[0].Id;
            await controller.RemoveCharacterAsync(idCharacter);

            // Act
            var result = await controller.RetrieveCharacterAsync(Guid.Empty);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

    }
}