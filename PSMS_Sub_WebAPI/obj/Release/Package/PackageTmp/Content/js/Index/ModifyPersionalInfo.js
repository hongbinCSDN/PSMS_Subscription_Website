Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        customer: []
        , showcustomer: {
            customer: [],
            isModifyEmail: "",
            verifyCode: ""
        }      //Modify / Add by Chester 2018.07.27
        , Token: ''
        , Multilingual: []
        , PromptMessage: []
        , Navigation: []
        , DropDownLanguage: []
        , Language_Value: "2"
        , UserInfo: {}
        //, IsModifyEmail: false
        //, IsSendVerifyEmail: false
        //, ResendTime: 120
        //, VerifyCode: ''
        //, ModifyEmailBtnWidth: 220
        , EnglishNamePrompt: ""
        , CompNamePrompt: ""
        , PhoneNumPrompt: ""
        , EmailPrompt: ""
    },
    mounted: function () {
        this.LoadMultilingual(),
            this.GetToken()
    },
    methods: {
        GetToken: function () {
            var api = GetCommonAPI_IE('System/GetToken');
            this.$http.get(api).then(function (response) {
                this.Token = response.body.TOKEN;  //Modify by bill 2018-6-17
                if (this.Token != null && this.Token != "") {
                    this.LoadPersionalInfo();
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
        //Modify / Add by Chester 2018.07.30
        //Logout: function () {
        //    var res = confirm(this.PromptMessage[6].Describe);
        //    if (res) {
        //        var api = GetCommonAPI_IE('System/RemoveToken');
        //        this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
        //            this.Token = "";
        //            window.location.href = "../Login/Login.html";
        //        }, function (response) { console.log(response); });
        //    } else
        //        return;
        //},
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
        //Modify end

        LoadPersionalInfo: function () {
            var token = this.Token;
            var api = GetCommonAPI_IE('System/GetCustomerPersonalInfo');     //Modify / Add by Chester 2018.07.30
            this.$http.get(api, { headers: { Authorization: 'bearer ' + token } })
                .then(function (response) {
                    this.customer = response.body.Data.Table[0];
                    this.showcustomer.customer = JSON.parse(JSON.stringify(this.customer)); //Modify / Add by Chester 2018.07.27
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal();
                });
        },
        LoadMultilingual: function () {
            var api = LoadMultilingual_API("ModifyPersionalInfo")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
            }, function (response) { console.log(response); });
        },

        //Modify / Add by Chester 2018.07.26
        ChangeLan: function () {
            var api = ChangeLan_API("ModifyPersionalInfo")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
            }, function (response) { console.log(response); });
        },
        OpenModifyModal: function () {
            this.EnglishNamePrompt = "";
            this.CompNamePrompt = "";
            this.PhoneNumPrompt = "";
            this.EmailPrompt = "";
            $('#UpdateModal').modal();
        },
        // Modify by Chester 2018.09.29
        UpdatePersonInfo: function () {

            if (this.showcustomer.customer.CUSTOMER_NAME == null || this.showcustomer.customer.CUSTOMER_NAME == "") {
                this.EnglishNamePrompt = this.PromptMessage[10].Describe;
            } else {
                this.EnglishNamePrompt = "";
            }


            //if (this.IsModifyEmail) {
            //    if (this.showcustomer.customer.EMAIL == null || this.showcustomer.customer.EMAIL == "") {
            //        this.EmailPrompt = this.PromptMessage[11].Describe;
            //    } else if (this.Ischeckmail(this.showcustomer.customer.EMAIL) == false) {
            //        this.EmailPrompt = this.PromptMessage[15].Describe;
            //    } else if (this.showcustomer.verifyCode == null || this.showcustomer.verifyCode == "") {
            //        this.EmailPrompt = this.PromptMessage[37].Describe;
            //    } else {
            //        this.EmailPrompt = "";
            //    }
            //}

            if (this.showcustomer.customer.COMPANY == null || this.showcustomer.customer.COMPANY == "") {
                this.CompNamePrompt = this.PromptMessage[12].Describe;
            } else {
                this.CompNamePrompt = "";
            }

            if (this.showcustomer.customer.PHONE == null || this.showcustomer.customer.PHONE == "") {
                this.PhoneNumPrompt = this.PromptMessage[13].Describe;
            } else if (this.showcustomer.customer.PHONE.length != 8 || isNaN(this.showcustomer.customer.PHONE) == true) {
                this.PhoneNumPrompt = this.PromptMessage[14].Describe;
            } else {
                this.PhoneNumPrompt = "";
            }

            if (this.showcustomer.customer.CUSTOMER_NAME == null || this.showcustomer.customer.CUSTOMER_NAME == "") {
                return;
            } else if (this.showcustomer.customer.COMPANY == null || this.showcustomer.customer.COMPANY == "") {
                return;
            } else if (this.showcustomer.customer.PHONE == null || this.showcustomer.customer.PHONE == "") {
                return;
            } else if (this.showcustomer.customer.PHONE.length != 8 || isNaN(this.showcustomer.customer.PHONE) == true) {
                return;
            }

            var api = GetCommonAPI('System/UpdateCustomerPersonalInfo');
            var token = this.Token;
            this.$http.post(api,
                this.showcustomer,
                { headers: { Authorization: 'bearer ' + token } })
                .then(function (response) {
                    mess = response.body.Data;
                    if (mess == 1) {
                        this.LoadPersionalInfo();
                        this.UserInfo.EMAIL = this.showcustomer.customer.EMAIL;//Modify / Add by Chester 2018.09.20
                        this.showcustomer.verifyCode = "";
                        //this.IsModifyEmail = false;
                        //this.IsSendVerifyEmail = false;
                        $('#UpdateModal').modal('hide');
                        WarningMessage('WarningMessage', this.PromptMessage[23].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();

                    }
                    else if (mess == 0) {
                        $('#UpdateModal').modal('hide');
                        WarningMessage('WarningMessage', this.PromptMessage[24].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    //Modify / Add by Chester 2018.08.10
                    else if (mess == -1) {
                        //$('#UpdateModal').modal('hide');
                        //WarningMessage('WarningMessage', this.PromptMessage[21].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        //$("#myModal").modal();
                        this.EmailPrompt = this.PromptMessage[21].Describe;
                    } else if (mess == -2) {
                        //$('#UpdateModal').modal('hide');
                        //WarningMessage('WarningMessage', this.PromptMessage[5].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        //$("#myModal").modal();
                        this.EmailPrompt = this.PromptMessage[5].Describe;
                    } else if (mess == -3) {
                        //$('#UpdateModal').modal('hide');
                        //WarningMessage('WarningMessage', this.PromptMessage[41].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        //$("#myModal").modal();
                        this.EmailPrompt = this.PromptMessage[41].Describe;
                    } else {
                        console.log(response);
                    }
                    //Modify End
                },
                    function (response) {
                        $('#UpdateModal').modal('hide');
                        WarningMessageAddbutton3('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                        $("#myModal").modal();
                    })

        },
        //Modify end
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
        // Modify end
        // Modify / Add by Chester 2018.09.28
        //ModifyEmail: function () {
        //    this.IsModifyEmail = !this.IsModifyEmail;
        //    this.showcustomer.isModifyEmail = this.IsModifyEmail;
        //    if (this.IsModifyEmail == false) {
        //        this.showcustomer.customer.EMAIL = this.UserInfo.EMAIL;
        //        this.showcustomer.verifyCode = "";
        //        this.IsModifyEmail = false;
        //        this.IsSendVerifyEmail = false;
        //        this.EmailPrompt = "";
        //    }
        //},
        //GetVerifyCodeEmail: function () {
        //    var api = GetCommonAPI_IE("System/GetOldAccountVerifyEmail");
        //    this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
        //        //WarningMessage('WarningMessage', this.PromptMessage[43].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
        //        //$("#myModal").modal();
        //        this.IsSendVerifyEmail = true;
        //        clearInterval(countdown);
        //        var countdown = setInterval(function () {
        //            if (vm.ResendTime > 0) {
        //                vm.ResendTime--;
        //            }
        //            else {
        //                vm.IsSendVerifyEmail = false;
        //                vm.ResendTime = 120;
        //                vm.showcustomer.verifyCode = "";
        //                vm.EmailPrompt = "";
        //                clearInterval(countdown);
        //            }
        //        }, 1000);
        //    }, function (response) {
        //        console.log(response);
        //    });
        //},
        CheckEnglishName: function () {
            if (this.showcustomer.customer.CUSTOMER_NAME == null || this.showcustomer.customer.CUSTOMER_NAME == "") {
                this.EnglishNamePrompt = this.PromptMessage[10].Describe;
            } else {
                this.EnglishNamePrompt = "";
            }
        },
        CheckCompany: function () {
            if (this.showcustomer.customer.COMPANY == null || this.showcustomer.customer.COMPANY == "") {
                this.CompNamePrompt = this.PromptMessage[12].Describe;
            } else {
                this.CompNamePrompt = "";
            }
        },
        CheckPhone: function () {
            if (this.showcustomer.customer.PHONE == null || this.showcustomer.customer.PHONE == "") {
                this.PhoneNumPrompt = this.PromptMessage[13].Describe;
            } else if (this.showcustomer.customer.PHONE.length != 8 || isNaN(this.showcustomer.customer.PHONE) == true) {
                this.PhoneNumPrompt = this.PromptMessage[14].Describe;
            } else {
                this.PhoneNumPrompt = "";
            }
        }

        // Modify end
    }
});