/**
 * Created by jm96 on 14-5-16.
 */
define(["app","_baseController"],function(app){
    app.register.controller("testDatepickerController",function($scope,$controller){
        angular.extend(this,$controller("_baseController",{$scope:$scope}));
        $scope.dateValue=new Date();
    });
});