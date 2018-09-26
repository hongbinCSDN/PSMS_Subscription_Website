function GetCommonAPI(func) {    
    var http_url = 'http://localhost:27018/' + func;   
    return http_url;
}
function GetCommonAPI_IE(func) {
    var ran = Math.random();
    var http_url = 'http://localhost:27018/' + func + '?' + ran;  
    return http_url;
}