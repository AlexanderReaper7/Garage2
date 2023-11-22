// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//sets the value of the chosen newType to add in the dropdownlist of AddNewVhehicleType.cshtml
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('selectedNewTypeDropdown').addEventListener('change', function () {
        var selectedTypeValue = this.value;
        document.getElementById('addNewType').value = selectedTypeValue;
    });
});

//sets the value of the chosen type in the dropdown searchlist in ParkedVehiclesIndex.cshtml
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('selectTypeDropDown').addEventListener('change', function () {
        var selectedValue = this.value;
        document.getElementById('selectedType').value = selectedValue;
    });
});

