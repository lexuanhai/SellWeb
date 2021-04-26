(function ($) {
    var self = this;
    self.Function = {
        Id: null,
        Name: "",
        ParentId: 0,
        IconCss: "",
        SortOrder: 0,
        Status: 1,
        URL:""
    }
    self.FunctionSearch = {
        Id: "",
        Name: null,
        ParentId: "",
    }
    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Init = function () {
        self.GetFunction();

        self.GetParentFunction();

        $(".btn-add").click(function () {
            self.SetValueDefault();
            $("#CreateEdit").css("display", "inline-block");
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })

        $(".btn-save").click(function () {
            self.Validate();
        })     

        $(".btn-submit-search").click(function () {
            var id = $("#code_user_search").val();
            var name = $('#name').val();
            var description = $('#description').val();

            self.RoleSearch = {
                Id: id,
                Name: name,
                Description: description,
            }
            self.GetRole(self.RoleSearch);

        })

        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin danh mục");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                self.GetFunctionById(id);
                $('#create').modal('show');
            }
        })

        $('body').on('click', '.btn-delete', function () {
            var id = $(this).attr('data-id');
            var name = $(this).attr('data-name');
            if (id !== null && id !== '') {
                self.confirm(name, id);
            }
        })
    }

    self.confirm = function (nameFunction, id) {
        bootbox.confirm({
            message: '<div class="title-delete"><p>Bạn có chắc muốn xóa danh mục ' + nameFunction + '?</p></div>',
            centerVertical: false,
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-success pull-left'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger '
                }
            },
            callback: function (result) {
                if (result === true) {
                    $.ajax({
                        type: "POST",
                        url: "/Admin/Function/Delete",
                        dataType: "json",
                        data: {
                            id: id
                        },
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (res) {
                            if (res.Status) {
                                self.GetFunction(FunctionSearch);
                                Notifly(res.Message, "success");
                            }
                            else {
                                $.notify(res.Message, 'error');
                            }
                        },
                        error: function () {
                        }
                    });
                }
            }
        });
    }

    self.GetParentFunction = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/Function/GetParentFunction",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var resFunction = response.data;
                    console.log(resFunction);
                    if (resFunction !== null) {
                        var _data = "";
                        $.each(resFunction, function (key, item) {
                            _data += "<option value=" + item.Id + ">" + item.Name + "</option>";
                        })
                        $("#parentId").html('<option value="0">Chọn danh mục</option>' + _data);
                    }
                }
            },
            error: function () {
            }
        });
    }
    // Set value default
    self.SetValueDefault = function () {
        self.Function.Id = null;
        $("#name").val("").attr("placeholder", "Nhập tên danh mục");
        $("#parentId").val(0);
        $("#iconCss").val("").attr("placeholder", "Nhập tên icon");
        $("#sortOrder").val("").attr("placeholder", "Thứ tự");
        $("#status").val(1).attr("placeholder", "Trạng thái hoạt động");
        
    }
    // Get User
    self.GetFunction = function (roleSearch) {
        $.ajax({
            type: "GET",
            url: "/Admin/Function/GetPaging",
            dataType: "json",
            data: roleSearch,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Results != null && response.Results.length > 0) {
                    var html = "";
                    $.each(response.Results, function (key, item) {
                        var nameStatus = "";
                        var classStatus = "";

                        switch (item.Status) {
                            case 1:
                                nameStatus = "Đã kích hoạt";
                                classStatus = "alert alert-success";
                                break;
                            case 2:
                                nameStatus = "Chờ kích hoạt";
                                classStatus = "alert alert-warning";
                                break;
                            case 3:
                                nameStatus = "Tạm ẩn";
                                classStatus = "alert alert-danger";
                                break;
                        }
                        html += "<tr>" +
                            "<td></td>" +
                            "<td>" + item.Name + "</td>" +
                            "<td>" + (item.NameParent !== null && item.NameParent !== 0 ? item.NameParent : "") + "</td>" +
                            "<td>" + (item.URL !== null ? item.URL : "") + "</td>" +
                            "<td>" + (item.IconCss !== null ? '<i class="' + item.IconCss+'"></i>'  : "") + "</td>" +
                            "<td>" + (item.SortOrder !== null ? item.SortOrder : "") + "</td>" +
                            "<td>" + (item.CreatedDate !== null ? dateFormatJson(item.CreatedDate) : "")  + "</td>" +
                            "<td>" + (item.UpdatedDate !== null ? dateFormatJson(item.UpdatedDate) : "") + "</td>" +
                            '<td><span class="' + classStatus + '">' + nameStatus + '</span></td>' +
                            '<td>' +
                            '<a class="btn-edit fa fa-pencil-square fa-lg" title = "Sửa" data-id=' + item.Id + ' data-id=' + item.Id + '  href="javascript:void(0)" ></a >' +
                            '<a class="btn-delete fa fa-trash-o fa-lg red" data-id=' + item.Id + ' data-name ="' + item.Name + '" title="xóa" href="javascript:void(0)"></a>' +
                            '</td >' +
                            "</tr>";
                    });
                }
                else {
                    html += '<tr><td colspan="8"> <a class="come-black" href="javascript:void(0)">Không có dữ liệu</a>  </td></tr>'
                }

                $(".table-content").html(html);

                self.addSerialNumber();
            },
            error: function () {
            }
        });
    }
    self.GetFunctionById = function (id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Function/GetFunctionById",
            dataType: "json",
            data: {
                id: id
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.status) {
                    var resFunction = response.data;
                    self.Function.Id = resFunction.Id;
                    self.Function.Name = resFunction.Name;
                    self.Function.ParentId = resFunction.ParentId;
                    self.Function.IconCss = resFunction.IconCss;
                    self.Function.SortOrder = resFunction.SortOrder;
                    self.Function.Status = resFunction.Status;
                    Set.SetValue();
                }
            },
            error: function () {
            }
        });
    }

    self.Validate = function () {
        $("#form_function").validate({
            rules:
            {
                name: {
                    required: true,
                },              
            },
            messages:
            {
                name: {
                    required: "Bạn chưa nhập tên danh mục.",
                }                        
            },
            submitHandler: function (form) {
                self.GetValue();

                self.submitFunction(self.Function);
            }
        });
    }

    self.GetValue = function () {
        self.Function.Name = $("#name").val();
        self.Function.ParentId = $("#parentId").val();
        self.Function.IconCss = $("#iconCss").val();
        self.Function.SortOrder = $("#sortOrder").val();
        self.Function.Status = $("#status").val();
        self.Function.URL = $("#url").val();
    }

    Set.SetValue = function () {
        if (self.Function.Id !== null && self.Function.Id !== '') {
            console.log(self.Function);
            $("#name").val(self.Function.Name);
            $("#parentId").val(self.Function.ParentId);
            $("#iconCss").val(self.Function.IconCss);
            $("#sortOrder").val(self.Function.SortOrder);
            $("#status").val(self.Function.Status);
        }
    }

    self.submitFunction = function (_function) {    
        var data = {
            Id: _function.Id,
            Name: _function.Name,
            ParentId: _function.ParentId,
            IconCss: _function.IconCss,
            SortOrder: _function.SortOrder,
            Status: _function.Status,
            URL: _function.URL
        }
        $.ajax({
            type: "POST",
            url: "/Admin/Function/SaveEntity",
            data: data,
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.status == true) {
                    self.GetFunction();
                    $('#create').modal('hide');
                    $.notify(response.message, 'success');
                    self.GetParentFunction();
                }
                else {
                    $.notify(response.message, 'error');
                }
            },
            error: function (eror) {
                console.log(eror);
            }
        });
    }

    $(document).ready(function () {
        self.Init();
    })
})(jQuery);