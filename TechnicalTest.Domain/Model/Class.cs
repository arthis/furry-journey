using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Model
{


    public class Class
    {
        private static string _warrior = "Warrior";
        private static string _druid = "Druid";
        private static string _deathKnight = "DeathKnight";
        private static string _mage = "Mage";


        private static Class _Warrior;
        private static Class _Druid;
        private static Class _DeathKnight;
        private static Class _Mage;

        public static Class Warrior
        {
            get
            {
                if (_Warrior == null) _Warrior = new Class(_warrior);
                return _Warrior;
            }
        }
        public static Class Druid
        {
            get
            {
                if (_Druid == null) _Druid = new Class(_druid);
                return _Druid;
            }
        }
        public static Class DeathKnight
        {
            get
            {
                if (_DeathKnight == null) _DeathKnight = new Class(_deathKnight);
                return _DeathKnight;
            }
        }
        public static Class Mage
        {
            get
            {
                if (_Mage == null) _Mage = new Class(_mage);
                return _Mage;
            }
        }

        private string _value;

        public Class(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("value cannot be null or empty");

            var list = new List<string>()
            {
                _warrior,
                _druid,
                _deathKnight,
                _mage
            };

            if (!list.Contains(value)) throw new Exception("value out of scope");

            _value = value;
        }

        public static bool TryParse(string value, out Class Class)
        {
            var dic = new Dictionary<string, Class>()
            {
                { _warrior, Warrior },
                { _druid, Druid },
                { _deathKnight, DeathKnight },
                { _mage, Mage }
            };
            var isSuccess = dic.TryGetValue(value, out Class);
            return isSuccess;
        }

        public override bool Equals(object obj)
        {
            var s = obj as string;
            if (s != null) return Equals(s);

            var f = obj as Class;
            if (f != null) return Equals(f);

            return false;
        }

        public bool Equals(string other)
        {
            return _value == other;
        }

        public bool Equals(Class other)
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
