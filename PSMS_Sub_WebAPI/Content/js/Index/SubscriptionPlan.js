Vue.http.options.emulateJSON = true;
var vm = new Vue({
    el: '#app',
    data: {
        Multilingual: [],
        PromptMessage: [],
        DropDownLanguage: [],
        Navigation: [],
        Language_Value: "2",
        Token: "",
        payment: {},
        showdiv: true,
        showdiv2: false,
        showdiv3: false,
        more: "",
        more2: "",
        more3: "",
        selectitem: "",
        UserInfo: {}  //Modify by bill 2018-6-17
    },
    mounted: function () {        
        this.LoadMultilingual()
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
                    WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                    $("#myModal").modal();
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
            var api = LoadMultilingual_API_IE("SubscriptionPlan")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;               
                this.DropDownLanguage = response.body.Table;
                this.Navigation = response.body.Table3;
                this.Language_Value = ViewDropDownLanguage();
                this.selectitem = this.Multilingual[50].Describe            
                this.GetToken();              
            }, function (response) { console.log(response); });
        },
        ChangeLan: function () {
            var api = ChangeLan_API_IE("SubscriptionPlan")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.more = this.Multilingual[9].Describe           
                this.selectitem = this.Multilingual[50].Describe
                
            }, function (response) { console.log(response); });
        },
        CheckDomainName: function (category,btn) {
           var api = GetCommonAPI('Payment/CheckDomain');
            var domain_name = null;
            domain_name = this.$refs.domain.value;
            if (domain_name == "" || domain_name == null) {
                //alert(this.PromptMessage[27].Describe);
       
                WarningMessage('WarningMessage', this.PromptMessage[27].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
               
                $("#myModal").modal('show');
               
               return;
            }
            //Add by bill 2018.8.20
            var pat = new RegExp("[^a-zA-Z0-9\_\u4e00-\u9fa5]", "i");
            if (pat.test(domain_name) == true) {
                WarningMessage('WarningMessage', this.PromptMessage[31].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                $("#myModal").modal();
                return;
            }
            //End  
            if (this.Token != null && this.Token != "") {
                this.$http.get(api, { params: { domain_name: domain_name }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    var message = response.body;
                    console.log(message);
                    if (message == "0") {                                              
                        WarningMessage('WarningMessage', this.PromptMessage[25].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else if (message != "0" && message != "1") {                        
                        WarningMessage('WarningMessage', this.PromptMessage[26].Describe + message, this.Navigation[9].Describe, this.PromptMessage[29].Describe);
                        $("#myModal").modal();
                    }
                    else {
                        WarningMessageAddbutton2("WarningMessage", this.Multilingual[52].Describe, domain_name + ".pssasl.com", this.Navigation[9].Describe, this.PromptMessage[30].Describe, this.PromptMessage[29].Describe, category,domain_name);
                        $("#myModal").modal();
                        //this.PaymentSubmit(category, domain_name);
                    }
                }, function (response) {
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal(); 
                });
            }
            else {               
                WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                $("#myModal").modal();
            }

        },
        PaymentSubmit: function (category, domain_name) {         
            if (this.Token != null && this.Token != "") {
                var form = document.getElementById('payFormCcard');
                var api = GetCommonAPI_IE("Payment/GetProductPostForm");
                this.$http.get(api, { params: { category: category }, headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
                    var data = response.body.Table[0];
                    this.$refs.merchantId.value = data.MERCHANTID;
                    this.$refs.amount.value = data.AMOUNT;
                    this.$refs.orderRef.value = data.ORDERREF;
                    this.$refs.currCode.value = data.CURRCODE;
                    this.$refs.mpsMode.value = data.MPSMODE;
                    this.$refs.successUrl.value = data.SUCCESSURL;
                    this.$refs.failUrl.value = data.FAILURL;
                    this.$refs.cancelUrl.value = data.CANCELURL;
                    this.$refs.payType.value = data.PAYTYPE;
                    this.$refs.lang.value = data.LANG;
                    this.$refs.payMethod.value = data.PAYMETHOD;
                    this.$refs.secureHash.value = data.SECUREHASH;
                    this.CreateOrder(data.ORDERREF, data.AMOUNT, category, domain_name);
                    form.submit();
                }, function (response) {              
                    WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                    $("#myModal").modal(); 
               });
            }
            else {               
                WarningMessage_Link('WarningMessage', this.PromptMessage[20].Describe, this.Navigation[9].Describe, this.PromptMessage[29].Describe, "'../Login/Login.html'");
                $("#myModal").modal();
            }
        },
        CreateOrder: function (orderRef, amount, category, domain_name) {
            var api = GetCommonAPI('Payment/CreateOrder');
            this.payment.ORDERREF = orderRef;
            this.payment.AMOUNT = amount;
            this.payment.PAYMENT_TYPE_ID = category;
            this.payment.DOMAIN_NAME = domain_name;
            this.$http.post(api, this.payment, { headers: { Authorization: 'bearer ' + this.Token } }).then(function (response) {
            }, function (response) {
                WarningMessageAddbutton3('WarningMessage', response.body, this.Navigation[9].Describe, this.PromptMessage[29].Describe, 'vm.RemoveToken()');
                $("#myModal").modal(); 
            });
        },
   
        Select: function (id) {
            document.getElementById("right" + id).style.display = "";
            document.getElementById("left" + id).style.display = "none";
            this. Getheight();           
        },
        Cancel: function (id) {
            document.getElementById("right" + id).style.display = "none";
            document.getElementById("left" + id).style.display = "";
            this.Getheight();
        },
        ShowUserdefined: function () {          
            document.getElementById("plan").style.display = "none";
            document.getElementById("userdefined").style.display = "";
            this.Getheight();
        },
        ShowPlan: function () {
            document.getElementById("plan").style.display = "";
            document.getElementById("userdefined").style.display = "none";
        },
        Getheight: function () {
            if ($(window).width() > 1180) {
                var height = $("#SelectedFeatures").height();               
                $("#allselect").css("margin-top", 0.38 * height + "px");
                $("#middleDiv").height($("#SelectedFeatures").height());
            }
        },
        dropdownShow: function (item) {                    
            if (item == "" || item==null) {
                item = this.Multilingual[50].Describe;
            }
            this.selectitem = item;
        },
        Allselect: function () {  
            document.getElementById("right6").style.display = "";
            document.getElementById("right7").style.display = "";
            document.getElementById("right8").style.display = "";
            document.getElementById("right9").style.display = "";
            document.getElementById("right10").style.display = ""; 
            document.getElementById("left6").style.display = "none";
            document.getElementById("left7").style.display = "none";
            document.getElementById("left8").style.display = "none";
            document.getElementById("left9").style.display = "none";
            document.getElementById("left10").style.display = "none";
            this.Getheight();
        },
        CancelAllselect: function () {
            document.getElementById("right6").style.display = "none";
            document.getElementById("right7").style.display = "none";
            document.getElementById("right8").style.display = "none";
            document.getElementById("right9").style.display = "none";
            document.getElementById("right10").style.display = "none"; 
            document.getElementById("left6").style.display = "";
            document.getElementById("left7").style.display = "";
            document.getElementById("left8").style.display = "";
            document.getElementById("left9").style.display = "";
            document.getElementById("left10").style.display = "";
            this.Getheight();
        },
        ShowFeatures: function() {
            if (this.showdiv == true) {
               
                this.showdiv = false;
                document.getElementById("circle").className += " rotate";
                
                document.getElementById("circle").addEventListener("webkitTransitionEnd", function () {
                    $(".featuers").show("solw");
                    document.getElementById("puls").innerHTML = '<i id="circle" style="color:#59b8bc;" class="fa fa-minus-circle" aria-hidden="true"></i>';
                })
            
            }
            else {
               
                this.showdiv = true;
                document.getElementById("circle").className += " rotate";
               
                document.getElementById("circle").addEventListener("webkitTransitionEnd", function () {
                    $(".featuers").hide("solw");
                    document.getElementById("puls").innerHTML = '<i id="circle" style="color:#59b8bc;" class="fa fa-plus-circle " aria-hidden="true"></i>';
                })
            }
        },
      
        }                        
    }
);

