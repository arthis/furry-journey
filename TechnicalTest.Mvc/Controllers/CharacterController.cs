using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechnicalTest.Domain.Services;
using TechnicalTest.Mvc.ActionFilters;
using TechnicalTest.Mvc.Models.Read;
using TechnicalTest.Mvc.Models.Commands;


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
                    Race = x.Race.ToString(),
                    Faction = x.Faction.ToString(),
                    Class = x.Class.ToString(),
                }
            );
        }

        public async Task<TechnicalTest.Domain.Model.ValidationResult> CreateCharacterAsync(CreateCharacter cmd)
        {
            //todo validate that fields are not empty
            Domain.Model.Faction faction = null;
            var isSuccessFaction = Domain.Model.Faction.TryParse(cmd.Faction, out faction);
            Domain.Model.Race race = null;
            var isSuccessRace = Domain.Model.Race.TryParse(cmd.Race, out race);
            Domain.Model.Class @class = null;
            var isSuccessClass = Domain.Model.Class.TryParse(cmd.Class, out @class);

            var result = await _serviceAccount.CreateCharacterAsync(cmd.Name,cmd.Level,race,faction, @class);
            return result;
        }
    }
}