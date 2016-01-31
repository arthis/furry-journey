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
    public class AccountTest
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
        public async Task create_Character()
        {
            // Arrange
            var wowAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() {  } };

            // Act
            var result = await wowAccount.CreateCharacterAsync("Cairne Bloodhoof", 99, Race.Tauren, Faction.Horde, Class.Druid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        [Test]
        public async Task orc_is_exclusively_hord()
        {
            // Arrange
            var orgrimm = new Character() { Id = Guid.NewGuid(), Name = "Orgrim Doomhammer", Class = Class.Warrior, Faction = Faction.Horde, Level = 99, Race = Race.Orc, IsActive = true };
            var Lorthemar = new Character() { Id = Guid.NewGuid(), Name = "Lor'themar Theron", Class = Class.Mage, Faction = Faction.Horde, Level = 99, Race = Race.BloodElf, IsActive = true };

            var cairne = new Character() { Id = Guid.NewGuid(), Name = "Cairne Bloodhoof", Class = Class.Druid, Faction = Faction.Horde, Level = 99, Race = Race.Tauren, IsActive = true };

            var wowAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>()  };

            // Act
            var result = await wowAccount.CreateCharacterAsync("Orgrim Doomhammer", 99, Race.Orc, Faction.Alliance, Class.Warrior);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }


    }
}