let searchByKey = $('#hiddenData').data('keysearchby');
let searchTermKey = $('#hiddenData').data('keysearchterm');
let pageKey = $('#hiddenData').data('keypage');

let loadedPage = localStorage.getItem(pageKey);
let loadedSearchBy = localStorage.getItem(searchByKey);
let loadedSearchTerm = localStorage.getItem(searchTermKey);

$(() => {
    if (loadedPage !== null) {
        getFilteredData(loadedPage, size, url, loadedSearchBy, loadedSearchTerm);

        $('#filterBy').val(loadedSearchBy);
        $('#search-term').val(loadedSearchTerm);
    } else {
        getFilteredData(0, size, url, loadedSearchBy, loadedSearchTerm);
    }
});

$('#filter').on('keyup', function () {
    let page = $('.pager-btn.btn-dark').data('page');
    let searchBy = $('#searchBy').val();
    let searchTerm = $('#search-term').val();

    if (filterBy !== 'none') {
        getFilteredData(page, size, url, searchBy, searchTerm);

        localStorage.setItem(pageKey, page);
        localStorage.setItem(searchByKey, searchBy);
        localStorage.setItem(searchTermKey, searchTerm);
    }
});

$('#filterBy').on('change', function () {
    if ($('#filterBy').val() === 'none') {
        localStorage.removeItem(pageKey);
        localStorage.removeItem(searchByKey);
        localStorage.removeItem(searchTermKey);
        $('#search-term').val('');

        let page = $('.pager-btn.btn-dark').data('page');
        let searchBy = $('#searchBy').val();
        let searchTerm = $('#search-term').val();
        getFilteredData(page, size, url, searchBy, searchTerm);
    }
});

function getFilteredData(page, size, url, searchBy, searchTerm) {
    let ajaxData = {
        page: page,
        size: size,
        searchBy: searchBy,
        searchTerm: searchTerm
    };

    $.ajax({
        type: 'GET',
        url: url,
        data: ajaxData,
        success: function (data) {
            $('#content').html(data);

            $('.pager-btn').removeClass('btn-secondary');
            $('.pager-btn').addClass('btn-primary');

            $('.pager-btn[data-page=' + page + ']').removeClass('btn-primary');
            $('.pager-btn[data-page=' + page + ']').addClass('btn-secondary');
        },
        error: function (data) {
            console.log('error', data);
        }
    });
}