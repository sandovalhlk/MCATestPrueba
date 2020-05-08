$(document).ready(function () {

});



$("#perfilId").on("click",function () {
    
    $("#rolActualLbl").text(localStorage.getItem("rolActual"));
});

//$('#logoutForm').click(function () {
//    //localStorage.removeItem("rolActual");
//    localStorage.clear();

//});