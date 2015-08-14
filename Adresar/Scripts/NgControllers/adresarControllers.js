angular.module('adresarApp.controllers', [])
.controller('adresarController', function($scope, $timeout, adresarService)
{
    $scope.naslov = "Prikaz kontakta";
    $scope.brojac = 0;
    $scope.detalji = [];
    $scope.trenutnaStranica = 0;

    pocetniPodaci(0);
    function pocetniPodaci(str) {
        adresarService.Detalji(str)
            .success(function (data) {
                for (i = 0; i < data.length; i++)
                {
                    $scope.detalji[i] = data[i];
                }
                console.log($scope.detalji);
            })
            .error(function (error) {
                $scope.status = 'Nije moguce dohvatiti podatke: ' + error.message;
                console.log($scope.status);
            });
    }

    $scope.podaci = function (x) {
        if (x == -1)
        { alert("Nema prethodnih kontakata."); return; }
        adresarService.Detalji(x)
            .success(function (data) {
                if (data.length == 0) { alert("Nema više kontakata."); return; }
                $scope.detalji = []
                for (i = 0; i < data.length; i++) {
                    $scope.detalji[i] = data[i];
                }
                console.log($scope.detalji);
                $scope.trenutnaStranica = x;
            })
            .error(function (error) {
                $scope.status = 'Nije moguce dohvatiti podatke: ' + error.message;
                console.log($scope.status);
            });
    }

    $scope.prikaz = function (i) {
        $scope.startFade = true;
        $timeout(function () {
            $scope.hidden = true;
            $scope.brojac = i;

            $scope.startShow = true;
            $timeout(function () {
                $scope.hidden = false;
                $scope.startFade = false;
                $scope.startShow = false;
            }, 250);
        }, 250);
    }

})