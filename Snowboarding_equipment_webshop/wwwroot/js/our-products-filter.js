$('#filter-btn').on('click', function () {
    $('.mobile-filter-container').css('left', '0');
    $('#close-filter-btn').css('display', 'block');
    $('body').css('overflow', 'hidden');
});

$('#close-filter-btn').on('click', function () {
    $('.mobile-filter-container').css('left', '-2000px');
    $('#close-filter-btn').css('display', 'none');
    $('body').css('overflow', 'auto');
});