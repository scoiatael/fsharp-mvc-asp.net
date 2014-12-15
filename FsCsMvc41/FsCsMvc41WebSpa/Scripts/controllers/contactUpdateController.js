(function (module) {
    module.contactUpdateController = function ($scope, $routeParams, $location, ContactsService) {
      var id = $routeParams.id;
      ContactsService.getAll(function (contacts) {
        $scope.contact = contacts[parseInt(id)];
      });
      $scope.updateContact = function () {
        ContactsService.updateItem(id, $scope.contact);
      };
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {});
