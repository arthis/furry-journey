﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Services
{
    public interface IServiceAccount
    {
        bool authenticate(string username, string password);
        Account getCurrentAccount();
        Task<IEnumerable<Character>> GetCharactersAsync();
        Task<bool> CreateCharacterAsync(string name, int level, Race race, Faction faction, Class @class);
        Task<bool> RemoveCharacterAsync(Guid idCharacter);
        Task<bool> RetrieveCharacterAsync(Guid idCharacter);
        
    }
}