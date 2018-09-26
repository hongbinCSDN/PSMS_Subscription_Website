Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: "#app",
    data: {
        Token: '',
        Multilingual: [],
        PromptMessage: [],
        Navigation: [],
        DropDownLanguage: [],
        Language_Value: '2',
        ProgressPercent: '',
        ProductInfo: {},
        ProductFuncs: [],
        UserInfo: {}  //Modify by bill 2018-6-17
    },
    mounted: function () {
        this.LoadMultilingual()      
    },
    methods: {
        GetToken: function () {
            var api = GetCommonAPI_IE('System/GetToken');
            this.$http.get(api).then(function (response) {
                this.Token = response.body.TOKEN;  //Modify by bill 2018-6-17
                if (this.Token != null && this.Token != "") {
                    this.LoadProductInfo();
                    this.$refs.logining.style.display = 'none';
                    this.$refs.signuping.style.display = 'none';
                    this.$refs.logouting.style.display = 'block';
                    this.$refs.personal.style.display = 'block';
                    //Modify by bill 2018-6-17  
                    this.UserInfo = response.body;
                    this.$refs.Portrait.style.display = 'block';
                    //End
                } else {
                    this.$refs.logining.style.display = 'block';
                    this.$refs.signuping.style.display = 'block';
                    WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            })
        },
        //Add by bill 2018-9-17
        HeadPortraithover: function () {
            //document.getElementById("Portrait").style.border = "1px solid";
            document.getElementById("userinfo").style.display = "";
        },
        HeadPortraithoverout: function () {
            document.getElementById("Portrait").style.border = "none";
            document.getElementById("userinfo").style.display = "none";

        }, // End
        Logout: function () {
            WarningMessageAddbutton('WarningMessage', this.PromptMessage[6].Describe, this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
            $("#myModal").modal();
        },
        RemoveToken: function () {
            var api = GetCommonAPI_IE('System/RemoveToken');
            this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                this.Token = "";
                window.location.href = "../Login/Login.html";
            }, function (response) { console.log(response); });
        },
        LoadMultilingual: function () {
            var api = LoadMultilingual_API("SubscriptedPlan");
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.GetToken();
            }, function (response) {
                console.log(response);
            });
        },
        ChangeLan: function () {
            var api = ChangeLan_API_IE('SubscriptedPlan');
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.LoadProductInfo();
            }, function (response) {
                console.log(response);
            })
        },
        LoadProductInfo: function () {
            if (this.GetQueryString('Ref') != null && this.GetQueryString('Ref') != '') {
                var api = GetCommonAPI_IE("System/GetProductInfo");
                this.$http.get(api, { params: { "orderref": this.GetQueryString('Ref') }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    this.ProductInfo = response.body.Data.Table[0];
                    this.LoadProductFunc();
                    this.LoadProgress();
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal(); 
                })
            }
            else {               
                var api = GetCommonAPI_IE("System/GetProductByUser");            
                this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    this.ProductInfo = response.body.Data.Table[0];
                    //Modify / Add by Chester 2018.08.10
                    if (this.ProductInfo != null && this.ProductInfo != "") {
                        this.LoadProductFunc();
                        this.LoadProgress();
                    } else {                   
                        WarningMessage_Link('WarningMessage', this.PromptMessage[28].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../index/SubscriptionPlan.html'");
                        $("#myModal").modal();
                    }
                    //Modify End
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe,this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal(); 
                });
            }

        },
        LoadProgress: function () {
            if (this.GetQueryString('Ref') != null && this.GetQueryString('Ref') != '') {
                var api = GetCommonAPI_IE('System/GetStatusPercent');
                this.$http.get(api, { params: { "orderref": this.GetQueryString('Ref') }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    this.ProgressPercent = response.body;
                    if (this.ProgressPercent == '100%') {
                        clearTimeout(this);
                    } else {
                        setTimeout(function () {
                           // console.log(vm.ProgressPercent);
                            vm.LoadProgress();
                        }, 5000);
                    };
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal();                                 
                });
            }
            else {
                var api = GetCommonAPI_IE('System/GetStatusPercentByUser');
                this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    this.ProgressPercent = response.body;
                    //console.log(this.ProgressPercent);
                    if (this.ProgressPercent == '100%') {
                        clearTimeout(this);
                    } else {
                        setTimeout(function () {
                            console.log(vm.ProgressPercent);
                            vm.LoadProgress();
                        }, 5000);
                    };
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal();  
                });
            }
        },
        LoadProductFunc: function () {
            var str = this.ProductInfo.FEATURES + '';
            this.ProductFuncs = str.split('.');
        },
        GetQueryString: function (name) {
            var queryString = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (queryString == null || queryString.length < 1) {
                return "";
            }
            return queryString[1];
        }


    }
})

$(function () { $("[data-toggle='popover']").popover(); });
