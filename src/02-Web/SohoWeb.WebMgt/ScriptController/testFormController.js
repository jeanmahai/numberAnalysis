define(["app","_baseController"], function (app) {
    app.register.controller("testFormController", function ($scope, $http,$controller) {
        
		angular.extend(this,$controller("_baseController",{$scope:$scope}));
		
		$scope.save = function () {
            $http.post("TestForm.aspx", {
                Name: $scope.name,
                Age:$scope.age
            }).success(function (req) {
                console.info(req);
            });
        };
        $scope.name = "sdf";
        $scope.age = "sdf";
    });
});