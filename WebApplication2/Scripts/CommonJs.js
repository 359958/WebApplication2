$(document).ready(function () {

    //$(".anchorDetail").click(function (e) {
    //    var $buttonClicked = $(this);
    //    var id = $buttonClicked.attr('data-id');

    //    callTicketBookingPopup(id);
    //});

    $(".CheckAvaliblity").click(function () {




        var $buttonClicked = $(this);
        var Movie = $buttonClicked.attr('data-id');
        callCheckavaliblity(Movie);



    });


    $(".Ticketdetails").click(function () {
        

        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        alert(id);
        checkticketdetails(id);
    });

    $("#btnBooktic").click(function () {
        var ClassId = $("#Showid").val();
        var BookingDate = $("#BookingDate").val();
        var Showid = $("#Showid").val();
        var NoTickets = $("#NoTickets").val();


        if (BookingDate == '') {
            alert('Please Select Show Date');
            return false;

        }
        else if (Showid == '') {
            alert('Please Select Show');
            return false;

        } else if (NoTickets == '') {
            alert('Please Select No of tickets');
            return false;

        }

    });



    $("#addMovieAdmin").click(function () {
        var Screen = $("#Screen").val();
        var Movie = $("#Movie").val();
        var from = $("#from").val();
        var date = $("#date").val();
        var path = $("#path").val();


        if (Screen == '') {
            alert('Please Select Screen');
            return false;

        }
        else if (Movie == '') {
            alert('Please Enter Movie name');
            return false;

        } else if (from == '') {
            alert('Please Enter From date');
            return false;

        }
        else if (date == '') {
            alert('Please Enter Upto date');
            return false;

        }
        else if (path == '') {
            alert('Please upload Image');
            return false;

        }

    });
});
var apiUrl = '@(System.Configuration.ConfigurationManager.AppSettings["WebapiUrl"].ToString())';
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


 
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/Booking/GetMoviePop',
        async: false,
        data: { "Id": id },

        success: function (data) {
            
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

function callCheckavaliblity(mid) {

    var fullDate = new Date();

    var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);

    var currentDate = twoDigitMonth + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/Booking/GetTicketStatus',
        async: false,
        data: { "date": currentDate, "movieID": mid },

        success: function (data) {
        
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

function CallReview(apiUrl) {
   
   

    $.ajax({
        type: "GET",
        url: apiUrl + "/admin/Review",
        success: function (data) {
            console.log(data);
            cogazureservice(data, apiUrl);
        },
        failure: function (data) {
            alert(xhr.status);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });


}

function cogazureservice(rstdata, apiUrl) {

    //var output = {};
    var documents = [];
    ;

    $.ajax({
        type: "POST",
        url: "https://uksouth.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment",
        headers: {
            "Ocp-Apim-Subscription-Key": "08f0ecdddba94967aff3c79493add13a",
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        data: rstdata,
        success: function (result) {
            SqlData(result.documents, apiUrl);
            $.each(result.documents, function (key, data) {

                documents.push({
                    "score": data.score,
                    "id": data.id
                });
            });
            //output.documents = documents;
            var jsonData = JSON.stringify(documents);
            console.log('---');
            console.log(jsonData);
            //SqlData(documents);

        },
        failure: function (data) {
            alert(xhr.status);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });

}

function SqlData(rstdata, apiUrl) {
   
    //console.log(rstdata);

    var Scores = { scores: rstdata };
    console.log('datamov');
    //console.log(datamov);
    $("#experience").html('');
    var list = $("#experience").append('<ul ></ul>').find('ul');
    $.ajax({
        type: "POST",
        url: apiUrl + "/admin/PostComments",
        data: Scores,

        success: function (data) {
            alert('Review Posted');
        },
        failure: function (data) {
            alert(xhr.status);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });

}

function Addseat(id) {
    
    var TeamDetailPostBackURL = '/Admin/AdminUserPage';
    var $buttonClicked = $(this);
   // var id = $buttonClicked.attr('data-id');
   // alert(id);
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: TeamDetailPostBackURL,
        contentType: "application/json; charset=utf-8",
        data: { "type": id },
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