$('body').on('click', '.btn-add-user-modal', function () {
    $.ajax({
        type: "GET",
        url: "/Saf/_AddUser",
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

$('body').on('click', '.btn-send-mail', function () {
    var body = "<text>Welcome to MicroBanker SMS Gateway System, <b>"
                + $('body').find('.credentials').find('table').find('tr:first').find('td:last').text()
                + "</b>. Following are your credentials:</text>"
                + $('body').find('.credentials').html()
                + "<br /> To visit the site, go to <a href='sms.microbankernepal.com.np' target='_blank'>http://sms.microbankernepal.com.np/</a>"
    var recipient = $('body').find("#email").val();

    $.ajax({
        type: "GET",
        url: "/Client/SendMail",
        data: { recipient: recipient, subject: "MicroBanker SMS Gateway Credential", body: body },
        success: function (data) {
            $('body').find('.add-user-modal-body').html(data);
        }
    })
})


$('body').on('click', '.btn-send-mail-approve-user', function () {
    var body = "<text>Welcome to MicroBanker SMS Gateway System. Following are your credentials:</text>"
                + $('body').find('.credentials').html()
                + "<br /> To visit the site, go to <a href='sms.microbankernepal.com.np' target='_blank'>http://sms.microbankernepal.com.np/</a>"
    var recipient = $('body').find("#email").val();
    var isChecked = $('body').find('.sendmail-checkbox').is(':checked');
    $.ajax({
        type: "GET",
        url: "/Saf/ApproveUser",
        data: { recipient: recipient, subject: "MicroBanker SMS Gateway Credential", body: body, isChecked:isChecked },
        success: function (data) {
            $('body').find('.user-approval-modal-body').html(data);
            var li = $('.sidebar').find('li').filter(function () {
                if ($(this).hasClass('active') == true) {
                    return $(this)
                }
            })
            $(li).trigger('click')
        }
    })
})



$('body').on('click', '.user-edit', function () {
    var username = $(this).closest('tr').attr("username")
    $.ajax({
        type: "GET",
        url: "/Saf/_EditUser",
        data: { username: username },
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

$('body').on('input', '.txt-user-search', function () {
    var filter = $(this).val().trim();
    $(this).closest('.partial-container').find('.table-user').find('tr').each(function () {
        if (!$(this).hasClass('parent')) {
            if ($(this).find('.td-fullname').text().search(new RegExp(filter, "i")) < 0) {
                $(this).hide()
            }
            else {
                $(this).show();
            }
        }

    });
});

