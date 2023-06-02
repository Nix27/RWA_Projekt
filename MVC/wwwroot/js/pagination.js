let pages = $('#hiddenData').data('pages');
let size = $('#hiddenData').data('size');

$('.pager-btn').on('click', function (event) {
    event.preventDefault();

    var page = $(this).data('page');
    getData(page, size);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.first-btn').on('click', function (event) {
    event.preventDefault();

    getData(0, size);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.last-btn').on('click', function (event) {
    event.preventDefault();

    getData(pages, size);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.previous-btn').on('click', function (event) {
    event.preventDefault();

    var page = $(this).data('page');
    console.log(page);

    if (page >= 0) {
        getData(page, size);
        $(this).data('page', page - 1);
        $('.next-btn').data('page', $(this).data('page') + 2);
        console.log($(this).data('page'));
    }
});

$('.next-btn').on('click', function (event) {
    event.preventDefault();

    var page = $(this).data('page');
    console.log(page);

    if (page <= pages) {
        getData(page, size);
        $(this).data('page', page + 1);
        $('.previous-btn').data('page', $(this).data('page') - 2);
        console.log($(this).data('page'));
    }
});

function getData(page, size) {
    var ajaxData = {
        page: page,
        size: size
    };

    $.ajax({
        type: 'GET',
        url: 'CountryTableBodyPartial',
        data: ajaxData,
        success: function (data) {
            $('#country-table-body').html(data);

            $('.pager-btn').removeClass('btn-dark');
            $('.pager-btn').addClass('btn-light');

            $('.pager-btn[data-page=' + page + ']').removeClass('btn-light');
            $('.pager-btn[data-page=' + page + ']').addClass('btn-dark');
        },
        error: function (data) {
            console.log('error', data);
        }
    });
}