(function () {
    function menuHeaderClick() {
        if ($(this).next("ul").css("display") == "block") return false;
        $(this).parent().parent().find("ul").slideUp();
        $(this).next("ul").slideDown();
        return false;
    }
    function resetContent() {
        var width = $(window).width();
        var height = $(window).height();
        var topHeight = $(".top").height();
        var bodyHeight = height - topHeight;
        $(".left,.right").height(bodyHeight);
    }
    function pageLoaded() {
        resetContent();
        $(".pure-menu-heading").bind("click", menuHeaderClick);
    };
    $(pageLoaded);
    $(window).bind("resize", resetContent);
})();