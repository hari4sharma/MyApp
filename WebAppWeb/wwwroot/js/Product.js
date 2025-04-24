var table = null;
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
                            <a href="/admin/product/Delete?id=${data}"> <i class="bi bi-trash"></i> </a >` 
                }
            }
        ]
    });
}); 