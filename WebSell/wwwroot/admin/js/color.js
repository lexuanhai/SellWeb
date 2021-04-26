﻿(function ($) {
    var self = this;
    self.Color = {
        Id: null,
        Name: null,
        Code:""
    }
    self.roleSearch = {
        Id: "",
        Name: null,
        Description: "",
    }
    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Init = function () {

        self.GetColor();

        $(".btn-add").click(function () {
            self.SetValueDefault();
            $("#CreateEdit").css("display", "inline-block");
        })
        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })
        $(".btn-save").click(function () {
            self.ValidateColor();
        })    

        $(".btn-submit-search").click(function () {
            var id = $("#code_user_search").val();
            var name = $('#name').val();
            var code = $('#code').val();

            self.RoleSearch = {
                Id: id,
                Name: name,
                Description: code,
            }
            self.GetColor(self.RoleSearch);

        })

        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin màu");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                self.GetColorById(id);
                $('#create').modal('show');
            }
        })

        $('body').on('click', '.btn-delete', function () {
            var id = $(this).attr('data-id');
            var name = $(this).attr('data-name');
            if (id !== null && id !== '') {
                self.confirmColor(name, id);
            }
        })

        $('body').on('click', '.btn-grant', function () {
            self._roleId = parseInt($(this).attr('data-id'));
            self.GetAll();        
            $("#modal-grantpermission").modal('show');
        })

    }
    self.confirmColor = function (name, id) {
        bootbox.confirm({
            message: '<div class="title-delete"><p>Bạn có chắc muốn xóa màu ' + name+'?</p></div>',
            centerVertical: true,
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
                        url: "/Admin/Color/Delete",
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
                                self.GetColor(roleSearch);
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
    // Set value default
    self.SetValueDefault = function () {
        self.Color.Id = null;
        $("#name").val("").attr("placeholder", "Nhập tên màu");
        $("#code").val("").attr("placeholder", "Nhập mã màu ");
    }
    // Get User
    self.GetColor = function (roleSearch) {
        $.ajax({
            type: "GET",
            url: "/Admin/Color/GetPaging",
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

                        html += "<tr>" +
                            "<td></td>" +
                            "<td>" + item.Name + "</td>" +
                            "<td>" + (item.Code !== null ? item.Description:"" ) + "</td>" +
                            "<td>" + (item.CreatedDate !== null ? dateFormatJson(item.CreatedDate) : "")  + "</td>" +
                            "<td>" + (item.UpdatedDate !== null ? dateFormatJson(item.UpdatedDate) : "")  + "</td>" +
                            '<td>' +
                            '<a class=" btn-edit fa fa-pencil-square fa-lg" title = "Sửa" data-id=' + item.Id + ' data-id=' + item.Id + '  href="javascript:void(0)" ></a >' +
                            '<a class=" btn-delete fa fa-trash-o fa-lg" data-id=' + item.Id + ' data-name ="' + item.Name + '" title="xóa" href="javascript:void(0)"></a>' +
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
    self.GetColorById = function (colorId) {
        $.ajax({
            type: "GET",
            url: "/Admin/Color/GetColorById",
            dataType: "json",
            data: {
                ColorId: colorId
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.status) {
                    var resRole = response.data;
                    self.Color.Id = resRole.Id;
                    self.Color.Name = resRole.Name;
                    self.Color.Description = resRole.Description;
                    Set.SetValue();
                }
            },
            error: function () {
            }
        });
    }    
  
    self.ValidateColor = function () {
        $("#form_color").validate({
            rules:
            {
                name: {
                    required: true,
                },
                code: {
                    required: true,
                },               

            },
            messages:
            {
                name: {
                    required: "Bạn chưa nhập tên màu.",
                },
                code: {
                    required: "Bạn chưa nhập mã màu"
                },                              
            },
            submitHandler: function (form) {

                self.GetValue();

                self.submitColor(self.Color);
            }
        });
    }
    self.GetValue = function () {
        self.Color.Name = $("#name").val();
        self.Color.code = $("#code").val();
    }
    Set.SetValue = function () {
        if (self.Color.Id !== null && self.Color.Id !== '') {
            $("#name").val(self.Color.Name);
            $("#code").val(self.Color.Code);
        }
    }
    self.submitColor = function (color) {    
        var data = {
            Id: color.Id,
            Name: color.Name,
            Code: color.Code
        }
        $.ajax({
            type: "POST",
            url: "/Admin/Color/SaveEntity",
            data: data,
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.Status == true) {
                    self.GetColor();
                    $('#create').modal('hide');
                    $.notify(response.Message, 'success');
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