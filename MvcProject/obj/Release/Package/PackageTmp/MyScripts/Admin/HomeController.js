ProjectApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');
        $routeProvider
            .when("/Admin/EditPageByUser/:id/", {
                templateUrl: "/AngularRouting/EditPageByUserDetails.html",
                controller: "EditPageByUserDetailsCTRL"
            });
        $routeProvider
            .when("/Home/Page/:id/", {
                templateUrl: "/AngularRouting/ViewPageDetails.html",
                controller: "ViewPageDetailsCTRL"
            });
    }]);

ProjectApp.controller('ViewPageDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce','$window', function ($scope, $routeParams, $http, $sce,$window) {


    let id = $routeParams.id;
    console.log(id)
    $scope.GetPage = function () {

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/Page/GetPagesByPageKey/" + id)

                .then(function (response) {
                    if (response.data.Data.length == 0) {
                        $window.location.href = '/Security/Unauthorized/'

                    } else {
                        $scope.ArtistName = response.data.Data[0].ArtistName;
                        $scope.Title = response.data.Data[0].Title;
                        $scope.Technique = response.data.Data[0].Technique;
                        $scope.Size = response.data.Data[0].Size;
                        $scope.DateYear = response.data.Data[0].DateYear;
                        $scope.Signature = response.data.Data[0].Signature;
                        $scope.Img = response.data.Data[0].Img;
                        $scope.Id = response.data.Data[0].Id;
                        $scope.Note = response.data.Data[0].Note;
                    }
                   




                })

        }

    }

    $scope.GetPage();



}]);

ProjectApp.controller('EditPageByUserDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce','$window', function ($scope, $routeParams, $http, $sce,$window) {


    let id = $routeParams.id;
    $scope.GetPage = function () {

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/Page/GetPage/" + id)

                .then(function (response) {
                    $scope.ArtistName = response.data.Data[0].ArtistName;
                    $scope.Title = response.data.Data[0].Title;
                    $scope.Technique = response.data.Data[0].Technique;
                    $scope.Size = response.data.Data[0].Size;
                    $scope.Signature = response.data.Data[0].Signature;
                    $scope.Note = response.data.Data[0].Note;
                    $scope.Img = response.data.Data[0].Img;
                    $scope.Id = response.data.Data[0].Id;



                })

        }

    }

    $scope.GetPage();


    $scope.postdata = function (Img, ArtistName, Title, Technique, Size, Signature, Note) {

        var data = {
            Id: $scope.Id,
            Img: Img,
            ArtistName: ArtistName,
            Title: Title,
            Technique: Technique,
            Size: Size,
            Signature: Signature,
            Note: Note
        };

        $http.post('/Page/UpdatePageByUser/', JSON.stringify(data)).then(function (response) {
            if (response.data)
                showMessage('success', null, 'Post Data Submitted Successfully!');
            document.getElementById('btnEditByUserDisable').disabled = 'disabled';
            $scope.RedirectToUrl = function () {
                setTimeout(function () { $window.location.href = '/Home/' }, 2000);
            }
            $scope.RedirectToUrl();
        }, function (response) {
            showMessage('warning', null, 'Service not Exists');
        });

    };

}]);

ProjectApp.controller("GetAllPageHomeCTRL", function ($scope, $http, NgTableParams, $window) {


    $scope.GoEditDetail = function (id) {
        window.location.href = '/Admin/EditPageByUser/' + id;
    }
    $scope.GoPageDetail = function (id) {
        window.location.href = '/Home/ViewPageDetails/' + id;
    }

 

    $scope.GetAllByUserId = function () {
        $http.get("/Page/GetPagesByUserId")
            .then(function (response) {
                $scope.data = response.data.Data;
                console.log($scope.data)

                var data = $scope.data;

                $scope.tableParams = new NgTableParams({}, { dataset: data });
            });

    }
    $scope.GetAllByUserId();


});


function showMessage(type, message, title) {
    Command: toastr[type](message, title)

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}