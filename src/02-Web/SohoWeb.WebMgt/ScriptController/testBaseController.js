/**
 * Created by jm96 on 14-5-12.
 */
define(["app", "_baseController"], function (app) {
    app.register.controller("testBaseController", function ($scope, $controller,$q,$N) {
        angular.extend(this, $controller("_baseController", {$scope: $scope}));
        $scope.my = "i am child";

        $scope.data = {};

        $scope.users = [
            {
                name: "name1",
                age: 1, id: 0
            },
            {
                name: "name2",
                age: 2, id: 1
            },
            {
                name: "name2",
                age: 2, id: 2
            },
            {
                name: "name2",
                age: 2, id: 3
            },
            {
                name: "name2",
                age: 2, id: 4
            },
            {
                name: "name2",
                age: 2, id: 5
            },
            {
                name: "name2",
                age: 2, id: 6
            }
        ];

        $scope.result = [];

        var pager= new N.Pager(1,3,function(){
            //真实环境应该使用$http,然后返回$http promise
            //模拟测试
            var deferred=$q.defer();
            $N.loading();
            setTimeout(function(){
                $scope.getData();
                deferred.resolve();
                $N.loaded();
            },1);
            return deferred.promise;
        });

        $scope.filter =pager;
        $scope.getData = function () {
            $scope.result = [];
            var start = ($scope.filter.index - 1) * $scope.filter.size;
            var end = $scope.filter.index * $scope.filter.size;
            angular.forEach($scope.users, function (val) {
                if (val.id <= end && val.id >= start) {
                    $scope.result.push(val);
                }
            });
            pager.setTotal(6);
        };
        $scope.addIndex = function () {
            $scope.filter.index++;
        };
    });
});