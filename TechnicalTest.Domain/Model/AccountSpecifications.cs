using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    public class Is_Always_True : ISpecification<Character, ValidationResult>
    {
        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            return true;
        }
    }

    public class Is_Horde : ISpecification<Character, ValidationResult>
    {
        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            return character.Faction == Faction.Horde;
        }

    }

    public class Is_Alliance : ISpecification<Character, ValidationResult>
    {
        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            return character.Faction == Faction.Alliance;
        }
    }

    public class Is_Orc_Tauren_or_Elf : ISpecification<Character, ValidationResult>
    {

        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            var result = 
                character.Race == Race.Orc
                || character.Race == Race.Tauren
                || character.Race == Race.BloodElf;

            validationResult.IsOk |= result;

            return result;
        }
    }

    public class Is_Human_Gnome_or_Worgen : ISpecification<Character, ValidationResult>
    {

        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            var result = character.Race == Race.Human
                || character.Race == Race.Gnome
                || character.Race == Race.Worgen;

            validationResult.IsOk |= result;

            return result;
        }
    }

    public class Is_Tauren_or_Worgen : ISpecification<Character, ValidationResult>
    {

        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            var result =
                character.Race == Race.Tauren
                || character.Race == Race.Worgen;
            validationResult.IsOk |= result;
            return result;
        }
    }

    public class Is_Druid : ISpecification<Character, ValidationResult>
    {

        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            var result = character.Class == Class.Druid;

            validationResult.IsOk |= result;
            return result;
        }
    }

    public class Is_BloodElf : ISpecification<Character, ValidationResult>
    {

        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            var result = character.Race == Race.BloodElf;

            validationResult.IsOk |= result;
            return result;
        }
    }

    public class Is_Warrior : ISpecification<Character, ValidationResult>
    {

        public bool IsSatisfiedBy(Character character, ValidationResult validationResult)
        {
            var result = character.Class == Class.Warrior;

            validationResult.IsOk |= result;
            return result;
        }
    }

    public static class AccountSpecifications
    {
        private static ISpecification<Character, ValidationResult> @Spec { get { return new Is_Always_True(); } }

        public static ISpecification<Character, ValidationResult> Factions {
            get {
                var is_Horde = new Is_Horde();
                var is_Alliance = new Is_Alliance();
                var is_Orc_Tauren_or_Elf = new Is_Orc_Tauren_or_Elf();
                var is_Human_Gnome_or_Worgen = new Is_Human_Gnome_or_Worgen();

                return @Spec.AndIf(is_Alliance).Then(is_Human_Gnome_or_Worgen)
                       .AndIf(is_Horde).Then(is_Orc_Tauren_or_Elf);
            }
        }

        public static ISpecification<Character, ValidationResult> DruidSpecifications
        {
            get
            {
                var is_Druid = new Is_Druid();
                var is_Tauren_or_Worgen = new Is_Tauren_or_Worgen();

                return @Spec.AndIf(is_Druid).Then(is_Tauren_or_Worgen);
            }
        }

        public static ISpecification<Character, ValidationResult> BloodElfSpecifications
        {
            get
            {
                var is_BloodElf = new Is_BloodElf();
                var is_warrior = new Is_Warrior();

                return @Spec.AndIf(is_BloodElf).Then(is_warrior.Not());
            }
        }

    }





}
