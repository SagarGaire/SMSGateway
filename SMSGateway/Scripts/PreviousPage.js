$('body').on('click', '.level-one', function () {
    var value = $('.level-one a').text();
    
    var currentPage = $('body').find('#currentPage').val();
    var search = $('body').find('#searchItem').val();
    if (value == ' Clients')                     
    {
        $.ajax({
            type: "GET",
            url: "/Client/_Index",
            data: data,
            success: function (data) {
                $('body').find('.partial-container').html('');
                $('body').find('.partial-container').append(data);
                $('.partial-container').fadeOut(0);
                callRequiredPage(currentPage, search);
            }
        });
    }
});

function callRequiredPage(currentPage, search) {
    $.ajax({
        type: "GET",
        url: "/Client/_ClientList",
        data: { pageNumber: currentPage, search: search },
        success: function (data) {
            $('body').find('.div-client-list-container').html('');
            $('body').find('.div-client-list-container').append(data);
            $('.partial-container').fadeIn(50);
        }
    })
}