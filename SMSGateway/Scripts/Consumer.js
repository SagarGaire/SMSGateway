$('body').on('click', '.btnGenerateReport', function () {
    var fromDate = $('body').find('.fromDate').val() + " ";
    var toDate = $('body').find('.toDate').val() + " ";
    var client = $('body').find('.client-list').val();
    var type = $('body').find('.clientType').val();

    if (fromDate != null || toDate != null || client != null || type != null) {
        $.ajax({
            type: "GET",
            url: "/Consumer/_ConsumerReport",
            data: { clientCode: client, clientType: type, fromDate: fromDate, toDate: toDate },
            success: function (data) {
                $('body').find('.consumer').html('')
                $('body').find('.consumer').append(data)
            }
        })
    }
    else if (client == null || fromDate != null || toDate != null || type != null) {
        $.ajax({
            type: "GET",
            url: "/Consumer/_ConsumerReport",
            data: { clientCode: db.Clients, clientType: type, fromDate: fromDate, toDate: toDate },
            success: function (data) {
                $('body').find('.consumer').html('')
                $('body').find('.consumer').append(data)
            }
        })
        //else (client == null && fromDate = null && toDate = null && type != null) {
        //    $.ajax({
        //        type: "GET",
        //        url: "/Consumer/_ConsumerReport",
        //        data: { clientCode: db.Clients, clientType: type, fromDate: fromDate, toDate: toDate },
        //        success: function (data) {
        //            $('body').find('.consumer').html('')
        //            $('body').find('.consumer').append(data)
        //        }
        //    })
        //}
    }
})

$('body').on('click', '.td-load-more', function () {
    var fromDate = $('body').find('.fromDate').val();
    var toDate = $('body').find('.toDate').val().val()
    var client = $('body').find('.clientCode').val();
    var rows = parseInt($('body').find('.table-consumer-report').find("tr:last").prev("tr").find(".td-sn").text());
    rows = rows + 50;

    if (fromDate != null || toDate != null || client != null || type != null) {
        $.ajax({
            type: "GET",
            url: "/Consumer/_ConsumerReport",
            data: { clientCode: client, clientType: type, fromDate: fromDate, toDate: toDate, rows: rows },
            success: function (data) {
                $('body').find('.consumer').html('')
                $('body').find('.consumer').append(data)
            }
        })
    }
})
