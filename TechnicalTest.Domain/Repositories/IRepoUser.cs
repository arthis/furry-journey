using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public interface IRepoUser
    {
        User Get(string username);
    }
}
