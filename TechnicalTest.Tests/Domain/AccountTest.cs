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
        public void remove_an_active_character_from_an_account()
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            newAccount.AddNewCharacter(Guid.NewGuid(), "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Warrior);
            var idCharacter = newAccount.Characters[0].Id;
            // Act
            var result = newAccount.RemoveCharacter(idCharacter);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Where(c=> c.IsActive).Count(), 0);
            Assert.AreEqual(newAccount.Characters.Where(c => !c.IsActive).Count(), 1);
        }

        [Test]
        public void remove_an_inactive_character_from_an_account()
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            newAccount.AddNewCharacter(Guid.NewGuid(), "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Warrior);
            var idCharacter = newAccount.Characters[0].Id;
            newAccount.RemoveCharacter(idCharacter);
            // Act
            var result = newAccount.RemoveCharacter(idCharacter);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Where(c => c.IsActive).Count(), 0);
            Assert.AreEqual(newAccount.Characters.Where(c => !c.IsActive).Count(), 1);
        }

        [Test]
        public void retrieve_a_inactive_character_from_an_account()
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            newAccount.AddNewCharacter(Guid.NewGuid(), "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Warrior);
            var idCharacter = newAccount.Characters[0].Id;
            newAccount.RemoveCharacter(idCharacter);

            // Act
            var result = newAccount.RetrieveCharacter(idCharacter);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Where(c => c.IsActive).Count(), 1);
            Assert.AreEqual(newAccount.Characters.Where(c => !c.IsActive).Count(), 0);
        }

        [Test]
        public void retrieve_a_active_character_from_an_account()
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            newAccount.AddNewCharacter(Guid.NewGuid(), "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Warrior);
            var idCharacter = newAccount.Characters[0].Id;

            // Act
            var result = newAccount.RetrieveCharacter(idCharacter);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Where(c => c.IsActive).Count(), 1);
            Assert.AreEqual(newAccount.Characters.Where(c => !c.IsActive).Count(), 0);
        }

        [Test]
        public void remove_an_character_from_an_empty_account()
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            var idCharacter = Guid.NewGuid();
            // Act
            var result = newAccount.RemoveCharacter(idCharacter);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Where(c => c.IsActive).Count(), 0);
            Assert.AreEqual(newAccount.Characters.Where(c => !c.IsActive).Count(), 0);
        }

        [Test]
        public void create_a_valid_character_on_an_empty_account()
        {
            // Arrange
            var wowAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() { } };

            // Act
            var result = wowAccount.AddNewCharacter(Guid.NewGuid(), "Cairne Bloodhoof", 100, RaceFactory.Tauren, FactionFactory.Horde, ClassFactory.Druid);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(wowAccount.Characters.Count(), 1);
        }


        static object[] Orc_Tauren_and_Blood_Elves_belong_to_the_Horde_Cases =
        {
            new object[] { "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Warrior },
            new object[] { "Lor'themar Theron", 100, RaceFactory.BloodElf, FactionFactory.Horde, ClassFactory.Mage },
            new object[] { "Cairne Bloodhoof", 100, RaceFactory.Tauren, FactionFactory.Horde, ClassFactory.Druid },
        };
        [Test, TestCaseSource("Orc_Tauren_and_Blood_Elves_belong_to_the_Horde_Cases")]
        public void Orc_Tauren_and_Blood_Elves_belong_to_the_Horde(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Count(), 1);
        }


        static object[] Orc_Tauren_and_Blood_Elves_do_not_belong_to_the_Alliance_Cases =
        {
            new object[] { "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Alliance, ClassFactory.Warrior },
            new object[] { "Lor'themar Theron", 100, RaceFactory.BloodElf, FactionFactory.Alliance, ClassFactory.Mage },
            new object[] { "Cairne Bloodhoof", 100, RaceFactory.Tauren, FactionFactory.Alliance, ClassFactory.Druid },
        };
        [Test, TestCaseSource("Orc_Tauren_and_Blood_Elves_do_not_belong_to_the_Alliance_Cases")]
        public void Orc_Tauren_and_Blood_Elves_do_not_belong_to_the_Alliance(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Count(), 0);
        }

        static object[] Human_Gnome_and_Worgen_belong_to_the_Alliance_Cases =
        {
            new object[] { "Varian Wrynn", 100, RaceFactory.Human, FactionFactory.Alliance, ClassFactory.Warrior },
            new object[] { "Gelbin Mekkatorque", 100, RaceFactory.Gnome, FactionFactory.Alliance, ClassFactory.Warrior },
            new object[] { "Genn Greymane", 100, RaceFactory.Worgen, FactionFactory.Alliance, ClassFactory.Druid },
        };
        [Test, TestCaseSource("Human_Gnome_and_Worgen_belong_to_the_Alliance_Cases")]
        public void Human_Gnome_and_Worgen_belong_to_the_Alliance(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Count(), 1);
        }

        static object[] Human_Gnome_and_Worgen_do_not_belong_to_the_Horde_Cases =
        {
            new object[] { "Varian Wrynn", 100, RaceFactory.Human, FactionFactory.Horde, ClassFactory.Warrior },
            new object[] { "Gelbin Mekkatorque", 100, RaceFactory.Gnome, FactionFactory.Horde, ClassFactory.Warrior },
            new object[] { "Genn Greymane", 100, RaceFactory.Worgen, FactionFactory.Horde, ClassFactory.Druid },
        };
        [Test, TestCaseSource("Human_Gnome_and_Worgen_do_not_belong_to_the_Horde_Cases")]
        public void Human_Gnome_and_Worgen_do_not_belong_to_the_Horde(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Count(), 0);
        }

        static object[] Worgen_and_Tauren_can_be_druid_Cases =
        {
            new object[] { "Cairne Bloodhoof", 100, RaceFactory.Tauren, FactionFactory.Horde, ClassFactory.Druid },
            new object[] { "Genn Greymane", 100, RaceFactory.Worgen, FactionFactory.Alliance, ClassFactory.Druid }
        };
        [Test, TestCaseSource("Worgen_and_Tauren_can_be_druid_Cases")]
        public void Worgen_and_Tauren_can_be_druid(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Count(), 1);
        }

        static object[] Human_Gnome_Orc_and_BloodElves_cannot_be_druid_Cases =
        {
            new object[] { "Varian Wrynn", 100, RaceFactory.Human, FactionFactory.Alliance, ClassFactory.Druid },
            new object[] { "Gelbin Mekkatorque", 100, RaceFactory.Gnome, FactionFactory.Alliance, ClassFactory.Druid },
            new object[] { "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Druid },
            new object[] { "Lor'themar Theron", 100, RaceFactory.BloodElf, FactionFactory.Horde, ClassFactory.Druid }
        };
        [Test, TestCaseSource("Human_Gnome_Orc_and_BloodElves_cannot_be_druid_Cases")]
        public void Human_Gnome_Orc_and_BloodElves_cannot_be_druid(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Count(), 0);
        }

        static object[] BloodElves_cannot_be_warrior_Cases =
        {
            new object[] { "Lor'themar Theron", 100, RaceFactory.BloodElf, FactionFactory.Horde, ClassFactory.Warrior }
        };
        [Test, TestCaseSource("BloodElves_cannot_be_warrior_Cases")]
        public void BloodElves_cannot_be_warrior(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Count(), 0);
        }


        static object[] Except_BloodElves_all_can_be_warrior_Cases =
        {
            new object[] { "Varian Wrynn", 100, RaceFactory.Human, FactionFactory.Alliance, ClassFactory.Warrior},
            new object[] { "Gelbin Mekkatorque", 100, RaceFactory.Gnome, FactionFactory.Alliance, ClassFactory.Warrior},
            new object[] { "Orgrim Doomhammer", 100, RaceFactory.Orc, FactionFactory.Horde, ClassFactory.Warrior},
            new object[] { "Cairne Bloodhoof", 100, RaceFactory.Tauren, FactionFactory.Horde, ClassFactory.Warrior},
            new object[] { "Genn Greymane", 100, RaceFactory.Worgen, FactionFactory.Alliance, ClassFactory.Warrior}
        };
        [Test, TestCaseSource("Except_BloodElves_all_can_be_warrior_Cases")]
        public void Except_BloodElves_all_can_be_warrior(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Count(), 1);
        }

        static object[] when_account_has_not_one_lvl_55__then_it_cannot_create_death_knight_Cases =
        {
            new object[] { "Genn Greymane", 100, RaceFactory.Worgen, FactionFactory.Alliance, ClassFactory.DeathKnight }
        };
        [Test, TestCaseSource("when_account_has_not_one_lvl_55__then_it_cannot_create_death_knight_Cases")]
        public void when_account_has_not_one_lvl_55__then_it_cannot_create_death_knight(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(newAccount.Characters.Count(), 0);
        }

        static object[] when_account_has_one_lvl_55__then_it_can_create_death_knight_Cases =
        {
            new object[] { "Genn Greymane", 100, RaceFactory.Worgen, FactionFactory.Alliance, ClassFactory.DeathKnight }
        };
        [Test, TestCaseSource("when_account_has_one_lvl_55__then_it_can_create_death_knight_Cases")]
        public void when_account_has_one_lvl_55__then_it_can_create_death_knight(string name, int level, Race race, Faction faction, Class @class)
        {
            //arrange 
            var newAccount = new Account() { Id = Guid.NewGuid(), Name = "wow", Password = "wow", Characters = new List<Character>() };
            newAccount.AddNewCharacter(Guid.NewGuid(), "Cairne Bloodhoof", 100, RaceFactory.Tauren, FactionFactory.Horde, ClassFactory.Druid);

            // Act
            var result = newAccount.AddNewCharacter(Guid.NewGuid(),name , level, race, faction, @class);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newAccount.Characters.Count(), 2);
        }


    }
}