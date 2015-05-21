var orderApp = angular.module("ordersApp", []);
$scope.orders = [];
$scope.newOrder={};
orderApp.controller('OrderContorller', function ($scope, $http) {

    $(function () {
//посылаем запрос api контроллеру, который возвращает заказы из файла
        getOrders();
        $scope.summary = true;
        $scope.createblock = false;
    });

    function getOrders() {
        $http.get('/api/order/GetAll').success(function (orders) {
            $scope.orders = orders;
        });
    };

    //добавление заказа
    $scope.save = function (editForm) {
        if (editForm.$valid) {
            $http.post('/api/order/Add', $scope.newOrder).success(function () {
                //обновляем
                getOrders();
                //показываем блок с заказами
                $scope.summary = true;
                $scope.createblock = false;
                $scope.newOrder.Name = "";
                $scope.newOrder.Count = "";
                $scope.newOrder.Address = "";
            });
        }
    };
    //кнопка "Создать заказ"
    $scope.create = function () {
        //показываем блок с формой добавления заказа
        $scope.summary = false;
        $scope.createblock = true;
    }

    //удаление
    $scope.delete = function(id) {
        $http.post('/api/order/Delete', {}, {params: {id: id}}).success(function () {
            getOrders();
        });
    }
    

}
);
