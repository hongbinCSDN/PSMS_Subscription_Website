Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        account: {}
        ,Multilingual: []
        ,PromptMessage: []
        , DropDownLanguage: []
        , Navigation: []
        ,Language_Value: "2"
    },
    mounted: function () {
        this.LoadMultilingual()              
    },
    methods: { 
        ActiveAccount: function () {         
            var api = GetCommonAPI('System/ActiveAccount');
            this.$http.post(api, this.account).then(function (response) {          
                if (response.body.Affected == 1) {
                    WarningMessage_Link('WarningMessage', response.body.Message, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
                    //window.location.href = '../Login/Login.html'
                }
                else {
                    WarningMessage_Link('WarningMessage', response.body.Message, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Index/Index.html'");
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            });
        },
        LoadMultilingual: function () {
            var api = LoadMultilingual_API("ActiveAccount");
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.GetRequest();
            }, function (response) { console.log(response); });
        },
        ChangeLan: function () {
            var api = ChangeLan_API("ActiveAccount");
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
            }, function (response) { console.log(response); });
        },
        GetRequest: function () {
            var url = location.search; //获取url中"?"符后的字串 
            var theRequest = new Object();
            if (url.indexOf("?") != -1) {
                var str = url.substr(1);
                strs = str.split("&");
                for (var i = 0; i < strs.length; i++) {
                    theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                }
            }
            this.account.CUSTOMER_NAME = theRequest.username;
            this.account.ACTIVECODE = theRequest.activecode;
            this.ActiveAccount();
        },
        //Tologin: function () {
        //    window.location.href = "../Login/Login.html";
        //}
    }
});



