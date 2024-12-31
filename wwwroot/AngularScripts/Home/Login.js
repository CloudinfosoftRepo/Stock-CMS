
var app = angular.module('Stock-CMS', []);

app.controller('StockLoginCtrl', function ($scope, $http, $window) {
    //$scope.publicKey = "";
    //$scope.initialize = function () {
    //    $scope.publicKey = document.getElementById('publicKey').value;
    //};

    $scope.submit = function () {
        /*var pass = $scope.password;*/
        //var publicKey = $scope.publicKey;
        //var publicKeyForge = forge.pki.publicKeyFromPem(publicKey);
        //var encryptedPasswordBytes = publicKeyForge.encrypt(pass, 'RSA-OAEP');
        //var encryptedPassword = forge.util.encode64(encryptedPasswordBytes);
        //$scope.passwordenc = encryptedPassword;

        $http({
            method: 'POST',
            url: '/Home/Login',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                loginUsername: $scope.username,
                password: $scope.password
            }
        }).then(function (response) {
            if (response.data.status == true) {

                //alert(result.message);
                //var url = '/Home/Dashboard';
                var url = '/Home/Dashboard';

                $window.location.href = url;
            }
            else {
                //alert(result.message);
                $window.alert(response.data.message);
            }
        }, function (response) {
            alert(response.data);
            console.error('Error:', response);
        });

    };
});