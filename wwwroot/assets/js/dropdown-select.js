(function (window, angular) {
    'use strict';

    const apps = angular.module('DropDownSelect', []);

    apps.run([
        '$templateCache',
        function ($templateCache) {
            $templateCache.put(
                'template/dropdownSelect.html',
                [
                    '<div class="ng-dropdown-container" ng-class="{\'disabled\': isDisabled}">',
                    '    <div class="ng-dropdown-text" ng-class="{\'disabled-text\': isDisabled}" ng-click="ToggleDropdown()">',
                    '        <span>{{ddModel[ddLabel] ? ddModel[ddLabel] : "-- Select -- "}}</span>',
                    '    </div>',
                    '    <ul class="ng-dropdown-options" ng-if="!isDisabled">',
                    '        <li class="ng-dropdown-item">',
                    '            <input type="text" class="ng-dropdown-search" ng-model="searchText" ng-disabled="isDisabled">',
                    '        </li>',
                    '        <li class="ng-dropdown-item" ng-click="Clear()" ng-class="{\'selected\':!ddModel[ddLabel]}">',
                    '            -- Select --',
                    '        </li>',
                    '        <li class="ng-dropdown-item" ng-repeat="item in ddData | filter:ddSearchFilter" ',
                    '            ng-click="SelectItem($event, item)" ng-class="{\'selected\': item.ID === ddModel.ID}">',
                    '            {{item[ddLabel]}}',
                    '        </li>',
                    '    </ul>',
                    '</div>'
                ].join('')
            );

        }
    ]);

    apps.directive('dropdownSelect', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    ddModel: '=',
                    ddData: '=',
                    ddLabel: '@',
                    ddChange: '&',
                    ngDisabled: '<' // Bind to the disabled state
                },
                controller: [
                    '$scope',
                    '$element',
                    '$document',
                    'filterFilter',
                    function ($scope, $element, $document, filterFilter) {
                        $scope.searchText = '';
                        $scope.isDisabled = false; // Initialize the disabled state

                        // Watch for changes in ngDisabled
                        $scope.$watch('ngDisabled', function (newValue) {
                            $scope.isDisabled = newValue;
                        });

                        $scope.ddSearchFilter = function (item) {
                            return (
                                !$scope.searchText ||
                                (item[$scope.ddLabel] &&
                                    item[$scope.ddLabel]
                                        .toLowerCase()
                                        .indexOf($scope.searchText.toLowerCase()) > -1)
                            );
                        };

                        $scope.SelectItem = function ($event, item) {
                            if ($scope.isDisabled) return; // Prevent selection if disabled
                            $scope.ddModel = item;
                            $element.removeClass('show');
                            $scope.searchText = '';
                            setTimeout(function () {
                                $scope.$eval($scope.ddChange);
                            }, 100);
                        };

                        $scope.Clear = function () {
                            if ($scope.isDisabled) return; // Prevent clearing if disabled
                            $scope.ddModel = '';
                            $element.removeClass('show');
                            $scope.searchText = '';
                            setTimeout(function () {
                                $scope.$eval($scope.ddChange);
                            }, 100);
                        };

                        $scope.ToggleDropdown = function () {
                            if ($scope.isDisabled) return; // Prevent toggling if disabled
                            $element.toggleClass('show');
                        };

                        $scope.$watch("ddData", function (newValue, oldValue) {
                            if (newValue !== oldValue) {
                                const SelectItem = filterFilter(newValue, $scope.ddModel);
                                if (!SelectItem || SelectItem.length < 1) {
                                    $scope.ddModel = '';
                                }
                            }
                        });

                        const onDocumentMouseUp = function ($event) {
                            if (!$event || !$event.target) {
                                return;
                            }

                            const isDropdownContainer =
                                $element.has($event.target).length === 1;

                            if (!isDropdownContainer) {
                                $element.removeClass('show');
                                $scope.searchText = '';
                            }
                        };

                        $document.on('mouseup', onDocumentMouseUp);

                        $scope.$on('$destroy', function () {
                            $document.off('mouseup', onDocumentMouseUp);
                        });
                    }
                ],
                templateUrl: 'template/dropdownSelect.html'
            };
        }
    ]);
})(window, window.angular);
