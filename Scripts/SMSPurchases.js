$('.content').on('click', '.sms-purchase-add', function () {
    debugger;
    $.ajax({
        type: "GET",
        url: "/SMSPurchases/_AddSMSPurchase",
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

$('.content').on('click', '.sms-purchase-cancel', function () {
    debugger;
    var recID = $(this).closest('tr').attr('recordId')
    $.ajax({
        type: "GET",
        url: "/SMSPurchases/_CancelSMSPurchase",
        data: { recID: recID },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);
            $('.myModal').modal('show');
        }
    })
})

$('.content').on('click', '.sms-purchase-edit', function () {
    debugger;
    var recID = $(this).closest('tr').attr('recordId')
    $.ajax({
        type: "GET",
        url: "/SMSPurchases/_EditSMSPurchase",
        data: { recID: recID },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);
            $('.myModal').modal('show');
        }
    })
})

$('body').on('click', '.btnCancelSMSPurchase', function () {
    debugger;
    var recID = $('body').find('#recID').val();
    $.ajax({
        type: "POST",
        url: "/SMSPurchases/CancelSMSPurchase",
        data: { recID: recID },
        success: function (data) {
            $('body').find('.cancel-sms-purchase-modal-body').html(data)
            $('body').find('.btnCancelSMSPurchase').hide()
            var li = $('.sidebar').find('li').filter(function () {
                if ($(this).hasClass('active') == true) {
                    return $(this)
                }
            })
            $(li).trigger('click')
        }
    })
})

$('body').on('change', '.txtSMSPurchaseQuantity', function () {
    var quantity = $(this).val();
    var rate = $('body').find('.txtSMSPurchaseRate').val()
    var subtotal = quantity * rate
    var total = (quantity * rate) + (((quantity * rate) * 13) / 100)
    $('body').find('.txtSMSPurchaseSubtotal').text(subtotal)
    $('body').find('.txtSMSPurchaseTotal').text(total)

})

$('body').on('change', '.txtSMSPurchaseRate', function () {
    var rate = $(this).val();
    var quantity = $('body').find('.txtSMSPurchaseQuantity').val()
    var subtotal = quantity * rate
    var total = (quantity * rate) + (((quantity * rate) * 13) / 100)
    $('body').find('.txtSMSPurchaseSubtotal').text(subtotal)
    $('body').find('.txtSMSPurchaseTotal').text(total)
})

$('body').on('change', '.chk-show-canceled-purchases', function () {
    debugger;
    var value = $(this).is(':checked')

    $('body').find('.tbl-sms-purchases-list').find('tr').filter(function () {
        if (value == true) {
            if ($(this).hasClass('display-none') == true) {
                $(this).removeClass('display-none')
                $(this).addClass('display-row')
            }
        }
        else {
            if ($(this).hasClass('display-row') == true) {
                $(this).removeClass('display-row')
                $(this).addClass('display-none')
            }
        }
    })
})