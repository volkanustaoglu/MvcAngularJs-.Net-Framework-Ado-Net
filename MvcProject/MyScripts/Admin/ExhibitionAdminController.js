ProjectApp.controller("GetAllExhibitionAdminCTRL", function ($scope, $http, NgTableParams, $window) {


    $scope.GoEditDetail = function (id) {
        window.location.href = '/Admin/EditExhibition/' + id;
    }



    $scope.GetAllExhibitions = function () {

        $http.get("/Exhibition/GetAllExhibitions")
            .then(function (response) {
                $scope.GetAllExhibitions = response.data.Data;

                console.log($scope.GetAllExhibitions)

                var data = $scope.GetAllExhibitions;


                $scope.tableParams = new NgTableParams({}, { dataset: data });
            });
    }


    $scope.GetAllExhibitions();

});