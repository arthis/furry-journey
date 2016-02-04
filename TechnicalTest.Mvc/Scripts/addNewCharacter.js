(function ($, core) {
    
        
    this.createForm = function (hasLevel55Character) {
        //todo missing validation here
        var self =  this;

        var idCharacterCreation = "characterCreation";
        var selectorCharacterCreation = "#" + idCharacterCreation;

        var templateAddNewCharacter = doT.template($("#addNewCharacterTemplate").html());

        //act as datasource for this viewmodel
        if (hasLevel55Character === null || hasLevel55Character=== undefined) throw "account is not defined";
        var _hasLevel55Character = hasLevel55Character;
        
        //declaration of model
        //------------------------
        var Character = function (id, name, level, race, faction, characterClass, isActive) {
            this.Id = id;
            this.Name = name;
            this.Level = level;
            this.Race = race;
            this.Faction = faction;
            this.Class = characterClass;
            this.IsActive = isActive;
        };

        //basic rules of the domain
        //---------------------
        var Faction = function () {
            var alliance = ["Human", "Gnome", "Worgen"];
            var horde = ["Orc", "Tauren", "BooldElf"];


            this.getFactionFromRaceChosen = function (race) {
                return alliance.indexOf(race) >= 0 ? "Alliance" : "Horde";
            }

        };

        //responsible to deal with the choices available for the class of character
        var Class = function (isDeathKnightAllowed) {

            var currentAvailableClasses = ["Warrior", "Mage","Druid"];

            if (isDeathKnightAllowed) currentAvailableClasses.push("DeathKnight");

            var apply = function () {
                $('[name=characterClass]').each(function () {
                    if (currentAvailableClasses.indexOf($(this).val()) == -1) {
                        $(this).prop('checked', false);
                        $(this).attr('disabled', true);
                        $(this).addClass("inactive");
                    }
                    else {
                        $(this).removeClass("inactive");
                        $(this).removeAttr('disabled');
                    }
                });
            };


            var Have = function (classCharacter) {
                var classCharacterIndex = currentAvailableClasses.indexOf(classCharacter);
                if (classCharacterIndex == -1) currentAvailableClasses.push(classCharacter);
            };

            var HaveNot = function (classCharacter) {
                var classCharacterIndex = currentAvailableClasses.indexOf(classCharacter);
                if (classCharacterIndex > -1) currentAvailableClasses.splice(classCharacterIndex, 1);
            };

            var onRaceChosen = function (r) {
                
                switch (r) {
                    case "Human":
                    case "Gnome":
                    case "Orc":
                        HaveNot("Druid");
                        Have("Warrior");
                        break;
                    case "BloodElf":
                        HaveNot("Druid");
                        HaveNot("Warrior");
                        break;
                    case "Tauren":
                    case "Worgen":
                        Have("Druid");
                        Have("Warrior");
                        break;
                }

                apply();
            };

            $.Topic('RaceChosen').subscribe(onRaceChosen);

            $('.characterClassInput').click(function (e) {
                $.Topic('ClassChosen').publish($(this).val());
            });

            apply();

        };

        //responsible to deal with the choices available for the rqce of character
        var Race = function () {
            var currentAvailableRaces = ["Human", "Gnome", "Worgen", "Orc", "Tauren", "BloodElf"];

            var apply = function () {
                $('[name=race]').each(function () {
                    if (currentAvailableRaces.indexOf($(this).val()) == -1) {
                        $(this).prop('checked', false);
                        $(this).attr('disabled', true);
                        $(this).addClass("inactive");
                    }
                    else {
                        $(this).removeClass("inactive");
                        $(this).removeAttr('disabled');
                    }   
                });
            };
        

        var Have = function (race) {
            var raceIndex = currentAvailableRaces.indexOf(race);
            if (raceIndex == -1) currentAvailableRaces.push(race);
        };

        var HaveNot = function (race) {
            var raceIndex = currentAvailableRaces.indexOf(race);
            if (raceIndex > -1) currentAvailableRaces.splice(raceIndex, 1);
        };

        var onClassChosen = function (r) {

            switch (r) {
                case "Mage":
                case "DeathKnight":
                    Have("Human");
                    Have("Gnome");
                    Have("Orc");
                    Have("BloodElf");
                    break;
                case "Warrior":
                    Have("Human");
                    Have("Gnome");
                    Have("Orc");
                    HaveNot("BloodElf");
                    break;
                case "Druid":
                    HaveNot("Human");
                    HaveNot("Gnome");
                    HaveNot("Orc");
                    HaveNot("BloodElf");
                    break;
            }

            apply();
        };

        $.Topic('ClassChosen').subscribe(onClassChosen);

        $('.raceInput').click(function (e) {
            $.Topic('RaceChosen').publish($(this).val());
        });

        apply();
    };


        
    //declaration of model
    //------------------------
    var Character = function (id, name, level, race, faction, characterClass, isActive) {
        this.Id = id;
        this.Name = name;
        this.Level = level;
        this.Race = race;
        this.Faction = faction;
        this.Class = characterClass;
        this.IsActive = isActive;
    };

    //declaration of commands
    //------------------------
    var RemoveCharacterCmd = function (id) {
        //todo check id valid here
        this.Id = id;
    };

    var RetrieveCharacterCmd = function (id) {
        //todo check id valid here
        this.Id = id;
    };

    var AddNewCharacterCmd = function (name, level, race, characterClass,faction) {
        //todo check validations here
        if (!name) throw "name must not be null or empty";
        if (!level) throw "level must not be null";
        if (!race) throw "race must not be null";
        if (!characterClass) throw "class must not be null";

        this.Id = core.guid();
        this.Name = name;
        this.Level = level;
        this.Race = race;
        this.Faction = faction.getFactionFromRaceChosen(race);
        this.Class = characterClass;
    };

    // render the validation messages using a basic alert
    var AlertValidation = function () {
        var _messages = [];
        this.push = function (msg) {
            _messages.push(msg);
        }

        this.display = function () {
            alert(_messages.join('\n'));
        }

        this.isValid = function () {
            return _messages.length == 0;
        }
    };

    //binding with the form
    //--------------------
    var createCmdFromAddNewForm = function (faction, Validation, f) {
        return function (e) {
            e.stopPropagation();
            e.preventDefault();

            var name = $("#formAddNewCharacter .nameInput").val();
            var level = $("#formAddNewCharacter .levelInput").val();
            var race = $("#formAddNewCharacter .raceInput:checked").val();
            var characterClass = $("#formAddNewCharacter .characterClassInput:checked").val();


            var validation = new Validation();
            if (!name) validation.push("The name is mandatory to create a new character.");
            if (!level) validation.push("The level is mandatory to create a new character.");
            if (!race) validation.push("The race is mandatory to create a new character.");
            if (!characterClass) validation.push("The class is mandatory to create a new character.");
                
            if (validation.isValid()) {
                var cmd = new AddNewCharacterCmd(name, level, race, characterClass, faction);
                f(cmd);
            }
            else {
                validation.display();
            }
                    
        }
    };

    //Navigation 
    //----------------------

    this.displayCharacterForm = function () {

        $(selectorCharacterCreation).html(templateAddNewCharacter());

        var faction = new Faction();
        var characterClass = new Class(_hasLevel55Character);
        var race = new Race();

        $(selectorCharacterCreation).on("click", ".addNewCharacterSubmit", createCmdFromAddNewForm(faction, AlertValidation, addNewCharacter));

    };


    //commands actions
    //---------------------------
    var addNewCharacter = function (cmd) {
        core.ajax("/Account/AddNewCharacterAsync", cmd)
            .done(function () {
                //update the datasource
                //using jquery, might think of a better tool to do this, a singleordefault would be nice
                var c = new Character(cmd.Id, cmd.Name, cmd.Level, cmd.Race, cmd.Faction, cmd.Class, true);
                    
                $.Topic('characterAdded').publish(c);
                $(selectorCharacterCreation).html('');
            })
            .fail(function (messages) {
                //a bit brutal should be handed nicely in the page, if time allows...
                for (var i = 0; i < messages.length; i++) alert(messages[i]);
            });
    };
        
}

}).call(this.AddNewCharacterViewModel || (this.AddNewCharacterViewModel = {}), jQuery, this.Core);