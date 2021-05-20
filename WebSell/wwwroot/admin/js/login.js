(function ($) {
    var self = this;
    self.User = {
        Id: null,
        FullName: null,
        BirthDay: null,
        UserName: "",
        Email: "",
        Address: "",
        PhoneNumber: "",
        Avatar: ""
    }
    self.UserSearch = {
        Id: "",
        FullName: null,
        Birthday: null,
        UserName: "",
        Email: "",
        Address: "",
        Phone: "",
    }
    self.lstRole = [];

    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Files = {};
    self.Init = function () {
        $(".btn-login").click(function () {
            var userName = $(".username").val();
            var password = $(".password").val();
            $.ajax({
                type: "POST",
                url: "/Admin/Login/Authen",
                data: {
                    userName: userName,
                    password: password
                },
                dateType: 'json',
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.Status == 1) {
                        $.notify(response.Message, 'success');
                        window.location.href = "/Admin/Home";
                    } else {
                        $.notify(response.Message, 'error');
                    }                 
                },
                error: function (eror) {
                    console.log(eror);
                }
            });
        })      
    }   

    $(document).ready(function () {
        $(document).ajaxSend(function (e, xhr, options) {
            if (options.type.toUpperCase() == "POST" || options.type.toUpperCase() == "PUT") {
                var token = $('form').find("input[name='__RequestVerificationToken']").val();
                xhr.setRequestHeader("RequestVerificationToken", token);
            }
        });
        self.Init();
    })
})(jQuery);