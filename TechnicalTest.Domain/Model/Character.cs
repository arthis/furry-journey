using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public Race Race { get; set; }
        public Faction Faction { get; set; }
        public Class Class { get; set; }
        public bool IsActive { get; set; }

        
    }
}
