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

        private async Task<bool> saveToFileAsync(Guid id, string data)
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
        public FileSystemRepoAccount()
        {
        }


        public async Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId)
        {
            var a = await GetByIdAsync(accountId);
            return a.Characters;
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            var dataDirectory = getdbPath();
            var dataFileFullPath = string.Format(@"{0}\{1}", dataDirectory, getDataFileName("account", id));

            if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);

            if (!File.Exists(dataFileFullPath))
                return new Account() { Id = id }; 
            
            
            var data = await ReadAllFileAsync(dataFileFullPath);
            return JsonConvert.DeserializeObject<Account>(data);

        }

        public Task<bool> SaveAsync(Account account)
        {
            var data = JsonConvert.SerializeObject(account);

            return saveToFileAsync(account.Id, data);
        }
    }
}
