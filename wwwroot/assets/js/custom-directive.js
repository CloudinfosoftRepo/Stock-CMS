
angular.module('Stock-CMS')

    // Define 'positive' directive
    .directive('positive', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, ngModel) {
                ngModel.$validators.positive = function (modelValue, viewValue) {
                    return modelValue == undefined || modelValue > 0;
                };
            }
        };
    })

    // Define 'contactNo' directive
    .directive('contactNo', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, ngModel) {
                ngModel.$validators.contactNo = function (modelValue) {
                    var value = modelValue;

                    // Regex to allow only digits and ensure exactly 10 digits
                    var regex = /^[0-9]{10}$/;

                    if (value == null || value == undefined) {
                        return true;
                    } else if (regex.test(value)) {
                        return true;
                    } else {
                        return false;
                    }
                };
            }
        };
    })

    // Define 'dynamicFilter' filter
    .filter('dynamicFilter', function () {
        return function (inputArray, searchQuery, columnNames) {
            if (!searchQuery || !columnNames || columnNames.length === 0) {
                return inputArray;
            }

            return inputArray.filter(function (item) {
                // Iterate over the specified column names to match the search query
                return columnNames.some(function (column) {
                    if (item[column]) {
                        return item[column].toString().toLowerCase().includes(searchQuery.toLowerCase());
                    }
                    return false;
                });
            });
        };
    })

    //excel export
    .directive('excelExport', function () {
        return {
            restrict: 'A',  // Attribute-based directive
            scope: {
                tableId: '@',    // The ID of the table to export
                fileName: '@',   // The name of the exported Excel file
                sheetName: '@',  // The name of the sheet inside the Excel file
                fileType: '@'    // File type (e.g., 'xlsx')
            },
            link: function (scope, element) {
                element.on('click', function () {
                    var tableId = scope.tableId || 'table';  // Default table ID
                    var fileName = scope.fileName || 'export';
                    var sheetName = scope.sheetName || 'Sheet1';
                    var fileType = scope.fileType || 'xlsx';

                    var tableElement = document.getElementById(tableId);
                    if (tableElement) {
                        var excelFile = XLSX.utils.table_to_book(tableElement, { sheet: sheetName });
                        XLSX.writeFile(excelFile, fileName + '.' + fileType);
                    } else {
                        console.error('Table element not found: ' + tableId);
                    }
                });
            }
        };
    })

    //Validating File Inputs takes file-model parameter
    .directive('fileModel', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                var model = $parse(attrs.fileModel);
                var modelSetter = model.assign;

                element.bind('change', function () {
                    scope.$apply(function () {
                        var file = element[0].files[0];
                        modelSetter(scope, file);
                        // Set validity dynamically for the field
                        ngModelCtrl.$setValidity('required', !!file);
                    });
                });
            }
        };
    }]);

