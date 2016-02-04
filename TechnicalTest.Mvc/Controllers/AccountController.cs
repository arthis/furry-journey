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
    public class AccountController : Controller
    {
        IServiceUser _serviceUser;
        IServiceAccount _serviceAccount;

        public AccountController(IServiceUser serviceUser,  IServiceAccount serviceAccount)
        {
            if (serviceAccount == null) throw new ArgumentNullException("serviceAccount cannot be null");
            if (serviceUser == null) throw new ArgumentNullException("serviceUser cannot be null");

            _serviceUser = serviceUser;
            _serviceAccount = serviceAccount;
        }

        public async Task<ActionResult> Index()
        {
            var characters = await GetAsync();
            var viewModel = new CharacterListView() { Characters = characters };
            return View(viewModel);
        }

        public async Task<IEnumerable<Character>> GetAsync()
        {
            var currentUser = _serviceUser.getCurrentUser();
            var result = await _serviceAccount.GetCharactersAsync(currentUser.Id);
            return result.Select(x =>
                new Character()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Level = x.Level,
                    Race = x.Race.ToString(),
                    Faction = x.Faction.ToString(),
                    Class = x.Class.ToString(),
                    IsActive = x.IsActive
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> AddNewCharacterAsync(CreateCharacterCmd cmd)
        {
            var cmdValidation = new CommandValidation();

            Domain.Model.Faction faction = null;
            Domain.Model.Race race = null;
            Domain.Model.Class @class = null;

            cmdValidation.validate(() => cmd.Id == Guid.Empty, "id cannot be empty");
            cmdValidation.validate(() => string.IsNullOrEmpty(cmd.Name), "name cannot be empty");
            cmdValidation.validate(() => !Domain.Model.ClassFactory.TryParse(cmd.Class, out @class), "class is not known");
            cmdValidation.validate(() => !Domain.Model.FactionFactory.TryParse(cmd.Faction, out faction), "faction is not known");
            cmdValidation.validate(() => !Domain.Model.RaceFactory.TryParse(cmd.Race, out race), "race is not known");
            //cmdValidation.validate(() => cmd.Level<1 || cmd.Level>100, "level is not available"); not in the rules.. :)

            if (!cmdValidation.IsValid())
                //todo give back some 4/5xx love?
                return Json(cmdValidation.ToResponse());
            else
            {
                var currentUser = _serviceUser.getCurrentUser();
                var result = await _serviceAccount.CreateCharacterAsync(currentUser.Id, cmd.Id, cmd.Name, cmd.Level, race, faction, @class);
                var executionResult = new ExecutionValidation(result );
                //todo give back some 4/5xx love if it failed?
                return Json(executionResult.ToResponse());
            }
                
        }

        [HttpPost]
        public async Task<ActionResult> RemoveCharacterAsync(RemoveCharacterCmd cmd)
        {
            var cmdValidation = new CommandValidation();

            cmdValidation.validate(() => cmd.Id==Guid.Empty, "idCharacter cannot be empty");

            if (!cmdValidation.IsValid())
                //todo give back some 4/5xx love?
                return Json( cmdValidation.ToResponse());
            else
            {
                var currentUser = _serviceUser.getCurrentUser();
                var result = await _serviceAccount.RemoveCharacterAsync(currentUser.Id, cmd.Id);

                var executionResult = new ExecutionValidation(result);
                //todo give back some 4/5xx love if it failed?
                return Json(executionResult.ToResponse());
            }
        }

        [HttpPost]
        public async Task<ActionResult> RetrieveCharacterAsync(RetrieveCharacterCmd cmd)
        {
            var cmdValidation = new CommandValidation();

            cmdValidation.validate(() => cmd.Id == Guid.Empty, "idCharacter cannot be empty");

            if (!cmdValidation.IsValid())
                //todo give back some 4/5xx love?
                return Json( cmdValidation.ToResponse());
            else
            {
                var currentUser = _serviceUser.getCurrentUser();
                var result = await _serviceAccount.RetrieveCharacterAsync(currentUser.Id, cmd.Id);

                var executionResult = new ExecutionValidation(result);
                //todo give back some 4/5xx love if it failed?
                return Json( executionResult.ToResponse());
            }
        }
    }
}