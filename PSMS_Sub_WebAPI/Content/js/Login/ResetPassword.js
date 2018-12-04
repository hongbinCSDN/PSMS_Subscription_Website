
Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        userId: "",
        code2: "",
        message: "",
        codemessage1: "",
        Multilingual: [],
        PromptMessage: [],
        DropDownLanguage: [],
        Navigation:[],
        Language_Value: "2",
        content: "",
        watitime: 120,
        email: "",
        code: "",
        codemessage: "",
        isdisavled: true
    },
    mounted: function () {
        this.LoadMultilingual()
    },
    methods: {
        LoadMultilingual: function () {
            var api = LoadMultilingual_API_IE("ResetPassword")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.content = this.Multilingual[11].Describe
            }, function (response) { console.log(response); });
        }, 
        ChangeLan: function () {
            var api = ChangeLan_API_IE("ResetPassword")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.content = this.Multilingual[11].Describe;
                this.Setmessage2();
                this.Setmessage();
            }, function (response) { console.log(response); });
        },
        QueryEmail: function () {
            var apiUrl = GetCommonAPI('System/GetEmailByUserId');
            this.message = "";
            this.codemessage1 = "";
            if (this.userId == "" || this.userId == null) {
                this.message = this.Multilingual[4].Describe;
                this.$refs.message1.style.display = 'block';
                return;
            }

            if (this.code2 == "" || this.code2 == null) {
                this.codemessage1 = this.Multilingual[5].Describe;
                this.$refs.message3.style.display = 'block';
                return;
            }

            var res = verifyCode.validate(this.code2);
            if (!res) {
                this.codemessage1 = this.Multilingual[6].Describe;
                this.$refs.message3.style.display = 'block';
                this.code2 = "";
                return;
            }
            var isEmail = this.isEmail(this.userId);
            if (isEmail) {
                apiUrl = GetCommonAPI('System/GetEmailbyEmail');
                this.$http.get(apiUrl, { params: { "email": this.userId } }).then(function (response) {
                    //console.log(response);
                    if (response.body == 0) {
                        this.message = this.Multilingual[7].Describe;
                        this.$refs.message1.style.display = 'block';
                        this.code2 = "";
                    } else {
                        $("#inputUserInfo").fadeOut("fast")
                        $("#sendEmail").fadeIn("slow")
                    }
                }, function (response) {
                    console.log(response);
                });
            }
            else {
                apiUrl = GetCommonAPI('System/GetEmailByUserId');
                this.$http.get(apiUrl, { params: { "userId": this.userId } }).then(function (response) {

                    if (response.body == 0) {
                        this.message = this.Multilingual[8].Describe;
                        this.$refs.message1.style.display = 'block';
                        this.userId = "";
                        this.code2 = "";
                    }

                    else {
                        $("#inputUserInfo").fadeOut("fast")
                        $("#sendEmail").fadeIn("slow")
                    }
                }, function (response) {
                    console.log(response);
                });
            }




        },
        isEmail: function (str) {
            var myReg = /^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/;
            if (myReg.test(str)) return true;
            return false;
        },
        Setmessage: function () {
            this.message = "";
            this.codemessage1 = "";
        },


        checkoutEmailCode: function () {

            var apiUrl = GetCommonAPI('System/JudgeEmailcode');
            if (this.watitime == 120) {
                if (this.isdisavled == true) {
                    this.codemessage = this.Multilingual[17].Describe
                    return;
                }
                if (this.code == null || this.code == "") {
                    this.codemessage = this.Multilingual[12].Describe
                    return;
                }
                if (this.isdisavled == false) {
                    this.codemessage = this.Multilingual[18].Describe
                    return;
                }
            }
            if (this.code == null || this.code == "") {
                this.codemessage = this.Multilingual[12].Describe
                return;
            }      
            this.$http.get(apiUrl, { params: { "resetpasswordemailcode": this.code } }).then(function (response) {
                console.log(response);
                if (response.body == 3) {
                    window.location.href = '../Login/resetpassword.html'
                }
                if (response.body == 1) {   
                    window.location.href = "../login/newpassword.html";
                }
                if (response.body == 0) {
                    this.codemessage = this.Multilingual[13].Describe
                    return;
                }
                //Add by bill 2018-9-19
                if (response.body == -1) {
                    this.codemessage = this.Multilingual[18].Describe
                    return;
                }
                //End
            });
        },
        SendEmail: function () {
            this.isdisavled = false;
            var apiUrl = GetCommonAPI('System/SendEmail');
            this.Countdown();
            document.getElementById("button_id").disabled = true
            this.$http.get(apiUrl,{ params: {"Language_Value":this.Language_Value } }).then(function (response) {               
                if (response.body == 3) {
                    window.location.reload();
                } else {
                    this.codemessage = this.Multilingual[14].Describe
                }
                if (response.body == 1) {
                    this.codemessage = this.Multilingual[20].Describe
                }
                

            }, function (response) { console.log(response); })
        },
        Setmessage2: function () {
            this.codemessage = "";
        },
        Countdown: function () {
            let interval = window.setInterval(function () {
                console.log("waittime:" + vm.watitime);
                console.log("multilingual:" + vm.Multilingual[15].Describe);
                vm.content = vm.watitime + vm.Multilingual[15].Describe;
                if ((vm.watitime--) <= 0) {
                    vm.isdisavled = false;
                    document.getElementById("button_id").disabled = false
                    window.clearInterval(interval);
                    vm.watitime = 120;
                    vm.content = vm.Multilingual[11].Describe; 
                }
            }, 1000);

        }


    }

});