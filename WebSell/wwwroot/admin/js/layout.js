(function ($) {
    $(document).ready(function () {
        $(".title-partent").click(function () {
            $(".silebar-menu li").removeClass("active");
            $(".title-partent").removeClass("active");
            $(this).parent().addClass("active");
            $(".silebar-menu li.active .child-menu").slideToggle(300);
        })
        $(".title-child-menu").click(function () {
            $(".title-sub-menu").removeClass("current-page");
            $(this).parent().addClass("current-page");
        })
        $(".res-menu").click(function () {
            var status = parseInt($(this).attr("data-status"));
            if (status === 0) {
                $(this).attr("data-status", "1");
                $(".box-left").css("display", "none");
                $(".box-right").css("width", "100%");
            }
            else {
                $(this).attr("data-status", "0");
                $(".box-left").css("display", "inline-block");
                $(".box-right").css("width", "83%");
            }
        })
    })
})(jQuery);