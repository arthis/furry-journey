using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public class SimpleRepoAccount : IRepoAccount
    {
        Dictionary<string, Account> _dataSource;

        public SimpleRepoAccount(Dictionary<string,Account> dataSource)
        {
            if (dataSource == null) throw new Exception("dataSource cannot be null");

            _dataSource = dataSource;
        }

        public Account Get(string username)
        {
            return _dataSource[username];
        }

        public Account GetById(Guid id)
        {
            //todo ugly stuff
            Func< KeyValuePair < string,Account >,bool> predicat =  x => x.Value.Id == id;
            return _dataSource.Any(predicat) ? _dataSource.Single(predicat).Value: null;
        }

        public Task<IEnumerable<Character>> GetCharactersAsync(Account account)
        {
            return Task.FromResult( _dataSource[account.Name].Characters.AsEnumerable());
        }

        public async Task<bool> SaveAsync(Account account)
        {
            await Task.Delay(100);
            return await Task.FromResult(true);
        }
    }
}
