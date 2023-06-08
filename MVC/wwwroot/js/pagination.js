let pages = $('#hiddenData').data('pages');
let size = $('#hiddenData').data('size');
let url = $('#hiddenData').data('url');

$(() => {
    let loadedPage = localStorage.getItem('page');
    let loadedFilterBy = localStorage.getItem($('#filterBy').attr('id'));
    let loadedFilter = localStorage.getItem($('#filter').attr('id'));

    if (loadedPage !== null) {
        getData(loadedPage, size, url, loadedFilterBy, loadedFilter);

        $('#filterBy').val(loadedFilterBy);
        $('#filter').val(loadedFilter);
    } else {
        getData(0, size, url, loadedFilterBy, loadedFilter);
    }
});

$('#filter').on('keyup', function () {
    let page = $('.pager-btn.btn-dark').data('page');
    let filterBy = $('#filterBy').val();
    let filter = $('#filter').val();

    if (filterBy !== 'none') {
        getData(page, size, url, filterBy, filter);

        localStorage.setItem('page', page);
        localStorage.setItem($('#filterBy').attr('id'), filterBy);
        localStorage.setItem($(this).attr('id'), filter);
    }
});

$('#filterBy').on('change', function () {
    if ($('#filterBy').val() === 'none') {
        localStorage.clear();
        $('#filter').val('');

        let page = $('.pager-btn.btn-dark').data('page');
        let filterBy = $('#filterBy').val();
        let filter = $('#filter').val();
        getData(page, size, url, filterBy, filter);
    }
});

$('.pager-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');
    getData(page, size, url, filterBy, filter);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.first-btn').on('click', function (event) {
    event.preventDefault();

    getData(0, size, url, filterBy, filter);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.last-btn').on('click', function (event) {
    event.preventDefault();

    getData(pages, size, url, filterBy, filter);

    $('.next-btn').data('page', $(this).data('page') + 1);
    $('.previous-btn').data('page', $(this).data('page') - 1);
});

$('.previous-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page >= 0) {
        getData(page, size, url, filterBy, filter);
        $(this).data('page', page - 1);
        $('.next-btn').data('page', $(this).data('page') + 2);
    }
});

$('.next-btn').on('click', function (event) {
    event.preventDefault();

    let page = $(this).data('page');

    if (page <= pages) {
        getData(page, size, url, filterBy, filter);
        $(this).data('page', page + 1);
        $('.previous-btn').data('page', $(this).data('page') - 2);
    }
});

function getData(page, size, url, filterBy, filter) {
    let ajaxData = {
        page: page,
        size: size,
        filterBy: filterBy,
        filter: filter
    };

    $.ajax({
        type: 'GET',
        url: url,
        data: ajaxData,
        success: function (data) {
            $('#table-body').html(data);

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