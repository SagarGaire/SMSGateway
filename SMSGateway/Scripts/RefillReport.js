
//$('body').on('change', '.datetime-filter', function () {
//    var isChecked = $(this).is(':checked')
//    if (isChecked) {
//        $('body').find('.fromDate, .toDate').removeAttr("disabled");
//        $('body').find('.fromDate, .toDate').datepicker("setDate", new Date())   
//    }
//    else {
//        $('body').find('.fromDate, .toDate').attr("disabled", "disabled");
//        $('body').find('.fromDate, .toDate').val('');
//    }
       
//})



//$('body').on('change', '.client-filter', function () {
//    var isChecked = $(this).is(':checked')
//    if (isChecked) {
//        $('body').find('.clientList').removeAttribute("disabled");
//        $('body').find('.clientList').val();
//    }
//    else {
//        $('body').find('.clientList').attr("disabled", "disabled");
//        $('body').find('.clientList').val('');
//    }
//})



//$('body').on('change', '.checkbox-filter-refill', function () {
//    debugger;
//    var isChecked = $(this).is(':checked')
//    var value = $(this).val();
//    if ($(this).hasClass('datetime-filter')) {
//        if (isChecked) {
//            $('body').find('.' + value).find('.fromDate, .toDate').datepicker("setDate", new Date())
//        }
//        else {
//            $('body').find('.' + value).find('.fromDate, .toDate').val('');
//        }
//    }
//    else if ($(this).hasClass('client-filter')) {
//        if (!isChecked) {
//            $('body').find('.' + value).find('.clientList').val('');
//        }


//    }
//    $('body').find('.' + value).toggleClass('display-none');
//})

$('body').on('change', '.txtPageNo-refill', function () {
    $('body').find('.pagination-text').text("");
    var fromDateTime = $('body').find('#fromDate').val();
    var toDateTime = $('body').find('#toDate').val();
    var client = $('body').find('#client').val();
    var newPageNo = parseInt($(this).val());
    var totalPages = $('body').find('#totalPages').val();

    if (newPageNo <= totalPages && newPageNo > 0) {
        $.ajax({
            type: "GET",
            url: "/RefillReport/_RefillReport",
            data: { fromDate: fromDateTime, toDate: toDateTime, client: client, pageNumber: newPageNo },
            traditional: true,
            success: function (data) {
                $('body').find('.refill-report').html('')
                $('body').find('.refill-report').append(data)
            }
        })
    }
    else if (newPageNo <= 0) {
        $('body').find('.pagination-message-refill').text("Invalid page number")
    }
    else {
        $('body').find('.pagination-message-refill').text("Selected page number exceeds the total number of pages")
    }
})


$('body').on('click', '.btnPage-refill', function () {
    $('body').find('.pagination-text').text("");
    var fromDateTime = $('body').find('#fromDate').val();
    var toDateTime = $('body').find('#toDate').val();
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
            url: "/RefillReport/_RefillReport",
            data: { fromDate: fromDateTime, toDate: toDateTime, client: client, pageNumber: newPageNo },
            traditional: true,
            success: function (data) {
                $('body').find('.refill-report').html('');
                $('body').find('.refill-report').append(data);
            }
        })
    }
    else if (newPageNo <= 0) {
        $('body').find('.pagination-message-refill').text("Invalid page number");
    }
    else {
        $('body').find('.pagination-message-refill').text("Selected page number exceeds the total number of pages");
    }
})





$('body').on('click', '.btnFilter-refill', function () {
    $('body').find('.pagination-text').text("");
    var fromDateTime = $('body').find('.fromDate').val();
    var toDateTime = $('body').find('.toDate').val();
    var client = $('body').find('.clientList').val();
    var clientString = "";

    //if ($('body').find('.datetime-filter').is(':checked') == false) {
    //    fromDateTime = null
    //    toDateTime = null
    //    $('body').find('#fromDate').val(fromDateTime);
    //    $('body').find('#toDate').val(toDateTime);
    //}


    //if ($('body').find('.client-filter').is(':checked') == false) {
    //    client = null
    //    $('body').find('#clients').val(client);
    //}
    //else {
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
    //}

    if (fromDateTime != null || toDateTime != null || client != null) {
        $.ajax({
            type: "GET",
            url: "/RefillReport/_RefillReport",
            data: { fromDate: fromDateTime, toDate: toDateTime, client: clientString, pageNumber: 1 },
            traditional: true,
            success: function (data) {
                $('body').find('.refill-report').html('')
                $('body').find('.refill-report').append(data)
            }
        })
    }

})




$('body').on('change', '.canceled-purchase-refills', function () {
    debugger;
    var value = $(this).is(':checked')

    $('body').find('.tbl-refill-report').find('tr').filter(function () {
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