$(function () {
    console.log("yah");
        $(".pickupdate").datepicker();
        
        $(".returndate").datepicker();
});


//$('#uservehicle').on('click', function () {
//    $(this).closest('.thumbnail').addClass('selectedVehicle');
//});

$('#carInventoryFilter').on('change', function (event) {
    console.log("supping");
    var form = $(event.target).parents('form');

    form.submit();
})