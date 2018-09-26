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
        UserInfo: {}  //Modify by bill 2018-6-17
    },
    mounted: function () {
        this.LoadMultilingual()       
    },
    methods: {
        JudgeLogin: function () {
            var api = GetCommonAPI_IE('System/GetToken');
            this.$http.get(api).then(function (response) {              
                this.Token = response.body.TOKEN;  //Modify by bill 2018-6-17                
                if (this.Token != "" && this.Token != null) {
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
            var api = LoadMultilingual_API_IE("Index")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.JudgeLogin();
            }, function (response) { console.log(response); });
        },
        ChangeLan: function () {
            var api = ChangeLan_API_IE("Index")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
            }, function (response) { console.log(response); });
        },
        Subscibe: function () {
            if (this.Token != "" && this.Token != null) {
                window.location.href = "../index/SubscriptionPlan.html";
            }
            else {
                //alert(this.PromptMessage[20].Describe);
                //window.location.href = "../Login/Log、in.html";
                WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                $("#myModal").modal();

            }
        }
       
    }
});

