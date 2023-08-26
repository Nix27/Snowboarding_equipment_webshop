$('#search-term').on('keyup', function () {
    let page = $('.pager-btn.btn-pagination').data('page');
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
            $('#content').html(data);

            $('.pager-btn').removeClass('btn-pagination');
            $('.pager-btn').addClass('btn-pagination-active');

            $('.pager-btn[data-page=' + page + ']').removeClass('btn-pagination-active');
            $('.pager-btn[data-page=' + page + ']').addClass('btn-pagination');
        },
        error: function (data) {
            console.log('error', data);
        }
    });
}