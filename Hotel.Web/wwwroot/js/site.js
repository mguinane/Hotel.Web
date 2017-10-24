$(function () {

    $(document).ajaxStop($.unblockUI); 

    $('#sortType').on('change', function () {
        sortFilterPageResults(1);
    });

    $('#hotelName').on('input', function () {
        sortFilterPageResults(1);
    });

    $(':checkbox').on('change', function () {
        sortFilterPageResults(1);
    });

    $('#minRating').on('input', function () {
        // trigger search only if number value entered or removed
        const minRating = $('#minRating').val();
        if ($.isNumeric(minRating) || minRating === '') {
            sortFilterPageResults(1);
        }
    });

    $('#maxRating').on('input', function () {
        // trigger search only if number value entered or removed
        const maxRating = $('#maxRating').val();
        if ($.isNumeric(maxRating) || maxRating === '') {
            sortFilterPageResults(1);
        }
    });

    $('#minCost').on('input', function () {
        // trigger search only if 2 digit number value entered or removed
        const minCost = $('#minCost').val();
        if ((minCost.length >= 2 && $.isNumeric(minCost)) || minCost === '') {
            sortFilterPageResults(1);
        }
    });

});

const sortFilterPageResults = page => {

    $.blockUI({ message: null });

    const hotelName = $('#hotelName').val();
    const sortType = getNumericValue($('#sortType').val());
    const minRating = getNumericValue($('#minRating').val());
    const maxRating = getNumericValue($('#maxRating').val());
    const minCost = getNumericValue($('#minCost').val());

    var stars = [];

    $('input:checkbox[name=stars]:checked').each(function() {
        const selectedStar = getNumericValue($(this).val());
        stars.push(selectedStar);
    });

    const criteria = {
        pageIndex: page,
        sortType: sortType,
        name: hotelName,
        stars: stars,
        minUserRating: minRating,
        maxUserRating: maxRating,
        minCost: minCost
    };

    fetch('/Hotels/Results', {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(criteria)
    }).then(response => {
        if (response.ok) {
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('text/html')) {
                return response.text();
            }
        }
        refreshPage('Search failed.');
        throw new Error('Search failed.');
    }, networkError => console.log(networkError.message)
    ).then(textResponse => {
        refreshPage(textResponse);
    }).catch(error => {
        console.log(error.message);
    });
};

const refreshPage = content => {
    $.unblockUI();
    $('div#hotelResults').html(content);
}

const getNumericValue = input => {
    if ($.isNumeric(input)) {
        return Number(input);
    } else {
        return 0;
    }
}
