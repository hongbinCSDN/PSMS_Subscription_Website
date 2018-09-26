Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        customer: {}
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
        RegisterCustomerAccount: function () {    
            var confirm_password = document.getElementById("signup-confirm-password").value;
            var pat = new RegExp("[^a-zA-Z0-9\_\u4e00-\u9fa5]", "i");
            if (this.customer.CUSTOMER_ID == null || this.customer.CUSTOMER_ID == "") {
                //alert(this.PromptMessage[8].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[8].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (pat.test(this.customer.CUSTOMER_ID) == true) {
                //alert(this.PromptMessage[19].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[19].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.customer.PASSWORD == null || this.customer.PASSWORD == "") {
                //alert(this.PromptMessage[9].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[9].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (confirm_password == null || confirm_password == "") {
                //alert(this.PromptMessage[18].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[18].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.customer.CUSTOMER_NAME == null || this.customer.CUSTOMER_NAME == "") {
                //alert(this.PromptMessage[10].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[10].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.customer.EMAIL == null || this.customer.EMAIL == "") {
                //alert(this.PromptMessage[11].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[11].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.customer.COMPANY == null || this.customer.COMPANY == "") {
                //alert(this.PromptMessage[12].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[12].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.customer.PHONE == null || this.customer.PHONE == "") {
                //alert(this.PromptMessage[13].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[13].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }           
            if (this.customer.PHONE.length != 8 || isNaN(this.customer.PHONE) == true) {
                //alert(this.PromptMessage[14].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[14].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }                 
            if (this.Ischeckmail() == false) {
                //alert(this.PromptMessage[15].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[15].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            if (this.customer.FIXED_TELEPHONE == null) {
                this.customer.FIXED_TELEPHONE = "";
            }
            if (this.customer.FIXED_TELEPHONE.length > 15) {
                //alert(this.PromptMessage[22].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[22].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }   
            if (this.customer.PASSWORD != confirm_password) {
                //alert(this.PromptMessage[16].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[16].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            var res = verifyCode.validate(document.getElementById("code_input").value);
            if (!res) {
                //alert(this.PromptMessage[5].Describe);
                WarningMessage('WarningMessage', this.PromptMessage[5].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            var api = GetCommonAPI('System/Register');
            this.$http.post(api, this.customer).then(function (response) {            
                var dt = response.body;
                if (dt.Data == 1) {
                    //alert(this.PromptMessage[3].Describe);
                    //window.location.href = "../Login/Login.html";
                    WarningMessage_Link('WarningMessage', this.PromptMessage[3].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();                
                }
                else if (dt.Data == 0) {
                    //alert(this.PromptMessage[4].Describe);
                    WarningMessage('WarningMessage', this.PromptMessage[4].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
                else if (dt.Data == -1) {                  
                    //alert(this.PromptMessage[21].Describe);
                    WarningMessage('WarningMessage', this.PromptMessage[21].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
                else if (dt.Data == -2) {                  
                    //alert(this.PromptMessage[4].Describe + this.PromptMessage[21].Describe);
                    WarningMessage('WarningMessage', this.PromptMessage[4].Describe + this.PromptMessage[21].Describe , this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                    $("#myModal").modal();
                }
            }, function (response) {
                console.log(response);
            })                  
        },
        LoadMultilingual: function() {
            var api = LoadMultilingual_API_IE("Register"); // Modify By Chester 2018.08.16
            this.$http.get(api).then(function(response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
            }, function(response) { console.log(response); });
        },
        ChangeLan: function() {
            var api = ChangeLan_API_IE("Register");
            this.$http.get(api).then(function(response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
            }, function(response) { console.log(response); });
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
        }
    }
});