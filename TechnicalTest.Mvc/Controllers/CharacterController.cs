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
    public class Response
    {
        public bool IsOk { get; set; }
        public List<string> Messages { get; set; }
    }

    public class CommandValidation
    {
        private bool _IsOk; 
        private List<string> _Messages { get; set; }

        public CommandValidation()
        {
            _IsOk = true;
            _Messages = new List<string>();
        }

        public void validate(Func<bool> predicate, string msg)
        {
            if (predicate())
            {
                _IsOk = false;
                _Messages.Add(msg);
            }
        }

        public bool IsValid()
        {
            return _IsOk;
        }

        public Response ToResponse()
        {
            return new Response()
            {
                IsOk = _IsOk,
                Messages = _Messages
            };
        }
    }

    public class ExecutionValidation
    {
        private bool _IsOk;
        private List<string> _Messages { get; set; }

        public ExecutionValidation(bool isOk)
        {
            _IsOk = isOk;
            if (!_IsOk) _Messages = new List<string>() { "execution went astray!" };
        }

        public bool IsValid()
        {
            return _IsOk;
        }

        public Response ToResponse()
        {
            return new Response()
            {
                IsOk = _IsOk,
                Messages = _Messages
            };
        }
    }



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

        public async Task<Response> CreateCharacterAsync(CreateCharacter cmd)
        {
            //todo validate that fields are not empty
            var cmdValidation = new CommandValidation();

            Domain.Model.Faction faction = null;
            Domain.Model.Race race = null;
            Domain.Model.Class @class = null;

            cmdValidation.validate(() => string.IsNullOrEmpty(cmd.Name), "name cannot be empty");
            cmdValidation.validate(() => !Domain.Model.Class.TryParse(cmd.Class, out @class), "class is not known");
            cmdValidation.validate(() => !Domain.Model.Faction.TryParse(cmd.Faction, out faction), "faction is not known");
            cmdValidation.validate(() => !Domain.Model.Race.TryParse(cmd.Race, out race), "race is not known");
            //cmdValidation.validate(() => cmd.Level<1 || cmd.Level>100, "level is not available"); not in the rules.. :)

            if (!cmdValidation.IsValid())
                return cmdValidation.ToResponse();
            else
            {
                var result = await _serviceAccount.CreateCharacterAsync(cmd.Name, cmd.Level, race, faction, @class);
                var executionResult = new ExecutionValidation(result );
                return executionResult.ToResponse();
            }
                
        }
    }
}