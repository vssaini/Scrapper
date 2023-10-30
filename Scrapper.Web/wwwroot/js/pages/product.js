var controller = 'Product';

const defaultPageSize = 10;

var product = function () {

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
        getProductRequest = function (pageNumber, pageSize) {

            const productRequest = {
                ProductId: $('#productId').val(),
                Page: {
                    Number: pageNumber,
                    Size: pageSize
                }
            }

            return productRequest;
        },

        fetchProductLogs = function (pageNumber, pageSize) {

            changeSearchStatus(true, $('#btnSearch'));

            const successCallback = (response) => {

                $('#productLogs').html(response);
                changeSearchStatus(false, $('#btnSearch'));
            }

            const errorCallback = (jqXhr, status, error) => {
                console.error(jqXhr);

                const errorMsg = formatErrorMessage(jqXhr, error);
                const msg = errorMsg ? errorMsg : `Error occurred while retrieving product logs.`;
                showMessage(msg, 'error');

                changeSearchStatus(false, $('#btnSearch'));
            };

            const prodRequest = getProductRequest(pageNumber, pageSize);
            ajaxCall('POST', `/${controller}/Logs`, successCallback, errorCallback, prodRequest, null, 'html');
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

                    fetchProductLogs(pageNumber, defaultPageSize);
                });
        },

        init = function () {

            changePage();
        };

    return {
        init: init
    };
}();