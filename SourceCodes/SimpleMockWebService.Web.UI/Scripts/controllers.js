(function (angular) {
    var module = angular.module("demoApp", []);
    module.controller("demoController", function ($scope, $http) {
        $scope.getHtml = function (method, url) {
            var config = {
                "method": method,
                "url": url
            };
            if (method == "post" || method == "put") {
                config.data = "=" + JSON.stringify({ "data": "dummy" });
                config.headers = { "Content-Type": "application/x-www-form-urlencoded" };
            }
            $http(config)
                .success(function (data) {
                    $scope.json = JSON.stringify(data, null, "    ");
                });
        };
    });
})(angular);
