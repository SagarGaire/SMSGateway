function SendEmailComplete(response) {
    $('body').find('.edit-user-modal-body').html(response.responseJSON.message)
    $('body').find('.btn-edit-user').hide()
    $('body').find('.btnCancel').text("Dismiss")
}

function AddClientSuccess(data) {
    $('body').find('.add-client-modal-body').html(data)
    $('body').find('.btnAddClient').hide()
    $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    var currentPage = $('body').find('#currentPage').val()
    var search = $('body').find('.txt-client-search').val()
    $.ajax({
        type: "GET",
        url: "/Client/_ClientList",
        data: { search: search, pageNumber: currentPage },
        success: function (data)
        {
            $('body').find('.div-client-list-container').html('')
            $('body').find('.div-client-list-container').append(data)
        }
    })
}

function AddSMSPurchaseSuccess(data) {
    $('body').find('.add-sms-purchase-modal-body').html(data)
    $('body').find('.btnAddSMSPurchase').hide()
    $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    var li = $('.sidebar').find('li').filter(function () {
        if ($(this).hasClass('active') == true) {
            return $(this)
        }
    })
    $(li).trigger('click')
}

function EditSMSPurchaseSuccess(data) {
    $('body').find('.edit-sms-purchase-modal-body').html(data)
    $('body').find('.btnEditSMSPurchase').hide()
    $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    var li = $('.sidebar').find('li').filter(function () {
        if ($(this).hasClass('active') == true) {
            return $(this)
        }
    })
    $(li).trigger('click')
}

function AddUserSuccess(data) {
    $('body').find('.add-user-modal-body').html(data)
    $('body').find('.btn-add-user').hide()
    $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    var li = $('.sidebar').find('li').filter(function () {
        if ($(this).hasClass('active') == true) {
            return $(this)
        }
    })
    $(li).trigger('click')
}

function EditClientResult(data) {
    $('body').find('.edit-client-modal-body').html(data)
    $('body').find('.btnEditClient').hide()
    $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    var currentPage = $('body').find('#currentPage').val()
    var search = $('body').find('.txt-client-search').val()
    $.ajax({
        type: "GET",
        url: "/Client/_ClientList",
        data: { search: search, pageNumber: currentPage },
        success: function (data) {
            $('body').find('.div-client-list-container').html('')
            $('body').find('.div-client-list-container').append(data)
        }
    })
}

function EditUserResult(data) {
    $('body').find('.edit-user-modal-body').html(data)
    $('body').find('.btn-edit-user').hide()
    $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    var li = $('.sidebar').find('li').filter(function () {
        if ($(this).hasClass('active') == true) {
            return $(this)
        }
    })
    $(li).trigger('click')
}

function ChangePasswordSuccess(data) {
    if (data.status == true) {
        $('body').find('.change-password-modal-body').html(data.message)
        $('body').find('.btn-submit-change-password').hide()
        $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
    }
    else {
        $('body').find('.change-password-modal-body').find('.div-model-error').html(data.message)
    }
}

$(document).ready(function () {
    
    $('.sidebar').on('click', 'li', function () {
        if ($(this).hasClass('header') == false) {
            var menu = '<li><i class="fa fa-bars"></i> Menu</li>'
            var subMenu = $(this).closest('.treeview').find('.parent').html()

            var redirectTo = $(this).text();
            if ($(this).hasClass('treeview') == false) {
                $('.sidebar').find('li').each(function () {
                    if ($(this).hasClass('treeview') == false) {
                        $(this).removeClass('active')
                    }
                })
                $(this).addClass('active')

                $.ajax({
                    type: "GET",
                    url: "/Redirection/Redirection",
                    data: { redirectTo: redirectTo },
                    success: function (data) {
                        $('body').find('.partial-container').html(data)
                    }
                })
                if (subMenu == undefined) {
                    $('.content-header').find('.breadcrumb').html(menu + '<li class="active level-one">' + $(this).html() + '</li>')
                }
                else {
                    $('.content-header').find('.breadcrumb').html(menu + '<li class="treeview">' + subMenu + '</li>' + '<li class="active level-two">' + $(this).html() + '</li>')
                    $('.content-header').find('.breadcrumb').find('.pull-right-container').remove()
                }
            }
        }
    })

    $(document).ajaxStart(function () {

        $('.wait').css('display', 'block');
        $('.transparent-background').show();
    });

    $(document).ajaxComplete(function () {

        $('.wait').css('display', 'none');
        $('.transparent-background').hide();
    });

    $('.btn-change-password').click(function () {
        $.ajax({
            type: "GET",
            url: "/Saf/_ChangePassword",
            success: function (data) {
                $('.modal-content').html('');
                $('.modal-content').append(data)

                var form = $('.modal-content').find('#form');

                $(form).removeData("validator");
                $(form).removeData("unobtrusivevalidation");
                $.validator.unobtrusive.parse(form);

                $('.myModal').modal('show');
            }
        })
    })
})
