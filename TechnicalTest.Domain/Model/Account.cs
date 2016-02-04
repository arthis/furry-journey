using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    /// <summary>
    /// aggregate root describing an account
    /// </summary>    
    public class Account
    {
        public Guid Id { get; set; }
        
        public List<Character> Characters { get; set;}

        public Account()
        {
            Characters = new List<Character>();
        }

        /// <summary>
        /// add a new character to this account, if the newly created character abides to some rules
        /// </summary>
        /// <param name="name">name of the character</param>
        /// <param name="level">level of the character</param>
        /// <param name="race">race of the character</param>
        /// <param name="faction">faction of the character</param>
        /// <param name="class">class of the character</param>
        /// <returns>return true if the creation wnet smoothly</returns>
        public bool AddNewCharacter(Guid id, string name, int level, Race race, Faction faction, Class @class)
        {
            var newCharacter = new Character()
            {
                Id = id,
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

        /// <summary>
        /// disable an existing active character of an account
        /// </summary>
        /// <param name="idCharacter">id of the character to remove</param>
        /// <returns>return true if an active character was found</returns>
        public bool RemoveCharacter(Guid idCharacter)
        {
            var character = Characters.FirstOrDefault(c => c.Id == idCharacter && c.IsActive);
            if (character == null) return false;

            character.Disable();
            return true;
        }

        /// <summary>
        /// enable an existing inactive character of an account
        /// </summary>
        /// <param name="idCharacter">id of the character to retrieve</param>
        /// <returns>return true if an inactive character was found</returns>
        public bool RetrieveCharacter(Guid idCharacter)
        {
            var character = Characters.FirstOrDefault(c => c.Id == idCharacter && !c.IsActive);

            if (character == null) return false;

            character.Enable();
            return true;
        }

        public override bool Equals(object obj)
        {
            var newAccount = obj as Account;

            if (newAccount != null) return Equals(newAccount);

            return false;
        }

        public bool Equals(Account u)
        {
            return u.Id== this.Id;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Id.GetHashCode();
            return hash;
        }

    }
}
