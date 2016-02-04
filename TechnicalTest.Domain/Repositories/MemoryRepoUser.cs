using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public class MemoryRepoUser : IRepoUser
    {
        Dictionary<string, User> _dataSource;

        public MemoryRepoUser(Dictionary<string,User> dataSource)
        {
            if (dataSource == null) throw new ArgumentNullException("dataSource cannot be null");

            _dataSource = dataSource;
        }

        public User Get(string username)
        {
            return _dataSource[username];
        }

    }
}
