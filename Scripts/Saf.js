$('body').on('click', '.btn-add-user-modal',function(){
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