//INCOMPLETE CODE FOR BACK BUTTON

$('body').on('click', '.btn-clients-previous-list', function () {
    var currentPage = $('body').find('#currentPage').val();

    $.ajax({
        type: "GET",
        url: "/Client/_Index",
        data: { pageNo: currentPage},
        success: function (data) {
            $('body').html('');
            $('body').append(data);
        }
    })
})