using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechnicalTest.Domain.Services;
using TechnicalTest.Mvc.ActionFilters;
using TechnicalTest.Mvc.Models.Commands;
using TechnicalTest.Mvc.Models.Read;

namespace TechnicalTest.Mvc.Controllers
{
    [BasicAuthentication]
    public class CharacterController : Controller
    {
        IServiceAccount _serviceAccount;

        public CharacterController(IServiceAccount serviceAccount)
        {
            if (serviceAccount == null) throw new Exception("serviceAccount cannot be null");
            _serviceAccount = serviceAccount;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IEnumerable<Character>> GetAsync()
        {
            var result = await _serviceAccount.GetCharactersAsync();
            return result.Select(x =>
                new Character()
                {
                    Name = x.Name,
                    Level = x.Level,
                    Race = (int)x.Race,
                    Faction = (int)x.Faction,
                    Class = (int)x.Class,
                }
            );
        }

        public async Task<TechnicalTest.Domain.Model.ValidationResult> CreateCharacterAsync(CreateCharacter cmd)
        {
            //todo validate that fields are not empty

            var result = await _serviceAccount.CreateCharacterAsync(cmd.Name,cmd.Level,cmd.Race,cmd.Faction,cmd.Class);
            return result;
        }
    }
}