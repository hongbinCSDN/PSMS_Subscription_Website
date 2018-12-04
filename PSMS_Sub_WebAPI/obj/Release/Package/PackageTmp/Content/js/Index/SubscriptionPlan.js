Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        Multilingual: [],
        PromptMessage: [],
        DropDownLanguage: [],
        Navigation: [],
        Language_Value: "2",
        Token: "",
        payment: {},
        selectitem: "",
        UserInfo: {}, //Modify by bill 2018-6-17
        Subscription_Agt_PSMS: [],//Add by Haskin 2018-10-16
        checkeds: [],//Add by Haskin 2018-10-16
        CheckedMessage: "",//Add by Haskin 2018-10-16
        domainname: "",//Add by Haskin 2018-10-16
        category: "",//Add by Haskin 2018-10-16
        // Add by chester 2018-10-17
        PayName: "",
        PayEmail: "",
        PayName_error: "",
        PayEmail_error: "",
        Cale_error: "",
        Mons: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    },
    mounted: function () {
        this.LoadMultilingual();
        this.LoadPrivacyPolicyMultilingual();
    },
    methods: {
        GetToken: function () {
            var api = GetCommonAPI_IE('System/GetToken');
            this.$http.get(api).then(function (response) {
                this.Token = response.body.TOKEN;  //Modify by bill 2018-6-17
                if (this.Token != null && this.Token != "") {
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
                    WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
                    this.$refs.logining.style.display = 'block';
                    this.$refs.signuping.style.display = 'block';
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
            var api = LoadMultilingual_API_IE("SubscriptionPlan")
            this.$http.get(api).then(function (response) {

                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.selectitem = this.Multilingual[50].Describe;
                this.GetToken();
            }, function (response) { console.log(response); });
        },
        LoadPrivacyPolicyMultilingual: function () {
            var api = LoadMultilingual_API_IE("Subscription_Agt_PSMS")
            this.$http.get(api).then(function (response) {
                this.Subscription_Agt_PSMS = response.body.Table1;
            }, function (response) { console.log(response); });
        },
        checkboxOnclick: function () {
            this.CheckedMessage = "";
        },
        ClearCheckMessage: function () {
            this.CheckedMessage = "";
            this.checkeds = [];

        },

        WarningDomain: function () {
            if (this.checkeds[0] == "true") {
                $('#identifier').modal('hide');
                $("#autoPayModal").modal();
                //WarningMessageAddbutton2("WarningMessage", this.Multilingual[52].Describe, this.domainname + ".pssasl.com", this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, this.category, this.domainname);
                //$("#myModal").modal();
            }
            else {
                this.CheckedMessage = this.Multilingual[57].Describe
            }


        },
        SubscriptionAgtModle: function () {

            $('#identifier').modal({
                keyboard: false
            })
        },
        ChangeLan: function () {
            var api = ChangeLan_API_IE("SubscriptionPlan")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.more = this.Multilingual[9].Describe;
                this.selectitem = this.Multilingual[50].Describe;
                this.LoadPrivacyPolicyMultilingual();

            }, function (response) { console.log(response); });
        },
        CheckDomainName: function (category, btn) {

            var api = GetCommonAPI('Payment/CheckDomain');
            var domain_name = null;
            domain_name = this.$refs.domain.value;
            if (domain_name == "" || domain_name == null) {
                //alert(this.PromptMessage[27].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[27].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal('show');
                return;
            }
            //Add by bill 2018.8.20
            var pat = new RegExp("[^a-zA-Z0-9\_\u4e00-\u9fa5]", "i");
            if (pat.test(domain_name) == true) {
                WarningMessage('WarningMessage', this.PromptMessage[31].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            //End  
            if (this.Token != null && this.Token != "") {
                this.$http.get(api, { params: { domain_name: domain_name }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    var message = response.body;
                    if (message == "0") {
                        WarningMessage('WarningMessage', this.PromptMessage[25].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else if (message != "0" && message != "1") {
                        WarningMessage('WarningMessage', this.PromptMessage[26].Describe + message, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else {
                        //add by haskin 2018-10-16
                        this.domainname = domain_name;
                        this.category = category;
                        this.SubscriptionAgtModle();

                        //Modify by haskin 2018-10-16
                        // WarningMessageAddbutton2("WarningMessage", this.Multilingual[52].Describe, domain_name + ".pssasl.com", this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, category, domain_name);
                        // $("#myModal").modal();
                        //end
                        //this.PaymentSubmit(category, domain_name);
                    }
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal();
                });
            }
            else {
                WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                $("#myModal").modal();
            }
        },
        PaymentSubmit: function (category, domain_name, payway) {
            if (this.Token != null && this.Token != "") {
                var form = document.getElementById('payFormCcard');
                var SP_form = document.getElementById('SP_payFormCcard');
                var api = GetCommonAPI_IE("Payment/GetProductPostForm");
                this.$http.get(api, { params: { category: category }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    var data = response.body.Table[0];
                    if (payway == "1") {
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
                        this.CreateOrder(data.ORDERREF, data.AMOUNT, category, domain_name, payway);
                        form.submit();
                    }
                    else if (payway == "2") {
                        var nowDate = new Date();

                        var endDate = new Date(); // Modify by Chester 20181108
                        this.$refs.SP_merchantId.value = data.MERCHANTID;
                        this.$refs.SP_amount.value = data.AMOUNT;
                        this.$refs.SP_orderRef.value = data.ORDERREF;
                        this.$refs.SP_currCode.value = data.CURRCODE;
                        this.$refs.SP_mpsMode.value = data.MPSMODE;
                        this.$refs.SP_successUrl.value = data.SUCCESSURL;
                        this.$refs.SP_failUrl.value = data.FAILURL;
                        this.$refs.SP_cancelUrl.value = data.CANCELURL;
                        this.$refs.SP_payType.value = data.PAYTYPE;
                        this.$refs.SP_lang.value = data.LANG;
                        this.$refs.SP_payMethod.value = data.PAYMETHOD;
                        this.$refs.SP_secureHash.value = data.SECUREHASH;
                        this.$refs.SP_appId.value = 'SP';
                        this.$refs.SP_schType.value = 'Month';
                        this.$refs.SP_nSch.value = '1';
                        this.$refs.SP_sMonth.value = nowDate.getMonth() + 1;
                        this.$refs.SP_sDay.value = nowDate.getDate();
                        this.$refs.SP_sYear.value = nowDate.getFullYear();
                        this.$refs.SP_eMonth.value = endDate.getMonth() + 1;
                        this.$refs.SP_eDay.value = endDate.getDate();
                        this.$refs.SP_eYear.value = endDate.getFullYear() + 99;
                        this.$refs.SP_name.value = $("#PayerName").val();
                        this.$refs.SP_email.value = $("#PayerEmail").val();
                        this.$refs.SP_schStatus.value = 'Active';
                        this.CreateOrder(data.ORDERREF, data.AMOUNT, category, domain_name, payway);
                        SP_form.submit();
                    }
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
        CreateOrder: function (orderRef, amount, category, domain_name, payway) {
            var api = GetCommonAPI('Payment/CreateOrder');
            this.payment.ORDERREF = orderRef;
            this.payment.AMOUNT = amount;
            this.payment.PAYMENT_TYPE_ID = category;
            this.payment.DOMAIN_NAME = domain_name;
            this.payment.PAYWAY_ID = payway;  // Add by bill 2018-10-18
            this.$http.post(api, this.payment, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal();
            });
        },

        NoAutoPay: function () {
            $("#autoPayModal").modal("hide");
            var domain_name = null;
            domain_name = this.$refs.domain.value;
            WarningMessageAddbutton2("WarningMessage", this.Multilingual[52].Describe, domain_name + ".pssasl.com", this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, this.category, domain_name, "1");
            $("#myModal").modal();
        },
        AutoPay: function () {
            $("#autoPayModal").modal("hide");
            $("#payMsgModal").modal();

        },
        ConfirmAutoPay: function () {

            if (this.PayName == null || this.PayName == "") {
                this.PayName_error = this.PromptMessage[46].Describe;
            } else {
                this.PayName_error = "";
            }

            if (this.PayEmail == null || this.PayEmail == "") {
                this.PayEmail_error = this.PromptMessage[47].Describe;
            } else if (this.Ischeckmail(this.PayEmail) == false) {
                this.PayEmail_error = this.PromptMessage[15].Describe;
            } else {
                this.PayEmail_error = "";
            }

            if (this.PayName == null || this.PayName == "") {
                return;
            } else if (this.PayEmail == null || this.PayEmail == "") {
                return;
            } else if (this.Ischeckmail(this.PayEmail) == false) {
                return;
            } else {
                $("#payMsgModal").modal("hide");
                var domain_name = null;
                domain_name = this.$refs.domain.value;
                WarningMessageAddbutton2("WarningMessage", this.Multilingual[52].Describe, domain_name + ".pssasl.com", this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, this.category, domain_name, "2");
                $("#myModal").modal();
            }


        },
        Ischeckmail: function (email) {
            if (email != "" && email != null) {
                var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
                isok = reg.test(email);
                if (!isok) {
                    return false;
                }
                else return true;
            }
        },

        ContactASL: function () {
            WarningMessageAddbutton('WarningMessage', this.PromptMessage[60].Describe, this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, 'vm.ConfirmContactASL()');
            $("#myModal").modal();
        },
        ConfirmContactASL: function () {
            var api = GetCommonAPI_IE("System/ContactASL");
            this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                if (response.body === "1") {
                    WarningMessage_checkIcon('WarningMessage', this.PromptMessage[59].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe)
                    $("#myModal").modal();
                } else {
                    WarningMessage('WarningMessage', this.PromptMessage[61].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $('#myModal').modal();
                }
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal();
            });
        }

    }
}
);

