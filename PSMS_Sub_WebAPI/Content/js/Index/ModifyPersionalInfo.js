Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        customer: []
        , showcustomer: []      //Modify / Add by Chester 2018.07.27
        , Token: ''
        , Multilingual: []
        , PromptMessage: []
        , Navigation: []
        , DropDownLanguage: []
        , Language_Value: "2"
        , UserInfo: {}
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
                    this.showcustomer = JSON.parse(JSON.stringify(this.customer)); //Modify / Add by Chester 2018.07.27
                }, function (response) {
                    //WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    //$("#myModal").modal();

                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
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
        UpdatePersonInfo: function () {

            if (this.showcustomer.CUSTOMER_NAME == null || this.showcustomer.CUSTOMER_NAME == "") {
                $('#UpdateModal').modal('hide');
                WarningMessage('WarningMessage', this.PromptMessage[10].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.showcustomer.EMAIL == null || this.showcustomer.EMAIL == "") {
                $('#UpdateModal').modal('hide');
                WarningMessage('WarningMessage', this.PromptMessage[11].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.Ischeckmail(this.showcustomer.EMAIL) == false) {
                //alert(this.PromptMessage[15].Describe);
                $('#UpdateModal').modal('hide');
                WarningMessage('WarningMessage', this.PromptMessage[15].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.showcustomer.PHONE == null || this.showcustomer.PHONE == "") {
                $('#UpdateModal').modal('hide');
                WarningMessage('WarningMessage', this.PromptMessage[13].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.showcustomer.PHONE.length != 8 || isNaN(this.showcustomer.PHONE) == true) {
                $('#UpdateModal').modal('hide');
                WarningMessage('WarningMessage', this.PromptMessage[14].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.showcustomer.COMPANY == null || this.showcustomer.COMPANY == "") {
                $('#UpdateModal').modal('hide');
                WarningMessage('WarningMessage', this.PromptMessage[12].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
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
                        this.UserInfo.EMAIL = this.showcustomer.EMAIL;//Modify / Add by Chester 2018.09.20
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
                        $('#UpdateModal').modal('hide');
                        WarningMessage('WarningMessage', this.PromptMessage[21].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    } else {
                        console.log(response);
                    }
                    //Modify End
                }, function (response) {
                    $('#UpdateModal').modal('hide');
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal(); 
                })

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
        }
        //Modify end



    }
});