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

        [Test]
        public async Task Create_character_on_an_empty_account()
        {
            // Arrange
            var wowAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() { } };

            // Act
            var result = await wowAccount.CreateCharacterAsync("Cairne Bloodhoof", 99, Race.Tauren, Faction.Horde, Class.Druid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }


        static object[] Orc_Tauren_and_Blood_Elves_belong_to_the_Horde_Cases =
        {
            new object[] { "Orgrim Doomhammer", 99, Race.Orc, Faction.Horde, Class.Warrior },
            new object[] { "Lor'themar Theron", 99, Race.BloodElf, Faction.Horde, Class.Mage },
            new object[] { "Cairne Bloodhoof", 99, Race.Tauren, Faction.Horde, Class.Druid },
        };
        [Test, TestCaseSource("Orc_Tauren_and_Blood_Elves_belong_to_the_Horde_Cases")]
        public async Task Orc_Tauren_and_Blood_Elves_belong_to_the_Horde(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }


        static object[] Orc_Tauren_and_Blood_Elves_do_not_belong_to_the_Alliance_Cases =
        {
            new object[] { "Orgrim Doomhammer", 99, Race.Orc, Faction.Alliance, Class.Warrior },
            new object[] { "Lor'themar Theron", 99, Race.BloodElf, Faction.Alliance, Class.Mage },
            new object[] { "Cairne Bloodhoof", 99, Race.Tauren, Faction.Alliance, Class.Druid },
        };
        [Test, TestCaseSource("Orc_Tauren_and_Blood_Elves_do_not_belong_to_the_Alliance_Cases")]
        public async Task Orc_Tauren_and_Blood_Elves_do_not_belong_to_the_Alliance(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

        static object[] Human_Gnome_and_Worgen_belong_to_the_Alliance_Cases =
        {
            new object[] { "Varian Wrynn", 99, Race.Human, Faction.Alliance, Class.Warrior },
            new object[] { "Gelbin Mekkatorque", 99, Race.Gnome, Faction.Alliance, Class.Warrior },
            new object[] { "Genn Greymane", 99, Race.Worgen, Faction.Alliance, Class.Druid },
        };
        [Test, TestCaseSource("Human_Gnome_and_Worgen_belong_to_the_Alliance_Cases")]
        public async Task Human_Gnome_and_Worgen_belong_to_the_Alliance(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        static object[] Human_Gnome_and_Worgen_do_not_belong_to_the_Horde_Cases =
        {
            new object[] { "Varian Wrynn", 99, Race.Human, Faction.Horde, Class.Warrior },
            new object[] { "Gelbin Mekkatorque", 99, Race.Gnome, Faction.Horde, Class.Warrior },
            new object[] { "Genn Greymane", 99, Race.Worgen, Faction.Horde, Class.Druid },
        };
        [Test, TestCaseSource("Human_Gnome_and_Worgen_do_not_belong_to_the_Horde_Cases")]
        public async Task Human_Gnome_and_Worgen_do_not_belong_to_the_Horde(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

        static object[] Worgen_and_Tauren_can_be_druid_Cases =
        {
            new object[] { "Cairne Bloodhoof", 99, Race.Tauren, Faction.Horde, Class.Druid },
            new object[] { "Genn Greymane", 99, Race.Worgen, Faction.Alliance, Class.Druid }
        };
        [Test, TestCaseSource("Worgen_and_Tauren_can_be_druid_Cases")]
        public async Task Worgen_and_Tauren_can_be_druid(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        static object[] Human_Gnome_Orc_and_BloodElves_cannot_be_druid_Cases =
        {
            new object[] { "Varian Wrynn", 99, Race.Human, Faction.Alliance, Class.Druid },
            new object[] { "Gelbin Mekkatorque", 99, Race.Gnome, Faction.Alliance, Class.Druid },
            new object[] { "Orgrim Doomhammer", 99, Race.Orc, Faction.Horde, Class.Druid },
            new object[] { "Lor'themar Theron", 99, Race.BloodElf, Faction.Horde, Class.Druid }
        };
        [Test, TestCaseSource("Human_Gnome_Orc_and_BloodElves_cannot_be_druid_Cases")]
        public async Task Human_Gnome_Orc_and_BloodElves_cannot_be_druid(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

        static object[] BloodElves_cannot_be_warrior_Cases =
        {
            new object[] { "Lor'themar Theron", 99, Race.BloodElf, Faction.Horde, Class.Warrior }
        };
        [Test, TestCaseSource("BloodElves_cannot_be_warrior_Cases")]
        public async Task BloodElves_cannot_be_warrior(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

        

        static object[] Except_BloodElves_all_can_be_warrior_Cases =
        {
            new object[] { "Varian Wrynn", 99, Race.Human, Faction.Alliance, Class.Warrior},
            new object[] { "Gelbin Mekkatorque", 99, Race.Gnome, Faction.Alliance, Class.Warrior},
            new object[] { "Orgrim Doomhammer", 99, Race.Orc, Faction.Horde, Class.Warrior},
            new object[] { "Cairne Bloodhoof", 99, Race.Tauren, Faction.Horde, Class.Warrior},
            new object[] { "Genn Greymane", 99, Race.Worgen, Faction.Alliance, Class.Warrior}
        };
        [Test, TestCaseSource("Except_BloodElves_all_can_be_warrior_Cases")]
        public async Task Except_BloodElves_all_can_be_warrior(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }

        static object[] when_account_has_not_one_lvl_55__then_it_cannot_create_death_knight_Cases =
        {
            new object[] { "Genn Greymane", 99, Race.Worgen, Faction.Alliance, Class.DeathKnight }
        };
        [Test, TestCaseSource("when_account_has_not_one_lvl_55__then_it_cannot_create_death_knight_Cases")]
        public async Task when_account_has_not_one_lvl_55__then_it_cannot_create_death_knight(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOk);
        }

        static object[] when_account_has_one_lvl_55__then_it_can_create_death_knight_Cases =
        {
            new object[] { "Genn Greymane", 99, Race.Worgen, Faction.Alliance, Class.DeathKnight }
        };
        [Test, TestCaseSource("when_account_has_one_lvl_55__then_it_can_create_death_knight_Cases")]
        public async Task when_account_has_one_lvl_55__then_it_can_create_death_knight(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            await newAccount.CreateCharacterAsync("Cairne Bloodhoof", 99, Race.Tauren, Faction.Horde, Class.Druid);

            // Act
            var result = await newAccount.CreateCharacterAsync(name, level, race, faction, @class);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOk);
        }


    }
}