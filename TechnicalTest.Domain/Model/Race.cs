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
    /// value object immutable describing the race of a character
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Race : ValueObject<string>
    {
        [JsonConstructor]
        public Race(string value) : base(value)
        {

        }
    }
    
    /// <summary>
    /// factory responsible for creating flyweight Race value objects and caching them
    /// </summary>
    public class RaceFactory
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
        private static Dictionary<string, Race> _dic;

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
        public static Dictionary<string, Race> Dic
        {
            get
            {
                if (_dic == null)
                    _dic = new Dictionary<string, Race> ()
                {
                    { _orc, Orc },
                    { _tauren, Tauren },
                    { _bloodelf, BloodElf },
                    { _human, Human },
                    { _gnome,Gnome },
                    { _worgen,Worgen }
                };
                return _dic;
            }
        }
        
        public static bool TryParse(string value, out Race race)
        {
            var isSuccess = Dic.TryGetValue(value, out race);
            return isSuccess;
        }

    }
}
