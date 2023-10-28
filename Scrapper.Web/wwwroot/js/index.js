var controller = 'Home';
var isDefaultSearch = true;

var isForeignStatement = true;
var startDate = moment();
var endDate = moment();

var home = function () {

    const changeSearchStatus = function (searching, btnSearch) {

        if (searching) {
            $(btnSearch).find('span.loader-border').removeClass('d-none');
            $(btnSearch).find("span:contains('Search')").text('Searching');
            $(btnSearch).addClass('disabled');
        } else {
            $(btnSearch).find('span.loader-border').addClass('d-none');
            $(btnSearch).find("span:contains('Searching')").text('Search');
            $(btnSearch).removeClass('disabled');
        }
    },

        validateRangeAndAccountNumber = function () {

            if (isForeignStatement) {
                const fsStartRange = $('#fsStartRange').val();
                const fsEndRange = $('#fsEndRange').val();

                if (fsStartRange.length === 0 || fsEndRange.length === 0) {
                    showMessage('Please enter start and end range in A00 or B00 format.', 'warning');
                    return false;
                }

                startDate = moment(fsStartRange);
                endDate = moment(fsEndRange);

            } else {
                const startRange = $('#startRange').val();
                const endRange = $('#endRange').val();

                if (startRange.length === 0 || endRange.length === 0) {
                    showMessage('Please enter both start and end date.', 'warning');
                    return false;
                }

                startDate = moment(startRange);
                endDate = moment(endRange);
            }

            if (startDate.isAfter(endDate)) {
                showMessage('Start date cannot be greater than end date.', 'warning');
                return false;
            }

            const accountNumber = $('#accountNumber').val();
            if (accountNumber.length === 0) {
                showMessage('Please enter account number.', 'warning');
                return false;
            }

            return true;
        },

        getSearchRequest = function (pageNumber, pageSize) {

           const searchRequest = {
               IsForeignStatement: isForeignStatement,
               DateRange: {
                   Start: startDate.format('YYYY-MM-DD'),
                   End: endDate.format('YYYY-MM-DD')
               },
               AccountNumber: $('#accountNumber').val(),
               Pagination: {
                   PageNumber: pageNumber,
                   PageSize: pageSize
               }
           }

           return searchRequest;
        },

        callSearchApi = function (pageNumber, pageSize) {

            const isValid = validateRangeAndAccountNumber();
            if (!isValid) {
                return;
            }

            changeSearchStatus(true, $('#btnSearch'));

            const successCallback = (response) => {

                $('#royalties').html(response);
                changeSearchStatus(false, $('#btnSearch'));
            }

            const errorCallback = (jqXhr, status, error) => {
                console.error(jqXhr);

                const errorMsg = formatErrorMessage(jqXhr, error);
                const msg = errorMsg ? errorMsg : `Error occurred while searching royalties.`;
                showMessage(msg, 'error');

                changeSearchStatus(false, $('#btnSearch'));
            };

            const searchRequest = getSearchRequest(pageNumber, pageSize);
            ajaxCall('POST', `/${controller}/SearchRoyalties`, successCallback, errorCallback, searchRequest, null, 'html');
        },

        callSearchApiWithDefaultOptions = function (pageNumber, pageSize) {

            changeSearchStatus(true, $('#btnSearch'));

            const successCallback = (response) => {

                $('#royalties').html(response);
                changeSearchStatus(false, $('#btnSearch'));
            }

            const errorCallback = (jqXhr, status, error) => {
                console.error(jqXhr);

                const errorMsg = formatErrorMessage(jqXhr, error);
                const msg = errorMsg ? errorMsg : `Error occurred while searching royalties.`;
                showMessage(msg, 'error');

                changeSearchStatus(false, $('#btnSearch'));
            };

            const searchRequest = {
                IsForeignStatement: isForeignStatement,
                DateRange: {
                    Start: moment([1753, 1, 1]),
                    End: moment()
                },
                AccountNumber: 0,
                Pagination: {
                    PageNumber: pageNumber,
                    PageSize: pageSize
                }
            }

            const url = `/${controller}/SearchRoyalties`;
            ajaxCall('POST', url, successCallback, errorCallback, searchRequest, null, 'html');
        },

        searchRoyalties = function () {

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

        downloadPdf = function () {

            const isValid = validateRangeAndAccountNumber();
            if (!isValid) {
                return false;
            }

            const req = getSearchRequest();

            const url = `/${controller}/DownloadPdf?IsForeignStatement=${req.IsForeignStatement}&DateRange.Start=${req.DateRange.Start}&DateRange.End=${req.DateRange.End}&AccountNumber=${req.AccountNumber}`;
            console.log(`Downloading PDF from URL - ${url}`);

            window.location.href = url;
            return false;
        },

        bindToggleEventHandler = function () {

            var chkToggle = document.querySelector('#chkToggle');

            chkToggle.addEventListener('change', function () {

                isForeignStatement = chkToggle.checked;

                if (isForeignStatement) {
                    $('#foreignStatement').removeClass('d-none');
                    $('#session').addClass('d-none');
                } else {
                    $('#foreignStatement').addClass('d-none');
                    $('#session').removeClass('d-none');
                }
            });
        },

        init = function () {

            changePage();
            bindToggleEventHandler();
        };

    return {
        init: init,
        searchRoyalties: searchRoyalties,
        downloadPdf: downloadPdf
    };
}();