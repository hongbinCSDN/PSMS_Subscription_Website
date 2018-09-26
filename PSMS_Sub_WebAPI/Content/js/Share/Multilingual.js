function LoadMultilingual_API(PageName) {
    var Multil_Cookie = document.cookie;
    var Multilingual_Id = null;
    var api = null;
    if (Multil_Cookie != null && Multil_Cookie != "") {
        var Multilingual_Id = document.cookie.split(";")[0].split("=")[1];
        var httpurl = GetCommonAPI('Multilingual/');
        //api = 'http://localhost:27018/Multilingual/' + PageName+'/' + Multilingual_Id;
        api = httpurl + PageName + '/' + Multilingual_Id;
    }
    else {
        Multilingual_Id = "2";
        //api = 'http://localhost:27018/Multilingual/' + PageName + '/' + Multilingual_Id;
        var httpurl = GetCommonAPI('Multilingual/');
        api = httpurl + PageName + '/' + Multilingual_Id;
    }
    //var MultilSelect = document.getElementById("MultilSelect");    MultilSelect.options.length
    //for (i = 0; i < SelectOptionLength ; i++) { 
    //    if (MultilSelect.options[i].value == Multilingual_Id) {
    //       MultilSelect.options[i].selected = true;
    //        break;
    //   }
    //}
    return api;
}

//Modify / Add by Chester 2018.08.16
function LoadMultilingual_API_IE(PageName) {
    var Multil_Cookie = document.cookie;
    var Multilingual_Id = null;
    var rand = Math.random();
    var api = null;
    if (Multil_Cookie != null && Multil_Cookie != "") {
        var Multilingual_Id = document.cookie.split(";")[0].split("=")[1];
        var httpurl = GetCommonAPI('Multilingual/');
        api = httpurl + PageName + '/' + Multilingual_Id + '?' + rand;
    }
    else {
        Multilingual_Id = "2";
        var httpurl = GetCommonAPI('Multilingual/');
        api = httpurl + PageName + '/' + Multilingual_Id + '?' + rand;
    }
    return api;
}
//Modify End

function ChangeLan_API(PageName) {
    var value = $('select option:selected').val();
    document.cookie = "Multilingual_Id=" + value + "; path=/";
    //var api = 'http://localhost:27018/Multilingual/' + PageName + '/' + value;
    var httpurl = GetCommonAPI('Multilingual/');
    api = httpurl + PageName + '/' + value;
    return api;
}

//Modify / Add By Chester 2018.08.14
function ChangeLan_API_IE(PageName) {
    var value = $('select option:selected').val();
    document.cookie = "Multilingual_Id=" + value + "; path=/";
    //var api = 'http://localhost:27018/Multilingual/' + PageName + '/' + value;
    var httpurl = GetCommonAPI('Multilingual/');
    var rand = Math.random();
    api = httpurl + PageName + '/' + value + '?' + rand;
    return api;
}
//Modify End

function ViewDropDownLanguage() {
    var Multil_Cookie = document.cookie;
    var Multilingual_Id ="2";
    if (Multil_Cookie != null && Multil_Cookie != "") {
        var Multilingual_Id = document.cookie.split(";")[0].split("=")[1];        
    }
    return Multilingual_Id; 
}