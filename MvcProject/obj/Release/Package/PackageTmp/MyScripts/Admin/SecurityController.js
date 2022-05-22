ProjectApp.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});




ProjectApp.controller("SecurityCTRL", function ($scope, $http, NgTableParams, $window) {


   


    $scope.usernamePasswordSituation = false;
    $scope.userAuthentication = false;


    $scope.chekUsername = function () {
        $scope.usernamePasswordSituation = false;
        $scope.userAuthentication = false;
    };

    $scope.checkPassword = function () {
        $scope.usernamePasswordSituation = false;
        $scope.userAuthentication = false;
    };





    $scope.Username = null;

    $scope.postdata = function (Username, Password,RememberMe) {


        if (Username == null || Username == undefined || Username === "") {
            $scope.usernamePasswordSituation = true;
        }
        if (Password == null || Password == undefined || Password === "") {
            $scope.usernamePasswordSituation = true;
        }

        if ($scope.usernamePasswordSituation !== true && $scope.usernamePasswordSituation !== true) {
            var data = {
                Username: Username,
                Password: Password,
                RememberMe: RememberMe
            };
            $http.post('/Security/Login/', JSON.stringify(data)).then(function (response) {

                if (response.data == false) {
                    $scope.userAuthentication = true;
                } /*else if (response.data.Message !== "") {*/
                //    showMessage('warning', null, response.data.Message);
                //    console.log(response.data)
                //}
                else {
                    window.location.href = '/';
                }

            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
        }
       

    };


    $scope.nameSurnameRegisterSituation = false;
    $scope.emailRegisterSituation = false;
    $scope.phoneRegisterSituation = false;
    $scope.usernameRegisterSituation = false;
    $scope.passwordRegisterSituation = false;
    $scope.confirmPasswordRegisterSituation = false;

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
    $scope.checkRegisterConfirmPassword = function () {
        $scope.confirmPasswordRegisterSituation = false;
    }

    function PhoneIsValid(p) {

        var phoneRe = /^[2-9]\d{2}[0-9]\d{2}\d{4}$/;

        var digits = p.replace(/\D/g, "");

        return phoneRe.test(digits);

    }



    function validateEmail(Email) {
        var re = /\S+@\S+\.\S+/;
        return re.test(Email);
    }
    var loginValidation = document.getElementById("username-error-ux");
    loginValidation.innerHTML = "Kullanıcı Adı veya Şifre boş bırakılamaz.";
    var loginValidation2 = document.getElementById("username-error-uxEmpty");
    loginValidation2.innerHTML = "Kullanıcı Adı veya Şifre Yanlış.";

    var forgetValidation = document.getElementById("email-error-forget");
    forgetValidation.innerHTML = "Email Adresi Boş Ya Da Formata Uygun Değil";

    var namesurnameValidationRegister = document.getElementById("namesurname-error-register");
    namesurnameValidationRegister.innerHTML = "Ad Soyad Boş Bırakılamaz.";
    var emailValidationRegister = document.getElementById("email-error-register");
    emailValidationRegister.innerHTML = "Email Adresi Boş Ya Da Formata Uygun Değil";
    var phoneValidationRegister = document.getElementById("phone-error-register");
    phoneValidationRegister.innerHTML = "Telefon Numarası Boş Ya Da Formata Uygun Değil";
    var usernameValidationRegister = document.getElementById("username-error-register");
    usernameValidationRegister.innerHTML = "Kullanıcı Adı Boş Bırakılamaz.";
    var passwordValidationRegister = document.getElementById("password-error-register");
    passwordValidationRegister.innerHTML = "Şifre Boş Bırakılamaz.";
    var confirmpasswordValidationRegister = document.getElementById("confirmemail-error-register");
    confirmpasswordValidationRegister.innerHTML = "Şifre Tekrarınız uyuşmamaktadır.";

    $scope.checkAllRegister = function (NameSurname, Email, PhoneNumber, Username, Password, ConfirmPassword, PhoneConfirmCode) {


       
        
      
        if (NameSurname == null || NameSurname == undefined || NameSurname === "") {
            $scope.nameSurnameRegisterSituation = true;
        } else {
            namesurnameValidationRegister.innerHTML = "";
            
            
        }
        if (Email == null || Email == undefined || Email === "" || validateEmail(Email) ==false) {
            $scope.emailRegisterSituation = true;
        } else {
            emailValidationRegister.innerHTML = "";
        }
       

        if (PhoneNumber == null || PhoneNumber == undefined || PhoneNumber === "" || PhoneIsValid(PhoneNumber) == false || PhoneNumber.indexOf('-') !== -1) {
            $scope.phoneRegisterSituation = true;
        } else {
            phoneValidationRegister.innerHTML = "";
           
        }
        if (Username == null || Username == undefined || Username === "") {
            $scope.usernameRegisterSituation = true;
        } else {
            usernameValidationRegister.innerHTML = "";
        }

        if (Password == null || Password == undefined || Password === "") {
            $scope.passwordRegisterSituation = true;
        } else {
            passwordValidationRegister.innerHTML = "";
            
        }

        if (Password != ConfirmPassword) {
            $scope.confirmPasswordRegisterSituation = true;
        } else {
            passwordValidationRegister.innerHTML = "";

        }
       
        if ((Username != null && Username != undefined && Username != "") && (Email != null && Email != undefined && Email != "" && validateEmail(Email) != false)) {
            var data = {
                Email: Email,
                Username: Username
            }
            $http.post('/Security/GetUserByEmailForCheck/', JSON.stringify(data)).then(function (response) {

                $scope.emailCheckControl = response.data.Data.length;

            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });


            $http.post('/Security/GetUserByNameForCheck/', JSON.stringify(data)).then(function (response) {

                $scope.usernameCheckControl = response.data.Data.length;

            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
        }
        if (PhoneNumber != null && PhoneNumber != undefined && PhoneNumber != "" && PhoneIsValid(PhoneNumber) != false && PhoneNumber.indexOf('-') == -1 &&
            NameSurname != null && NameSurname != undefined || NameSurname != "" && Email != null && Email != undefined && Email != "" && validateEmail(Email) != false &&
            Username != null && Username != undefined && Username != "" && Password != null && Password != undefined && Password != "") {
            var dataSend = {
                PhoneNumber: PhoneNumber,
                PhoneConfirmCode: PhoneConfirmCode
            }
            $http.post('/Security/SendPhoneMessage/', JSON.stringify(dataSend)).then(function (response) {
                
            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
        }
        
    }





    $scope.registerPostData = function (IdentityNumber, NameSurname, Email, PhoneNumber, Address, Username, Password, ConfirmPassword, PhoneConfirmCode) {

        var EditPhoneNumber = ((PhoneNumber.replace(/\s+/g, '')) - 0);

        

        var DialCode = dialCodeValue.selectedCountryData.dialCode;
        var DialCodeIso = dialCodeValue.selectedCountryData.iso2;
        var data = {
            IdentityNumber: IdentityNumber,
            NameSurname: NameSurname,
            Email: Email,
            PhoneNumber: EditPhoneNumber,
            DialCode: DialCode,
            DialCodeIso: DialCodeIso,
            Address: Address,
            Username: Username,
            Password: Password,
            ConfirmPassword: ConfirmPassword,
            PhoneConfirmCode: PhoneConfirmCode
        };
        if ($scope.nameSurnameRegisterSituation !== true || $scope.emailRegisterSituation !== true || $scope.phoneRegisterSituation !== true || $scope.usernameRegisterSituation !== true || $scope.passwordRegisterSituation !== true || $scope.confirmPasswordRegisterSituation!==true) {
            $http.post('/Security/Register/', JSON.stringify(data)).then(function (response) {
                if (response.data)
                    showMessage('success', null, 'Kaydınız Başarılıyla Tamamlandı!');
                document.getElementById('btnRegisterDisable').disabled = 'disabled';
                $scope.RedirectToUrl = function () {
                    setTimeout(function () { $window.location.href = '/Security/' }, 2000);
                }
                $scope.RedirectToUrl();
            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
        }

     
    }





    $scope.ForgetEmail = function (Email) {

        $scope.emailForgetSituation = false;

        $scope.checkForgetEmail = function () {
            $scope.emailForgetSituation = false;
        }

        if (Email == null || Email == undefined || Email === "" || validateEmail(Email) == false) {
            $scope.emailForgetSituation = true;
        }
        if ($scope.emailForgetSituation !== true) {
            var data = {
                Email: Email
            }

            $http.post('/Security/GetUserByEmail/', JSON.stringify(data)).then(function (response) {
                if (response.data.IsSuccess == true)
                    showMessage('success', null, 'Şifre Sıfırlama Linki Email Adresinize Gönderilmiştir!');
                if (response.data.IsSuccess == false)
                    showMessage('warning', null, response.data.Message);


                $scope.RedirectToUrl = function () {
                    setTimeout(function () { $window.location.href = '/Security/' }, 2000);
                }
                $scope.RedirectToUrl();
            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
        }

    }


});




ProjectApp.controller("ResetCTRL", function ($scope, $http, $window) {

    var passwordValidationReset = document.getElementById("password-error-reset");
    passwordValidationReset.innerHTML = "Şifre Boş Bırakılamaz.";
    var confirmpasswordValidationReset = document.getElementById("confirmemail-error-reset");
    confirmpasswordValidationReset.innerHTML = "Şifre Tekrarınız uyuşmamaktadır.";
   



    var userIdQuery = getParameterByName('userId');
    var tokenQuery = getParameterByName('token');

    $scope.passwordResetSituation = false;
    $scope.confirmPasswordResetSituation = false;

    $scope.checkResetPassword = function () {
        $scope.passwordResetSituation = false;
    }
    $scope.checkResetConfirmPassword = function () {
        $scope.confirmPasswordResetSituation = false;
    }

    $scope.ResetPasswordNow = function (Password, ConfirmPassword) {

        if (Password == null || Password == undefined || Password === "") {
            $scope.passwordResetSituation = true;
        }
        if (Password !== ConfirmPassword) {
            $scope.confirmPasswordResetSituation = true;
        }
        if ($scope.passwordResetSituation !== true && $scope.confirmPasswordResetSituation !== true) {
            var data = {
                Id: userIdQuery,
                UserKey: tokenQuery,
                Password: Password
            }
            $http.post('/Security/ResetPasswordNow/', JSON.stringify(data)).then(function (response) {
                if (response.data)
                    showMessage('success', null, 'Şifreniz Başarıyla Değişti!');
                $scope.RedirectToUrl = function () {
                    setTimeout(function () { $window.location.href = '/Security/' }, 2000);
                }
                $scope.RedirectToUrl();
            }, function (response) {
                showMessage('warning', null, 'Service not Exists');
            });
        }


    }



});

function showMessage(type, message, title) {
    Command: toastr[type](message, title)

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

