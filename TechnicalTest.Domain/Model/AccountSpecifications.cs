using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    public class Has_Level55_Character : ISpecification<Account>
    {

        public bool IsSatisfiedBy(Account account)
        {
            return account.Characters.Any(c=> c.Level>=55);
        }
    }

    public class Is_Character_DeathKnight : ISpecification<Account>
    {
        Character _character;

        public Is_Character_DeathKnight(Character character)
        {
            if (character== null) throw new ArgumentNullException("character cannot be null");

            _character = character;
        }
        public bool IsSatisfiedBy(Account account)
        {
            return _character.Class == ClassFactory.DeathKnight;
        }
    }

    public static class AccountSpecifications
    {
        private class Is_Always_True : ISpecification<Account>
        {
            public bool IsSatisfiedBy(Account account)
            {
                return true;
            }
        }

        private static ISpecification<Account> @Spec { get { return new Is_Always_True(); } }

        public static ISpecification<Account> DeathKnightSpecifications(Character character)
        {
            var is_Character_DeathKnight = new Is_Character_DeathKnight(character);
            var has_Level55_Character = new Has_Level55_Character();

            return @Spec.AndIf(is_Character_DeathKnight).Then(has_Level55_Character);
        }


    }





}
