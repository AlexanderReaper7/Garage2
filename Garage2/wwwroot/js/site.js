// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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