using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.Domain.Model
{

    public class ValidationResult
    {
        public bool IsOk { get; set; }
        public List<string> Messages { get; set; }

        
    }
}
