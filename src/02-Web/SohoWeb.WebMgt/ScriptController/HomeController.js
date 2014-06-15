/**
 * Created by Jeanma on 14-5-5.
 */
define(["app"],function(app){
    return app.register.controller("homeController",["$scope",function($scope){
        $scope.home=function(){
            alert("home");
        };
    }]);
});