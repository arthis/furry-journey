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
    /// value object immutable describing the faction of a character
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Faction : ValueObject<string>
    {
        [JsonConstructor]
        public Faction(string value) : base(value)
        {

        }
    }

    /// <summary>
    /// factory responsible for creating flyweight faction value objects and caching them
    /// </summary>
    public class FactionFactory
    {
        private static string _alliance = "Alliance";
        private static string _horde = "Horde";

        private static Faction _Horde;
        private static Faction _Alliance;
        private static Dictionary<string, Faction> _dic;

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
        public static Dictionary<string, Faction> Dic
        {
            get
            {
                if (_dic == null)
                    _dic = new Dictionary<string, Faction>()
                {
                    { _horde, Horde },
                    { _alliance, Alliance }
                };
                return _dic;
            }
        }
        
        public static bool TryParse(string value, out Faction faction)
        {
            var isSuccess = Dic.TryGetValue(value, out faction);
            return isSuccess;
        }

    }
}
