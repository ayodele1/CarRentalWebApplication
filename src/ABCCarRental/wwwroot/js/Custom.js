$(function () {
    console.log("yah");
        $(".pickupdate").datepicker();
        
        $(".returndate").datepicker();
});


$('#uservehicle').on('click', function () {
    $(this).closest('.thumbnail').addClass('selectedVehicle');
});