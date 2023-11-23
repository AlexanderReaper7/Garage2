$(document).ready(function () {
    $("#FirstName, #LastName").on("input", function () {
        validateNames();
    });

    function validateNames() {
        var firstName = $("#FirstName").val();
        var lastName = $("#LastName").val();

        if (firstName === lastName) {
            $("#nameErrorMessage").text("First name and last name cannot be the same.");
        } else {
            $("#nameErrorMessage").text("");
        }
    }
});