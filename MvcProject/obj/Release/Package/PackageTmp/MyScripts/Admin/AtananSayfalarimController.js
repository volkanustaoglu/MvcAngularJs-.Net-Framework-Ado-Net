ProjectApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');
        $routeProvider
            .when("/AtananSayfalarim/Duzenleme/:id/", {
                templateUrl: "/AngularRouting/EditPageByUserDetails.html",
                controller: "EditPageByUserDetailsCTRL"
            });
        $routeProvider
            .when("/Tag/:id/", {
                templateUrl: "/AngularRouting/ViewPageDetails.html",
                controller: "ViewPageDetailsCTRL"
            });
    }]);


ProjectApp.service('AtananSayfalarimService', function ($http) {
    this.UpdatePageByUser = function (data, files, success, error) {

        var fd = new FormData();
        fd.append(data, files);
        fd.append('data', JSON.stringify(data));


        var request = {
            method: 'POST',
            url: '/Page/UpdatePageByUser',
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

ProjectApp.controller('ViewPageDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce', '$window', function ($scope, $routeParams, $http, $sce, $window) {


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

ProjectApp.controller('EditPageByUserDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce', '$window', 'AtananSayfalarimService', function ($scope, $routeParams, $http, $sce, $window, AtananSayfalarimService) {



    $scope.files = [];



    $scope.fileNameChanged = function (file) {

        if (file.files != undefined && file.files.length > 0) {

            for (var i = 0; i < file.files.length; i++) {

                $scope.files = file.files[0];
                console.log($scope.files)

            }

        }

    };

    $scope.GetOnlineUserRole = function () {
        $http.get("/User/GetOnlineUserRole")
            .then(function (response) {
                $scope.getOnlineRole = response.data.Data[0].Role;
                if ($scope.getOnlineRole !=='Gallery') {
                    var link = document.getElementById('hideExhibitionSelect');
                    link.style.display = 'none';

                }
            });

    }
    $scope.GetOnlineUserRole();



    let id = $routeParams.id;
    $scope.GetPage = function () {

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/Page/GetPageWithExhibition/" + id)

                .then(function (response) {
                    console.log(response)
                    $scope.ArtistName = response.data.Data[0].ArtistName;
                    $scope.Title = response.data.Data[0].Title;
                    $scope.Technique = response.data.Data[0].Technique;
                    $scope.Size = response.data.Data[0].Size;
                    $scope.Signature = response.data.Data[0].Signature;
                    $scope.Note = response.data.Data[0].Note;
                    $scope.Img = response.data.Data[0].Img;
                    $scope.Id = response.data.Data[0].Id;
                    $scope.ExhibitionId = response.data.Data[0].ExhibitionId;
                    $scope.ExhibitionName = response.data.Data[0].ExhibitionName;




                    $scope.GetGalleryWithUsernameForUserId = function () {
                        $http.get("/GalleryMethod/GetGalleryWithUsernameForUserId")
                            .then(function (response) {
                                $scope.resultData = response.data.Data;

                                if ($scope.resultData.length != 0) {
                                    $scope.GaleryId = response.data.Data[0].Id

                                    $scope.GetGetAllExhibitionsForGalleryId = function () {


                                        var id = $scope.GaleryId


                                        $http.get("/Exhibition/GetAllExhibitionsForGalleryId/" + id)

                                            .then(function (response) {

                                                $scope.GetAllExhibitionsData = response.data.Data

                                                $scope.obj = { "selected": $scope.ExhibitionName == "" ? null : $scope.ExhibitionName };

                                                $scope.FindUserId = function (ExhibitionSelectedId) {
                                                    if (ExhibitionSelectedId == null) {
                                                        $scope.EditExhibitionIdSelected = 0
                                                    } else {
                                                        $scope.GetAllExhibitionsData.forEach(function (item) {
                                                            if (item.Name == ExhibitionSelectedId) {
                                                                $scope.EditExhibitionIdSelected = item.Id;

                                                            }
                                                        })

                                                    }
                                                }


                                                //var link = document.getElementById('nav-ask');
                                                //link.style.display = 'none'; //or
                                                //link.style.visibility = 'hidden';



                                            })

                                    }


                                    $scope.GetGetAllExhibitionsForGalleryId();

                                }


                            });






                    }
                    $scope.GetGalleryWithUsernameForUserId();



                })

        }

    }

    $scope.GetPage();



    $scope.ImagePathUrl = ''
    $scope.postdata = function (Img, ArtistName, Title, Technique, Size, Signature, Note) {


        var data = {
            Id: $scope.Id,
            Img: Img,
            ArtistName: ArtistName,
            Title: Title,
            Technique: Technique,
            Size: Size,
            Signature: Signature,
            Note: Note,
            ExhibitionId: $scope.EditExhibitionIdSelected
        };

        AtananSayfalarimService.UpdatePageByUser(data,

            $scope.files,

            function success(response) {


                if (response.IsSuccess) {

                    document.getElementById('btnEditByUserDisable').disabled = 'disabled';
                    $.notify("Başarıyla Kaydedildi!", "success");
                    $scope.RedirectToUrl = function () {
                        setTimeout(function () { $window.location.href = '/AtananSayfalarim/' }, 200);
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

}]);

ProjectApp.controller("GetAllPageHomeCTRL", function ($scope, $http, NgTableParams, $window) {


    $scope.GoEditDetail = function (id) {
        window.location.href = "/AtananSayfalarim/Duzenleme/" + id;
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


