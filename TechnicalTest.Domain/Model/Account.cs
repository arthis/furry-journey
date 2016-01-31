using System;
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

        public bool CreateCharacter(string name, int level, Race race, Faction faction, Class @class)
        {
            var newCharacter = new Character()
            {
                Name = name,
                Level = level,
                Race = race,
                Faction = faction,
                Class = @class
            };

            if ( CharacterSpecifications.Factions
                            .And(CharacterSpecifications.DruidSpecifications)
                            .And(CharacterSpecifications.BloodElfSpecifications)
                            .IsSatisfiedBy(newCharacter)
                            && AccountSpecifications.DeathKnightSpecifications(newCharacter)
                            .IsSatisfiedBy(this))
            {
                Characters.Add(newCharacter);
                return true;
            }
            else
                return false;
        }

        public bool RemoveCharacter(Guid idCharacter)
        {
            var character = Characters.FirstOrDefault(c => c.Id == idCharacter && c.IsActive);
            if (character == null) return false;

            character.Disable();
            return true;
        }

        public bool RetrieveCharacter(Guid idCharacter)
        {
            var character = Characters.FirstOrDefault(c => c.Id == idCharacter && !c.IsActive);

            if (character == null) return false;

            character.Enable();
            return true;
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
