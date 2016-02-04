(function ($, core,addNewCharacter) {

    if (core == null) throw "core must not be null";
    if (addNewCharacter == null) throw "addNewCharacter must not be null";

    //todo missing validation here
    var self = this;

    var idMain = "main";
    var idListContainer = "charactersList";
    var idViewAddNewCharacter = "viewAddNewCharacter";

    var selectorMainContainer = "#" + idMain;
    var selectorListContainer = "#" + idListContainer;
    var selectorIdViewAddNewCharacter = "#" + idViewAddNewCharacter;

    
    var templateMain = doT.template($("#mainTemplate").html());
    var templateItemList = doT.template($("#characterTemplate").html());

    
    //act as datasource for this viewmodel
    var characters = JSON.parse($("#accountData").html());
    

    //some duck typing to check if it is an array
    if (!characters || characters.length === undefined) throw "unable to parse characters";

    
       

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


    //Navigation 
    //----------------------

    this.displayList = function () {
        //render the main view
        var mainHtml = templateMain();
        $(selectorMainContainer).html(mainHtml);

        //render its list
        var characterListHtml = [];
        for (var i = 0; i < characters.length; i++)
            characterListHtml.push(templateItemList(characters[i]));
        $(selectorListContainer).html(characterListHtml.join(''));
    };

    this.displayCharacterForm = function () {
        //todo fake navigation
        $(selectorMainContainer).html('');

        var hasLevel55Character = $.grep(characters, function (e) { return e.Level >= 55; });

        var formAddNewCharacter = new addNewCharacter.createForm(hasLevel55Character);

        formAddNewCharacter.displayCharacterForm();

    };

    var removeCharacter = function (id) {
        core.ajax("/Account/RemoveCharacterAsync", new RemoveCharacterCmd(id))
            .done(function () {
                $.Topic('characterRemoved').publish(id);
            })
            .fail(function (messages) {
                //a bit brutal should be handed nicely in the page, if time allows...
                for (var i = 0; i < messages.length; i++) alert(messages[i]);
            });
    };

    var retrieveCharacter = function (id) {
        core.ajax("/Account/RetrieveCharacterAsync", new RetrieveCharacterCmd(id))
            .done(function () {
                $.Topic('characterRetrieved').publish(id);
            })
            .fail(function (messages) {
                //a bit brutal should be handed nicely in the page, if time allows...
                for (var i = 0; i < messages.length; i++) alert(messages[i]);
            });
    }


    //public functions
    //---------------------
    this.init = function () {
        //initial diplay of the list
        self.displayList();

        //subscribe to event via event bubbling
        
        $(selectorMainContainer).on("click", ".viewAddNewCharacter", self.displayCharacterForm);
        $(selectorMainContainer).on("click", ".removeCharacter", core.doAction(removeCharacter));
        $(selectorMainContainer).on("click", ".retrieveCharacter", core.doAction(retrieveCharacter));

    };

    //subscriptions to events coming from other modules

    //projections to the datasource
    //-----------------------------------
    var onCharacterAdded = function (c) {
        characters.push(c);
        $.Topic('characterListChanged').publish();
    }

    var onCharacterRetrieved = function (id) {
        //using jquery, might think of a better tool to do this, a singleordefault would be nice
        var charactersFound = $.grep(characters, function (e) { return e.Id == id; });
        if (charactersFound.length == 1) {
            charactersFound[0].IsActive = true;
            $.Topic('characterListChanged').publish();
        }

    };

    var onCharacterRemoved = function (id) {
        //using jquery, might think of a better tool to do this, a singleordefault would be nice
        var charactersFound = $.grep(characters, function (e) { return e.Id == id; });
        if (charactersFound.length == 1) {
            charactersFound[0].IsActive = false;
            $.Topic('characterListChanged').publish();
        };
    };

    //projections to the html
    //-----------------------------------
    //update the rendered html
    var onCharacterListChanged = function () {
        self.displayList();
    }

    $.Topic('characterAdded').subscribe(onCharacterAdded);
    $.Topic('characterRemoved').subscribe(onCharacterRemoved);
    $.Topic('characterRetrieved').subscribe(onCharacterRetrieved);
    $.Topic('characterListChanged').subscribe(onCharacterListChanged);

    

}).call(this.CharacterViewModel || (this.CharacterViewModel = {}), jQuery, this.Core, this.AddNewCharacterViewModel);