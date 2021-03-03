$.connection.hub.start().done(function () {
    
}).fail(function () {
    alert("Connection Failed");
})

$.connection.UpdateApprovalCount.client.sendData = function (data) {
    $('body .notification').html('');
    $('body .notification').append(data);
}