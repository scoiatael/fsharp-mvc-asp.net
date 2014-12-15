(function (util) {    
    angular.module("contactsApp.service", [], function ($provide) {
        $provide.factory("ContactsService", ["$http", "$location", function($http, $location) {
            var contactService = {};
            var contacts = [];

            contactService.getAll = function(callback) {
                if (contacts.length === 0) {
                    $http.get("/api/contacts").success(function(data) {
                      contacts = data;
                      callback(contacts);
                    }).error(function() {
                      toastr.error("There was an error downloading contacts", "<sad face>");
                    });
                } else {
                    callback(contacts);
                }
            };

            contactService.addItem = function (item) {
                contacts.push(item);
                $http.post("/api/contacts",
                    JSON.stringify(item), {
                    headers: {
                      'Content-Type': 'application/json'
                    },
                })
                .success(function () {
                    toastr.success("You have successfully created a new contact!", "Success!");
                    $location.path("/");
                })
                .error(function () {
                    contacts.pop();
                    toastr.error("There was an error creating your new contact", "<sad face>");
                });
            };

            contactService.deleteItem = function (id) {
              console.log("Before splice of", id, contacts);
              var contact = contacts.splice(id, id+1)[0];
              console.log("Sending request to delete ", contact, "from", contacts);
              $http.delete("/api/contacts", {
                  data: JSON.stringify(contact), 
                  headers: {
                    'Content-Type': 'application/json'
                  },
              })
              .success(function () {
                  toastr.success("You have successfully deleted contact!", "Success!");
                  $location.path("/");
              })
              .error(function () {
                  contacts.splice(id, id, contact);
                  toastr.error("There was an error deleting contact", "<sad face>");
              });
            }

            contactService.updateItem = function (id, contact) {
              old_contact = contacts[id];
              contacts[id] = contact;
              $http.put( "/api/contacts",
                  JSON.stringify(contact), {
                  headers: {
                    'Content-Type': 'application/json'
                  },
              })
              .success(function () {
                  toastr.success("You have successfully updated contact!", "Success!");
                  $location.path("/");
              })
              .error(function () {
                  contacts[id] = old_contact;
                  toastr.error("There was an error updating contact", "<sad face>");
              });
            };

            return contactService;
        }]);
    });
    
    angular.module("contactsapp", ["contactsApp.service"])
        .config(["$routeProvider", function ($routeProvider) {
            $routeProvider
                .when("/create", { templateUrl: util.buildTemplateUrl("contactCreate.htm") })
                .when("/delete/:id", { templateUrl: util.buildTemplateUrl("contactDelete.htm") })
                .when("/update/:id", { templateUrl: util.buildTemplateUrl("contactUpdate.htm") })
                .otherwise({ redirectTo: "/", templateUrl: util.buildTemplateUrl("contactDetail.htm") });
        }]);
})(appFsMvc.utility);

