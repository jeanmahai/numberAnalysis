define(["app"], function (app) {

    app.register.controller("LogController", function ($scope, $http) {

        $scope.data = {};

        $scope.result = [];

        $scope.select = function () {

            var me = this;

            var filter = {
                PageIndex: $scope.pager.index,
                PageSize: $scope.pager.size
            };

            angular.extend(filter, me.data);

            $http.post("/ControlPanel/QueryLogs", filter).success(function (res) {

                $scope.result = res.ResultList;

            });

        };

        $scope.pager = new N.Pager(1, 10, function () {

            $scope.select();

        });

        $scope.classes = [];
        $http.post("/ControlPanel/GetLogClasses").success(function (res) {

            $scope.classes = res;

        });

    });

    return app;
});