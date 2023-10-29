var controller = 'Home';
var isDefaultSearch = true;

var startDate = moment();
var endDate = moment();

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
                Pagination: {
                    PageNumber: pageNumber,
                    PageSize: pageSize
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
                Pagination: {
                    PageNumber: pageNumber,
                    PageSize: pageSize
                }
            }

            const url = `/${controller}/SearchScrapes`;
            ajaxCall('POST', url, successCallback, errorCallback, searchRequest, null, 'html');
        },

        searchScrapes = function () {

            isDefaultSearch = false;
            callSearchApi(1, 10);
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
                        callSearchApiWithDefaultOptions(pageNumber, 10);
                    else
                        callSearchApi(pageNumber, 10);
                });
        },

        showProductLogs = function (productId) {

            const url = `/Product?productId=${productId}`;
            redirectToUrl(url);
        },

    init = function () {

            changePage();
        };

    return {
        init: init,
        searchScrapes: searchScrapes,
        showProductLogs: showProductLogs
    };
}();