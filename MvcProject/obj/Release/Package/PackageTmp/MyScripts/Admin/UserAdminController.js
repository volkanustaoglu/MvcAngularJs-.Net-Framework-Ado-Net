ProjectApp.config([
    '$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        }).hashPrefix('!');
        $routeProvider
            .when("/Admin/EditUser/:id/", {
                templateUrl: "/AngularRouting/EditUserDetails.html",
                controller: "EditUserDetailsCTRL"
            });
    }]);


ProjectApp.controller('EditUserDetailsCTRL', ['$scope', '$routeParams', '$http', '$sce','$window', function ($scope, $routeParams, $http, $sce,$window) {

    var dialCodeValue = [];
 
    
    let id = $routeParams.id;

    $scope.GetUser = function () {

    

        if (id === undefined) {

            return 'Undefined value!';

        }

        else {

            $http.get("/User/GetUser/" + id)
            
                .then(function (response) {
                    $scope.Username = response.data.Data[0].Username;
                    $scope.Email = response.data.Data[0].Email;
                    $scope.Role = response.data.Data[0].Role;
                    $scope.NameSurname = response.data.Data[0].NameSurname;
                    $scope.Address = response.data.Data[0].Address;
                    $scope.IdentityNumber = response.data.Data[0].IdentityNumber;
                    $scope.ConfirmEmail = response.data.Data[0].ConfirmEmail;
                    $scope.ConfirmPhone = response.data.Data[0].ConfirmPhone;
                    $scope.PhoneNumber = response.data.Data[0].PhoneNumber;
                    $scope.DialCode = response.data.Data[0].DialCode;
                    $scope.TaxAdministration = response.data.Data[0].TaxAdministration;
                    $scope.TaxNumber = response.data.Data[0].TaxNumber;
                    $scope.Id = response.data.Data[0].Id;
                    $scope.DialCodeIso = response.data.Data[0].DialCodeIso;

               
            


                  
                })
            var input = document.querySelector("#phone");
            dialCodeValue = window.intlTelInput(input, {
                separateDialCode: true,

                preferredCountries: ["tr"],
                initialCountry: "auto",



               

                geoIpLookup: function (callback) {
                    $.get("/User/GetUser/" + id, function () { }, "jsonp").always(function (resp) {
                        
                        var JsonParse = JSON.parse(resp.responseText);
                        var countryCode = (JsonParse.Data[0] && JsonParse.Data[0].DialCode) ? JsonParse.Data[0].DialCode : "90";
                  
                        var countryCodeDial = Number(countryCode);
                        var mergePhoneNumber = "+" + JsonParse.Data[0].DialCode + JsonParse.Data[0].PhoneNumber;

                        dialCodeValue.setNumber(mergePhoneNumber);
                     
                        callback(dialCodeValue.countryCodes[countryCodeDial][0]);
                    });
                },
            

                utilsScript: "~/MetronicAdmin/assets/build/js/utils.js",
            });

            //geoIpLookup: function (success, failure) {
            //    $.get("https://ipinfo.io?token=360bf8f04cdd54", function () { }, "jsonp").always(function (resp) {
            //        var countryCode = (resp && resp.country) ? resp.country : "";
            //        console.log(resp)
            //        success(countryCode);
            //    });
            //},
 
           
        }
    
     
        
    }

    $scope.GetUser();



    $scope.postdata = function (Username, Email, Password, Role, NameSurname, Address, IdentityNumber, ConfirmEmail, ConfirmPhone, TaxAdministration, TaxNumber) {

        var getPhoneNumber = document.getElementById("phone").value

        var DialCode = dialCodeValue.selectedCountryData.dialCode
        var DialCodeIso = dialCodeValue.selectedCountryData.iso2;

        var data = {
            Id: $scope.Id,
            Username: Username,
            Email: Email,
            Role: Role,
            Password: Password,
            NameSurname: NameSurname,
            Address: Address,
            IdentityNumber: IdentityNumber,
            ConfirmEmail: ConfirmEmail,
            ConfirmPhone: ConfirmPhone,
            PhoneNumber: getPhoneNumber,
            TaxAdministration: TaxAdministration,
            TaxNumber: TaxNumber,
            DialCode: DialCode,
            DialCodeIso: DialCodeIso
        };
  
        $http.post('/User/UpdateUser/', JSON.stringify(data)).then(function (response) {
            if (response.data)
                showMessage('success', null, 'Post Data Submitted Successfully!');
            document.getElementById('btnDisabledUpdate').disabled = 'disabled';
            $scope.RedirectToUrl = function () {
                setTimeout(function () { $window.location.href = '/Admin/UserAdmin/' }, 2000);
            }
            $scope.RedirectToUrl();
        }, function (response) {
            showMessage('warning', null, 'Service not Exists');
        });

    };

}]);

ProjectApp.controller("GetAllUserAdminCTRL", function ($scope, $http, NgTableParams, $window) {


    $scope.GoEditDetail = function (id) {
        window.location.href = '/Admin/EditUser/' + id;
    }


    $scope.GetAllUsers = function () {
        $http.get("/User/GetAllUsers")
            .then(function (response) {
                var data = response.data.Data;
              
                $scope.tableParams = new NgTableParams({
                    page: 1,
                    count: 10}, { dataset: data });

            });

    }
    $scope.GetAllUsers();



    $scope.deletedata = function (event) {
        var data = {
            Id: $(event.target).attr("data-id")
        };

        swal({
            title: "Are you sure?",
            text: "Are you sure delete this user?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {

                    $http.post('/User/DeleteUser/' + data.Id, JSON.stringify(data)).then(function (response) {
                        if (response.data) {
                            showMessage('success', null, 'Data Deleted Successfully!');
                            $scope.RedirectToUrl = function () {
                                setTimeout(function () { location.href = '/Admin/UserAdmin' }, 2000);
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


 

    function PhoneIsValid(p) {

        var phoneRe = /^[2-9]\d{2}[2-9]\d{2}\d{4}$/;

        var digits = p.replace(/\D/g, "");

        return phoneRe.test(digits);

    }

    function validateEmail(Email) {
        var re = /\S+@\S+\.\S+/;
        return re.test(Email);
    }

    $scope.nameSurnameRegisterSituation = false;
    $scope.emailRegisterSituation = false;
    $scope.phoneRegisterSituation = false;
    $scope.usernameRegisterSituation = false;
    $scope.passwordRegisterSituation = false;

    $scope.checkRegisterNameSurname = function () {
        $scope.nameSurnameRegisterSituation = false;

    };
    $scope.checkRegisterEmail = function () {
        $scope.emailRegisterSituation = false;
    }
    $scope.checkRegisterPhone = function () {
        $scope.phoneRegisterSituation = false;
    }
    $scope.checkRegisterUsername = function () {
        $scope.usernameRegisterSituation = false;
    }
    $scope.checkRegisterPassword = function () {
        $scope.passwordRegisterSituation = false;
    }

    $scope.postdata = function (Username, Email, Password, NameSurname, Address, IdentityNumber, PhoneNumber, TaxAdministration, TaxNumber) {

    


        if (NameSurname == null || NameSurname == undefined || NameSurname === "") {
            $scope.nameSurnameRegisterSituation = true;
        }
        if (Email == null || Email == undefined || Email === "" || validateEmail(Email) == false) {
            $scope.emailRegisterSituation = true;
        }

        if (PhoneNumber == null || PhoneNumber == undefined || PhoneNumber === "" || PhoneIsValid(PhoneNumber) == false || PhoneNumber.indexOf('-') !== -1) {
            $scope.phoneRegisterSituation = true;
        }
        if (Username == null || Username == undefined || Username === "") {
            $scope.usernameRegisterSituation = true;
        }
        if (Password == null || Password == undefined || Password === "") {
            $scope.passwordRegisterSituation = true;
        }
        var DialCode = dialCodeValue.selectedCountryData.dialCode
        var DialCodeIso = dialCodeValue.selectedCountryData.iso2;

        var data = {
            Username: Username,
            Email: Email,
            Password: Password,
            NameSurname: NameSurname,
            Address: Address,
            IdentityNumber: IdentityNumber,
            PhoneNumber: PhoneNumber,
            DialCode: DialCode,
            DialCodeIso: DialCodeIso,
            TaxAdministration: TaxAdministration,
            TaxNumber: TaxNumber
        };
      

        if ($scope.emailRegisterSituation !== true && $scope.phoneRegisterSituation !== true && $scope.nameSurnameRegisterSituation !== true && $scope.usernameRegisterSituation !== true && $scope.passwordRegisterSituation !== true) {
            $http.post('/User/CreateUser/', JSON.stringify(data)).then(function (response) {
                if (response.data)
                    showMessage('success', null, 'Post Data Submitted Successfully!');
                document.getElementById('btnDisabled').disabled = 'disabled';
                $scope.RedirectToUrl = function () {
                    setTimeout(function () { $window.location.href = '/Admin/UserAdmin/' }, 2000);
                }
                $scope.RedirectToUrl();
            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
          
        }

        

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

