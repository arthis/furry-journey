using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public class MemoryRepoAccount : IRepoAccount
    {
        Dictionary<Guid, Account> _dataSource;
        readonly object _loker = new Object();

        public MemoryRepoAccount(Dictionary<Guid,Account> dataSource)
        {
            if (dataSource == null) throw new ArgumentNullException("dataSource cannot be null");

            _dataSource = dataSource;
        }

        public Task<Account> GetByIdAsync(Guid id)
        {
            lock (_loker)
            {
                if (!_dataSource.ContainsKey(id))
                {
                    _dataSource.Add(id, new Account() { Id = id});
                }
            }
            
            return Task.FromResult(_dataSource[id]);
        }

        public async Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId)
        {
            var account = await GetByIdAsync(accountId);

            return account.Characters.AsEnumerable();
        }

        public async Task<bool> SaveAsync(Account account)
        {
            //fictious delay to get the answer
            await Task.Delay(100);
            return true;
        }
    }
}
