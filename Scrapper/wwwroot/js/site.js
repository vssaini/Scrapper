// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict';

var controller = 'Home';

var site = function () {

    const init = function () {


    },
        scrapeWesco = function () {

            const btnDefaultTxt = 'Scrape from Wesco';
            changeButtonState('#btnScrape', 'Scraping...', true);

            const successCallback = (response) => {

                changeButtonState('#btnScrape', btnDefaultTxt, false);

                switch (response.type) {
                    case 'Ok':
                        if (response.data) {
                            showMessage(response.successMessage, 'success');
                        } else {
                            showMessage(response.errorMessage, 'error');
                        }
                        break;

                    case 'Error':
                        showMessage(response.errorMessage, 'error');
                        break;
                }
            }

            const errorCallback = (jqXhr, status, error) => {
                console.error(jqXhr);

                const errorMsg = formatErrorMessage(jqXhr, error);
                const msg = errorMsg ? errorMsg : `Error occurred while scraping Wesco.`;
                showMessage(msg, 'error');

                changeButtonState('#btnScrape', btnDefaultTxt, false);
            };

            ajaxCall('POST', `/${controller}/TakeScreenshot`, successCallback, errorCallback, null, null);
        };

    return {
        init: init,
        scrapeWesco: scrapeWesco
    }
}();