using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Services
{
    public interface IServiceUser
    {
        bool authenticate(string username, string password);
        User getCurrentUser();
        
    }
}