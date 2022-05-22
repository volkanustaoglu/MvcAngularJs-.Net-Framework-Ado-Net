ProjectApp.controller("KullaniciBilgilerimCTRL", function ($scope, $http, NgTableParams, $window) {




    $scope.GetUserForEdit = function () {
        $http.get("/User/GetUserForEdit")
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

            });

        var input = document.querySelector("#phone");
        dialCodeValue = window.intlTelInput(input, {
            separateDialCode: true,

            preferredCountries: ["tr"],
            initialCountry: "auto",





            geoIpLookup: function (callback) {
                $.get("/User/GetUserForEdit/", function () { }, "json").always(function (resp) {

                  
                  
                    var countryCode = (resp.Data[0] && resp.Data[0].DialCode) ? resp.Data[0].DialCode : "90";

                    var countryCodeDial = Number(countryCode);
                    var mergePhoneNumber = "+" + resp.Data[0].DialCode + resp.Data[0].PhoneNumber;

                    dialCodeValue.setNumber(mergePhoneNumber);

                    callback(dialCodeValue.countryCodes[countryCodeDial][0]);
                });
            },


            utilsScript: "~/MetronicAdmin/assets/build/js/utils.js",
        });





    }
    $scope.GetUserForEdit();

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
    var usernameValidation = document.getElementById("username-error");
    usernameValidation.innerHTML = "Kullanıcı Adı Boş Bırakılamaz.";
    var emailValidation = document.getElementById("email2-error");
    emailValidation.innerHTML = "Email Adresi Boş Ya Da Formata Uygun Değil";

    $scope.postdata = function (Username, Email, Password, NameSurname, Address, IdentityNumber, TaxAdministration, TaxNumber) {


         if (NameSurname == null || NameSurname == undefined || NameSurname === "") {
            $scope.nameSurnameRegisterSituation = true;
        }
        if (Email == null || Email == undefined || Email === "" || validateEmail(Email) == false) {
            $scope.emailRegisterSituation = true;
         
        }

        
        if (Username == null || Username == undefined || Username === "") {
            $scope.usernameRegisterSituation = true;
        }
        if (Password == null || Password == undefined || Password === "") {
            $scope.passwordRegisterSituation = true;
        }

        var getPhoneNumber = document.getElementById("phone").value
   


        if (getPhoneNumber == null || getPhoneNumber == undefined || getPhoneNumber === "" || PhoneIsValid(getPhoneNumber) == false || getPhoneNumber.indexOf('-') !== -1) {
            $scope.phoneRegisterSituation = true;
        
        }

        var DialCode = dialCodeValue.selectedCountryData.dialCode
        var DialCodeIso = dialCodeValue.selectedCountryData.iso2;

        var data = {
            Id: $scope.Id,
            Username: Username,
            Email: Email,
            Password: Password,
            NameSurname: NameSurname,
            Address: Address,
            IdentityNumber: IdentityNumber,
            PhoneNumber: getPhoneNumber,
            TaxAdministration: TaxAdministration,
            TaxNumber: TaxNumber,
            DialCode: DialCode,
            DialCodeIso: DialCodeIso
        };

        if (Email != null && Email != undefined && Email != "" && validateEmail(Email) != false &&
            Username != null && Username != undefined && Username != "") {

       
               $http.post('/User/UpdateUser/', JSON.stringify(data)).then(function (response) {
            if (response.data)
                   document.getElementById('btnDisabledUpdate').disabled = 'disabled';
                   $.notify("Başarıyla Kaydedildi!", "success");
                   $scope.RedirectToUrl = function () {
                       setTimeout(function () { $window.location.href = '/KullaniciBilgilerim/' }, 200);
                   }
                   $scope.RedirectToUrl();
                   
         
        }, function (response) {
            $.notify("Hata Oluştu!", "error");
        });


        }
     

    };


});


