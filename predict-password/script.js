$(document).ready(function () {
    $('input').keyup(function () {
        $("#results").empty();
        var userSearch = $(this).val();
        
        if (userSearch.length > 0 && userSearch.length < 7) {
            $.ajax({
                url: "PredictPassword2.asmx/SearchForPasswords",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({ search: userSearch }),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $("#results").empty();
                    var resultArray = JSON.parse(result.d);
                    displayResults(resultArray, userSearch);
                }
            });
        } else if (userSearch.length >= 7) {
            $("#results").append($("<p></p>").append("No predicted passwords for: " + userSearch));
        }
    });

});

function displayResults(result, search) {
    if (result.length == 0 && search.trim() != "") {
        $("#results").append($("<p></p>").append("No predicted passwords for: " + search));
    }
    else {
        for (var i = 0; i < result.length; i++) {
            $("#results").append($("<p></p>").append(result[i]));
        }
    }
}

