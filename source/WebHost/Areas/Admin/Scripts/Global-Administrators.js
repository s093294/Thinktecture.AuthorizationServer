﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */


$(function () {

    var svc = new as.Service("admin/GlobalAdministrators");

    function GlobalAdministrators(list) {
        this.administrators = ko.mapping.fromJS(list);
        this.nameToAdd = ko.observable("");
        as.util.addRequired(this, "nameToAdd", "Name To Add");
        as.util.addAnyErrors(this);
    }
    
    GlobalAdministrators.prototype.addAdmin = function(){
        var vm = this;
        svc.post(vm.nameToAdd()).then(function (data) {
            vm.nameToAdd("");
            vm.administrators.push(ko.mapping.fromJS(data));
        });
    }

    GlobalAdministrators.prototype.deleteAdmin = function(item){
        var vm = this;
        svc.delete(item.id()).then(function () {
            vm.administrators.remove(item);
        });
    }

    svc.get().then(function (data) {
        var vm = new GlobalAdministrators(data);
        ko.applyBindings(vm);
    });
});
