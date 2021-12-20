function triggerClickClientList() {
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

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Client/Notification",
        success: function (data) {
            $('.notification').html('');
            $('.notification').append(data);
        }
    })
})



$('.content').on('click', '.client-add', function () {
    var pageNo = $('body').find('#currentPage').val()
    var search = $('body').find('.txt-client-search').val()
    $.ajax({
        type: "GET",
        url: "/Client/_AddClient",
        data: { search: search, pageNumber: pageNo },
        success: function (data) {

            $('.modal-content-lg').html('');
            $('.modal-content-lg').append(data).find('.txtPasskey').val('');

            var form = $('.modal-content-lg').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModalLg').modal('show');
        }
    })
})

$('body').on('click', '.client-info', function () {
    var param = $(this).closest('tr').attr('clientCode')
    $.ajax({
        type: "GET",
        url: "/Client/_InfoClient",
        data: { id: param },
        success: function (data) {
            $('.modal-content-lg').html('');
            $('.modal-content-lg').append(data);

            var form = $('.modal-content-lg').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModalLg').modal('show');
        }
    })
})


function btnCopyClient() {
    $('.modal-content-lg').append('<textarea class="copytoclipboard"></textarea>');
    var textbox = $('.copytoclipboard');

    var copytext = $('.copyme').html();
    copytext = copytext.replace(/<li>/g, '');
    copytext = copytext.replace(/<\/\li>/g, '');

    $('.copytoclipboard').val(copytext);
    textbox.focus();
    textbox.select();
    document.execCommand('copy');

    $('.copytoclipboard').remove();
}

$('body').on('click', '.client-edit', function () {
    var param = $(this).closest('tr').attr('clientCode')
    $.ajax({
        type: "GET",
        url: "/Client/_EditClient",
        data: { id: param },
        success: function (data) {
            $('.modal-content-lg').html('');
            $('.modal-content-lg').append(data);

            var form = $('.modal-content-lg').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModalLg').modal('show');
        }
    })
})

$('body').on('click', '.balance-refill', function () {
    var balance = $(this).closest('tr').find('.td-balance').text();
    var clientCode = $(this).closest('tr').attr('clientCode')
    var clientName = $(this).closest('tr').find('.clientName').text();
  
    $.ajax({
        type: "GET",
        url: "/Client/_RefillBalance",
        data: { clientCode: clientCode, clientName: clientName, balance: balance },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);

            var form = $('.modal-content').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModal').modal('show');
        }
    })
})

$('body').on('click', '.confirm-change-status', function () {
    var param = $(this).closest('tr').attr('clientCode')
    $.ajax({
        type: "GET",
        url: "/Client/_ConfirmChangeStatus",
        data: { id: param },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);

            var form = $('.modal-content').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModal').modal('show');
        }
    })
})

$('body').on('click', '.change-status', function () {
    debugger;
    var param = $('body').find('#ClientCode').val();
    $.ajax({
        type: "POST",
        url: "/Client/_ChangeStatus",
        data: { id: param },
        success: function (data) {
            $('body').find('.change-status, .change-status-modal-body').remove();
            $('body').find('.btnCancel').text('Dismiss');
            $('body').find('.successmessage').text('The selected client client has been changed successfully.');

            triggerClickClientList();
        }
    })
})

$('body').on('click', '.btn-generate-passkey', function () {
    var container = $(this).parent().parent().find('.txtPassKey')
    $.ajax({
        type: "GET",
        url: "/Client/_GetUUID",
        success: function (data) {
            $(container).val(data)
        }
    })
})

$('body').on('mousedown', '.btn-show-passkey', function () {
    $(this).parent().parent().find('.txtPassKey').attr('type', 'text')
})

$('body').on('mouseup', '.btn-show-passkey', function () {
    $(this).parent().parent().find('.txtPassKey').attr('type', 'password')
})

$('body').on('click', '.btnAddRecipient', function () {
    $('body').find('.recipient').show();
    $('body').find('.btn-container').show();
    $('body').find('.btn-send-email-container').show();
    $(this).closest('div').hide();
})

$('body').on('click', '.btnAddCC', function () {
    $('body').find('.cc').show();
    $('body').find('.multiple-email-info').show();
})

$('body').on('click', '.btn-send-email', function () {
    debugger;
    var recipient = $('body').find('.recipient').find('input:text').val();
    var cc = $('body').find('.cc').find('input:text').val();
    var isUser
    var body = "<text>Welcome to MicroBanker SMS Gateway System. Following are your credentials:</text><br /><br />"
        + $('body').find('.credentials').html()

    if ($('body').find('.credentials').hasClass('exists') == true) {
        body = body + "<br />Use your EmailID and your Passkey to login to our Client Portal by following this <a href='http://sms.microbankernepal.com.np:81/' target='_blank'>link</a>.";
    }

    $.ajax({
        type: "GET",
        url: "/Client/SendMail",
        data: { recipient: recipient, CC: cc, subject: "MicroBanker SMS Gateway Credential", body: body },
        success: function (data) {
            $('body').find('.add-client-modal-body').html(data);
        }
    })
})

$('body').on('click', '.btnRefillBalance', function () {
    debugger;
    var container = $('body').find('.refill-balance-modal-body');
    var clientCode = $(container).find('div:first').find('text').text();
    var billno = $(container).find('.txtbillNo').val();
    var clientName = $(container).find('.txt-client-name').text();
    var currentBalance = $(container).find('.txtCurrentBalance').text();
    var updatedBalance = $(container).find('.txtTotalBalance').text();
    var refillBalance = $(container).find('.txtBalanceToRefill').val();
    var remarks = $(container).last('div').find('textarea').val();
   
    $.ajax({
        type: "GET",
        url: "/Client/_ConfirmRefill",
        data: { clientCode: clientCode, clientName: clientName, currentBalance: currentBalance, newBalance: updatedBalance, refillAmount: refillBalance, remarks: remarks, billno: billno },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);
        }
    })
})

$('body').on('change', '.txtBalanceToRefill', function () {
    var refill = $(this).val();
    if (parseInt(refill) < 0) {
        $(this).val('')
    }
    else {
        var currentBalance = $(this).closest('.modal-body').find('.txtCurrentBalance').text();
        var totalBalance = parseInt(refill) + parseInt(currentBalance);
        if (currentBalance < totalBalance) {
            $(this).closest('.modal-body').find('.txtTotalBalance').html(totalBalance)
            $('body').find('.btnRefillBalance').prop("disabled", false)
        }
    }
})

$('body').on('click', '.btnCancelRefill', function () {
    debugger;
    var container = $('body').find('.refill-balance-modal-body');
    var currentBalance = $(container).find('.currentBalance').text()
    var clientCode = $(container).find('.clientCode').text()
    var clientName = $(container).find('.clientName').text()
    var newBalance = $(container).find('.newBalance').text()
    var remarks = $(container).find('.remarks').text()
    var refillAmount = $(container).find('.refillAmount').text()
    var billNo = $(container).find('.billno').text()
    $.ajax({
        type: "GET",
        url: "/Client/_RefillBalance",
        data: { clientCode: clientCode, clientName: clientName, balance: currentBalance, newBalance: newBalance, refillAmount: refillAmount, remarks: remarks, billno: billNo },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);

            var form = $('.modal-content').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModal').modal('show');
        }
    })
})

$('body').on('click', '.btnConfirmRefill', function () {
    var container = $('body').find('.refill-balance-modal-body');
    var clientCode = $(container).find('.clientCode').text()
    var clientName = $(container).find('.clientName').text()
    var remarks = $(container).find('.remarks').text()
    var refillAmount = $(container).find('.refillAmount').text()
    var billno = $(container).find('.billno').text()

    $.ajax({
        type: "GET",
        url: "/Client/RefillBalance",
        data: { clientCode: clientCode, refillAmount: refillAmount, remarks: remarks, billno: billno },
        success: function (data) {
            $('body').find('.refill-balance-modal-body').append(data)
            $('body').find('.btnConfirmRefill').hide()
            $('body').find('.btnCancelRefill').hide()
            $('body').find('.btnCancel').show()
        }
    })
})

$('body').on('click', '.clientName', function () {
    var clientCode = $(this).closest('tr').attr('clientCode')
    $.ajax({
        type: "GET",
        url: "/Client/_ClientInfo",
        data: { clientCode: clientCode },
        success: function (data) {
            $('body').find('.partial-container').html('')
            $('body').find('.partial-container').append(data)
            $('html, body').animate({ scrollTop: 0 }, 0);
        }
    })
})

$('body').on('click', '.undo-refill', function () {
    var refillId = $(this).closest('tr').attr('refillId')
    $.ajax({
        type: "GET",
        url: "/Client/_UndoRefill",
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);
            $('body').find('.modal-body').find('span').text(refillId)

            $('.myModal').modal('show');
        }
    })
})

$('body').on('click', '.edit-refill', function () {
    if ($(this).find('a').text() == "Edit") {
        var value = $(this).closest('tr').find('.td-remarks').text()
        var bill = $(this).closest('tr').find('.td-bill-no').text()
        $('body').find('.div-original-remarks').text(value)
        $('body').find('.div-original-bill-no').text(bill)
        $(this).find('a').text('Cancel')
        $(this).closest('tr').find('.td-remarks').html('<input type="text" class="txt-edit-refill" value="' + value + '" />')
        $(this).closest('tr').find('.td-bill-no').html('<input type="text" class="txt-edit-bill-no" value="' + bill + '" />')

    }
    else {
        var value = $(this).closest('tr').find('.td-remarks').find('input').val()
        var bill  = $(this).closest('tr').find('.td-bill-no').find('input').val()
        $(this).find('a').text('Edit')
        $(this).closest('tr').find('.td-remarks').html('')
        $(this).closest('tr').find('.td-bill-no').html('')
        $(this).closest('tr').find('.td-remarks').text($('body').find('.div-original-remarks').text())
        $(this).closest('tr').find('.td-bill-no').text($('body').find('.div-original-bill-no').text())
    }


})

$('body').on('keypress', '.txt-edit-refill', function (e) {
    var container = $(this).closest('tr')
    if (e.keyCode == 13 || e.keyCode == 9) {
        var refillId = $(this).closest('tr').attr('refillId');
        var remarks = $(this).closest('tr').find('.td-remarks').find('input').val();
        var billno = $(this).closest('tr').find('.td-bill-no').find('input').val();
        $.ajax({
            type: "GET",
            url: "/Client/EditRefill",
            data: { refillId: refillId, remarks: remarks, billno: billno },
            success: function (data) {
                if (data.editSuccess == true) {
                    $(container).find('.td-remarks').html('')
                    $(container).find('.td-remarks').text(remarks)
                    $(container).find('.td-bill-no').html('')
                    $(container).find('.td-bill-no').text(billno)
                    $('body').find('.message').html('<text class="text-success"><b>' + data.message + '</b></text>').show()
                    setTimeout(function () {
                        $('body').find('.message').fadeOut('1000')
                    }, 1000)
                    $(container).find('.edit-refill').find('a').text('Edit')
                }
                else {
                    $('body').find('.message').html('<text class="text-danger"><b>' + data.message + '</b></text>')
                }
            }
        })
    }
})

$('body').on('keydown', '.txt-edit-refill', function (e) {
    if (e.keyCode == 9) {
        var container = $(this).closest('tr')
        var refillId = $(this).closest('tr').attr('refillId');
        var remarks = $(this).closest('tr').find('.td-remarks').find('input').val();
        var billno = $(this).closest('tr').find('.td-bill-no').find('input').val();
        $.ajax({
            type: "GET",
            url: "/Client/EditRefill",
            data: { refillId: refillId, remarks: remarks, billno: billno },
            success: function (data) {
                if (data.editSuccess == true) {
                    $(container).find('.td-remarks').html('')
                    $(container).find('.td-remarks').text(remarks)
                    $(container).find('.td-bill-no').html('')
                    $(container).find('.td-bill-no').text(billno)
                    $('body').find('.message').html('<text class="text-success"><b>' + data.message + '</b></text>').show()
                    setTimeout(function () {
                        $('body').find('.message').fadeOut('1000')
                    }, 1000)
                    $(container).find('.edit-refill').find('a').text('Edit')
                }
                else {
                    $('body').find('.message').html('<text class="text-danger"><b>' + data.message + '</b></text>')
                }
            }
        })
    }
})

$('body').on('click', '.btnUndoRefill', function () {
    var refillId = $('body').find('.modal-body').find('span').text();
    var clientCode = $('body').find('.tbl-client-info').find('tr:first').next('tr').find('td:last').text()
    var canceled = $('body').find('.canceled-refills').is(':checked')

    $.ajax({
        type: "GET",
        url: "/Client/UndoRefill",
        data: { refillId: refillId, clientCode: clientCode, canceled: canceled },
        success: function (data) {

            $('body').find('.refill-history').html('')
            $('body').find('.refill-history').append(data)

            $('.modal-body').html('');

            var error = $('body').find('.refill-history').find('#errorMessage').val();
            debugger;
            if (error == "") {
                $('body').find('.modal-body').html("<text class='text-success'>Selected refill has been undone</text>");
            }
            else {
                $('body').find('.modal-body').append(error)
            }

            $('body').find('.modal-footer').find('input:first').hide()
            $('body').find('.modal-footer').find('.btnCancel').text('Dismiss')
        }
    })
})

$('body').on('change', '.canceled-refills', function () {
    debugger;
    var clientCode = $('body').find('.tbl-client-info').find('tr:first').next('tr').find('td:last').text()
    var value = $(this).is(":checked")
    $.ajax({
        type: "GET",
        url: "/Client/_RefillHistory",
        data: { clientCode: clientCode, canceled: value },
        success: function (data) {
            $('.refill-history').html('')
            $('.refill-history').append(data)
        }
    })
})

$('body').on('change', '.colorpicker', function () {
    var value = $(this).val();
    $('body').find('.color-preview').css("background-color", $(this).val());
})

$('body').on('change', '.chk-post-billing', function () {
    var value = $(this).is(':checked')

    if (value == true) {
        $('body').find('.div-monthly-limit').show()
    }
    else {
        $('body').find('.div-monthly-limit').hide()
    }
})

$('body').on('change', '.chk-edit-post-billing', function () {
    var value = $(this).is(':checked')

    if (value == true) {
        $('body').find('.div-monthly-limit').show()
    }
    else {
        $('body').find('.div-monthly-limit').hide()
    }
})

$('body').on('click', '.btnClientPage', function () {   //For Next Page or Previous Page with Next or Previous Button
    $('body').find('.pagination-text').text("");
    var pageNo = parseInt($(this).closest('.pagination').find('input:text').val());
    var totalPages = $('body').find('#totalPages').val();
    var search = $('body').find('.txt-client-search').val();
    var newPageNo = pageNo;

    if ($(this).hasClass('btnNextPage') == true) {
        newPageNo = pageNo + 1;
    }
    else {
        newPageNo = pageNo - 1;
    }

    if (pageNo != newPageNo && newPageNo > 0 && newPageNo <= totalPages) {
        $.ajax({
            type: "GET",
            url: "/Client/_ClientList",
            data: { pageNumber: newPageNo, search: search },
            traditional: true,
            success: function (data) {
                $('body').find('.div-client-list-container').html('');
                $('body').find('.div-client-list-container').append(data);
                $('html, body').animate({ scrollTop: 0 }, 0);
            }
        })
    }
    else if (newPageNo <= 0) {
        $('body').find('.pagination-message').text("Invalid page number");
        $('body').find('.pagination').find('input:text').val(pageNo)
    }
    else {
        $('body').find('.pagination-message').text("Selected page number exceeds the total number of pages");
        $('body').find('.pagination').find('input:text').val(pageNo)
    }
})

$('body').on('change', '.txtClientPageNo', function () {    //For Next or Previous Page by entering Page Number
    $('body').find('.pagination-text').text("");
    var pageNo = $('body').find('#currentPage').val();
    var newPageNo = parseInt($(this).val());
    var search = $('body').find('.txt-client-search').val();
    var totalPages = $('body').find('#totalPages').val();

    if (newPageNo <= totalPages && newPageNo > 0) {
        $.ajax({
            type: "GET",
            url: "/Client/_ClientList",
            data: { pageNumber: newPageNo, search: search },
            traditional: true,
            success: function (data) {
                $('body').find('.div-client-list-container').html('')
                $('body').find('.div-client-list-container').append(data)
                $('html, body').animate({ scrollTop: 0 }, 0);
            }
        })
    }
    else if (newPageNo <= 0) {
        $('body').find('.pagination-message').text("Invalid page number")
        $('body').find('.pagination').find('input:text').val(pageNo)
    }
    else {
        $('body').find('.pagination-message').text("Selected page number exceeds the total number of pages")
        $('body').find('.pagination').find('input:text').val(pageNo)
    }
})

$('body').on('click', '.btn-client-search', function () {
    var search = $(this).closest('.client-search').find('.txt-client-search').val()
    $.ajax({
        type: "GET",
        url: "/Client/_ClientList",
        data: { search: search },
        success: function (data) {
            $('body').find('.div-client-list-container').html('')
            $('body').find('.div-client-list-container').append(data)
        }
    })
})

$('body').on('change', '.txt-client-search', function () {
    $('body').find('.btn-client-search').trigger('click')
})


//$('body').on('input', '.txt-client-search', function () {
//    var filter = $(this).val().trim();
//    $(this).closest('.partial-container').find('.table-client').find('tr').each(function () {
//        if (!$(this).hasClass('parent')) {
//            if ($(this).find('.clientName').text().search(new RegExp(filter, "i")) < 0) {
//                $(this).hide()
//            }
//            else {
//                $(this).show();
//            }
//        }

//    });
//});

$('body').on('click', '.btn-edit-client-info', function () {
    var contact = $('body').find('.td-contact').text();
    var email = $('body').find('.td-email').text();
    var alertLimit = $('body').find('.td-alertlimit').text();

    $(this).hide();
    $('body').find('.btn-submit-client-info').removeClass('display-none');
    $('body').find('.btn-cancel-edit-client-info').removeClass('display-none');

    $('body').find('.td-contact').html("<input type='text' class='txt-client-info-contact' value='" + contact + "' />")
    $('body').find('.td-email').html("<input type='text' class='txt-client-info-email' value='" + email + "' />")
    $('body').find('.td-alertlimit').html("<input type='text' class='txt-client-info-alertlimit' value='" + alertLimit + "' />")

    $('body').find('.txt-client-info-contact').focus()
})

$('body').on('click', '.btn-cancel-edit-client-info', function () {
    var contact = $('body').find('.txt-client-info-contact').val();
    var email = $('body').find('.txt-client-info-email').val();
    var alertLimit = $('body').find('.txt-client-info-alertlimit').val();

    $(this).addClass('display-none')
    $('body').find('.btn-submit-client-info').addClass('display-none')
    $('body').find('.btn-edit-client-info').show();

    $('body').find('.td-contact').html(contact)
    $('body').find('.td-email').html(email)
    $('body').find('.td-alertlimit').html(alertLimit)
})

$('body').on('click', '.btn-submit-client-info', function () {
    var clientCode = $('body').find('.td-clientcode').text();
    var contact = $('body').find('.txt-client-info-contact').val();
    var email = $('body').find('.txt-client-info-email').val();
    var alertLimit = $('body').find('.txt-client-info-alertlimit').val();

    $.ajax({
        type: "GET",
        url: "/ClientPortal/_EditClientInfo",
        data: { clientCode: clientCode, contact: contact, email: email, alertLimit: alertLimit },
        success: function (data) {
            if (data.status = true) {
                $('body').find('.btn-submit-client-info').addClass('display-none')
                $('body').find('.btn-cancel-edit-client-info').addClass('display-none')
                $('body').find('.btn-edit-client-info').show();

                $('body').find('.td-contact').html(contact)
                $('body').find('.td-email').html(email)
                $('body').find('.td-alertlimit').html(alertLimit)

                $('body').find('.message').html(data.message)
                setTimeout(function () {
                    $('body').find('.message').animate({
                        opacity: 0
                    }, 1000, function () {
                        $('body').find('.message').html('');
                        $('body').find('.message').css("opacity", 1);
                    })
                }, 2000)
                return;
            }
            $('body').find('.message').html(data.message)
        }
    })
})


$('body').on('click', '.client-approval-edit', function () {
    var clientID = $(this).closest('tr').attr('clientcode')
    $.ajax({
        type: "GET",
        url: "/Saf/AddApprovedUser",
        data: { clientID:clientID },
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


