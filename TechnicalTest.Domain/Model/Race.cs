using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Model
{
    public class Race
    {
        private static string _orc = "Orc";
        private static string _tauren = "Tauren";
        private static string _bloodelf = "BloodElf";
        private static string _human = "Human";
        private static string _gnome = "Gnome";
        private static string _worgen = "Worgen";

        private static Race _Orc;
        private static Race _Tauren;
        private static Race _BloodElf;
        private static Race _Human;
        private static Race _Gnome;
        private static Race _Worgen;

        public static Race Orc
        {
            get
            {
                if (_Orc == null) _Orc = new Race(_orc);
                return _Orc;
            }
        }
        public static Race Tauren
        {
            get
            {
                if (_Tauren == null) _Tauren = new Race(_tauren);
                return _Tauren;
            }
        }
        public static Race BloodElf
        {
            get
            {
                if (_BloodElf == null) _BloodElf = new Race(_bloodelf);
                return _BloodElf;
            }
        }
        public static Race Human
        {
            get
            {
                if (_Human == null) _Human = new Race(_human);
                return _Human;
            }
        }
        public static Race Gnome
        {
            get
            {
                if (_Gnome == null) _Gnome = new Race(_gnome);
                return _Gnome;
            }
        }
        public static Race Worgen
        {
            get
            {
                if (_Worgen == null) _Worgen = new Race(_worgen);
                return _Worgen;
            }
        }

        
        private string _value;
       
        public Race(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("value cannot be null or empty");

            var dic = new List<string>()
            {
                _orc,
                _tauren, 
                _bloodelf, 
                _human, 
                _gnome,
                _worgen
            };
            if (!dic.Contains(value)) throw new Exception("value out of scope");

            _value = value;
        }

        public static bool TryParse(string value, out Race race)
        {
            var dic = new Dictionary<string, Race>()
            {
                { _orc, Orc },
                { _tauren, Tauren },
                { _bloodelf, BloodElf },
                { _human, Human },
                { _gnome,Gnome },
                { _worgen,Worgen }
            };
            return dic.TryGetValue(value, out race);
        }

        public override bool Equals(object obj)
        {
            var s = obj as string;
            if (s != null) return Equals(s);

            var f = obj as Race;
            if (f != null) return Equals(f);

            return false;
        }

        public bool Equals(string other)
        {
            return _value == other;
        }

        public bool Equals(Race other)
        {
            return _value == other._value;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + _value.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
