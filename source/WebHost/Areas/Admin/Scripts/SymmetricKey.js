﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */


$(function () {
    var svc = new as.Service("admin/SymmetricKeys");

    function SymmetricKey(data) {
        var vm = this;
        vm.isNew = ko.observable(!data);
        data = data || {
            id : 0,
            name: "",
            value:""
        };
        ko.mapping.fromJS(data, null, vm);

        as.util.addRequired(this, "name", "Name");
        as.util.addRequired(this, "value", "Value");
        as.util.addAnyErrors(this);

        vm.editDescription = ko.computed(function () {
            return vm.isNew() ? "New" : "Manage";
        });

        vm.createKey = function () {
            svc.get().then(function(data){
                vm.value(data.value);
            });
        };

        vm.save = function () {
            if (vm.isNew()) {
                svc.post(ko.mapping.toJS(vm)).then(function (data, status, xhr) {
                    window.location = window.location + '#' + data.id;
                    vm.isNew(false);
                    vm.id(data.id);
                });
            }
            else {
                svc.put(ko.mapping.toJS(vm), vm.id());
            }
        };
    }

    if (window.location.hash) {
        var id = window.location.hash.substring(1);
        svc.get(id).then(function (data) {
            var vm = new SymmetricKey(data);
            ko.applyBindings(vm);
        });
    }
    else {
        var vm = new SymmetricKey();
        ko.applyBindings(vm);
    }
});
