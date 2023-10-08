$('#mobile-menu-btn').on('click', function() {
    $('.mobile-menu').css('top', '4rem');
});

$('#btn-close').on('click', function () {
    $('.mobile-menu').css('top', '-50rem');
});

$('.info-btn').on('click', function () {
    $('.app-info').css('left', '0');
});

$('.close-info-btn').on('click', function () {
    $('.app-info').css('left', '-500px');
})
