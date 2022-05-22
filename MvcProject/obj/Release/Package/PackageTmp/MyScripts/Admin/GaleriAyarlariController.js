//$.notify("Access granted", "success");
//$.notify("Do not press this button", "info");
//$.notify("Warning: Self-destruct in 3.. 2..", "warn");
//$.notify("BOOM!", "error");




ProjectApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');

        $routeProvider
            .when("/Gallery/:id/", {
                templateUrl: "/AngularRouting/ViewGalleryDetails.html",
                controller: "ViewGalleryDetailsCTRL"
            });
    }]);









ProjectApp.service('GaleriAyarlariService', function ($http) {
    this.UpdateGalleryByUser = function (data, files, success, error) {

        var fd = new FormData();
        fd.append(data, files);
        fd.append('data', JSON.stringify(data));


        var request = {
            method: 'POST',
            url: '/GalleryMethod/UpdateGalleryByUser',
            data: fd,
            headers: {
                'Content-Type': undefined
            }
        };

        $http(request).then(
            function (response) {

                if (success) {

                    success(response.data);


                }

            }, error);

    }
});



ProjectApp.controller('ViewGalleryDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce', '$window', function ($scope, $routeParams, $http, $sce, $window) {


    let id = $routeParams.id;
    console.log(id)
    $scope.GetPage = function () {

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/GalleryMethod/GetGalleryByName/" + id)

                .then(function (response) {
                    if (response.data.Data.length == 0) {
                        $window.location.href = '/Security/Unauthorized/'

                    } else {
                        console.log(response.data.Data)
                        $scope.Name = response.data.Data[0].Name;
                        $scope.Input1 = response.data.Data[0].Input1;
                        $scope.Input2 = response.data.Data[0].Input2;
                        $scope.Img = response.data.Data[0].Img;
                        $scope.Id = response.data.Data[0].Id;
                    }



                })

        }

    }

    $scope.GetPage();



}]);

ProjectApp.controller("GaleriAyarlariCTRL", function ($scope, $http, NgTableParams, $window, GaleriAyarlariService) {

  


    $scope.files = [];



    $scope.fileNameChanged = function (file) {

        if (file.files != undefined && file.files.length > 0) {

            for (var i = 0; i < file.files.length; i++) {

                $scope.files = file.files[0];
                console.log($scope.files)

            }

        }

    };

    $scope.GetGalleryWithUsernameForUserId = function () {
        $http.get("/GalleryMethod/GetGalleryWithUsernameForUserId")
            .then(function (response) {
                $scope.resultData = response.data.Data;

                if ($scope.resultData.length != 0) {

                    $scope.Img = response.data.Data[0].Img;
                    $scope.Name = response.data.Data[0].Name;
                    $scope.Input1 = response.data.Data[0].Input1;
                    $scope.Input2 = response.data.Data[0].Input2;
                    $scope.Id = response.data.Data[0].UserId;
                    $scope.GaleryId = response.data.Data[0].Id

                    $scope.GetGetAllExhibitionsForGalleryId = function () {


                        var id = $scope.GaleryId


                        $http.get("/Exhibition/GetAllExhibitionsForGalleryId/" + id)

                            .then(function (response) {

                                $scope.GetAllExhibitionsData = response.data.Data

                            
                                $(document).ready(function () {
                                    $('#exhibitionsTable').DataTable({
                                        language: {
                                            url: '//cdn.datatables.net/plug-ins/1.10.12/i18n/Turkish.json'
                                        }
                                        
                                    });
                                });


                            })

                    }


                    $scope.GetGetAllExhibitionsForGalleryId();

                }


            });






    }
    $scope.GetGalleryWithUsernameForUserId();






    $scope.postdata = function (Img, Name, Input1, Input2) {




        var data = {
            Id: $scope.Id,
            Img: Img,
            Name: Name,
            Input1: Input1,
            Input2: Input2
        };
        GaleriAyarlariService.UpdateGalleryByUser(data,

            $scope.files,

            function success(response) {


                if (response.IsSuccess) {

                    document.getElementById('btnDisabledUpdate').disabled = 'disabled';
                    $.notify("Başarıyla Kaydedildi!", "success");
                    $scope.RedirectToUrl = function () {
                        setTimeout(function () { $window.location.href = '/GaleriAyarlari/' }, 200);
                    }
                    $scope.RedirectToUrl();


                } else {
                    $.notify("Kaydedilmedi!", "warn");

                }

            },

            function error() {

                $.notify("Hata Oluştu!", "error");

            });



    };


    $scope.postExhibition = function (Name, Description) {

        var data = {
            Name: Name,
            Description: Description,
            GalleryId: $scope.GaleryId
        }

        $http.post('/Exhibition/CreateExhibition/', JSON.stringify(data)).then(function (response) {
            if (response.data)
                document.getElementById('btnDisabledUpdateExibition').disabled = 'disabled';
            $.notify("Başarıyla Kaydedildi!", "success");
            $scope.RedirectToUrl = function () {
                setTimeout(function () { $window.location.href = '/Sergilerim/' }, 200);
            }
            $scope.RedirectToUrl();
        }, function (response) {
            $.notify("Hata Oluştu!", "error");
        });
    }



    $scope.deletedata = function (event) {
     
       

        swal({
            title: "Emin misiniz?",
            text: "Sergiyi silmek istediğinizden emin misiniz?",
            icon: "warning",
            buttons: ["İptal", "Evet"],
            dangerMode: true,
           
        })
            .then((willDelete) => {
                if (willDelete) {

             
                    $http.post('/Exhibition/DeleteExhibition/' + event,).then(function (response) {
                        if (response.data) {
                            $.notify("Başarıyla Silindi!", "success");
                            $scope.RedirectToUrl = function () {
                                setTimeout(function () { $window.location.href = '/Sergilerim/' }, 200);
                            }
                            $scope.RedirectToUrl();
                        }

                        else
                            $.notify("Hata Oluştu!", "error");
                    });


                } else {
                }

            });
    };


});
