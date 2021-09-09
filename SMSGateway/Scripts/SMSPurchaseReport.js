$('body').on('click', '.btnGenerateSPReport', function () {
    var fromDate = $('body').find('.fromDate').val();
    var toDate = $('body').find('.toDate').val();
    var supp = $('body').find('.supplier-list').val();
    $.ajax({
        type: "GET",
        url: "/SMSPurchaseReport/_SMSPurchaseReport",
        data: { supplier: supp, fromDate: fromDate, toDate: toDate },
        success: function (data) {
            $('body').find('.sms-purchase-report').html('')
            $('body').find('.sms-purchase-report').append(data)
        }
    })
})
