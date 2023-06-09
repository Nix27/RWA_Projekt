let filterByKey = $('#hiddenData').data('keyfilterby');
let filterKey = $('#hiddenData').data('keyfilter');
let pageKey = $('#hiddenData').data('keypage');

let loadedPage = localStorage.getItem(pageKey);
let loadedFilterBy = localStorage.getItem(filterByKey);
let loadedFilter = localStorage.getItem(filterKey);

$(() => {
    if (loadedPage !== null) {
        getFilteredData(loadedPage, size, url, loadedFilterBy, loadedFilter);

        $('#filterBy').val(loadedFilterBy);
        $('#filter').val(loadedFilter);
    } else {
        getFilteredData(0, size, url, loadedFilterBy, loadedFilter);
    }
});

$('#filter').on('keyup', function () {
    let page = $('.pager-btn.btn-dark').data('page');
    let filterBy = $('#filterBy').val();
    let filter = $('#filter').val();

    if (filterBy !== 'none') {
        getFilteredData(page, size, url, filterBy, filter);

        localStorage.setItem(pageKey, page);
        localStorage.setItem(filterByKey, filterBy);
        localStorage.setItem(filterKey, filter);
    }
});

$('#filterBy').on('change', function () {
    if ($('#filterBy').val() === 'none') {
        localStorage.removeItem(pageKey);
        localStorage.removeItem(filterByKey);
        localStorage.removeItem(filterKey);
        $('#filter').val('');

        let page = $('.pager-btn.btn-dark').data('page');
        let filterBy = $('#filterBy').val();
        let filter = $('#filter').val();
        getFilteredData(page, size, url, filterBy, filter);
    }
});

function getFilteredData(page, size, url, filterBy, filter) {
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
            $('#content').html(data);

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