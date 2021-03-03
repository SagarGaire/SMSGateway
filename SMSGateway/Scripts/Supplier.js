$('body').on('click', '.supplier-add', function () {
    $.ajax({
        type: "GET",
        url: "/Supplier/_AddSupplier",
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

$('body').on('click', '.supplier-remove', function () {
    var param = $(this).closest('tr').attr('supplierID')
    $.ajax({
        type: "GET",
        url: "/Supplier/_ConfirmRemove",
        data: { id: param },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);

            var form = $('.modal-content').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModal').modal('show');
        }
    })
})

$('body').on('click', '.confirm-remove-supplier', function () {
    debugger;
    var param = $('body').find('#Supplierid').val()
    $(this).hide();
    $.ajax({
        type: "GET",
        url: "/Supplier/_RemoveSupplier",
        data: { id: param },
        success: function (data) {
            $('body').find('.remove-supplier-modal-body').html(data)
            $('body').find('.btnCancel').text('Dismiss')

            triggerClick();
        }
    })
})



function triggerClick() {
    
    $.ajax({
        type: "GET",
        url: "/Supplier/_SupplierList",
       
        success: function (data) {
            $('body').find('.partial-container').html('')
            $('body').find('.partial-container').append(data)
        }
    })
}


$('body').on('click', '.supplier-edit', function () {
    var param = $(this).closest('tr').attr('supplierID')
    $.ajax({
        type: "GET",
        url: "/Supplier/_EditSupplier",
        data: { id: param },
        success: function (data) {
            $('.modal-content').html('');
            $('.modal-content').append(data);

            var form = $('.modal-content').find('#form');

            $(form).removeData("validator");
            $(form).removeData("unobtrusivevalidation");
            $.validator.unobtrusive.parse(form);

            $('.myModal').modal('show');
        }
    })
})