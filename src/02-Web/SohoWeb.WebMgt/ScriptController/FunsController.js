define(["app"], function (app) {
    app.register.controller("FunsController", function ($scope, $http,$routeParams) {
        $scope.data = {};
        $scope.result = [];
        $scope.status = [];

        $http.post("/ControlPanel/GetCommonStatusList").success(function (res) {
            $scope.status = res;
        });

        $scope.pager = new N.Pager(1, 10, function () {
            $scope.select();
        });
        $scope.select = function () {
            var filter = {
                PageIndex: $scope.pager.index,
                PageSize:$scope.pager.size
            };
            angular.extend($scope.data, filter);

            $http.post("/ControlPanel/QueryFunctions", $scope.data).success(function (res) {
                $scope.pager.setTotal(res.TotalCount);
                $scope.result = res.ResultList;
            });
        };
        $scope.update = function () {
            $http.post("/ControlPanel/UpdateFunction", $scope.data).success(function (res) {
                alert("修改成功!");
            });
        };
        $scope.insert = function () {
            $http.post("/ControlPanel/InsertFunctions", $scope.data).success(function (res) {
                alert("添加成功!");
            });
        };
        $scope.delete = function () {
            if (!confirm("是否真的要删除?")) return;
            var sysNos = [];
            angular.forEach($scope.result, function (value) {
                if (value.Checked) {
                    sysNos.push(value.SysNo);
                }
            });
            $http.post("/ControlPanel/DeleteFunction", sysNos).success(function (res) {
                alert("删除成功!");
            });
        };
        $scope.updateStatus = function () {
            $http.post("/ControlPanel/UpdateFunctionStatus", $scope.data).success(function (res) {
                alert("状态更新成功!");
            });
        };

        function getFuns(sysNo) {
            $http.post("/ControlPanel/GetFunctionsBySysNo", { SysNo: sysNo }).success(function (res) {
                $scope.data = res;
            });
        }
        if ($routeParams && $routeParams["SysNo"] && $routeParams["SysNo"]>0) {
            getFuns($routeParams["SysNo"]);
        }
    });
});