$('#search-term').on('keyup', function () {
    let page = 0;
    let searchTerm = $('#search-term').val();

    getSearchedData(page, size, url, searchTerm);
});

function getSearchedData(page, size, url, searchTerm) {
    let ajaxData = {
        page: page,
        size: size,
        searchTerm: searchTerm
    };

    $.ajax({
        type: 'GET',
        url: url,
        data: ajaxData,
        success: function (data) {
            $('.content').html(data);
        },
        error: function (data) {
            console.log('error', data);
        }
    });
}