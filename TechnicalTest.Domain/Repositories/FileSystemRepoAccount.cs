using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Model;

namespace TechnicalTest.Domain.Repositories
{
    public class FileSystemRepoAccount : IRepoAccount
    {
        Dictionary<string, Account> _dataSourceUsers;

        private string getdbPath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            return currentDirectory + @"\db";
        }

        private string getDataFileName(string ar, Guid id)
        {
            return string.Format("data-{0}-{1}.json", ar, id.ToString());
        }

        private async Task<string> ReadAllFileAsync(string filename)
        {
            using (var file = File.OpenRead(filename))
            {
                using (var ms = new MemoryStream())
                {
                    byte[] buff = new byte[file.Length];
                    await file.ReadAsync(buff, 0, (int)file.Length);

                    return System.Text.Encoding.UTF8.GetString(buff);
                }
            }
        }

        private async Task<Account> hydrateAggregateAsync(Guid id)
        {
            var dataDirectory = getdbPath();
            var dataFileFullPath = string.Format(@"{0}\{1}", dataDirectory, getDataFileName("account", id));

            if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
            var data = await ReadAllFileAsync(dataFileFullPath);

            return JsonConvert.DeserializeObject<Account>(data);
        }

        private async Task<bool> saveAsync(Guid id, string data)
        {

            var dataDirectory = getdbPath();
            var dataFileFullPath = string.Format(@"{0}\{1}", dataDirectory, getDataFileName("account", id));

            if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
            var streamWriter = File.CreateText(dataFileFullPath);

            await streamWriter.WriteAsync(data);

            streamWriter.Dispose();

            return true;
        }

        //create the repo and populate the datasource of known accounts
        public FileSystemRepoAccount(Dictionary<string, Account> usersKnown)
        {
            if (usersKnown == null) throw new ArgumentNullException("usersKnown cannot be null");
            _dataSourceUsers = usersKnown;

            foreach ( string fileName in Directory.EnumerateFiles(getdbPath()))
            {
                var data = ReadAllFileAsync(fileName).Result;
                var account = JsonConvert.DeserializeObject<Account>(data);
                _dataSourceUsers.Add(account.Name, account);
            };
        }


        public Account Get(string username)
        {
            return _dataSourceUsers[username];
        }

        public async Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId)
        {
            var a = await hydrateAggregateAsync(accountId);
            return a.Characters;
        }

        public Account GetById(Guid id)
        {
            //todo ugly stuff
            Func<KeyValuePair<string, Account>, bool> predicat = x => x.Value.Id == id;
            return _dataSourceUsers.Any(predicat) ? _dataSourceUsers.Single(predicat).Value : null;
        }

        public Task<bool> SaveAsync(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
