$('body').on('change', '.checkbox-filter', function () {
    debugger;
    var isChecked = $(this).is(':checked')
    var value = $(this).val();
    $('body').find('.' + value).toggleClass('display-none');
})

$('body').on('change', '.txtPageNo', function () {
    $('body').find('.pagination-text').text("");
    var fromDateTime = $('body').find('#fromDate').val();
    var toDateTime = $('body').find('#toDate').val();
    var recipients = $('body').find('#recipients').val();
    var status = $('body').find('#status').val();
    var client = $('body').find('#client').val();
    var newPageNo = parseInt($(this).val());
    var totalPages = $('body').find('#totalPages').val();

    if (newPageNo <= totalPages && newPageNo > 0) {
        $.ajax({
            type: "GET",
            url: "/OutgoingSMS/_OutgoingSMSList",
            data: { fromDate: fromDateTime, toDate: toDateTime, recipients: recipients, status: status, client: client, pageNumber: newPageNo },
            traditional: true,
            success: function (data) {
                $('body').find('.outgoing-sms-list').html('')
                $('body').find('.outgoing-sms-list').append(data)
            }
        })
    }
    else if (newPageNo <= 0){
        $('body').find('.pagination-message').text("Invalid page number")
    }
    else {
        $('body').find('.pagination-message').text("Selected page number exceeds the total number of pages")
    }
})


$('body').on('click', '.btnPage', function () {
    $('body').find('.pagination-text').text("");
    var fromDateTime = $('body').find('#fromDate').val();
    var toDateTime = $('body').find('#toDate').val();
    var recipients = $('body').find('#recipients').val();
    var status = $('body').find('#status').val();
    var client = $('body').find('#client').val();
    var pageNo = parseInt($(this).closest('.pagination').find('input:text').val());
    var totalPages = $('body').find('#totalPages').val();
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
            url: "/OutgoingSMS/_OutgoingSMSList",
            data: { fromDate: fromDateTime, toDate: toDateTime, recipients: recipients, status: status, client: client, pageNumber: newPageNo },
            traditional: true,
            success: function (data) {
                $('body').find('.outgoing-sms-list').html('');
                $('body').find('.outgoing-sms-list').append(data);
            }
        })
    }
    else if (newPageNo <= 0) {
        $('body').find('.pagination-message').text("Invalid page number");
    }
    else {
        $('body').find('.pagination-message').text("Selected page number exceeds the total number of pages");
    }
})

$('body').on('click', '.btnFilter', function () {
    $('body').find('.pagination-text').text("");
    var fromDateTime = $('body').find('.fromDate').val() + " " + $('body').find('.fromTime').val();
    var toDateTime = $('body').find('.toDate').val() + " " + $('body').find('.toTime').val()
    var recipients = $('body').find('.div-recipient-filter').find('input:text').val()
    var status = $('body').find('.statusList').val();
    var client = $('body').find('.clientList').val();    
    var statusString = "";
    var clientString = "";

    if ($('body').find('.datetime-filter').is(':checked') == false) {
        fromDateTime = null
        toDateTime = null
        $('body').find('#fromDate').val(fromDateTime);
        $('body').find('#toDate').val(toDateTime);
    }
    if ($('body').find('.recipient-filter').is(':checked') == false) {
        recipients = null
        $('body').find('#recipients').val(recipients);
    }
    if ($('body').find('.status-filter').is(':checked') == false) {
        status = null
        $('body').find('#status').val(status);
    }
    else {
        if (status != null) {
            var length = $(status).length;
            for (i = 0; i < length; i++) {
                if (i != length - 1) {
                    statusString = statusString + status[i] + ",";
                }
                else {
                    statusString = statusString + status[i];
                }
            }
        }
    }
    if ($('body').find('.client-filter').is(':checked') == false) {
        client = null
        $('body').find('#clients').val(client);
    }
    else {
        if (client != null) {
            var length = $(client).length;
            for (i = 0; i < length; i++) {
                if (i != length - 1) {
                    clientString = clientString + client[i] + ",";
                }
                else {
                    clientString = clientString + client[i];
                }
            }
            $('body').find('#clients').val(clientString);
        }
    }

    if (fromDateTime || null || toDateTime != null || recipients != null || status != null || client != null) {
        $.ajax({
            type: "GET",
            url: "/OutgoingSMS/_OutgoingSMSList",
            data: { fromDate: fromDateTime, toDate: toDateTime, recipients: recipients, status: statusString, client: clientString, pageNumber: 1 },
            traditional: true,
            success: function (data) {
                $('body').find('.outgoing-sms-list').html('')
                $('body').find('.outgoing-sms-list').append(data)
            }
        })
    }
})

