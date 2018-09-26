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
        isdivcenter: true,
        UserInfo: {}  //Modify by bill 2018-6-17
    },
    mounted: function () {
            this.LoadMultilingual(),
            this.shownavigation()          
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
            var api = LoadMultilingual_API("Features")
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
            var api = ChangeLan_API("Features")
            this.$http.get(api).then(function (response) {
                this.Multilingual = response.body.Table1;
                this.PromptMessage = response.body.Table2;
                this.Navigation = response.body.Table3;
                this.more = this.Multilingual[9].Describe
               

            }, function (response) { console.log(response); });
        },
        SetImghever: function () {
            document.getElementsByClassName("item1")[0].style.cursor = "url('../../Content/images/features/zoomout-1.ico'), auto";          

        },
        clcikactive: function (id) {
            $(".liactive").removeClass("liactive");
            $("#" + id).addClass("liactive");
        },
        shownavigation: function () {
            var istrue1 = istrue2 = istrue3 = istrue4 = istrue5 = istrue6 = istrue7 = istrue8 = 0, isdivcenter = 0;
           
            $(window).scroll(function () {
                
                var th = $(window).scrollTop();
                var carousel = $('#myCarousel').outerHeight();
                var nav_1 = $('#1').outerHeight();
                var nav_2 = $('#2').outerHeight();
                var nav_3 = $('#3').outerHeight();
                var nav_4 = $('#4').outerHeight();
                var nav_5 = $('#5').outerHeight();
                var nav_6 = $('#6').outerHeight();
                var nav_7 = $('#7').outerHeight();
                if (th > 100 && th < 600) {
                    $(".liactive").removeClass("liactive");
                    $("#nav1").addClass("liactive");
                }
                if (th > carousel && th < carousel + nav_1) {

                    $(".liactive").removeClass("liactive");
                    $("#nav2").addClass("liactive");
                }
                if (th > carousel + nav_1 && th < carousel + nav_1 + nav_2) {
                    $(".liactive").removeClass("liactive");
                    $("#nav3").addClass("liactive");
                }
                if (th > carousel + nav_1 + nav_2 && carousel + nav_1 + nav_2 + nav_3) {
                    $(".liactive").removeClass("liactive");
                    $("#nav4").addClass("liactive");
                }
                if (th > carousel + nav_1 + nav_2 + nav_3 && th < carousel + nav_1 + nav_2 + nav_3 + nav_4) {
                    $(".liactive").removeClass("liactive");
                    $("#nav5").addClass("liactive");
                }
                if (th > carousel + nav_1 + nav_2 + nav_3 + nav_4 && carousel + nav_1 + nav_2 + nav_3 + nav_4 + nav_5) {
                    $(".liactive").removeClass("liactive");
                    $("#nav6").addClass("liactive");
                }
                if (th > carousel + nav_1 + nav_2 + nav_3 + nav_4 + nav_5 && th < carousel + nav_1 + nav_2 + nav_3 + nav_4 + nav_5 + nav_6) {
                    $(".liactive").removeClass("liactive");
                    $("#nav7").addClass("liactive");
                }
                if (th > carousel + nav_1 + nav_2 + nav_3 + nav_4 + nav_5 + nav_6) {
                    $(".liactive").removeClass("liactive");
                    $("#nav8").addClass("liactive");
                }






                //滚轮滚到某个时刻发生动作 The roller active at some point
                if (th > $("#1").offset().top - 600 && istrue1 == 0) {
                    document.getElementById("img1").style.visibility = "visible"
                    document.getElementById("con1").style.visibility = "visible"
                    document.getElementById("con1").className += " company_nameright";
                    document.getElementById("img1").className += " company_nameleft"
                    istrue1 = 1;
                }
                if (th > $("#2").offset().top - 600 && istrue2 == 0) {
                    document.getElementById("img2").style.visibility = "visible"
                    document.getElementById("con2").style.visibility = "visible"
                    document.getElementById("con2").className += " company_nameright";
                    document.getElementById("img2").className += " company_nameleft"
                    istrue2 = 1;
                }
                if (th > $("#3").offset().top - 600 && istrue3 == 0) {
                    document.getElementById("img3").style.visibility = "visible"
                    document.getElementById("con3").style.visibility = "visible"
                    document.getElementById("con3").className += " company_nameright";
                    document.getElementById("img3").className += " company_nameleft"
                    istrue3 = 1;
                }
                if (th > $("#4").offset().top - 600 && istrue4 == 0) {
                    document.getElementById("img4").style.visibility = "visible"
                    document.getElementById("con4").style.visibility = "visible"
                    document.getElementById("con4").className += " company_nameright";
                    document.getElementById("img4").className += " company_nameleft"
                    istrue4 = 1;
                }
                if (th > $("#5").offset().top - 600 && istrue5 == 0) {
                    document.getElementById("img5").style.visibility = "visible"
                    document.getElementById("con5").style.visibility = "visible"
                    document.getElementById("con5").className += " company_nameright";
                    document.getElementById("img5").className += " company_nameleft"
                    istrue5 = 1;
                }
                if (th > $("#6").offset().top - 600 && istrue6 == 0) {
                    document.getElementById("img6").style.visibility = "visible"
                    document.getElementById("con6").style.visibility = "visible"
                    document.getElementById("con6").className += " company_nameright";

                    document.getElementById("img6").className += " company_nameleft"
                    istrue6 = 1;
                }
                if (th > $("#7").offset().top - 600 && istrue7 == 0) {
                    document.getElementById("img7").style.visibility = "visible"
                    document.getElementById("con7").style.visibility = "visible"

                    document.getElementById("con7").className += " company_nameright";
                    document.getElementById("img7").className += " company_nameleft"
                    istrue7 = 1;
                }
                
                if (th > 600 && th < 4000) {
                    
                    document.getElementById("navigation").style.display = "";                

                }
                else {
                    document.getElementById("navigation").style.display = "none";
                }
               
              
                ///let the div of item1 and div of item2 vertical center parent div 
                //add by Haskin 20180831
                if ($(".item2:eq(0)").outerHeight() == 78 || isdivcenter>2) {
                  //  console.log($(".item2:eq(0)").outerHeight());
                   
                }
                else{
                    //console.log("123");
                if ($(".item2:eq(0)").outerHeight() > $(".item1:eq(0)").outerHeight()) {

                    var height = ($(".item2:eq(0)").outerHeight() - $(".item1:eq(0)").outerHeight()) / 2
                    document.getElementsByClassName("item1")[0].style.marginTop = height + "px"
                    document.getElementsByClassName("item2")[0].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(0)").outerHeight() - $(".item2:eq(0)").outerHeight()) / 2
                    document.getElementsByClassName("item2")[0].style.marginTop = height1 + "px"
                    document.getElementsByClassName("item1")[0].style.marginTop = 0 + "px";
                }
                if ($(".item2:eq(1)").outerHeight() > $(".item1:eq(1)").outerHeight()) {
                    var height = ($(".item2:eq(1)").outerHeight() - $(".item1:eq(1)").outerHeight()) / 2
                    document.getElementsByClassName("item1")[1].style.marginTop = height + "px"
                    document.getElementsByClassName("item2")[1].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(1)").outerHeight() - $(".item2:eq(1)").outerHeight()) / 2
                    document.getElementsByClassName("item2")[1].style.marginTop = height1 + "px"
                    document.getElementsByClassName("item1")[1].style.marginTop = 0 + "px";
                }

                if ($(".item2:eq(2)").outerHeight() > $(".item1:eq(2)").outerHeight()) {

                    var height = ($(".item2:eq(2)").outerHeight() - $(".item1:eq(2)").outerHeight()) / 2;
                    document.getElementsByClassName("item1")[2].style.marginTop = height + "px"
                    document.getElementsByClassName("item2")[2].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(2)").outerHeight() - $(".item2:eq(2)").outerHeight()) / 2;
                    document.getElementsByClassName("item2")[2].style.marginTop = height1 + "px";
                    document.getElementsByClassName("item1")[2].style.marginTop = 0 + "px";
                }

                if ($(".item2:eq(3)").outerHeight() > $(".item1:eq(3)").outerHeight()) {

                    var height = ($(".item2:eq(3)").outerHeight() - $(".item1:eq(3)").outerHeight()) / 2;
                    document.getElementsByClassName("item1")[3].style.marginTop = height + "px";
                    document.getElementsByClassName("item2")[3].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(3)").outerHeight() - $(".item2:eq(3)").outerHeight()) / 2;
                    document.getElementsByClassName("item2")[3].style.marginTop = height1 + "px";
                    document.getElementsByClassName("item1")[3].style.marginTop = 0 + "px";
                }
                if ($(".item2:eq(4)").outerHeight() > $(".item1:eq(4)").outerHeight()) {

                    var height = ($(".item2:eq(4)").outerHeight() - $(".item1:eq(4)").outerHeight()) / 2;

                    document.getElementsByClassName("item1")[4].style.marginTop = height + "px";
                    document.getElementsByClassName("item2")[4].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(4)").outerHeight() - $(".item2:eq(4)").outerHeight()) / 2;
                    document.getElementsByClassName("item2")[4].style.marginTop = height1 + "px";
                    document.getElementsByClassName("item1")[4].style.marginTop = 0 + "px";
                    
                }

             
                if ($(".item2:eq(5)").outerHeight() > $(".item1:eq(5)").outerHeight()) {

                    var height = ($(".item2:eq(5)").outerHeight() - $(".item1:eq(5)").outerHeight()) / 2

                    document.getElementsByClassName("item1")[5].style.marginTop = height + "px"
                    document.getElementsByClassName("item2")[5].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(5)").outerHeight() - $(".item2:eq(5)").outerHeight()) / 2
                    document.getElementsByClassName("item2")[5].style.marginTop = height1 + "px"
                    document.getElementsByClassName("item1")[5].style.marginTop = 0 + "px";
                }
                if ($(".item2:eq(6)").outerHeight() > $(".item1:eq(6)").outerHeight()) {

                    var height = ($(".item2:eq(6)").outerHeight() - $(".item1:eq(6)").outerHeight()) / 2

                    document.getElementsByClassName("item1")[6].style.marginTop = height + "px"
                    document.getElementsByClassName("item2")[6].style.marginTop = 0 + "px";
                }
                else {

                    var height1 = ($(".item1:eq(6)").outerHeight() - $(".item2:eq(6)").outerHeight()) / 2
                    document.getElementsByClassName("item2")[6].style.marginTop = height1 + "px"
                    document.getElementsByClassName("item1")[6].style.marginTop = 0 + "px";
                }
                 
                    isdivcenter++;

                 }
               


            });

        },     
        Imgdetails: function (src) {
            $("#chgImg").attr("src", src);
            document.body.style.overflow = "hidden";
            $("#imgdetails").fadeIn("slow");
        },
        bigimg: function (obj) {
            var zoom = parseInt(obj.style.zoom, 10) || 100;
            zoom += event.wheelDelta / 10;
            if (zoom > 50 && zoom <= 180) {
                obj.style.zoom = zoom + '%';
                document.getElementById("chgImg").style.cursor = "url('../../Content/images/features/zoomout-1.ico'), auto";             
               
            }
            if (zoom > 180 && zoom <= 200) {
                obj.style.zoom = zoom + '%';
               
                document.getElementById("chgImg").style.cursor = "url('../../Content/images/features/zoomout-2.ico'), auto";
               
            }
            return false;
        },
        fadeout: function () {
            document.body.style.overflow = "auto";
            document.getElementById("chgImg").style.zoom = "100%";
            document.getElementById("imgdetails").style.display = "none"
        },
        Divhover: function (className) {
              //Modify by Haskin 20180919
            document.getElementsByClassName("topboder")[className].style.width = "100%";
            document.getElementsByClassName("leftboder")[className].style.height = "100%";
            document.getElementsByClassName("rightboder")[className].style.height = "100%";
            document.getElementsByClassName("bottomboder")[className].style.width = "100%";
              //Modify by Haskin 20180919
        },
        Divhoverout: function (className) {
            //Modify by Haskin 20180919
            document.getElementsByClassName("topboder")[className].style.width = "0%";
            document.getElementsByClassName("leftboder")[className].style.height = "0%";
            document.getElementsByClassName("rightboder")[className].style.height = "0%";
            document.getElementsByClassName("bottomboder")[className].style.width = "0%";

            //end
            


        },
    },

});



