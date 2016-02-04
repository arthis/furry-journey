using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{

    /// <summary>
    /// value object immutable describing the class of a character
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Class : ValueObject<string>
    {
        [JsonConstructor]
        public Class(string value) : base(value)
        {

        }
    }

    /// <summary>
    /// factory responsible for creating flyweight class value objects and caching them
    /// </summary>
    public class ClassFactory
    {
        private static string _warrior = "Warrior";
        private static string _druid = "Druid";
        private static string _deathKnight = "DeathKnight";
        private static string _mage = "Mage";

        private static Class _Warrior;
        private static Class _Druid;
        private static Class _DeathKnight;
        private static Class _Mage;
        private static Dictionary<string, Class> _dic;

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
        public static Dictionary<string, Class> Dic
        {
            get
            {
                if (_dic == null)
                    _dic = new Dictionary<string, Class>()
                {
                    { _warrior, Warrior },
                    { _druid, Druid },
                    { _deathKnight, DeathKnight },
                    { _mage, Mage }
                };
                return _dic;
            }
        }

        public static bool TryParse(string value, out Class @class)
        {
            var isSuccess = Dic.TryGetValue(value, out @class);
            return isSuccess;
        }
        
    }
}
