$('body').on('click', '.btnGenerateStatement', function () {
    var fromDateTime = $('body').find('.fromDate').val() + " " + $('body').find('.fromTime').val();
    var toDateTime = $('body').find('.toDate').val() + " " + $('body').find('.toTime').val()
    var client = $('body').find('.client-list').val();
    
    if (fromDateTime != null || toDateTime != null || client != null) {
        $.ajax({
            type: "GET",
            url: "/Statement/_GenerateStatement",
            data: { clientCode: client, fromDateTime: fromDateTime, toDateTime: toDateTime },
            success: function (data) {
                $('body').find('.statement').html('')
                $('body').find('.statement').append(data)
            }
        })
    }
})

$('body').on('click', '.td-load-more', function () {
    var fromDateTime = $('body').find('.fromDate').val() + " " + $('body').find('.fromTime').val();
    var toDateTime = $('body').find('.toDate').val() + " " + $('body').find('.toTime').val()
    var client = $('body').find('.client-list').val();
    var rows = parseInt($('body').find('.table-statement').find("tr:last").prev("tr").find(".td-sn").text());
    rows = rows + 50;

    if (fromDateTime != null || toDateTime != null || client != null) {
        $.ajax({
            type: "GET",
            url: "/Statement/_GenerateStatement",
            data: { clientCode: client, fromDateTime: fromDateTime, toDateTime: toDateTime, rows: rows },
            success: function (data) {
                $('body').find('.statement').html('')
                $('body').find('.statement').append(data)
            }
        })
    }
})