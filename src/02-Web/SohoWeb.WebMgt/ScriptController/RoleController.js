define(["app"], function (app) {
    app.register.controller("RoleController", function ($scope, $http, $routeParams) {
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
                PageSize: $scope.pager.size
            };
            angular.extend($scope.data, filter);

            $http.post("/ControlPanel/QueryRoles", $scope.data).success(function (res) {
                $scope.pager.setTotal(res.TotalCount);
                $scope.result = res.ResultList;
            });
        };
        $scope.update = function () {
            $http.post("/ControlPanel/UpdateRole", $scope.data).success(function (res) {
                alert("修改成功!");
            });
        };
        $scope.insert = function () {
            $http.post("/ControlPanel/InsertRoles", $scope.data).success(function (res) {
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
            $http.post("/ControlPanel/DeleteRole", sysNos).success(function (res) {
                alert("删除成功!");
            });
        };
        $scope.updateStatus = function () {
            $http.post("/ControlPanel/UpdateRoleStatus", $scope.data).success(function (res) {
                alert("状态更新成功!");
            });
        };
        $scope.saveRoleAllotFun = function () {
            var postData = [];
            $("#selected li").each(function () {
                var me = $(this);
                postData.push({
                    RoleSysNo: $scope.data.SysNo,
                    FunctionSysNo: me.attr("SysNo")
                });
            });
            $http.post("/ControlPanel/SaveRoleFunctions",postData).success(function () {
                alert("保存成功");
            });
        };

        function getFuns(sysNo) {
            $http.post("/ControlPanel/GetRolesBySysNo", { SysNo: sysNo }).success(function (res) {
                $scope.data = res;
            });
        }
        if ($routeParams && $routeParams["SysNo"] && $routeParams["SysNo"] > 0) {
            getFuns($routeParams["SysNo"]);
        }
        if ($routeParams && $routeParams["RoleSysNo"] && $routeParams["RoleSysNo"] > 0) {
            getFuns($routeParams["RoleSysNo"]);
            $http.post("/ControlPanel/GetRoleFunctionsInfo", [$routeParams["RoleSysNo"]]).success(function (res) {
                $scope.selected = res.ExistsFunctions;
                $scope.unselected = res.NotExistsFunctions;
            });
        }
    });
});