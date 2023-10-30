var controller = 'Home';
var isDefaultSearch = true;

var startDate = moment();
var endDate = moment();

const defaultPageSize = 10;
const asc = 'ASC';
const desc = 'DESC';

var home = function () {

    const changeSearchStatus = function (searching, btnSearch) {

        if (searching) {
            $(btnSearch).find('span.loader-border').removeClass('d-none');
            $(btnSearch).find('i.fa-search').addClass('d-none');
            $(btnSearch).find("span:contains('Search')").text('Searching');
            $(btnSearch).addClass('disabled');
        } else {
            $(btnSearch).find('span.loader-border').addClass('d-none');
            $(btnSearch).find('i.fa-search').removeClass('d-none');
            $(btnSearch).find("span:contains('Searching')").text('Search');
            $(btnSearch).removeClass('disabled');
        }
    },

        validateDateRangeAndProductId = function () {

            const startRange = $('#startRange').val();
            const endRange = $('#endRange').val();

            if (startRange.length === 0 || endRange.length === 0) {
                showMessage('Please enter both start and end date.', 'warning');
                return false;
            }

            startDate = moment(startRange);
            endDate = moment(endRange);

            if (startDate.isAfter(endDate)) {
                showMessage('Start date cannot be greater than end date.', 'warning');
                return false;
            }

            //const searchTxt = $('#searchTxt').val();
            //if (searchTxt.length === 0) {
            //    showMessage('Please enter product id or product name to search.', 'warning');
            //    return false;
            //}

            return true;
        },

        getSearchRequest = function (pageNumber, pageSize) {

            const searchRequest = {
                DateRange: {
                    Start: startDate.format('YYYY-MM-DD'),
                    End: endDate.format('YYYY-MM-DD')
                },
                SearchText: $('#searchTxt').val(),
                Page: {
                    Number: pageNumber,
                    Size: pageSize
                },
                Sort: {
                    Column: $('#sortColumn').val(),
                    Direction: $('#sortDirection').val()
                }
            }

            return searchRequest;
        },

        callSearchApi = function (pageNumber, pageSize) {

            const isValid = validateDateRangeAndProductId();
            if (!isValid) {
                return;
            }

            changeSearchStatus(true, $('#btnSearch'));

            const successCallback = (response) => {

                $('#scrapes').html(response);
                applySortClass();
                changeSearchStatus(false, $('#btnSearch'));
            }

            const errorCallback = (jqXhr, status, error) => {
                console.error(jqXhr);

                const errorMsg = formatErrorMessage(jqXhr, error);
                const msg = errorMsg ? errorMsg : `Error occurred while searching scrapes.`;
                showMessage(msg, 'error');

                changeSearchStatus(false, $('#btnSearch'));
            };

            const searchRequest = getSearchRequest(pageNumber, pageSize);
            ajaxCall('POST', `/${controller}/SearchScrapes`, successCallback, errorCallback, searchRequest, null, 'html');
        },

        callSearchApiWithDefaultOptions = function (pageNumber, pageSize) {

            changeSearchStatus(true, $('#btnSearch'));

            const successCallback = (response) => {

                $('#scrapes').html(response);
                applySortClass();
                changeSearchStatus(false, $('#btnSearch'));
            }

            const errorCallback = (jqXhr, status, error) => {
                console.error(jqXhr);

                const errorMsg = formatErrorMessage(jqXhr, error);
                const msg = errorMsg ? errorMsg : `Error occurred while searching scrapes.`;
                showMessage(msg, 'error');

                changeSearchStatus(false, $('#btnSearch'));
            };

            const searchRequest = {
                DateRange: {
                    Start: moment([1753, 1, 1]),
                    End: moment()
                },
                SearchText: $('#searchTxt').val(),
                Page: {
                    Number: pageNumber,
                    Size: pageSize
                },
                Sort: {
                    Column: $('#sortColumn').val(),
                    Direction: $('#sortDirection').val()
                }
            }

            const url = `/${controller}/SearchScrapes`;
            ajaxCall('POST', url, successCallback, errorCallback, searchRequest, null, 'html');
        },

        searchScrapes = function () {

            isDefaultSearch = false;
            callSearchApi(1, defaultPageSize);
        },

        applySortClass = function () {

            // Reset
            $('table th i.fa-caret-up').remove();
            $('table th i.fa-caret-down').remove();

            const currentSort = $('#sortColumn').val();
            const heading = $(`#h-${currentSort}`);

            const sortDirection = $('#sortDirection').val();
            heading.append(sortDirection === asc
                ? '<i class="fas fa-sort-amount-down pl-1"></i>'
                : '<i class="fas fa-sort-amount-up pl-1"></i>');
        },

        sortScrapes = function (sortCol) {

            if ($('#sortColumn').val() === sortCol) {

                if ($('#sortDirection').val() === asc) {
                    $('#sortDirection').val(desc);
                }
                else {
                    $('#sortDirection').val(asc);
                }
            }
            else {
                $('#sortColumn').val(sortCol);
                $('#sortDirection').val(asc);
            }

            searchScrapes();
        },

        changePage = function () {
            $('body').on('click',
                '.pagination li',
                function () {
                    const $this = $(this);

                    var pageNumber = 1;

                    // ReSharper disable once UnknownCssClass
                    if ($this.hasClass('PagedList-skipToPrevious')) {
                        const pageNumberText = $this.closest('.pagination').find('li.active a').text();
                        pageNumber = parseInt(pageNumberText) - 1;

                        // ReSharper disable once UnknownCssClass
                    } else if ($this.hasClass('PagedList-skipToNext')) {
                        const pageNumberText = $this.closest('.pagination').find('li.active a').text();
                        pageNumber = parseInt(pageNumberText) + 1;
                    }
                    else if ($this.hasClass('page-item')) {
                        let pageNumberText = $this.find('a').text();
                        if (pageNumberText === 'First') {
                            pageNumberText = '1';
                        } else if (pageNumberText === 'Last') {
                            pageNumberText = $this.closest('ul').data('last');
                        }
                        pageNumber = parseInt(pageNumberText);
                    }

                    if (isDefaultSearch)
                        callSearchApiWithDefaultOptions(pageNumber, defaultPageSize);
                    else
                        callSearchApi(pageNumber, defaultPageSize);
                });
        },

        showProductLogs = function (productId) {

            const url = `/Product?productId=${productId}`;
            redirectToUrl(url);
        },

        init = function () {

            changePage();
            applySortClass();
        };

    return {
        init: init,
        searchScrapes: searchScrapes,
        showProductLogs: showProductLogs,
        sortScrapes: sortScrapes
    };
}();