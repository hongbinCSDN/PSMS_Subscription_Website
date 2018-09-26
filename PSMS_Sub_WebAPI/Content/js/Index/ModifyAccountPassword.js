Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        Multilingual: [],
        PromptMessage: [],
        Navigation: [],
        DropDownLanguage: [],
        Language_Value: "2",
        IsSendEmail: false,
        ResendTime: 120,
        UserInfo: {},
        confirm_password:"",
        PasswordModel: {
            oldPwd: "",
            newPwd: "",
            verifyCode:""
        }
    },
    mounted: function () {
        this.LoadMultilingual();
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
                    this.$refs.logining.style.display = 'block';
                    this.$refs.signuping.style.display = 'block';
                    WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            });
        },
        HeadPortraithover: function () {
            document.getElementById("userinfo").style.display = "";
        },
        HeadPortraithoverout: function () {
            document.getElementById("Portrait").style.border = "none";
            document.getElementById("userinfo").style.display = "none";

        }, 
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
            var api = LoadMultilingual_API_IE("ModifyPassword")
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
            var api = ChangeLan_API_IE("ModifyPassword")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
            }, function (response) { console.log(response); });
        },
        GetVerifyCodeEmail: function () {
            var api = GetCommonAPI_IE("System/GetVerifyCodeEmail");
            this.$http.get(api, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                WarningMessage('WarningMessage', this.PromptMessage[43].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                this.IsSendEmail = true;
                clearInterval(countdown);
                var countdown = setInterval(function () {
                    if (vm.ResendTime > 0) {
                        vm.ResendTime--;
                    }
                    else {
                        vm.IsSendEmail = false;
                        vm.ResendTime = 120;
                        clearInterval(countdown);
                    }
                }, 1000);
            }, function (response) {
                console.log(response);
            });
        },
        ModifyPassword: function () {
            var api = GetCommonAPI_IE("System/ModifyPassword");
            if (this.PasswordModel.oldPwd == null || this.PasswordModel.oldPwd == "") {
                WarningMessage('WarningMessage', this.PromptMessage[35].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.PasswordModel.newPwd == null || this.PasswordModel.newPwd == "") {
                WarningMessage('WarningMessage', this.PromptMessage[36].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.confirm_password == null || this.confirm_password == "") {
                WarningMessage('WarningMessage', this.PromptMessage[18].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.PasswordModel.verifyCode == null || this.PasswordModel.verifyCode == "") {
                WarningMessage('WarningMessage', this.PromptMessage[37].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.PasswordModel.newPwd != this.confirm_password) {
                WarningMessage('WarningMessage', this.PromptMessage[16].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            this.$http.post(api, this.PasswordModel, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                var result = response.body;
                if (result == -3) {
                    WarningMessage('WarningMessage', this.PromptMessage[5].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                } else if (result == 0) {
                    WarningMessage('WarningMessage', this.PromptMessage[39].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                } else if (result == -2) {
                    WarningMessage('WarningMessage', this.PromptMessage[38].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                } else if (result == 1) {
                    WarningMessage_Link('WarningMessage', this.PromptMessage[42].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe,"'../index/index.html'");
                    $("#myModal").modal();
                } else if (result == -4) {
                    WarningMessage('WarningMessage', this.PromptMessage[41].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                } else {
                    WarningMessage('WarningMessage', this.PromptMessage[40].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            })
        },
    }
})