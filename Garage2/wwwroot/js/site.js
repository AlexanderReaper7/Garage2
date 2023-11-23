$(document).ready(function () {
    // Write the url ?parameters to each of their input elements on the page. Effectively retaining the values between page loads.
    (new URL(window.location.href)).searchParams.forEach((x, y) => document.getElementById(y).value = x);
});
$(document).ready(function () {
    // Store the initial values of the editable fields
    var initialValues = {
        RegistrationNumber: $("#registrationNumber").val(),
        Color: $("#color").val(),
        Brand: $("#brand").val(),
        Model: $("#model").val(),
        NumberOfWheels: $("#numberOfWheels").val()
        // Retrieve other properties as needed
    };

    // Track changes when the user edits a field
    $('input').on('input', function () {
        var fieldName = $(this).attr('name');
        if (initialValues[fieldName] !== $(this).val()) {
            $(this).addClass('edited');
        } else {
            $(this).removeClass('edited');
        }
    });

    // Show a pop-up when the form is submitted
    $('form').submit(function (e) {
        e.preventDefault(); // Prevent the form from submitting normally

        var changedFields = $('input.edited');

        if (changedFields.length > 0) {
            // Build a message displaying the changes
            var message = "You have made these changes :\n";
            changedFields.each(function () {
                var fieldName = $(this).attr('name');
                message += fieldName + ": " + initialValues[fieldName] + " => " + $(this).val() + "\n";
            });

            // Show a Bootstrap modal with the message and center it
            var changesModal = $('#changesModal');
            changesModal.find('#changesMessage').text(message);
            changesModal.modal('show');
            changesModal.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({
                    width: 'auto',
                    'max-width': '80%'
                });
            });

            // Perform AJAX form submission
            $.ajax({
                url: $(this).attr('action'),
                type: $(this).attr('method'),
                data: $(this).serialize(),
                success: function (data) {
                    // Handle success response if needed
                    // For example, you can update the UI or perform other actions
                    console.log("Form submitted successfully");
                },
                error: function (xhr, status, error) {
                    // Handle error response if needed
                    console.error("Form submission failed:", error);
                },
                complete: function () {
                    // Close the modal after submission
                    changesModal.modal('hide');
                }
            });
        }
    });

    // Close the modal when the "Close" button inside the modal is clicked
    $('#changesModal').on('click', '#changesModalCloseButton', function () {
        $('#changesModal').modal('hide');
    });
});

$(document).ready(function () {
    // Retrieve initial values from hidden inputs
    var initialValues = {
        PersonNumber: $("#personNumber").val(),
        FirstName: $("#firstName").val(),
        LastName: $("#lastName").val(),
        Membership: $("#membership").val()
        // Add other properties as needed
    };

    // Track changes when the user edits a field
    $('input').on('input', function () {
        var fieldName = $(this).attr('name');
        if (initialValues[fieldName] !== $(this).val()) {
            $(this).addClass('edited');
        } else {
            $(this).removeClass('edited');
        }
    });

    // Show a pop-up when the form is submitted
    $('form').submit(function (e) {
        e.preventDefault(); // Prevent the form from submitting normally

        var changedFields = $('input.edited');

        if (changedFields.length > 0) {
            // Build a message displaying the changes
            var message = "You have made these changes :\n";
            changedFields.each(function () {
                var fieldName = $(this).attr('name');
                message += fieldName + ": " + initialValues[fieldName] + " => " + $(this).val() + "\n";
            });

            // Show a Bootstrap modal with the message and center it
            var changesModal = $('#changesModal');
            changesModal.find('#changesMessage').text(message);
            changesModal.modal('show');
            changesModal.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({
                    width: 'auto',
                    'max-width': '80%'
                });
            });

            // Perform AJAX form submission
            $.ajax({
                url: $(this).attr('action'),
                type: $(this).attr('method'),
                data: $(this).serialize(),
                success: function (data) {
                    // Handle success response if needed
                    // For example, you can update the UI or perform other actions
                    console.log("Form submitted successfully");
                },
                error: function (xhr, status, error) {
                    // Handle error response if needed
                    console.error("Form submission failed:", error);
                },
                complete: function () {
                    // Close the modal after submission
                    changesModal.modal('hide');
                }
            });
        }
    });

    // Close the modal when the "Close" button inside the modal is clicked
    $('#changesModal').on('click', '#changesModalCloseButton', function () {
        $('#changesModal').modal('hide');
    });
});

$(document).ready(function () {
    // Retrieve initial values from hidden inputs
    var initialValues = {
        Name: $("#name").val(),
        Size: $("#size").val()
        // Add other properties as needed
    };

    // Track changes when the user edits a field
    $('input').on('input', function () {
        var fieldName = $(this).attr('name');
        if (initialValues[fieldName] !== $(this).val()) {
            $(this).addClass('edited');
        } else {
            $(this).removeClass('edited');
        }
    });

    // Show a pop-up when the form is submitted
    $('form').submit(function (e) {
        e.preventDefault(); // Prevent the form from submitting normally

        var changedFields = $('input.edited');

        if (changedFields.length > 0) {
            // Build a message displaying the changes
            var message = "You have made these changes :\n";
            changedFields.each(function () {
                var fieldName = $(this).attr('name');
                message += fieldName + ": " + initialValues[fieldName] + " => " + $(this).val() + "\n";
            });

            // Show a Bootstrap modal with the message and center it
            var changesModal = $('#changesModal');
            changesModal.find('#changesMessage').text(message);
            changesModal.modal('show');
            changesModal.on('shown.bs.modal', function () {
                $(this).find('.modal-dialog').css({
                    width: 'auto',
                    'max-width': '80%'
                });
            });

            // Perform AJAX form submission
            $.ajax({
                url: $(this).attr('action'),
                type: $(this).attr('method'),
                data: $(this).serialize(),
                success: function (data) {
                    // Handle success response if needed
                    // For example, you can update the UI or perform other actions
                    console.log("Form submitted successfully");
                },
                error: function (xhr, status, error) {
                    // Handle error response if needed
                    console.error("Form submission failed:", error);
                },
                complete: function () {
                    // Close the modal after submission
                    changesModal.modal('hide');
                }
            });
        }
    });

    // Close the modal when the "Close" button inside the modal is clicked
    $('#changesModal').on('click', '#changesModalCloseButton', function () {
        $('#changesModal').modal('hide');
    });
});