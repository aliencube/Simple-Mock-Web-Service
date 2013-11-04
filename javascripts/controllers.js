(function (angular) {
    var module = angular.module("markdownApp", []);

    module.value("converter", function ($scope, $http) {
        return converterService($scope, $http);
    });

    module.controller("markdownController", function ($scope, $http, converter) {
        $scope.init = function () {
            $scope.converterService = converter($scope, $http);
        };

        $scope.getHtml = function (filename) {
            return $scope.converterService.getHtml(filename);
        };
    });
})(angular);