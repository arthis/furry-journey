using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalTest.Mvc.Models.Commands
{
    public class CreateCharacter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Race { get; set; }
        public int Faction { get; set; }
        public int Class { get; set; }
    }
}