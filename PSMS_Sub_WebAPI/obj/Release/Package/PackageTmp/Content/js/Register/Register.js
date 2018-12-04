Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        customer: {}
        , Multilingual: []
        , PromptMessage: []
        , DropDownLanguage: []
        , Navigation: []
        , Language_Value: "2"
        , Username_error: ""
        , Password_error: ""
        , ConfirmPasswrod_error: ""
        , EnglishName_error: ""
        , Email_error: ""
        , Company_error: ""
        , Phone_error: ""
        , Telphone_error: ""
        , VerifyCode_error: ""
    },
    mounted: function () {
        this.LoadMultilingual()
    },
    methods: {
        RegisterCustomerAccount: function () {
            var confirm_password = document.getElementById("signup-confirm-password").value;
            var pat = new RegExp("[^a-zA-Z0-9\_\u4e00-\u9fa5]", "i");

            this.CheckUsername();
            this.CheckPassword();
            this.CheckConfirmPassword();
            this.CheckEnglishName();
            this.CheckEmail();
            this.CheckCompanyName();
            this.CheckPhone();
            this.CheckTelPhone();
            this.CheckVerifyCode();

            if (this.customer.CUSTOMER_ID == null || this.customer.CUSTOMER_ID == "") {
                return;
            } else if (pat.test(this.customer.CUSTOMER_ID) == true) {
                return;
            } else if (this.customer.PASSWORD == null || this.customer.PASSWORD == "") {
                return;
            } else if (confirm_password == null || confirm_password == "") {
                return;
            } else if (this.customer.PASSWORD != confirm_password) {
                return;
            } else if (this.customer.CUSTOMER_NAME == null || this.customer.CUSTOMER_NAME == "") {
                return;
            } else if (this.customer.EMAIL == null || this.customer.EMAIL == "") {
                return;
            } else if (this.Ischeckmail() == false) {
                return;
            } else if (this.customer.COMPANY == null || this.customer.COMPANY == "") {
                return;
            } else if (this.customer.PHONE == null || this.customer.PHONE == "") {
                return;
            } else if (this.customer.PHONE.length != 8 || isNaN(this.customer.PHONE) == true) {
                return;
            } else {

            }

            if (this.customer.FIXED_TELEPHONE == null) {
                this.customer.FIXED_TELEPHONE = "";
            }
            if (this.customer.FIXED_TELEPHONE.length > 15) {
                return;
            }

            var res = verifyCode.validate(document.getElementById("code_input").value);
            if (!res) {
                return;
            }

            var api = GetCommonAPI('System/Register');
            this.$http.post(api, this.customer).then(function (response) {
                var dt = response.body;
                if (dt.Data == 1) {
                    //window.location.href = "../Login/Login.html";
                    WarningMessage_Link('WarningMessage', this.PromptMessage[3].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
                }
                else if (dt.Data == 0) {
                    WarningMessage('WarningMessage', this.PromptMessage[4].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
                else if (dt.Data == -1) {
                    WarningMessage('WarningMessage', this.PromptMessage[21].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
                else if (dt.Data == -2) {
                    WarningMessage('WarningMessage', this.PromptMessage[4].Describe + this.PromptMessage[21].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            })
        },
        LoadMultilingual: function () {
            var api = LoadMultilingual_API_IE("Register"); // Modify By Chester 2018.08.16
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
            }, function (response) { console.log(response); });
        },
        ChangeLan: function () {
            var api = ChangeLan_API_IE("Register");
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.CleanError();

            }, function (response) { console.log(response); });
        },
        Ischeckmail: function () {
            if (this.customer.EMAIL != "" && this.customer.EMAIL != null) {
                var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
                isok = reg.test(this.customer.EMAIL);
                if (!isok) {
                    return false;
                }
                else return true;
            }
        },
        CheckUsername: function () {
            var pat = new RegExp("[^a-zA-Z0-9\_\u4e00-\u9fa5]", "i");
            if (this.customer.CUSTOMER_ID == null || this.customer.CUSTOMER_ID == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[8].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Username_error = this.PromptMessage[8].Describe;
            } else if (pat.test(this.customer.CUSTOMER_ID) == true) {
                //WarningMessage('WarningMessage', this.PromptMessage[19].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Username_error = this.PromptMessage[19].Describe;
            } else {
                this.Username_error = "";
            }
        },
        CheckPassword: function () {
            var confirm_password = document.getElementById("signup-confirm-password").value;

            if (this.customer.PASSWORD == null || this.customer.PASSWORD == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[9].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Password_error = this.PromptMessage[9].Describe;
            }
            else {
                this.Password_error = "";
            }
        },
        CheckConfirmPassword: function () {
            var confirm_password = document.getElementById("signup-confirm-password").value;
            if (confirm_password == null || confirm_password == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[18].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.ConfirmPasswrod_error = this.PromptMessage[18].Describe;
            }
            else if (this.customer.PASSWORD != confirm_password) {
                //WarningMessage('WarningMessage', this.PromptMessage[16].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.ConfirmPasswrod_error = this.PromptMessage[16].Describe;
            }
            else {
                this.ConfirmPasswrod_error = "";
            }
        },
        CheckEnglishName: function () {
            if (this.customer.CUSTOMER_NAME == null || this.customer.CUSTOMER_NAME == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[10].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.EnglishName_error = this.PromptMessage[10].Describe;
            } else {
                this.EnglishName_error = "";
            }
        },
        CheckEmail: function () {
            if (this.customer.EMAIL == null || this.customer.EMAIL == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[11].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Email_error = this.PromptMessage[11].Describe;
            }
            else if (this.Ischeckmail() == false) {
                //WarningMessage('WarningMessage', this.PromptMessage[15].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Email_error = this.PromptMessage[15].Describe;
            }
            else {
                this.Email_error = "";
            }
        },
        CheckCompanyName: function () {
            if (this.customer.COMPANY == null || this.customer.COMPANY == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[12].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Company_error = this.PromptMessage[12].Describe;
            } else {
                this.Company_error = "";
            }
        },
        CheckPhone: function () {
            if (this.customer.PHONE == null || this.customer.PHONE == "") {
                //WarningMessage('WarningMessage', this.PromptMessage[13].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Phone_error = this.PromptMessage[13].Describe;
            }
            else if (this.customer.PHONE.length != 8 || isNaN(this.customer.PHONE) == true) {
                //WarningMessage('WarningMessage', this.PromptMessage[14].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Phone_error = this.PromptMessage[14].Describe;
            }
            else {
                this.Phone_error = "";
            }
        },
        CheckTelPhone: function () {
            if (this.customer.FIXED_TELEPHONE == null) {
                this.customer.FIXED_TELEPHONE = "";
            }
            if (this.customer.FIXED_TELEPHONE.length > 15) {
                //WarningMessage('WarningMessage', this.PromptMessage[22].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.Telphone_error = this.PromptMessage[22].Describe;
            } else {
                this.Telphone_error = "";
            }
        },
        CheckVerifyCode: function () {
            var res = verifyCode.validate(document.getElementById("code_input").value);
            if (document.getElementById("code_input").value == null || document.getElementById("code_input").value == "") {
                this.VerifyCode_error = this.PromptMessage[37].Describe;
            }
            else if (!res) {
                //WarningMessage('WarningMessage', this.PromptMessage[5].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                //$("#myModal").modal();
                this.VerifyCode_error = this.PromptMessage[5].Describe;
            } else {
                this.VerifyCode_error = "";
            }
        },
        CleanError: function () {
            this.Username_error = "";
            this.Password_error = "";
            this.ConfirmPasswrod_error = "";
            this.EnglishName_error = "";
            this.Email_error = "";
            this.Company_error = "";
            this.Phone_error = "";
            this.Telphone_error = "";
            this.VerifyCode_error = "";
        }
    }
});