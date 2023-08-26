$('#search-term').on('keyup', function () {
    let page = $('.pager-btn.btn-dark').data('page');
    let searchTerm = $('#search-term').val();
    console.log(size);
    console.log(url);

    getFilteredData(page, size, url, searchTerm);
});

function getFilteredData(page, size, url, searchTerm) {
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
            $('#content').html(data);

            $('.pager-btn').removeClass('btn-dark');
            $('.pager-btn').addClass('btn-light');

            $('.pager-btn[data-page=' + page + ']').removeClass('btn-light');
            $('.pager-btn[data-page=' + page + ']').addClass('btn-dark');
        },
        error: function (data) {
            console.log('error', data);
        }
    });
}