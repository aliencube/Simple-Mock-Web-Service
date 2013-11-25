(function (angular) {
    var module = angular.module("demoApp", []);
    
    module.controller("demoController", function ($scope, $http) {
        $scope.items = [
            {
                "title": "Get List of Contents",
                "method": "get",
                "url": "/api/contents",
                "result": "<- Click for result"
            },
            {
                "title": "Get Content #1",
                "method": "get",
                "url": "/api/content/1",
                "result": "<- Click for result"
            },
            {
                "title": "Post Content",
                "method": "post",
                "url": "/api/content",
                "result": "<- Click for result"
            },
            {
                "title": "Put Content #1",
                "method": "put",
                "url": "/api/content/1",
                "result": "<- Click for result"
            },
            {
                "title": "Delete Content #1",
                "method": "delete",
                "url": "/api/content/1",
                "result": "<- Click for result"
            }
        ];

        $scope.getJsonResult = function (index, method, url) {
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
                    $scope.items[index].result = JSON.stringify(data, null, "    ");
                });
        };
    });
})(angular);
