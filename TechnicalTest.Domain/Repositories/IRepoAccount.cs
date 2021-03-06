﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public interface IRepoAccount
    {
        Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId);
        Task<Account> GetByIdAsync(Guid id);
        Task<bool> SaveAsync(Account account);
    }
}
