$('body').on('click', '.expected-balance-info', function () {
    $.ajax({
        type: "GET",
        url: "/Dashboard/_ExpectedBalance",
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);

            $('.myModal').modal('show');
        }
    })
})