using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    
    public class Is_Horde : ISpecification<Character>
    {
        public bool IsSatisfiedBy(Character character )
        {
            return character.Faction == FactionFactory.Horde;
        }

    }

    public class Is_Alliance : ISpecification<Character>
    {
        public bool IsSatisfiedBy(Character character )
        {
            return character.Faction == FactionFactory.Alliance;
        }
    }

    public class Is_Orc_Tauren_or_Elf : ISpecification<Character>
    {

        public bool IsSatisfiedBy(Character character )
        {
            return 
                character.Race == RaceFactory.Orc
                || character.Race == RaceFactory.Tauren
                || character.Race == RaceFactory.BloodElf;
        }
    }

    public class Is_Human_Gnome_or_Worgen : ISpecification<Character>
    {

        public bool IsSatisfiedBy(Character character )
        {
            return character.Race == RaceFactory.Human
                || character.Race == RaceFactory.Gnome
                || character.Race == RaceFactory.Worgen;
        }
    }

    public class Is_Tauren_or_Worgen : ISpecification<Character>
    {

        public bool IsSatisfiedBy(Character character )
        {
            return character.Race == RaceFactory.Tauren
                   || character.Race == RaceFactory.Worgen;
        }
    }

    public class Is_Druid : ISpecification<Character>
    {

        public bool IsSatisfiedBy(Character character )
        {
            return character.Class == ClassFactory.Druid;
        }
    }

    public class Is_BloodElf : ISpecification<Character>
    {

        public bool IsSatisfiedBy(Character character )
        {
            return character.Race == RaceFactory.BloodElf;
        }
    }

    public class Is_Warrior : ISpecification<Character>
    {

        public bool IsSatisfiedBy(Character character )
        {
            return character.Class == ClassFactory.Warrior;
        }
    }


    public static class CharacterSpecifications
    {
        private class Is_Always_True : ISpecification<Character>
        {
            public bool IsSatisfiedBy(Character character)
            {
                return true;
            }
        }

        private static ISpecification<Character> @Spec { get { return new Is_Always_True(); } }

        public static ISpecification<Character> Factions {
            get {
                var is_Horde = new Is_Horde();
                var is_Alliance = new Is_Alliance();
                var is_Orc_Tauren_or_Elf = new Is_Orc_Tauren_or_Elf();
                var is_Human_Gnome_or_Worgen = new Is_Human_Gnome_or_Worgen();

                return @Spec.AndIf(is_Alliance).Then(is_Human_Gnome_or_Worgen)
                       .AndIf(is_Horde).Then(is_Orc_Tauren_or_Elf);
            }
        }

        public static ISpecification<Character> DruidSpecifications
        {
            get
            {
                var is_Druid = new Is_Druid();
                var is_Tauren_or_Worgen = new Is_Tauren_or_Worgen();

                return @Spec.AndIf(is_Druid).Then(is_Tauren_or_Worgen);
            }
        }

        public static ISpecification<Character> BloodElfSpecifications
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
