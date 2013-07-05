﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */


$(function () {
    var svc = new as.Service("admin/Applications");

    function Applications(list) {
        this.applications = ko.mapping.fromJS(list);
    }
    Applications.prototype.deleteApp = function (item) {
        var vm = this;
        svc.delete(item.id()).then(function () {
            vm.applications.remove(item);
        });
    }

    svc.get().then(function (data) {
        var vm = new Applications(data);
        ko.applyBindings(vm);
    });
});
