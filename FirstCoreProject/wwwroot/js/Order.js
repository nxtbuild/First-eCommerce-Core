var dataTable;

$(document).ready(function () {

    var url = window.location.search;
    if (url.includes("pending")) {
        OrderTable("pending")
    }
    else {
        if (url.includes("approved")) {
            OrderTable("approved");
        } else {
            if (url.includes("shipped")) {
                OrderTable("shipped");
            } 
            else {
                if (url.includes("underprocess")) {
                    OrderTable("underprocess");
                }
                else {
                    OrderTable("all");
                }
            }
        }
    }

   
});


function OrderTable(status) {
    dataTable = $('#myTable').DataTable({

        "ajax": {

            "url": "/Admin/Order/AllOrders?status=" + status
        }
        ,
        "columns": [
            { "data": "name" },
            { "data": "phone" },

            { "data": "orderStatus" },
            { "data": "dateOfOrder" },
            { "data": "orderTotal" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/order/orderDetails?id=${data}" class="btn btn-success btn-md" ><i class="fa-solid fa-eye"></i></a>

                    `
                }
            },


        ]


    });
}

