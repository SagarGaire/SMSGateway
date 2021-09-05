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

$('body').on('click', '.td-load-more', function () {
    var fromDate = $('body').find('.fromDate').val();
    var toDate = $('body').find('.toDate').val().val()
    var supp = $('body').find('.supplier').val();
    var rows = parseInt($('body').find('.table-sms-purchase-report').find("tr:last").prev("tr").find(".td-sn").text());
    rows = rows + 20;
    $.ajax({
        type: "GET",
        url: "/SMSPurchaseReport/_SMSPurchaseReport",
        data: { supplier: supp, fromDate: fromDate, toDate: toDate, rows: rows },
        success: function (data) {
            $('body').find('.sms-purchase-report').html('')
            $('body').find('.sms-purchase-report').append(data)
        }
    })
})
