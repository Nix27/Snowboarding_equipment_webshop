let tableId = $('#tableDate').data('tableid');

$('#search-term').on('keyup', function () {
    let page = 1;
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
            $('#table-body-content').html(data);
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