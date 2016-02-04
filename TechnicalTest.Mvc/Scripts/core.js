(function () {

    this.guid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0,
              v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    this.doAction = function(f){
        return function (e) {
            e.preventDefault();
            e.stopPropagation();

            var id = $(e.target).data("id");
            f(id);
        };
    };

    this.ajax = function (url, cmd) {
        var deferred = $.Deferred();
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            data: cmd
        }).done(function (result) {
            if (result.IsOk) deferred.resolve();
            else deferred.reject(result.Messages);
        }).fail(function (e) {
            console.log("fatal error, cannot communicate properly with the server. " + e);
        })
        return deferred.promise();
    };

    var topics = {};

    jQuery.Topic = function (id) {
        var callbacks,
            topic = id && topics[id];
        if (!topic) {
            callbacks = jQuery.Callbacks();
            topic = {
                publish: callbacks.fire,
                subscribe: callbacks.add,
                unsubscribe: callbacks.remove
            };
            if (id) {
                topics[id] = topic;
            }
        }
        return topic;
    };

    



}).call(this.Core || (this.Core = {}));