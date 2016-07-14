'use strict';
/*global angular */

/*
    This is the controller which contains all the actions which has to be carried out from the UI. The functions which
    are explained here include the getall, create and delete methods
*/
angular.module('mean.toDo')
    .controller('ToDoController', ['$scope', 'Todos',
        function ($scope, Todos) {
            //formData is the data entered in the text field which has to be empty initially
            $scope.formData = {};
            /*
              loading is the variable which contained the Boolean value which indicates which the loading indicator
              should be displayed
            */
            $scope.loading = true;
            /* GET -
               when landing on the page, get all todos and show them
               use the service to get all the todos */
            Todos.get()
                .success(function (data) {
                    //assign the todos which are obtained from the service response to show in the UI
                    $scope.todos = data;
                    //Hide the loading indicator
                    $scope.loading = false;
                });

            /* CREATE -
               when submitting the add form, send the text to the node API */
            $scope.createTodo = function () {

                /* validate the formData to make sure that something is there
                   if form is empty, nothing will happen */
                if ($scope.formData.text !== undefined) {
                    //show the loading indicator when the create operation is called
                    $scope.loading = true;

                    // call the create function from our service (returns a promise object)
                    Todos.create($scope.formData)

                        // if successful creation, call our get function to get all the new todos
                        .success(function (data) {
                            $scope.loading = false;
                            // clear the form so our user is ready to enter another
                            $scope.formData = {};
                            // assign our new list of todos
                            $scope.todos = data;
                        });
                }
            };

            // DELETE
            // delete a todo after checking it
            $scope.deleteTodo = function (id) {
                //show the loading indicator when the delete operation is called
                $scope.loading = true;
                //Call the delete service with the id of the ToDo as input
                Todos.delete(id)
                    // if successful creation, call our get function to get all the new todos
                    .success(function (data) {
                        $scope.loading = false;
                        // assign our new list of todos
                        $scope.todos = data;
                    });
            };
        }]);
