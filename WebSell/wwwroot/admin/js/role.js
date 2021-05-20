(function ($) {
    var self = this;
    self.Role = {
        Id: null,
        Name: null,
        Description:""
    }
    self._roleId = "";
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
    self.Files = {};

    self.Init = function () {
        self.GetRole();
        $(".btn-add").click(function () {
            self.SetValueDefault();
            $("#CreateEdit").css("display", "inline-block");
        })        
        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })

        $(".btn-save").click(function () {
            self.ValidateUser();
        })     
       
        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin người dùng");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                //self.Reset()
                self.GetRoleById(id);
                $('#create').modal('show');
            }
        })

        $('body').on('click', '.btn-delete', function () {
            var id = $(this).attr('data-id');
            var name = $(this).attr('data-name');
            if (id !== null && id !== '') {
                self.confirmRole(name, id);
            }
        })

        $('body').on('click', '.btn-grant', function () {
            self._roleId = $(this).attr('data-id');
            self.GetAll();        
            $("#modal-grantpermission").modal('show');
        })

        $("body").on('click', '#btnSavePermission', function () {
            var listPermmission = [];

            $.each($('#tblFunction tbody tr'), function (i, item) {
                listPermmission.push({
                    RoleId: self._roleId,
                    FunctionId: $(item).data('id'),
                    CanRead: $(item).find('.ckView').first().prop('checked'),
                    CanCreate: $(item).find('.ckAdd').first().prop('checked'),
                    CanUpdate: $(item).find('.ckEdit').first().prop('checked'),
                    CanDelete: $(item).find('.ckDelete').first().prop('checked'),
                });
            });
            $.ajax({
                type: "POST",
                url: "/Admin/Role/SavePermission",
                data: {
                    listPermmission: listPermmission,
                    roleId: self._roleId
                },
                beforeSend: function () {
                    
                },
                success: function (response) {
                    if (response.Status) {
                        $.notify('gán quyền thành công', 'success');                       
                        $('#modal-grantpermission').modal('hide');
                    } else {
                        $.notify('gán quyền không thành công', 'error');                           
                    }
                },
                error: function () {                   
                }
            });
        });
    }
    self.confirmRole = function (nameUser, id) {
        bootbox.confirm({
            message: '<div class="title-delete"><p>Bạn có chắc muốn xóa quyền ' + nameUser+'?</p></div>',
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
                        url: "/Admin/Role/Delete",
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
                                self.GetRole(roleSearch);
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
        self.Role.Id = null;
        $("#name").val("").attr("placeholder", "Nhập tên quyền");
        $("#description").val("").attr("placeholder", "Mô tả tên quyền ");
    }
    // Get User
    self.GetRole = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/Role/GetAll",
            dataType: "json",          
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var data = response;
                if (data != null && data.length > 0) {
                    var html = "";
                    $.each(data, function (key, item) {

                        html += "<tr>" +
                            "<td></td>" +
                            "<td>" + item.Name + "</td>" +
                            "<td>" + (item.Description !== null ? item.Description:"" ) + "</td>" +
                            "<td>" + (item.CreatedDate !== null ? dateFormatJson(item.CreatedDate) : "")  + "</td>" +
                            "<td>" + (item.UpdatedDate !== null ? dateFormatJson(item.UpdatedDate) : "")  + "</td>" +
                            '<td>' +
                            '<a class=" btn-xs btn-primary btn-grant fa fa-eye-slash" title = "Gán quyền" data-id=' + item.Id + ' href="javascript:void(0)" ></a >' +
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
    self.GetRoleById = function (roleId) {
        $.ajax({
            type: "GET",
            url: "/Admin/Role/GetRoleById",
            dataType: "json",
            data: {
                id: roleId
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var resRole = response;
                self.Role.Id = resRole.Id;
                self.Role.Name = resRole.Name;
                self.Role.Description = resRole.Description;
                Set.SetValue();
            },
            error: function () {
            }
        });
    }    
    self.GetAll = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/Role/ListAllFunction",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Data !== null && response.Data.length > 0) {
                    var data = response.Data;
                    var html = "";
                    $.each(data, function (key, item) {
                        console.log(item.ParentId);
                        var partentFunction = item.ParentId !== null && parseInt(item.ParentId) > 0 ? "treegrid-parent-" + item.ParentId : "";
                        html += '<tr class="treegrid-' + item.Id + ' ' + partentFunction + '" data-id=' + item.Id + '>' +
                            "<td>" + item.Name + "</td>" +
                            "<td><label><input type='checkbox' value=" + item.Id + " class='ckView'><span class='text'>Allow</span> </label></td>" +
                            "<td><label><input type='checkbox' value=" + item.Id + " class='ckAdd'><span class='text'>Allow</span> </label></td>" +
                            "<td><label><input type='checkbox' value=" + item.Id + " class='ckEdit'><span class='text'>Allow</span> </label></td>" +
                            "<td><label><input type='checkbox' value=" + item.Id + " class='ckDelete'><span class='text'>Allow</span> </label></td>" +
                            "</tr > ";
                    })

                    $("#lst-data-function").html(html);
                }
                $('.tree').treegrid();
                $('#ckCheckAllView').on('click', function () {
                    $('.ckView').prop('checked', $(this).prop('checked'));
                });

                $('#ckCheckAllCreate').on('click', function () {
                    $('.ckAdd').prop('checked', $(this).prop('checked'));
                });
                $('#ckCheckAllEdit').on('click', function () {
                    $('.ckEdit').prop('checked', $(this).prop('checked'));
                });
                $('#ckCheckAllDelete').on('click', function () {
                    $('.ckDelete').prop('checked', $(this).prop('checked'));
                });

                $('.ckView').on('click', function () {
                    if ($('.ckView:checked').length == response.length) {
                        $('#ckCheckAllView').prop('checked', true);
                    } else {
                        $('#ckCheckAllView').prop('checked', false);
                    }
                });
                $('.ckAdd').on('click', function () {
                    if ($('.ckAdd:checked').length == response.length) {
                        $('#ckCheckAllCreate').prop('checked', true);
                    } else {
                        $('#ckCheckAllCreate').prop('checked', false);
                    }
                });
                $('.ckEdit').on('click', function () {
                    if ($('.ckEdit:checked').length == response.length) {
                        $('#ckCheckAllEdit').prop('checked', true);
                    } else {
                        $('#ckCheckAllEdit').prop('checked', false);
                    }
                });
                $('.ckDelete').on('click', function () {
                    if ($('.ckDelete:checked').length == response.length) {
                        $('#ckCheckAllDelete').prop('checked', true);
                    } else {
                        $('#ckCheckAllDelete').prop('checked', false);
                    }
                });
                self.GetPerssionByRole(self._roleId);
            },
            error: function () {
            }
        });
    }
    self.GetPerssionByRole= function(roleId) {
        $.ajax({
            type: "GET",
            url: "/Admin/Role/ListFunctionByRole",
            dataType: "json",
            data: {
                roleId: roleId
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {               
                console.log(response);
                if (response !== null && response.length > 0) {
                    var data = response;
                    $.each($('#tblFunction tbody tr'), function (i, item) {
                        $.each(data, function (j, jitem) {
                            if (jitem.FunctionId == $(item).data('id')) {
                                $(item).find('.ckView').first().prop('checked', jitem.CanRead);
                                $(item).find('.ckAdd').first().prop('checked', jitem.CanCreate);
                                $(item).find('.ckEdit').first().prop('checked', jitem.CanUpdate);
                                $(item).find('.ckDelete').first().prop('checked', jitem.CanDelete);
                            }
                        });
                    });
                    if ($('.ckView:checked').length == $('#tblFunction tbody tr .ckView').length) {
                        $('#ckCheckAllView').prop('checked', true);
                    } else {
                        $('#ckCheckAllView').prop('checked', false);
                    }
                    if ($('.ckAdd:checked').length == $('#tblFunction tbody tr .ckAdd').length) {
                        $('#ckCheckAllCreate').prop('checked', true);
                    } else {
                        $('#ckCheckAllCreate').prop('checked', false);
                    }
                    if ($('.ckEdit:checked').length == $('#tblFunction tbody tr .ckEdit').length) {
                        $('#ckCheckAllEdit').prop('checked', true);
                    } else {
                        $('#ckCheckAllEdit').prop('checked', false);
                    }
                    if ($('.ckDelete:checked').length == $('#tblFunction tbody tr .ckDelete').length) {
                        $('#ckCheckAllDelete').prop('checked', true);
                    } else {
                        $('#ckCheckAllDelete').prop('checked', false);
                    }
                }
                
            },
            error: function () {
            }
        });
    }
    self.ValidateUser = function () {
        $("#form_role").validate({
            rules:
            {
                name: {
                    required: true,
                },
                description: {
                    required: true,
                },               

            },
            messages:
            {
                name: {
                    required: "Bạn chưa nhập tên quyền.",
                },
                description: {
                    required: "Bạn chưa nhập số mô tả"
                },                              
            },
            submitHandler: function (form) {
                self.GetValue();
                self.SubmitRole(self.Role);
            }
        });
    }
    self.GetValue = function () {
        self.Role.Name = $("#name").val();
        self.Role.Description = $("#description").val();
    }
    Set.SetValue = function () {
        if (self.Role.Id !== null && self.Role.Id !== '') {
            $("#name").val(self.Role.Name);
            $("#description").val(self.Role.Description);
        }
    }
    self.SubmitRole = function (role) {    
        var data = {
            Id: role.Id,
            Name: role.Name,
            Description: role.Description
        }
        $.ajax({
            type: "POST",
            url: "/Admin/Role/SaveEntity",
            data: data,
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.Status == true) {
                    self.GetRole();
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
    self.Reset = function () {
        $("#name").val("");
        $("#description").val("");
    }
    $(document).ready(function () {
        self.Init();
    })
})(jQuery);