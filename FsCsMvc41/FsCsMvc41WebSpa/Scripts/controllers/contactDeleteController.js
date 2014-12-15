(function (module) {
    module.contactDeleteController = function ($scope, $routeParams, $location, ContactsService) {
      var id = $routeParams.id;
 
      $scope.deleteContact = function (really) {
        var redirectToRoot = function () {
          $location.path("/");
        }
        if (really) {
          ContactsService.deleteItem(id);
        } else {
          redirectToRoot();
        }
      };
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {});
