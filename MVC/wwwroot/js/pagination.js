let pages = $('#hiddenData').data('pages');
let size = $('#hiddenData').data('size');
let url = $('#hiddenData').data('url');

$('.pager-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');
    getData(page, size, url);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.first-btn').on('click', function (event) {
    event.preventDefault();

    getData(0, size, url);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.last-btn').on('click', function (event) {
    event.preventDefault();

    getData(pages, size, url);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.previous-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page >= 0) {
        getData(page, size, url);
        $(this).data('page', page - 1);
        $('.next-btn').data('page', $(this).data('page') + 2);
    }
});

$('.next-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page <= pages) {
        getData(page, size, url);
        $(this).data('page', page + 1);
        $('.previous-btn').data('page', $(this).data('page') - 2);
    }
});