function WarningMessage(id, warningmessage, header, confirm) {
    
    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1"  class="modal  fade" id="myModal"><div  class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>'+header+'</stong></p> </div > '+
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-exclamation-circle "></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        ' <button id="confirm" type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn btn-block btn-cta-primary" data-dismiss="modal">' + confirm + '</button></div></div></div ></div > ';


    
}
//Add by Haskin 20181026
//the icon is success
function WarningMessage_checkIcon(id, warningmessage, header, confirm) {

    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1"  class="modal  fade" id="myModal"><div  class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc;height:100px" class="fa fa-check-circle"></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        ' <button id="confirm" type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn btn-block btn-cta-primary" data-dismiss="modal">' + confirm + '</button></div></div></div ></div > ';



}
//Add by Haskin 20181026
//the icon is success 

function WarningMessage_Link_checkIcon(id, warningmessage, header, confirm, link) {
    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1" class="modal" id="myModal"><div class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-check-circle "></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        ' <button type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn btn-block btn-cta-primary" data-dismiss="modal" onclick="window.location.href=' + link + '">' + confirm + '</button></div></div></div ></div > ';
}
function WarningMessage_Link(id, warningmessage, header, confirm, link) {
    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1" class="modal" id="myModal"><div class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-exclamation-circle "></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        ' <button type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn btn-block btn-cta-primary" data-dismiss="modal" onclick="window.location.href=' + link + '">' + confirm + '</button></div></div></div ></div > ';
}
function WarningMessageAddbutton(id, warningmessage, header, cancel, confirm, optionfunction) {
   
   
    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1"  class="modal" id="myModal"><div  class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-exclamation-circle "></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        '<div class="row" > <div class=" col-xs-6 col-sm-6 col-md-6   col-lg-6" > <button type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn " data-dismiss="modal">' + cancel + '</button></div><div class=" col-xs-6 col-sm-6 col-md-6   col-lg-6" ><button style=" margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" type="button" onclick=' + optionfunction + ' class="btn btn-block btn-cta-primary" >' + confirm + '</button ></div ></div ></div ></div ></div ></div > ';
}
//function WarningMessageAddbutton2(id, warningmessage, website, header, cancel, confirm, category, domain_name) {
//    $(".modal-backdrop").remove();
//    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1"  class="modal" id="myModal"><div  class="modal-dialog modal-md">' +
//        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
//        '<div class="modal-body">' +
//        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-exclamation-circle "></i></p> <br /> ' +
//        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
//        '<p class="text text-center" style="color:black;font-size:x-large">' + website + ' </p>' +
//        '</div> <div class="modal-footer" style="border-top:none">' +
//        '<div class="row" > <div class=" col-xs-6 col-sm-6 col-md-6   col-lg-6" > <button type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn " data-dismiss="modal">' + cancel + '</button></div><div class=" col-xs-6 col-sm-6 col-md-6   col-lg-6" ><button style=" margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" type="button" onclick=vm.PaymentSubmit(' + category + ',"' + domain_name + '")  class="btn btn-block btn-cta-primary" >' + confirm + '</button ></div ></div ></div ></div ></div ></div > ';
//}

function WarningMessageAddbutton2(id, warningmessage, website, header, cancel, confirm, category, domain_name, payway) {
    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1"  class="modal" id="myModal"><div  class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-exclamation-circle "></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + website + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        '<div class="row" > <div class=" col-xs-6 col-sm-6 col-md-6   col-lg-6" > <button type="button" style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn " data-dismiss="modal">' + cancel + '</button></div><div class=" col-xs-6 col-sm-6 col-md-6   col-lg-6" ><button style=" margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" type="button" onclick=vm.PaymentSubmit(' + category + ',"' + domain_name + '",'+payway+')  class="btn btn-block btn-cta-primary" >' + confirm + '</button ></div ></div ></div ></div ></div ></div > ';
}

function WarningMessageAddbutton3(id, warningmessage, header, confirm, optionfunction) {
    $(".modal-backdrop").remove();
    document.getElementById(id).innerHTML = '<div data-keyboard="false" data-backdrop="static" tabindex="-1" class="modal" id="myModal"><div  class="modal-dialog modal-md">' +
        ' <div style="margin-top:30%" class="modal-content"> <div style="background:#59b8bc;height:60px;border-radius:5px;"  class="modal-header"> <p style="font-family:inherit; text-align:center;color:white;line-height:26px;font-size:26px"><stong>' + header + '</stong></p> </div > ' +
        '<div class="modal-body">' +
        '<p style="text-align:center;"> <i style="font-size:100px;color:#59b8bc" class="fa fa-exclamation-circle "></i></p> <br /> ' +
        '<p class="text text-center" style="color:black;font-size:x-large">' + warningmessage + ' </p>' +
        '</div> <div class="modal-footer" style="border-top:none">' +
        ' <button type="button" onclick = ' + optionfunction + ' style="height:-20%; margin-left:auto;margin-right:auto; width :60%;font-family:inherit;" class="btn btn-block btn-cta-primary" data-dismiss="modal">' + confirm + '</button></div></div></div ></div > ';
} 