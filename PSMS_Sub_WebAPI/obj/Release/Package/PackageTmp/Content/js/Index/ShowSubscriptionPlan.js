
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
        UserInfo: {},  //Modify by bill 2018-6-17
        AutoPayMsg: {},  //Modify by chester 2018-10-22
        AutoPayDetail: [],
        DropDownPayMethod: [],
        IsUpdateCardNumber: false,
        ModalType: "1",
        PayMethodValue: "1",
        CardNum: "",
        HolderName: "",
        ExpireDate: "",
        CardNum_error: "",
        HolderName_error: "",
        ExpireDate_error: "",
        IsSendVerifyEmail: false,
        ResendTime: 120,
        Verifycode: "",
        Verifycode_error: "",
        IsBackdrop: false,
        ECSStatus: {
            run: 'Running',
            start: 'Starting',
            stopping: 'Stopping',
            stopped: 'Stopped'
        },
        RDSStatus: {
            run: 'Running',
            error: 'Abnormal'
        },
        SLBStatus: {
            run: 'active',
            stop: 'inactive'
        },
        DomainStatus: {
            run: 'ENABLE',
            stop: 'DISABLE'
        },
        countdown: null,
        IsHasECS: false,
        IsHasRDS: false,
        IsHasSLB: false,
        IsHasDomain: false
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
        FormatStrDate: function (time) {
            var date = String(time).split(' ')[0];
            var result = date.split('-')[0] + '/' + date.split('-')[1] + '/' + date.split('-')[2];
            return result;
        },
        FormatFrequency: function (fq) {
            var result = 'Monthly';
            if (fq === '1 Day') {
                result = 'Daily';
            }
            return result;
        }
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
                    WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
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
                this.CheckECS();
                this.CheckRDS();
                this.CheckSLB();
                this.CheckDomains();
                this.IsRefresh = true;
                var refresh_Timeout = setTimeout(function () {
                    vm.IsRefresh = false;
                }, 1000);
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal();
            })
        },
        AutoCheckStatus: function () {
            clearInterval(ini);
            var ini = setInterval(function () {
                vm.CheckStatus();
            }, 60000)
        },
        //Add by bill 2018.8.31
        CheckIsCanRenew: function () {
            var api = GetCommonAPI("Payment/CheckRenew");
            this.$http.get(api, { params: { orederref: this.Ref }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                if (response.body == "1") {
                    this.Renewal();
                }
                else {
                    WarningMessage('WarningMessage', this.PromptMessage[33].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
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
                WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal();
            });
        },
        //End
        // Add by Chester 2018.10.25
        AutoPayment: function () {
            var api = GetCommonAPI('Payment/AutoPayment');
            this.$http.get(api, { params: { orderRef: this.Subscription.ORDERREF }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                this.AutoPayMsg = response.body[0];
                this.AutoPayMsg.account = this.FormatAccount(this.AutoPayMsg.account);
                this.IsBackdrop = true;
                $("body").addClass("custom-modal-open");
            }, function (response) { });
        },
        AutoPaymentDetail: function () {
            this.ModalType = '2';
            var api = GetCommonAPI_IE('Payment/AutoPaymentDetail');
            this.$http.get(api, { params: { schPayId: this.AutoPayMsg.mSchPayId }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                this.AutoPayDetail = response.body;
            }, function (response) {

            });
        },
        UpdateCardNumber: function () {
            var api = GetCommonAPI_IE("Payment/AutoPaymentMethod");
            this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                this.IsUpdateCardNumber = true;
                this.DropDownPayMethod = response.body;
            }, function (response) {
                console.log(response);
            });
            laydate.render({
                elem: '#cardCale',
                lang: 'en',
                type: 'month',
                btns: ['confirm']
            });
        },
        CancelUpdate: function () {
            this.IsUpdateCardNumber = false;
            this.CleanError();
        },
        UpdateReturnAutoPay: function () {
            this.IsUpdateCardNumber = false;
            this.CleanError();
        },
        ReturnAutoPayMsg: function () {
            this.ModalType = '1';
        },
        CheckCardNum: function () {
            if (this.CardNum == null || this.CardNum == "") {
                this.CardNum_error = this.PromptMessage[49].Describe;
            } else {
                this.CardNum_error = "";
            }
        },
        CheckHolderName: function () {
            if (this.HolderName == null || this.HolderName == "") {
                this.HolderName_error = this.PromptMessage[50].Describe;
            } else {
                this.HolderName_error = "";
            }
        },
        CheckExpireDate: function () {
            this.ExpireDate = $("#cardCale").val();
            if (this.ExpireDate == null || this.ExpireDate == "") {
                this.ExpireDate_error = this.PromptMessage[51].Describe;
            } else {
                this.ExpireDate_error = "";
            }
        },
        UpdateCardNum: function () {
            this.CheckCardNum();
            this.CheckHolderName();
            this.CheckExpireDate();
            if (this.CardNum == null || this.CardNum == '') {
                return;
            } else if (this.HolderName == null || this.HolderName == '') {
                return;
            } else if (this.ExpireDate == null || this.ExpireDate == '') {
                return;
            }
            else {
                //Add by bill 2018-10-23
                var api = GetCommonAPI("Payment/UpdatePaymentCardInfo");
                var pMethod = "VISA"
                if (this.PayMethodValue == "2") {
                    pMethod = "Master";
                }
                this.$http.get(api, { params: { orderRef: this.Subscription.ORDERREF, mSchPayId: this.AutoPayMsg.mSchPayId, pMethod: pMethod, orderAcct: this.CardNum, holderName: this.HolderName, expireDate: this.ExpireDate }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    if (response.body == "0") {
                        this.IsUpdateCardNumber = false;
                        this.AutoPayment();
                        WarningMessage('WarningMessage', this.PromptMessage[52].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();

                    }
                    else if (response.body == "100") {
                        WarningMessage('WarningMessage', this.PromptMessage[53].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else if (response.body == "200") {
                        WarningMessage('WarningMessage', this.PromptMessage[54].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else {
                        WarningMessage('WarningMessage', this.PromptMessage[55].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                }, function (response) {
                    console.log(response);
                });
            }
        },
        CleanError: function () {
            this.CardNum_error = "";
            this.HolderName_error = "";
            this.ExpireDate_error = "";
        },
        CancelAutoPay: function () {
            this.ModalType = '3';
        },
        CancelVerifyEmail: function () {
            this.ModalType = '1';
            this.Verifycode_error = "";
            this.IsSendVerifyEmail = false;
            this.ResendTime = 120;
            clearInterval(this.countdown);
        },
        GetVerifyCodeEmail: function () {
            var api = GetCommonAPI_IE("System/GetCancelAutoPayVerifyEmail");

            this.IsSendVerifyEmail = true;
            this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                if (response.body === "1") {
                    WarningMessage_checkIcon('WarningMessage', this.PromptMessage[43].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                    clearInterval(this.countdown);
                    this.countdown = setInterval(function () {
                        if (vm.ResendTime > 0) {
                            vm.ResendTime--;
                        }
                        else {
                            vm.IsSendVerifyEmail = false;
                            vm.ResendTime = 120;
                            clearInterval(vm.countdown);
                        }
                    }, 1000);
                } else {
                    this.IsSendVerifyEmail = false;
                    WarningMessage('WarningMessage', this.PromptMessage[61].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $('#myModal').modal();
                }

            }, function (response) {
                console.log(response);
            });
        },
        VerifyAutoPayEmail: function () {
            var api = GetCommonAPI_IE("System/VerifyEmailCode");
            this.CheckVerifycode();
            if (this.Verifycode == null || this.Verifycode == "") {
                return;
            }
            this.$http.post(api, { '': this.Verifycode }, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                var result = response.body;
                if (result === 1) {
                    this.Verifycode_error = "";
                    this.IsSendVerifyEmail = false;
                    this.ResendTime = 120;
                    this.Verifycode = "";
                    clearInterval(this.countdown);

                    this.ModalType = "1";
                    $("#AutoPayMsg_Modal").modal("hide");
                    this.IsBackdrop = false;
                    $("body").removeClass("custom-modal-open");
                    WarningMessageAddbutton('WarningMessage', this.PromptMessage[56].Describe, this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, 'vm.CancelSchPaymentOrder()');
                    $("#myModal").modal();
                } else if (result === -1) {
                    WarningMessage('WarningMessage', this.PromptMessage[41].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                } else {
                    WarningMessage('WarningMessage', this.PromptMessage[5].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            });
        },
        CheckVerifycode: function () {
            if (this.Verifycode == null || this.Verifycode == "") {
                this.Verifycode_error = this.PromptMessage[37].Describe;
            } else {
                this.Verifycode_error = "";
            }
        },
        CloseModal: function () {
            this.IsBackdrop = false;
            $("body").removeClass("custom-modal-open");
            this.Verifycode_error = "";
            this.IsSendVerifyEmail = false;
            this.ResendTime = 120;
            this.Verifycode = "";
            clearInterval(this.countdown);
        },
        FormatAccount: function (account) {
            var pre = account.replace(account.substr(0, account.length - 4), '************');
            return pre;
        },
        CheckECS: function () {
            if (this.Status[0] === this.ECSStatus.run || this.Status[0] === this.ECSStatus.start || this.Status[0] === this.ECSStatus.stopping || this.Status[0] === this.ECSStatus.stopped) {
                this.IsHasECS = true;
            } else {
                this.IsHasECS = false;
            }
        },
        CheckRDS: function () {
            if (this.Status[2] === this.RDSStatus.run || this.Status[2] === this.RDSStatus.error) {
                this.IsHasRDS = true;
            } else {
                this.IsHasRDS = false;
            }
        },
        CheckSLB: function () {
            if (this.Status[1] === this.SLBStatus.run || this.Status[1] === this.SLBStatus.stop) {
                this.IsHasSLB = true;
            } else {
                this.IsHasSLB = false;
            }
        },
        CheckDomains: function () {
            if (this.Status[3] === this.DomainStatus.run || this.Status[3] === this.DomainStatus.stop) {
                this.IsHasDomain = true;
            } else {
                this.IsHasDomain = false;
            }
        },
        // Add end

        //Add by bill 2018-10-29
        CancelSchPaymentOrder: function () {
            var api = GetCommonAPI("Payment/CancelPaymentOrder");
            this.$http.get(api, { params: { orderRef: this.Subscription.ORDERREF }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                if (response.body == "0") {
                    WarningMessage('WarningMessage', this.PromptMessage[57].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                    this.LoadSubscription();
                }
                else {
                    WarningMessage('WarningMessage', this.PromptMessage[58].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            });
        }

    }
});

$(function () { $("[data-toggle='popover']").popover(); });