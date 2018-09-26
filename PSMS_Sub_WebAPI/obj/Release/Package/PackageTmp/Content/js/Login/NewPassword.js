Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        password: "",
        cpassword: "",
        message: "",
        message2: "",
        Multilingual: [],
        PromptMessage: [],
        DropDownLanguage: [],
        Navigation: [],
        Language_Value: "2",
        
    },
    mounted: function () {
        this.checkEmailcode(),
        this.LoadMultilingual()


    },
    methods: {
        LoadMultilingual: function () {
            var api = LoadMultilingual_API_IE("NewPassword")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                
            }, function (response) { console.log(response); });
        },

        
        ChangeLan: function () {
            var api = ChangeLan_API_IE("NewPassword")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.Setmessage();
            }, function (response) { console.log(response); });
        },    
        checkoutPassword: function () {
            var api = GetCommonAPI('/System/Updatepassword');
            this.message = "";
            this.message2 = "";
            if (this.password == null || this.password == "") {
                this.message = this.Multilingual[4].Describe
                this.cpassword = ""
                return;
            }
            if (this.cpassword == "" || this.cpassword == null) {
                this.message2 = this.Multilingual[5].Describe;
                return;
            }
            if (this.cpassword != this.password) {
                this.message = this.Multilingual[6].Describe;
                this.password = ""
                this.cpassword = ""
                return;
            }
            this.$http.get(api, { params: { "password": this.password } }).then(function (response) {
                if (response.body > 0) {
                    WarningMessage_Link('WarningMessage', this.Multilingual[7].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();         
                }
                else {
                    WarningMessage_Link('WarningMessage', this.Multilingual[10].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/resetpassword.html'");
                    $("#myModal").modal();            
                }
                if (response.body == "false") {
                    window.location.href = '../Login /resetpassword.html';
                }               
            }, function (response) {
                console.log(response)
            })
        },
        Setmessage: function () {
            this.message = "";
            this.message2 = "";
        },
        checkEmailcode: function () {
            var apiUrl = GetCommonAPI('System/GetIsEmailcode');
            this.$http.get(apiUrl).then(function (response) {
                 if (response.body == 3) {
                    window.location.href = '../Login/resetpassword.html'
                }
                if (response.body == false) {
                    window.location.href = '../Login/resetPassword.html'
                }
                

            }, function (response) { console.log(response); })           
        }
    }
});


