$(document).ready(function () {

    $(".anchorDetail").click(function () {
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');

        callTicketBookingPopup(id);
    });


    $(".Ticketdetails").click(function () {
        alert();

        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        alert(id);
        checkticketdetails(id);
    });

});

function checkticketdetails(id) {
    var options = { "backdrop": "static", keyboard: true };
    var TeamDetailPostBackURL = '/Booking/MyTicketDetails';
    $.ajax({
        type: "GET",
        url: TeamDetailPostBackURL,
        contentType: "application/json; charset=utf-8",
        data: { "Id": id },
        datatype: "json",
        success: function (data) {

            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#myModal').modal('show');

        },
        error: function (XHR, errorText, status) {
            console.log(XHR.responseText);
        }
    });
}

function callTicketBookingPopup(id) {


    alert(id);
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/Booking/GetMoviePop',

        data: { "Id": id },

        success: function (data) {
            alert('Suss');
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#myModal').modal('show');

        },
        error: function (XHR, errorText, status) {
            console.log(XHR.responseText);
            alert(XHR.responseText);
        }
    });
}