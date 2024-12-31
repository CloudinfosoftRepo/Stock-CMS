function showLoader() {
    $('#loader').css('display', 'flex');
    $('body').css('pointer-events', 'none');
}
function hideLoader() {
    $('#loader').css('display', 'none');
    $('body').css('pointer-events', 'auto');
}