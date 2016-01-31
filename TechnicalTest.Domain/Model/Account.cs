﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    

    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public List<Character> Characters { get; set; }

        public Task<ValidationResult> CreateCharacterAsync(string name, int level, Race race, Faction faction, Class @class)
        {
            //if (race == "Human" && faction == "Hord")
            //    return Task.FromResult(new ValidationResult()
            //    {
            //        IsOk = false,
            //        Messages = new List<string>() { "Human do not belong to hord" }
            //    });

            var newCharacter = new Character()
            {
                Name = name,
                Level = level,
                Race = race,
                Faction = faction,
                Class = @class
            };
            var validationResult = new ValidationResult() { IsOk = true };

            AccountSpecifications.Factions
            .And(AccountSpecifications.DruidSpecifications)
            .And(AccountSpecifications.BloodElfSpecifications)
            .IsSatisfiedBy(newCharacter, validationResult);

            return Task.FromResult(validationResult);
        }

        public override bool Equals(object obj)
        {
            var newUser = obj as Account;

            if (newUser != null) return Equals(newUser);

            return false;
        }

        public bool Equals(Account u)
        {
            return Name == u.Name && Password == u.Password;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Id.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + Password.GetHashCode();
            return hash;
        }
    }
}