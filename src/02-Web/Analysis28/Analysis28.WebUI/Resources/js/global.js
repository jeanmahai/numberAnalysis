// JavaScript Document

function hideAllForm() {
    hideForgetpswForm();
    hideLoginForm();
    hideRegisterForm();
}
//登录
function showLoginForm() {
    hideAllForm();
    $(".form-login").showByMask("mask,mask-login", $(".page"));
    $(".form-login").position({of:$(".items")});
    $(".login").unbind("click", showLoginForm);
    $(".form-login .btn-close").bind("click", hideLoginForm);
}
function hideLoginForm() {
    $(".form-login").hideByMask();
    $(".login").bind("click", showLoginForm);
    $(".form-login .btn-close").unbind("click");
    return false;
}
//注册
function showRegisterForm() {
    hideAllForm();
    $(".form-register").showByMask("mask,mask-login", $(".page"));
    $(".form-register").position({ of: $(".items") });
    $(".register").unbind("click", showRegisterForm);
    $(".form-register .btn-close").bind("click", hideRegisterForm);
}
function hideRegisterForm() {
    $(".form-register").hideByMask();
    $(".register").bind("click", showRegisterForm);
    $(".form-register .btn-close").unbind("click", hideRegisterForm);
    return false;
}
//重置页面的布局
function resetPagePos() {
    $(".page").position({
        of: $(document)
    });
}
//发布项目
function showPublishProject() {
    $(".form-publishproject").showByMask("mask,mask-publishproject", $(".page"));
    $(".form-publishproject").position({ of: $(".page") });
    $(".publish").unbind("click", showPublishProject);
    $(".form-publishproject .btn-close").bind("click", hidePublishProject);
}
function hidePublishProject() {
    $(".form-publishproject").hideByMask();
    $(".publish").bind("click", showPublishProject);
    $(".form-publishproject .btn-close").unbind("click", hidePublishProject);
    return false;
}
//忘记密码
function showForgetpswForm() {
    hideAllForm();
    $(".form-forgetpsw").showByMask("mask,mask-login", $(".page"));
    $(".form-forgetpsw").position({ of: $(".items") });
    //$(".forgetpsw").unbind("click", showForgetpswForm);
    $(".form-forgetpsw .btn-close").bind("click", hideForgetpswForm);
}
function hideForgetpswForm() {
    $(".form-forgetpsw").hideByMask();
    //$(".forgetpsw").bind("click", showForgetpswForm);
    $(".form-forgetpsw .btn-close").unbind("click", hideForgetpswForm);
    return false;
}

//init
$(function () {

    initPlacholder();

    //reset page position
    $(window).bind("resize", resetPagePos);
    resetPagePos();

    $(".login").bind("click", showLoginForm);
    $(".register").bind("click", showRegisterForm);
    $(".publish").bind("click", showPublishProject);
});