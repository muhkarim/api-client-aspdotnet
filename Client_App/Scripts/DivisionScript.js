var table = null;
var Departments = []

$(document).ready(function () {
    //debugger;
    table = $('#div-table').DataTable({
        "ajax": {
            url: "/Division/LoadDivision",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 4 },
            { "searchable": false, "targets": 4 }
        ],
        "columns": [
            { "data": "DivisionName", "name": "DivisionName"},
            { "data": "DepartmentName", "name": "Department" },
            {
                "data": "CreateDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "UpdateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD/MM/YYYY');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="BtnEdit" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(' + row.Id + ')"><i class="mdi mdi-pencil"></i></button> &nbsp; <button type="button" class="btn btn-danger" id="BtnDelete" data-toggle="tooltip" data-placement="top" title="Hapus" onclick="return Delete(' + row.Id + ')"><i class="mdi mdi-delete"></i></button>';
                }
            },
        ]
    });

    // tooltip
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

    // hide button update modal
    $('#Update').hide();

    LoadDepartment($('#DepartmentSelect')); // id department option


});


function Save() {
    //debugger;
    var Division = new Object();
    Division.Name = $('#Name').val();
    Division.DepartmentId = $('#DepartmentSelect').val();

    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'info',
            title: 'Require',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else if ($('#DepartmentSelect').val() == "" || $('#DepartmentSelect').val() == null) {
        Swal.fire({
            icon: 'info',
            title: 'Require',
            text: 'Department Cannot be Empty',
        })
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: '/Division/InsertOrUpdate/',
            data: Division
        }).then((result) => {
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    position: 'center',
                    type: 'success',
                    showConfirmButton: false,
                    timer: 1500,
                    title: 'Added Succesfully'
                }).then(function () {
                    table.ajax.reload();
                    ClearScreen(); // delete value name department
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add Division', 'error');
                ShowModal();
            }
        })
    }
    
}

// code dropdown select
function LoadDepartment(element) {
    if (Departments.length == 0) {
        $.ajax({
            type: "Get",
            url: "/Department/LoadDepartment", // Controller department
            success: function (data) {
                Departments = data;
                renderDepartment(element);
            }
        })
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    //debugger;
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());

    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.Id).text(val.Name));
    })
}

// end code dropdown select

function GetById(Id) {
    //debugger;
    $.ajax({
        url: "/Division/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //debugger;
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.DivisionName);
            $('#DepartmentSelect').val(obj.DepartmentId);
            $("#exampleModal").modal('show');
            $("#Save").hide();
            $('#Update').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}

function Edit() {
    var Division = new Object();
    Division.Id = $('#Id').val();
    Division.Name = $('#Name').val();
    Division.DepartmentId = $('#DepartmentSelect').val();
    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: '/Division/InsertorUpdate/',
            data: Division
        }).then((result) => {
            //debugger;
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    position: 'center',
                    title: 'Division Updated Successfully',
                    timer: 1500
                }).then(function () {
                    table.ajax.reload();
                    ClearScreen();
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Failed to Update',
                })
                ClearScreen();
            }
        })
    }
}


function Delete(Id) {
    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!"
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "Division/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 1500
                    }).then(function () {
                        table.ajax.reload();
                        ClearScreen();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to Delete',
                    })
                    ClearScreen();
                }
            })
        }
    })
}


function ClearScreen() {
    $('#Name').val('');
    $('#DepartmentSelect').val('');
    $('#Save').show();
    $('#Update').hide();
    $('#Delete').hide();
    $('#exampleModal').modal('hide');
}


document.getElementById("btncreate").addEventListener("click", function () {
    $('#Id').val('');
    $('#Name').val('');
    $('#Save').show();
    $('#Update').hide();
    $('#Delete').hide();
    $('#exampleModal').modal('show');
    LoadDepartment($('#DepartmentSelect')); // id department option

});
