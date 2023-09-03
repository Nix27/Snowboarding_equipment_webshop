let filterByKey = $('#hiddenData').data('keyfilterby');
let searchTermKey = $('#hiddenData').data('keysearchterm');
let pageKey = $('#hiddenData').data('keypage');

let loadedPage = localStorage.getItem(pageKey);
let loadedFilterBy = localStorage.getItem(filterByKey);
let loadedSearchTerm = localStorage.getItem(searchTermKey);

$(() => {
    if (loadedPage !== null) {
        getFilteredData(loadedPage, size, url, loadedFilterBy, loadedSearchTerm);

        $('#filterBy').val(loadedFilterBy);
        $('#search-term').val(loadedSearchTerm);
    } else {
        getFilteredData(0, size, url, loadedFilterBy, loadedSearchTerm);
    }
});

$('#filter').on('keyup', function () {
    let page = $('.pager-btn.btn-dark').data('page');
    let filterBy = $('#filterBy').val();
    let searchTerm = $('#search-term').val();

    if (filterBy !== 'none') {
        getFilteredData(page, size, url, filterBy, searchTerm);

        localStorage.setItem(pageKey, page);
        localStorage.setItem(filterByKey, filterBy);
        localStorage.setItem(searchTermKey, searchTerm);
    }
});

$('#filterBy').on('change', function () {
    if ($('#filterBy').val() === 'none') {
        localStorage.removeItem(pageKey);
        localStorage.removeItem(filterByKey);
        localStorage.removeItem(searchTermKey);
        $('#search-term').val('');

        let page = $('.pager-btn.btn-dark').data('page');
        let filterBy = $('#filterBy').val();
        let searchTerm = $('#search-term').val();
        getFilteredData(page, size, url, filterBy, searchTerm);
    }
});

function getFilteredData(page, size, url, filterBy, searchTerm) {
    let ajaxData = {
        page: page,
        size: size,
        filterBy: filterBy,
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