(function (angular) {
    var module = angular.module("demoApp", []);
    
    module.value("demo", function ($scope, $http) {
        return demoService($scope, $http);
    });

    module.controller("demoController", function ($scope, $http, demo) {
        $scope.demoService = demo($scope, $http);

        $scope.items = $scope.demoService.items;

        $scope.getJsonResult = function (index, method, url) {
            $scope.demoService.getJsonResult(method, url, $scope.items[index]);
        };
    });
})(angular);
