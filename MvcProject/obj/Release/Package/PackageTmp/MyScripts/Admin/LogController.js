ProjectApp.controller("GetAllLogCTRL", function ($scope, $http, NgTableParams, $window) {





    $scope.GetAllEditorEditLogs = function () {
        $http.get("/Log/GetAllEditorEditLogs")
            .then(function (response) {
                var data = response.data.Data;
                console.log(data)

                $scope.tableParams = new NgTableParams({
                    page: 1,
                    count: 10
                }, { dataset: data });

            });

    }
    $scope.GetAllEditorEditLogs();


});