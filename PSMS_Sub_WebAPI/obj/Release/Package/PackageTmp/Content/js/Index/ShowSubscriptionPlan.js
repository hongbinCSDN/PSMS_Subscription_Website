Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        Multilingual: [],
        PromptMessage: [],
        Navigation: [],
        DropDownLanguage: [],
        Language_Value: "2",
        Token: '',
        Subscription: {},
        Status: [],
        IsRefresh: false,
        Ref: '',
        payment: {},
        UserInfo: {}  //Modify by bill 2018-6-17
    },
    mounted: function () {
        this.LoadMultilingual()           
    },
    filters: {
        FormatDate: function (time) {
            var date = new Date(time);
            var fmt = 'yyyy/MM/dd hh : mm';
            if (/(y+)/.test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (date.getFullYear() + '').substr(4 - RegExp.$1.length));
            }
            let o = {
                'M+': date.getMonth() + 1,
                'd+': date.getDate(),
                'h+': date.getHours(),
                'm+': date.getMinutes(),
                's+': date.getSeconds()
            };
            for (let k in o) {
                if (new RegExp('(' + k + ')').test(fmt)) {
                    let str = o[k] + '';
                    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? str : (('00' + str).substr(str.length)));
                }
            }
            return fmt;
        },
    },
    methods: {
        GetToken: function () {
            var api = GetCommonAPI_IE('System/GetToken');
            this.$http.get(api).then(function (response) {
                this.Token = response.body.TOKEN;  //Modify by bill 2018-6-17             
                if (this.Token != null && this.Token != "") {
                    this.LoadSubscription();
                    this.AutoCheckStatus();
                    this.$refs.logining.style.display = 'none';
                    this.$refs.signuping.style.display = 'none';
                    this.$refs.logouting.style.display = 'block';
                    this.$refs.personal.style.display = 'block';
                    //Modify by bill 2018-6-17  
                    this.UserInfo = response.body;
                    this.$refs.Portrait.style.display = 'block';
                    //End
                }
                else {
                    this.$refs.logining.style.display = 'block';
                    this.$refs.signuping.style.display = 'block';
                    WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            });
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
            var api = LoadMultilingual_API_IE("ShowSubscriptionPlan")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.GetToken();
            }, function (response) { console.log(response); });
        },
        ChangeLan: function () {
            var api = ChangeLan_API_IE("ShowSubscriptionPlan")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.LoadSubscription();

            }, function (response) { console.log(response); });
        },
        LoadSubscription: function () {
            this.Ref = this.GetQueryString('Ref');
            if (this.Ref != null && this.Ref != '') {
                var api = GetCommonAPI_IE("System/GetSubscriptionDetail");
                this.$http.get(api, { params: { "orderref": this.Ref }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {                    
                    this.Subscription = response.body.Table[0];
                    this.CheckStatus();                   
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal(); 
                })
            }
        },
        GetQueryString: function (name) {
            var queryString = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (queryString == null || queryString.length < 1) {
                return "";
            }
            return queryString[1];
        },
        CheckStatus: function () {
            var api = GetCommonAPI_IE("System/CheckSubscriptionStatus");
            this.$http.get(api, { params: { "orderref": this.Ref }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                this.Status = response.body;
                this.IsRefresh = true;
                var refresh_Timeout = setTimeout(function () {
                    vm.IsRefresh = false;
                }, 1000);
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal(); 
            })
        },
        AutoCheckStatus: function () {
            clearInterval(ini);
            var ini=setInterval(function () {
                vm.CheckStatus();
            }, 60000)
        },
        //Add by bill 2018.8.31
        CheckIsCanRenew: function () {
            var api = GetCommonAPI("Payment/CheckRenew");
            this.$http.get(api, { params: { orederref: this.Ref } ,headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                console.log(response);
                if (response.body == "1") {
                    this.Renewal();
                }
                else {
                    WarningMessage('WarningMessage', this.PromptMessage[33].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal();
            });

        },
        Renewal: function () {
            WarningMessageAddbutton2("WarningMessage", this.Multilingual[24].Describe, this.Subscription.DOMAIN_NAME + ".pssasl.com", this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, this.Subscription.PAYMENT_TYPE_ID, this.Subscription.DOMAIN_NAME);
            $("#myModal").modal();       
        },    
        PaymentSubmit: function (category, domain_name) {
            if (this.Token != null && this.Token != "") {
                    var form = document.getElementById('payFormCcard');               
                    var api = GetCommonAPI_IE("Payment/GetProductPostForm");
                    this.$http.get(api, { params: { category: category }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                        var data = response.body.Table[0];
                        this.$refs.merchantId.value = data.MERCHANTID;
                        this.$refs.amount.value = data.AMOUNT;
                        this.$refs.orderRef.value = data.ORDERREF;
                        this.$refs.currCode.value = data.CURRCODE;
                        this.$refs.mpsMode.value = data.MPSMODE;
                        this.$refs.successUrl.value = data.SUCCESSURL;
                        this.$refs.failUrl.value = data.FAILURL;
                        this.$refs.cancelUrl.value = data.CANCELURL;
                        this.$refs.payType.value = data.PAYTYPE;
                        this.$refs.lang.value = data.LANG;
                        this.$refs.payMethod.value = data.PAYMETHOD;
                        this.$refs.secureHash.value = data.SECUREHASH;
                        this.CreateRenewalOrder(data.ORDERREF, data.AMOUNT, category, domain_name);
                        form.submit();
                    }, function (response) {
                        WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                        $("#myModal").modal();
                    });               
                
            }
            else {
                WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                $("#myModal").modal();
            }
        },
        CreateRenewalOrder: function (orderRef, amount, category, domain_name) {
            var api = GetCommonAPI('Payment/CreateOrder');
            this.payment.ORDERREF = orderRef;
            this.payment.AMOUNT = amount;
            this.payment.PAYMENT_TYPE_ID = category;
            this.payment.DOMAIN_NAME = domain_name;
            this.payment.RENEWAL_LAST_ORDERREF = this.Ref;
            this.$http.post(api, this.payment, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {             
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal();
            });
        }  //End

    }
});

$(function () { $("[data-toggle='popover']").popover(); });