$('#thumbnail-image').on('change', function () {
    let image = this.files[0];
    let reader = new FileReader();

    reader.onload = function (e) {
        $('#thumbnail').attr('src', e.target.result);
    };

    reader.readAsDataURL(image);
});

$('#gallery-images').on('change', function () {
    let images = Array.from(this.files);
    $('#gallery').empty();

    images.forEach(image => {
        let reader = new FileReader();

        reader.onload = function (e) {
            $('#gallery').append(`<img src='${e.target.result}' alt='Gallery image' class='gallery-image' />`);
        };

        reader.readAsDataURL(image);
    });
});