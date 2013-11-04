var converterService = function ($scope, $http) {
    var getHtml = function (filename) {
        $http.get(filename).success(function (data) {
            var converter = new Markdown.Converter().makeHtml;
            var html = converter(data);
            return html;
        });
    };
};