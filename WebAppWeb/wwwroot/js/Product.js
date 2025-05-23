﻿var table = null;
$(document).ready(function () {
    table = new DataTable('#myTable', {
        "ajax": {
            "url": "/admin/product/AllProducts"
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `<a href="/admin/product/CreateUpdate?id=${data}" ><i class="bi bi-pencil-square"></i> </a>
                            <a onClick=RemoveProduct("/admin/product/Delete/${data}")> <i class="bi bi-trash"></i> </a >`
                }
            }
        ]
    });
});

function RemoveProduct(url) {
    console.log(url);
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        table.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
            });
        }
    });
}