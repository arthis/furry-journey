using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalTest.Mvc.Models.Read
{
    public class CharacterListView
    {
        public IEnumerable<Character> Characters { get; set; }
    }
}