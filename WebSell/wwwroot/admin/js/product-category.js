(function ($) {
    var self = this;
    self.ProductCategory = {
        Id: null,
        Name: "",
        ParentId: null,
        Status: 1
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
        self.GetProductCateogory();

        self.GetParentProductCategory();

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

        //$(".btn-submit-search").click(function () {
        //    var id = $("#code_user_search").val();
        //    var name = $('#name').val();
        //    var description = $('#description').val();

        //    self.RoleSearch = {
        //        Id: id,
        //        Name: name,
        //        Description: description,
        //    }
        //    self.GetRole(self.RoleSearch);

        //})

        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin danh mục");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                self.GetProductCategoryById(id);
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
                        url: "/Admin/ProductCategory/Delete",
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
                                self.GetProductCateogory();
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

    self.GetParentProductCategory = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/ProductCategory/GetParentProductCategory",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var resFunction = response.Data;
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
        self.ProductCategory.Id = null;
        $("#name").val("").attr("placeholder", "Nhập tên danh mục");
        $("#parentId").val(0);
        $("#iconCss").val("").attr("placeholder", "Nhập tên icon");
        $("#sortOrder").val("").attr("placeholder", "Thứ tự");
        $("#status").val(1).attr("placeholder", "Trạng thái hoạt động");
        
    }

    self.GetProductCateogory = function (roleSearch) {
        $.ajax({
            type: "GET",
            url: "/Admin/ProductCategory/GetPaging",
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

    self.GetProductCategoryById = function (id) {
        $.ajax({
            type: "GET",
            url: "/Admin/ProductCategory/GetProductCategoryById",
            dataType: "json",
            data: {
                id: id
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var res = response.Data;
                    self.ProductCategory.Id = res.Id;
                    self.ProductCategory.Name = res.Name;
                    self.ProductCategory.ParentId = res.ParentId;
                    self.ProductCategory.Status = res.Status;
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
                self.submitFunction(self.ProductCategory);
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
                self.submitFunction(self.ProductCategory);
            }
        });
    }

    self.GetValue = function () {
        var parentId = parseInt($("#parentId").val());
        self.ProductCategory.Name = $("#name").val();
        self.ProductCategory.ParentId = parentId !== null && parentId !== 0 ? parentId : null;
        self.ProductCategory.Status = $("#status").val();
    }

    Set.SetValue = function () {
        if (self.ProductCategory.Id !== null && self.ProductCategory.Id !== '') {
            console.log(self.ProductCategory);
            $("#name").val(self.ProductCategory.Name);
            $("#parentId").val(self.ProductCategory.ParentId);
            $("#status").val(self.ProductCategory.Status);
        }
    }

    self.submitFunction = function (productCategory) {    
        var data = {
            Id: productCategory.Id,
            Name: productCategory.Name,
            ParentId: productCategory.ParentId,
            Status: productCategory.Status
        }
        $.ajax({
            type: "POST",
            url: "/Admin/ProductCategory/SaveEntity",
            data: data,
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.Status == true) {
                    self.GetProductCateogory();
                    $('#create').modal('hide');
                    $.notify(response.Message, 'success');
                    self.GetParentProductCategory();
                }
                else {
                    $.notify(response.Message, 'error');
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