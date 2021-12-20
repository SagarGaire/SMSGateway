$('body').on('click', '.btnGenerateRevenueReport', function () {
    var fromDate = $('body').find('.fromDate').val() + " ";
    var toDate = $('body').find('.toDate').val() + " ";
    $.ajax({
        type: "GET",
        url: "/RevenueReport/_RevenueReport",
        data: { fromDate: fromDate, toDate: toDate },
        success: function (data) {
            $('body').find('.revenue-report').html('')
            $('body').find('.revenue-report').append(data)
        }
    })
})