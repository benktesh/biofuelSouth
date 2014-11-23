var ViewModel = function () {
    var self = this;
    self.productivity = ko.observableArray();
    self.error = ko.observable();

 
    self.newProductivity = {
        Id: ko.observable(),
        CountyId: ko.observable(),
        CropType: ko.observable(),
        Yield: ko.observable(),
    }

    var productivityUri = '/api/productivity/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllProductivity() {
        ajaxHelper(productivityUri, 'GET').done(function (data) {
            self.productivity(data);
        });
    }



    self.addProductivity = function (formElement) {
        var productivity = {
           //Id is intentionally left blank as db will create identity by its own
            CountyId: self.newProductivity.CountyId(),
            CropType: self.newProductivity.CropType(),
            Yield: self.newProductivity.Yield(),
        };
        ajaxHelper(productivityUri, 'POST', productivity).done(function (item) {
            self.productivity.push(item);
        });
    }
    // Fetch the initial data.
    getAllProductivity();
};

ko.applyBindings(new ViewModel());