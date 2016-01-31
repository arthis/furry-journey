using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Model
{
    public class Faction
    {
        private static string _alliance = "Alliance";
        private static string _horde = "Horde";

        private static Faction _Horde;
        private static Faction _Alliance;
           
        public static Faction Alliance
        {
            get
            {
                if (_Alliance == null) _Alliance = new Faction(_alliance);
                return _Alliance;
            }
        }
        public static Faction Horde
        {
            get
            {
                if (_Horde == null) _Horde = new Faction(_horde);
                return _Horde;
            }
        }
        
        private string _value;

        public Faction(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("value cannot be null or empty");
            
            var list = new List<string>()
            {
                _horde,
                _alliance, 
            };

            if (!list.Contains(value)) throw new Exception("value out of scope");

            _value = value;
        }

        public static bool TryParse(string value, out Faction faction)
        {
            var dic = new Dictionary<string, Faction>()
            {
                { _horde, Horde },
                { _alliance, Alliance }
            };
            var isSuccess = dic.TryGetValue(value, out faction);
            return isSuccess;
        }

        public override bool Equals(object obj)
        {
            var s = obj as string;
            if (s != null) return Equals(s);

            var f = obj as Faction;
            if (f != null) return Equals(f);

            return false;
        }

        public bool Equals(string other)
        {
            return _value == other;
        }

        public bool Equals(Faction other)
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
