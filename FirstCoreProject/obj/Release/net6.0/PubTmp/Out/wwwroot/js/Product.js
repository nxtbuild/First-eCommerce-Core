var dataTable;

$(document).ready(function () {
    dataTable = $('#myTable').DataTable({

        "ajax": {

            "url":"/Admin/Product/AllProducts"
        }
        ,
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Product/CreateUpdate?id=${data}" class="btn btn-success btn-md" ><i class="fa-solid fa-edit"></i></a>
                        <a onClick=RemoveProduct("/Admin/Product/Delete/${data}") class="btn btn-danger btn-md"><i class="fa-solid fa-trash"></i></a>


                    `
                }
            },


        ]


    });
});

function RemoveProduct(url) {
    Swal.fire({
        title: 'Are you sure?',
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
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })



        }
    })



}
