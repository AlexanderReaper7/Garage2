// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//sets the value of the choosen type in the dropdown searchlist in ParkedVehiclesIndex.cshtml
document.getElementById('selectTypeDropDown').addEventListener('change', function () {
    var selectedValue = this.value;
    document.getElementById('selectedType').value = selectedValue;
});

