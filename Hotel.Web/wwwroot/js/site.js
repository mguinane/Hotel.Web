$(function () {

    $(document).ajaxStop($.unblockUI); 

    $('#sortType').on('change', function () {
        SortFilterPageResults(1);
    });

    $('#hotelName').on('input', function () {
        SortFilterPageResults(1);
    });

    $(":checkbox").on('change', function () {
        SortFilterPageResults(1);
    });

    $('#minRating').on('input', function () {
        // trigger search only if number value entered or removed
        var minRating = $('#minRating').val();
        if ($.isNumeric(minRating) || minRating === "") {
            SortFilterPageResults(1);
        }
    });

    $('#maxRating').on('input', function () {
        // trigger search only if number value entered or removed
        var maxRating = $('#maxRating').val();
        if ($.isNumeric(maxRating) || maxRating === "") {
            SortFilterPageResults(1);
        }
    });

    $('#minCost').on('input', function () {
        // trigger search only if 2 digit number value entered or removed
        var minCost = $('#minCost').val();
        if ((minCost.length >= 2 && $.isNumeric(minCost)) || minCost === "") {
            SortFilterPageResults(1);
        }
    });

});

function SortFilterPageResults(page) {

    $.blockUI({ message: null });

    var hotelName = $('#hotelName').val();
    var sortType = GetNumericValue($('#sortType').val());
    var minRating = GetNumericValue($('#minRating').val());
    var maxRating = GetNumericValue($('#maxRating').val());
    var minCost = GetNumericValue($('#minCost').val());

    var stars = [];
    $("input:checkbox[name=stars]:checked").each(function () {
        var selectedStar = GetNumericValue($(this).val());
        stars.push(selectedStar);
    });

    var criteria = {
        pageIndex: page,
        sortType: sortType,
        name: hotelName,
        stars: stars,
        minUserRating: minRating,
        maxUserRating: maxRating,
        minCost: minCost
    };

    $.ajax({
        type: 'POST',
        url: '/Hotels/Results',
        data: JSON.stringify(criteria),
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('div#hotelResults').html(result);
        },
        error: function (result) {
            $.unblockUI();
            $('div#hotelResults').html("Search failed.");
        }
    });
}

function GetNumericValue(input) {
    if ($.isNumeric(input)) {
        return Number(input);
    } else {
        return 0;
    }
}
