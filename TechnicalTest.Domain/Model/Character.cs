using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Core;

namespace TechnicalTest.Domain.Model
{
    /// <summary>
    /// entity object describing a character
    /// </summary>
    public class Character
    {
        private bool _isActive ;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public Race Race { get; set; }
        public Faction Faction { get; set; }
        public Class Class { get; set; }
        public bool IsActive { get { return _isActive; } }

        public Character()
        {
            _isActive = true;
        }

        public void Disable()
        {
            _isActive = false;
        }

        public void Enable()
        {
            _isActive = true;
        }

    }
}
