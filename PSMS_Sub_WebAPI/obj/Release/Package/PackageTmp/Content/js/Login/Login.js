Vue.http.options.emulateJSON = true;
var Token = '';
var vm = new Vue({
    el: '#app',
    data: {
        customer: {}
        , session: {}
        , Multilingual: []
        , PromptMessage: []
        , DropDownLanguage: []
        , Navigation: []
        , Language_Value: "2"
    },
    mounted: function () {
        this.LoadMultilingual()
    },
    methods: {
        Login: function () {
            if ((this.customer.CUSTOMER_ID != null && this.customer.CUSTOMER_ID != "") && (this.customer.PASSWORD != null && this.customer.PASSWORD != "")) {

                var api = GetCommonAPI("System/Login");
                this.$http.post(api, this.customer).then(function (response) {
                    var data = response.body;
                    if (data == "0") {
                        WarningMessage('WarningMessage', this.PromptMessage[1].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    } else if (data == "-1") {
                        WarningMessage('WarningMessage', this.PromptMessage[7].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    } else if (data == "-2") {
                        WarningMessage('WarningMessage', this.PromptMessage[34].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else {
                        if (this.GetQueryString("Type") == "renew") {
                            window.location.href = "../Index/SubscriptionPlanList.html";
                        } else {
                            window.location.href = "../Index/Index.html";
                        }
                    }
                },
                    function (response) {
                        console.log(response);
                    });
            }
            else {
                //add by Haskin 2018.08.13
                WarningMessage('WarningMessage', this.PromptMessage[2].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                //end
            }
        },
        GetQueryString: function (name) {
            var queryString = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (queryString == null || queryString.length < 1) {
                return "";
            }
            return queryString[1];
        },
        LoadMultilingual: function () {
            var api = LoadMultilingual_API("Login");
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();

            }, function (response) { console.log(response); });
        },
        ChangeLan: function () {
            var api = ChangeLan_API("Login");
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
            }, function (response) { console.log(response); });
        }



    }
});


