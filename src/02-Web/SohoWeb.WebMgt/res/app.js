/**
 * Created by Jeanma on 14-5-5.
 */
define(window["appConfig"].angularModualJS, function (angularAMD) {
    var cfg = window["appConfig"];
    var app = angular.module("app", cfg.angularModualNames);

	if(appConfig.unitTest){
		app._angularAMD_=angularAMD;
	}
	
    //config $N
    app.run(function ($N) {
        $N.showLoading = cfg.showLoading;
        if (cfg.loadingDom) {
            $N.dom = cfg.loadingDom;
        }
        if (cfg.loadingDelay) {
            $N.loadingDelay = cfg.loadingDelay;
        }
    });

    //config url route
    //#region 静态配置路由
    var routeOps = window["appRouteUrl"];
    app.config(["$routeProvider", function ($routeProvider) {
        angular.forEach(routeOps, function (val) {
            if (val.redirectTo) {
                $routeProvider.otherwise(val);
            }
            else {
                var routeUrl = val.routeUrl;
                delete val.routeUrl;
                $routeProvider.when(routeUrl, angularAMD.route(val));
            }
        });
    }]);
    //#endregion


    //#region 动态路由
    //动态routing,目前已经禁用了,由于功能还不成熟,暂停使用
    //需要做的东西::controller/:view/:params
    //1.动态去加载对应的view
    //2.动态去加载对应的controller
    //3.处理参数
    //modify angularAMD line angularAMD.prototype.route, line 108~111

    //#endregion
    //function getController(url) {
    //    var index = url.indexOf("#");
    //    var sub = url.substring(index + 1);
    //    var strs = sub.split("/");
    //    if (strs.length > 1) {
    //        var ctlName = strs[1] + "Controller";
    //        console.info("controller:" + ctlName);
    //        return ctlName;
    //    }
    //    throw new Error("controller不存在");
    //}
    //app.config(["$routeProvider", function ($routeProvider, $routeParams) {
    //    $routeProvider.
    //        when("/:controller/:view", angularAMD.route({
    //            templateUrl: function ($routeParams) {
    //                var view = $routeParams.view;
    //                if (view) {
    //                    view = view.replace("-", "/");
    //                    return appConfig.viewBasePath + view + ".html";
    //                }
    //                throw new Error("view不存在");
    //            },
    //            controller: function () {
    //                console.info("get controller");
    //                return getController(window.location.href);
    //            }
    //        })).
    //        when("/:controller", angularAMD.route({
    //            templateUrl: function ($routeParams) {
    //                var view = $routeParams.controller
    //                if (view) {
    //                    return appConfig.viewBasePath + view + ".html";
    //                }
    //                throw new Error("view不存在");
    //            },
    //            controller: function () {
    //                console.info("get controller");
    //                return getController(window.location.href);
    //            }
    //        })).
    //        otherwise({redirectTo:appConfig.index});
    //}]);


    //interceptor http
    app.factory("httpInterceptor", ["$N","$q", function ($N,$q) {
        return {
            'request': function (config) {
                //处理自定义headers
                config.headers["x-soho-app-id"] = appConfig.appId || 'ed1ff821c83e4aeb8c142ec08871361f';
                //处理loading
                $N.loading(config);
                return config || $q.when(config);
            },
            'response': function (response) {
                //处理自定义的headers
                //处理loaded
                $N.loaded(response);

                if (angular.isObject(response)) {
                    if (angular.isObject(response.data)) {
                        if (response.data.Success === false) {
                            switch (response.data.Code) {
                                //not login
                                case 1000000:
                                    $N.goto(appConfig.login || "");
                                    break;
                                default:
                                    alert(response.data.Message);
                            }
                            throw new Error(response.data.Message);
                        }
                        else {
                            response.data = response.data.Data;
                            return response;
                        }
                    }
                }

                return response || $q.when(response);
            }
        };
    }]);

    //http provider
    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push("httpInterceptor");
        //$httpProvider.defaults.headers.post["Content-Type"] = "application/x-www-form-urlencoded";
    });

	
	//_baseController
	if(appConfig.parentController){
	    app.controller("_parentController", appConfig.parentController);
	}
	
    //start
    angularAMD.bootstrap(app);
	
    return app;
});
//javascript 1.8 及以上的功能 在移动端部分浏览器没有此功能
if (!Function.prototype.bind) {
    Function.prototype.bind = function (oThis) {
        if (typeof this !== "function") {
            // closest thing possible to the ECMAScript 5 internal IsCallable function
            throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
        }

        var aArgs = Array.prototype.slice.call(arguments, 1),
            fToBind = this,
            fNOP = function () {
            },
            fBound = function () {
                return fToBind.apply(this instanceof fNOP && oThis
                    ? this
                    : oThis,
                    aArgs.concat(Array.prototype.slice.call(arguments)));
            };

        fNOP.prototype = this.prototype;
        fBound.prototype = new fNOP();

        return fBound;
    };
}

/*
 * 开发directive的命名规则,如:定义是的名字为myPager,使用时的名字为my-pager. 潜规则
 * */
angular.module("NProvider", ["ng"]).
    directive("myPager", function ($timeout) {
        return {
            require: "ngModel",
            restrict: "ACE",
            link: function (scope, element, attrs, controller) {
                var pageInfo = scope[attrs.ngModel];

                var first = false;
                if (attrs["firstLoad"]) {
                    first = angular.uppercase(attrs["firstLoad"]) === "TRUE";
                }
                var btnPre = element.find(".prev");
                var btnNext = element.find(".next");
                var timer = null;

                function setViewValue(value) {
                    console.info("set new value");
                    if (controller) {
                        if (value.index < 1) value.index = 1;
                        if (value.index > value.totalPage) value.index = value.totalPage;

                        controller.$setViewValue(value);
                        scope.$apply();
                    }
                }

                function render(value) {
                    console.info("render");
                    if (value.index <= 1) btnPre.attr("disabled", "");
                    else btnPre.removeAttr("disabled");
                    if (value.index >= value.totalPage) btnNext.attr("disabled", "");
                    else btnNext.removeAttr("disabled");
                    if (controller) {
                        console.info("controller render");
                        controller.$render();
                    }
                }

                function pageChange(value) {
                    console.info("page change");
                    first = false;

                    var promise = value.change();
                    if (promise && promise["finally"]) {
                        promise["finally"](function () {
                            console.info("resolve page change");
                            value.pages = [];
                            for (var i = 1; i <= value.totalPage; i++) value.pages.push(i);
                            value.pages.push("...");
                        });
                    }
                }

                function prev() {
                    if (angular.element(this).attr("disabled")) return false;
                    pageInfo.index--;
                    setViewValue(pageInfo);
                    return false;
                }

                function next() {
                    if (angular.element(this).attr("disabled")) return false;
                    pageInfo.index++;
                    setViewValue(pageInfo);
                    return false;
                }

                btnPre.attr("disabled", "").bind("click", prev);
                btnNext.attr("disabled", "").bind("click", next);

                scope.$watch(function (s) {
                    return s[attrs.ngModel];
                }, function (newVal, oldVal) {
                    console.info(newVal);
                    console.info(oldVal);
                    console.info("data change");
                    if (timer) clearTimeout(timer);
                    timer = setTimeout(function () {
                        //index,size changed or first=true
                        if (newVal.index !== oldVal.index
                            || newVal.size !== oldVal.size
                            || first) {
                            pageChange(newVal);
                        }
                        render(newVal);
                    }, 500);
                }, true);
            }
        };
    }).
    provider("$N", function () {
        function N() {
            this.showLoading = false;
            this.dom = null;
            this.timeout = null;
            this.width = null;
            this.loadingDelay = 500;
        };
        N.prototype = {
            loading: function (config) {
                if (this.showLoading) {
                    if (!this.dom) {
                        this.dom = document.createElement("div");
                        this.dom.innerHTML = "<div class='circle'></div><div class='circle1'></div>";
                        this.dom.setAttribute("class", "n-loading")

                        document.body.appendChild(this.dom);
                    }
                    if (this.timeout) {
                        clearTimeout(this.timeout);
                        this.timeout = null;
                    }
                    angular.element(this.dom).addClass("loading-running");
                }
            },
            loaded: function (response) {
                if (this.showLoading) {
                    if (!this.width) {
                        if (window.getComputedStyle)
                            this.width = parseInt(window.getComputedStyle(this.dom).width);
                        else if (this.dom.currentStyle) {
                            this.width = parseInt(this.dom.currentStyle.width);
                        }
                    }

                    function hideLoading() {
                        //this.dom.style.right = "-" + this.width + "px";
                        angular.element(this.dom).removeClass("loading-running");
                    };
                    this.timeout = setTimeout(hideLoading.bind(this), this.loadingDelay);
                }
            },
            goto: function (url) {
                if (url.indexOf("#") > 0) { }
                else {
                    window.location.href = url;
                }
            }
        };
        this.$get = function () {
            return new N();
        };
    });

(function () {
    if (!window["N"]) window["N"] = {};
    //分页实体
    function pager(index, size, onChange) {
        this.index = index || 1;
        this.size = size || 0;
        this.change = onChange || angular.noop();
        this.total = 0;
        this.pages = [];
        this.totalPage = 0;
    }

    pager.prototype = {
        setTotal: function (t) {
            this.total = t;
            if (this.size > 0) this.totalPage = t / this.size;
            return this;
        },
        setSize: function (s) {
            this.size = s;
            return this;
        },
        goto: function (index, current, evt) {
            index = parseInt(index);
            if (isNaN(index)) return;
            if (index < 1) return;
            if (index > this.totalPage) return;
            if (index === this.index) return;
            this.index = index;
            this.change();
        }
    };
    window["N"]["Pager"] = pager;
})();
