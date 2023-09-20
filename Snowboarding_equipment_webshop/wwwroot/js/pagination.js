let pages = $('#hiddenData').data('pages');
let size = $('#hiddenData').data('size');
let url = $('#hiddenData').data('url');

$('.pager-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');
    getPagedData(page, size, url);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.first-btn').on('click', function (event) {
    event.preventDefault();

    if ($('.pager-btn.btn-pagination-active').data('page') !== $('.first-btn').data('page')) {
        getPagedData(1, size, url);

        $('.next-btn').data('page', $(this).data('page') + 1);
        $('.previous-btn').data('page', $(this).data('page') - 1);
    }
});

$('.last-btn').on('click', function (event) {
    event.preventDefault();

    if ($('.pager-btn.btn-pagination-active').data('page') !== $('.last-btn').data('page')) {
        getPagedData(pages, size, url);

        $('.next-btn').data('page', $(this).data('page') + 1);
        $('.previous-btn').data('page', $(this).data('page') - 1);
    }
});

$('.previous-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page >= 1) {
        getPagedData(page, size, url);
        $(this).data('page', page - 1);
        $('.next-btn').data('page', $(this).data('page') + 2);
    }
});

$('.next-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page <= pages) {
        getPagedData(page, size, url);
        $(this).data('page', page + 1);
        $('.previous-btn').data('page', $(this).data('page') - 2);
    }
});

function getPagedData(page, size, url) {
    let ajaxData = {
        page: page,
        size: size
    };

    $.ajax({
        type: 'GET',
        url: url,
        data: ajaxData,
        success: function (data) {
            $('#table-body-content').html(data);

            $('.pager-btn').removeClass('btn-pagination-active');
            $('.pager-btn').addClass('btn-pagination');

            $('.pager-btn[data-page=' + page + ']').removeClass('btn-pagination');
            $('.pager-btn[data-page=' + page + ']').addClass('btn-pagination-active');
        },
        error: function (data) {
            console.log('error', data);
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            });
        }
    });
}