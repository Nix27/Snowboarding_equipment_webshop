﻿let pages = $('#hiddenData').data('pages');
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

    getPagedData(0, size, url);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.last-btn').on('click', function (event) {
    event.preventDefault();

    getPagedData(pages, size, url);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.previous-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page >= 0) {
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