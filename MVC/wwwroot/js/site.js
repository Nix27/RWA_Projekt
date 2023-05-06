$(() => {
    var username = localStorage.getItem('username');

    $('#username').text(username);
});