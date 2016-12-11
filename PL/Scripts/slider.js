$(document).ready(function () {
    var minPrice = 1;
    var maxPrice = 100000;
    var min = sessionStorage.getItem("minimum");
    var max = sessionStorage.getItem("maximum");
    if (max === null) {
        max = maxPrice;
    }

    var outputSpan = $('#spanOutput');
    var sliderDiv = $('#slider');
    sliderDiv.slider({
        range: true,
        min: minPrice,
        max: maxPrice,
        values: [min, max],
        slide: function (event, ui) { outputSpan.html(ui.values[0] + ' - ' + ui.values[1] + ' €'); },
        stop: function (event, ui) {
            $('#txtMin').val(ui.values[0]);
            $('#txtMax').val(ui.values[1]);
        }
    });
    outputSpan.html(sliderDiv.slider('values', 0) + ' - ' + sliderDiv.slider('values', 1) + ' €');
    $('#txtMin').val(sliderDiv.slider('values', 0));
    $('#txtMax').val(sliderDiv.slider('values', 1));
});

function saveRange() {
    var minimum = $('#slider').slider("values", 0);
    sessionStorage.setItem("minimum", minimum);
    var maximum = $('#slider').slider("values", 1);
    sessionStorage.setItem("maximum", maximum);
}

function resetRange() {
    sessionStorage.setItem("minimum", 1);
    sessionStorage.setItem("maximum", 100000);
}