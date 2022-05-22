ProjectApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');
        $routeProvider
            .when("/Admin/EditGallery/:id/", {
                templateUrl: "/AngularRouting/EditGalleryDetails.html",
                controller: "EditGalleryDetailsCTRL"
            });
    }]);


ProjectApp.service('GalleryAdminService', function ($http) {
    this.UpdateGallery = function (data, files, success, error) {

        var fd = new FormData();
        fd.append(data, files);
        fd.append('data', JSON.stringify(data));


        var request = {
            method: 'POST',
            url: '/GalleryMethod/UpdateGallery',
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


ProjectApp.controller('EditGalleryDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce', '$window', 'GalleryAdminService', function ($scope, $routeParams, $http, $sce, $window, GalleryAdminService ) {



    $scope.files = [];



    $scope.fileNameChanged = function (file) {

        if (file.files != undefined && file.files.length > 0) {

            for (var i = 0; i < file.files.length; i++) {

                $scope.files = file.files[0];
                console.log($scope.files)

            }

        }

    };

    let id = $routeParams.id;
    $scope.GetGallery = function () {

        $scope.GetAllGalleriesWithUsername = function () {

            $http.get("/GalleryMethod/GetAllGalleriesWithUsername")
                .then(function (response) {
                    var pages = [];
                    $scope.pagesWithUsernameDatas = response.data.Data;

                    $scope.pagesWithUsernameDatas.forEach((item) => {
                        if (item.UserId == 0) {
                            item.Username = "Atama Yapılmadı"
                            pages.push(item)
                        } else {
                            pages.push(item)
                        }

                    });

                    $scope.GetAllGallery= pages;

                });
        }


        $scope.GetAllGalleriesWithUsername();

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/GalleryMethod/GetGalleryWithUsername/" + id)

                .then(function (response) {
                    console.log(response)
                    $scope.Name = response.data.Data[0].Name;
                    $scope.Img = response.data.Data[0].Img;
                    $scope.Input1 = response.data.Data[0].Input1;
                    $scope.Input2 = response.data.Data[0].Input2;
                    $scope.Id = response.data.Data[0].Id;
                    $scope.Username = response.data.Data[0].Username;
                    $scope.UserId = response.data.Data[0].UserId;


                    $scope.FindUserId = function (UserSelectedId) {

                        if (UserSelectedId == null) {
                            $scope.EditUserIdSelected = 0
                        } else {

                            $http.get("/User/GetAllUsersRoleGallery")
                                .then(function (responseResult) {
                                    $scope.usersResult = responseResult.data.Data;

                                   

                                    $scope.usersResult.forEach(function (item) {
                                        
                                      
                                        if (item.Username == UserSelectedId) {
                                            $scope.EditUserIdSelected = item.Id;
                                        }
                                    });

                                });
                        }

                    }

                    $scope.FindUserId($scope.Username);

                    $scope.obj = { "selected": $scope.Username == "" ? null : $scope.Username };


                    $http.get("/User/GetAllUsersRoleGallery")
                        .then(function (responseUser) {
                            
                            $scope.users = responseUser.data.Data;
                            console.log($scope.users)


                            $scope.GetAllGallery.forEach(function (item) {
                                if (item.UserId !== 0 && item.UserId != $scope.UserId) {
                                    console.log(item.UserId)

                                    var index = $scope.users.map(x => {
                                        return x.Id;
                                    }).indexOf(item.UserId);

                                    $scope.users.splice(index, 1);
                                    console.log($scope.users);
                                }

                            })

                        });



                })

        }


    }

    $scope.GetGallery();




    $scope.postdata = function (Name, Img, Input1, Input2) {


        var data = {
            Id: $scope.Id,
            Name: Name,
            UserId: $scope.EditUserIdSelected,
            Img: Img,
            Input1: Input1,
            Input2: Input2
        };
        GalleryAdminService.UpdateGallery(data,

            $scope.files,

            function success(response) {


                if (response.IsSuccess) {

                    document.getElementById('btnDisabledUpdate').disabled = 'disabled';
                    showMessage('success', null, 'Başarıyla Kaydedildi!');
                    $window.location.href = '/Admin/GalleryAdmin/';
                } else {


                       showMessage('warning', null, 'Kaydedilmedi!');
             

                }

            },

            function error() {
             
                  showMessage('danger', null, 'Hata oluştu!');

            });


        //$http.post('/GalleryMethod/UpdateGallery/', JSON.stringify(data)).then(function (response) {
        //    if (response.data)
        //        showMessage('success', null, 'Post Data Submitted Successfully!');
        //    document.getElementById('btnDisabledUpdate').disabled = 'disabled';
        //    $scope.RedirectToUrl = function () {
        //        setTimeout(function () { $window.location.href = '/Admin/GalleryAdmin/' }, 2000);
        //    }
        //    $scope.RedirectToUrl();
        //}, function (response) {
        //    showMessage('warning', null, 'Service not Exists');
        //});

    };

}]);

ProjectApp.controller("GetAllGalleryAdminCTRL", function ($scope, $http, NgTableParams, $window) {


    $scope.GoEditDetail = function (id) {
        window.location.href = '/Admin/EditGallery/' + id;
    }



    $scope.GetAllGalleriesWithUsername = function () {

        $http.get("/GalleryMethod/GetAllGalleriesWithUsername")
            .then(function (response) {
                var pages = [];
                $scope.pagesWithUsernameDatas = response.data.Data;

                $scope.pagesWithUsernameDatas.forEach((item) => {
                    if (item.UserId == 0) {
                        item.Username = "Atama Yapılmadı"
                        pages.push(item)
                    } else {
                        pages.push(item)
                    }

                });

                var data = pages;


                $scope.tableParams = new NgTableParams({}, { dataset: data });
            });
    }


    $scope.GetAllGalleriesWithUsername();




    $scope.deletedata = function (event) {
        var data = {
            Id: $(event.target).attr("data-id")
        };

        swal({
            title: "Are you sure?",
            text: "Are you sure delete this Gallery?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {

                    $http.post('/GalleryMethod/DeleteGallery/' + data.Id, JSON.stringify(data)).then(function (response) {
                        if (response.data) {
                            showMessage('success', null, 'Data Deleted Successfully!');
                            $scope.RedirectToUrl = function () {
                                setTimeout(function () { location.href = '/Admin/GalleryAdmin' }, 2000);
                            }
                            $scope.RedirectToUrl();
                        }

                        else
                            showMessage('warning', null, 'Service not Exists');
                    });


                } else {
                }

            });
    };




    $scope.Name = null;

    $scope.postdata = function (Name) {

        var data = {
            Name: Name
        };

        $http.post('/GalleryMethod/CreateGallery/', JSON.stringify(data)).then(function (response) {
            if (response.data)
                showMessage('success', null, 'Post Data Submitted Successfully!');
            document.getElementById('btnDisabled').disabled = 'disabled';

            $scope.RedirectToUrl = function () {
                setTimeout(function () { $window.location.href = '/Admin/GalleryAdmin/' }, 2000);
            }
            $scope.RedirectToUrl();
        }, function (response) {
            showMessage('warning', null, 'Service not Exists');
        });

    };

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

