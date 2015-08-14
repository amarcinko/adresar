angular.module('adresarApp.services', [])
.factory('adresarService', ['$http', function ($http) {

    var adresarService = {};

    adresarService.Detalji = function (str) {
        return $http.get('/Home/Detalji/?str='+str);
    };
    return adresarService;

}]);