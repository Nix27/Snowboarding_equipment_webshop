$(() => {
    let dataTable;

    loadTable();
})

function loadTable() {
    dataTable = $('#category-table').DataTable({
        'ajax': {
            'url': '/Admin/Category/GetAllCategoriesAsync'
        },
        'columns': [
            { 'data': 'name', 'width': '80%' },
            {
                'data': 'id',
                'render': function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Category/UpdateCategory?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil"></i></a>
                            <a onclick=Delete("/Admin/Category/DeleteCategory/"+${data}) class="btn btn-danger mx-2"><i class="bi bi-trash3"></i></a>
                        </div>
                    `
                },
                'width': '20%'
            }
        ]
    });
}

function Delete(url){
    Swal.fire({
        title: 'Are you sure you want delete this?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Successfully deleted',
                            'success'
                        )
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Unable to delete, something went wrong!'
                        })
                    }
                }
            })
        }
    });
}