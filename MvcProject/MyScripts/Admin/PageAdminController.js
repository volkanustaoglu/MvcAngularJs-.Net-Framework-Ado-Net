ProjectApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');
        $routeProvider
            .when("/Admin/EditPage/:id/", {
                templateUrl: "/AngularRouting/EditPageDetails.html",
                controller: "EditPageDetailsCTRL"
            });
    }]);


ProjectApp.service('PageAdminService', function ($http) {
    this.UpdatePage = function (data, files, success, error) {

        var fd = new FormData();
        fd.append(data, files);
        fd.append('data', JSON.stringify(data));


        var request = {
            method: 'POST',
            url: '/Page/UpdatePage',
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

ProjectApp.controller('EditPageDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce', '$window','PageAdminService', function ($scope, $routeParams, $http, $sce, $window,PageAdminService) {


    $scope.files = [];



    $scope.fileNameChanged = function (file) {

        if (file.files != undefined && file.files.length > 0) {

            for (var i = 0; i < file.files.length; i++) {

                $scope.files = file.files[0];

            }

        }

    };
   
    let id = $routeParams.id;
    $scope.GetPage = function () {

     

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/Page/GetPageWithUsername/" + id)

                .then(function (response) {
                    $scope.PageId = response.data.Data[0].PageId;
                    $scope.UserId = response.data.Data[0].UserId;
                    $scope.ArtistName = response.data.Data[0].ArtistName;
                    $scope.Title = response.data.Data[0].Title;
                    $scope.Technique = response.data.Data[0].Technique;
                    $scope.Size = response.data.Data[0].Size;
                    $scope.DateYear = response.data.Data[0].DateYear;
                    $scope.Signature = response.data.Data[0].Signature;
                    $scope.Img = response.data.Data[0].Img;
                    $scope.Id = response.data.Data[0].Id;
                    $scope.Note = response.data.Data[0].Note;
                    $scope.Username = response.data.Data[0].Username;


                    $scope.FindUserId = function (UserSelectedId) {

                        if (UserSelectedId == null) {
                            $scope.EditUserIdSelected = 0
                        } else {

                            $http.get("/User/GetAllUsers")
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

                
                    $http.get("/User/GetAllUsers")
                        .then(function (responseUser) {
                            $scope.users = responseUser.data.Data;

                        });

                })

        }

        
    }

    $scope.GetPage();
    
 


    $scope.postdata = function (PageId, Img, ArtistName, Title, Technique, Size, DateYear, Signature, Note) {

        
        var data = {
            Id : $scope.Id,
            PageId: PageId,
            UserId: $scope.EditUserIdSelected,
            Img: Img,
            ArtistName: ArtistName,
            Title: Title,
            Technique: Technique,
            Size: Size,
            DateYear: DateYear,
            Signature: Signature,
            Note: Note
        };

        PageAdminService.UpdatePage(data,

            $scope.files,

            function success(response) {


                if (response.IsSuccess) {

                    document.getElementById('btnDisabledUpdate').disabled = 'disabled';
                    showMessage('success', null, 'Başarıyla Kaydedildi!');
                    $window.location.href = '/Admin/PageAdmin/';
                } else {


                    /*   showMessage('warning', null, 'Kaydedilmedi!');*/
                    alert('Kaydetmedi')

                }

            },

            function error() {
                alert('Hata')
              /*  showMessage('danger', null, 'Hata oluştu!');*/

            });
        //$http.post('/Page/UpdatePage/', JSON.stringify(data)).then(function (response) {
        //    if (response.data)
        //        showMessage('success', null, 'Post Data Submitted Successfully!');
        //    document.getElementById('btnDisabledUpdate').disabled = 'disabled';
        //    $scope.RedirectToUrl = function () {
        //        setTimeout(function () { $window.location.href = '/Admin/PageAdmin/' }, 2000);
        //    }
        //    $scope.RedirectToUrl();
        //}, function (response) {
        //    showMessage('warning', null, 'Service not Exists');
        //});

    };

}]);

ProjectApp.controller("GetAllPageAdminCTRL", function ($scope, $http, NgTableParams, $window) {


    $scope.GoEditDetail = function (id) {
        window.location.href = '/Admin/EditPage/' + id;
    }



    $scope.GetAllPagesWithUsername = function () {

        $http.get("/Page/GetAllPagesWithUsername")
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


    $scope.GetAllPagesWithUsername();
    



    $scope.deletedata = function (event) {
        var data = {
            Id: $(event.target).attr("data-id")
        };

        swal({
            title: "Are you sure?",
            text: "Are you sure delete this page?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {

                    $http.post('/Page/DeletePage/' + data.Id, JSON.stringify(data)).then(function (response) {
                        if (response.data) {
                            showMessage('success', null, 'Data Deleted Successfully!');
                            $scope.RedirectToUrl = function () {
                                setTimeout(function () { location.href = '/Admin/PageAdmin' }, 2000);
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




    $scope.PageId = null;

    $scope.postdata = function (PageId) {
        
        var data = {
            PageId: PageId
        };

        $http.post('/Page/CreatePage/', JSON.stringify(data)).then(function (response) {
            if (response.data)
                showMessage('success', null, 'Post Data Submitted Successfully!');
            document.getElementById('btnDisabled').disabled = 'disabled';
           
            $scope.RedirectToUrl = function () {
                setTimeout(function () { $window.location.href = '/Admin/PageAdmin/' }, 2000);
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

