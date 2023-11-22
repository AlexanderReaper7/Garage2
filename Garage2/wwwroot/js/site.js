$(document).ready(function () {
    // Write the url ?parameters to each of their input elements on the page. Effectively retaining the values between page loads.
    (new URL(window.location.href)).searchParams.forEach((x, y) => document.getElementById(y).value = x);
});
