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
using System.Web.Mvc;

namespace TechnicalTest.Tests
{
    [TestFixture]
    public class AccountControllerTest
    {

        public Response AssertResponse(ActionResult result)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result);
            var response = ((JsonResult)result).Data as Response;
            Assert.IsNotNull(response);
            return response;
        }

        public IRepoAccount createFakeRepo()
        {
            var orgrimm = new Character() { Id = Guid.NewGuid(), Name = "Orgrim Doomhammer", Class = ClassFactory.Warrior, Faction = FactionFactory.Horde, Level = 100, Race = RaceFactory.Orc };
            var cairne = new Character() { Id = Guid.NewGuid(), Name = "Cairne Bloodhoof", Class = ClassFactory.Druid, Faction = FactionFactory.Horde, Level = 100, Race = RaceFactory.Tauren  };
            var Lorthemar = new Character() { Id = Guid.NewGuid(), Name = "Lor'themar Theron", Class = ClassFactory.Mage, Faction = FactionFactory.Horde, Level = 100, Race = RaceFactory.BloodElf };

            var wowAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() { orgrimm, Lorthemar } };
            var wow2Account = new Account() { Id = Guid.NewGuid(), Name = "wow2", Password = "wow2", Characters = new List<Character>() { cairne } };

            var ds = new Dictionary<string, Account>() { { "wow", wowAccount }, { "wow2", wow2Account } };

            return new MemoryRepoAccount(ds);
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
            var cmd = new CreateCharacterCmd()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = ClassFactory.Warrior.ToString(),
                Faction = FactionFactory.Horde.ToString(),
                Level = 100,
                Race = RaceFactory.Orc.ToString()
            };

            // Act
            var result  = await controller.AddNewCharacterAsync(cmd);

            // Assert
            var response = AssertResponse(result);
            Assert.IsTrue(response.IsOk);
        }

        static object[] send_invalid_command_to_create_Cases =
        {
            new object[] { Guid.NewGuid(), "",  ClassFactory.Warrior.ToString(),  FactionFactory.Horde.ToString(), 100 ,RaceFactory.Orc.ToString()  },
            new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  "Wawa",  FactionFactory.Horde.ToString(), 100 ,RaceFactory.Orc.ToString()  },
            new object[] { Guid.NewGuid(), "Orgrim Doomhammer", ClassFactory.Warrior.ToString(),  "Ordos", 100 ,RaceFactory.Orc.ToString()  },
            //new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  ClassFactory.Warrior.ToString(),  Faction.Horde.ToString(), 0 ,RaceFactory.Orc.ToString()  },
            //new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  ClassFactory.Warrior.ToString(),  Faction.Horde.ToString(), 101 ,RaceFactory.Orc.ToString()  },
            new object[] { Guid.NewGuid(), "Orgrim Doomhammer",  ClassFactory.Warrior.ToString(),  FactionFactory.Horde.ToString(), 100 , "ordos"  }

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
            var cmd = new CreateCharacterCmd()
            {
                Id = id,
                Name = name,
                Class = @class,
                Faction = faction,
                Level = level,
                Race = race
            };

            // Act
            var result = await controller.AddNewCharacterAsync(cmd);

            // Assert
            var response = AssertResponse(result);
            Assert.IsFalse(response.IsOk);
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
            var cmd = new CreateCharacterCmd()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = ClassFactory.Warrior.ToString(),
                Faction = FactionFactory.Horde.ToString(),
                Level = 100,
                Race = RaceFactory.Orc.ToString()
            };

            // Act
            var result = await controller.AddNewCharacterAsync(cmd);

            // Assert
            var response = AssertResponse(result);
            Assert.IsTrue(response.IsOk);
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
            var cmd = new CreateCharacterCmd() {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = ClassFactory.Warrior.ToString(),
                Faction = FactionFactory.Horde.ToString(),
                Level = 100,
                Race = RaceFactory.Orc.ToString()
            };
            await controller.AddNewCharacterAsync(cmd);
            var currentAccount = serviceAccount.getCurrentAccount();
            var idCharacter = currentAccount.Characters[0].Id;
            var cmdRemove = new RemoveCharacterCmd() { Id = idCharacter };

            // Act
            var result = await controller.RemoveCharacterAsync(cmdRemove);

            // Assert
            var response = AssertResponse(result);
            Assert.IsTrue(response.IsOk);
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
            var cmd = new CreateCharacterCmd()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = ClassFactory.Warrior.ToString(),
                Faction = FactionFactory.Horde.ToString(),
                Level = 100,
                Race = RaceFactory.Orc.ToString()
            };
            await controller.AddNewCharacterAsync(cmd);
            var cmdRemove = new RemoveCharacterCmd() { Id = Guid.Empty };

            // Act
            var result = await controller.RemoveCharacterAsync(cmdRemove);

            // Assert
            var response = AssertResponse(result);
            Assert.IsFalse(response.IsOk);
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
            var cmd = new CreateCharacterCmd()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = ClassFactory.Warrior.ToString(),
                Faction = FactionFactory.Horde.ToString(),
                Level = 100,
                Race = RaceFactory.Orc.ToString()
            };
            await controller.AddNewCharacterAsync(cmd);
            var currentAccount = serviceAccount.getCurrentAccount();
            var idCharacter = currentAccount.Characters[0].Id;
            await controller.RemoveCharacterAsync(new RemoveCharacterCmd() { Id = idCharacter });

            var cmdRetrieve = new RetrieveCharacterCmd() { Id = idCharacter };
            // Act
            var result = await controller.RetrieveCharacterAsync(cmdRetrieve);

            // Assert
            var response = AssertResponse(result);
            Assert.IsTrue(response.IsOk);
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
            var cmd = new CreateCharacterCmd()
            {
                Id = Guid.NewGuid(),
                Name = "Orgrim Doomhammer",
                Class = ClassFactory.Warrior.ToString(),
                Faction = FactionFactory.Horde.ToString(),
                Level = 100,
                Race = RaceFactory.Orc.ToString()
            };
            await controller.AddNewCharacterAsync(cmd);
            var currentAccount = serviceAccount.getCurrentAccount();
            var idCharacter = currentAccount.Characters[0].Id;
            await controller.RemoveCharacterAsync(new RemoveCharacterCmd() {  Id=idCharacter});
            var retrieveCmd = new RetrieveCharacterCmd() { Id = Guid.Empty };

            // Act
            var result = await controller.RetrieveCharacterAsync(retrieveCmd);

            // Assert
            var response = AssertResponse(result);
            Assert.IsFalse(response.IsOk);
        }

    }
}