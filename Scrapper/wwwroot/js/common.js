'use strict';

/**
* Make AJAX request.
*/
var ajaxCall = function (method, url, successCallback, errorCallback, model = null, contentType = null, dataType = null) {
    try {

        // Ref - http://api.jquery.com/jquery.ajax/
        // contentType is the type of data you're sending. 'application/json; charset=utf-8' is default.
        // dataType is the type of data you're expecting back from the server: json, html, text, etc. 'json' is default.

        $.ajax({
            url: url,
            method: method,
            data: method === 'POST' ? JSON.stringify(model) : model,
            contentType: contentType ? contentType : 'application/json',

            dataType: dataType ? dataType : 'json',
            success: successCallback,
            error: errorCallback
        });
    } catch (err) {
        showMessage(`${err.name} : ${err.message}`, null, 'error');
    }
};

/**
* Format error message based on status code and error thrown.
*/
var formatErrorMessage = function (jqXhr, error) {
    if (jqXhr.status === 0) {
        return ('Not connected.\nPlease verify your network connection.');
    } else if (jqXhr.status === 404) {
        return ('The requested page not found.');
    } else if (jqXhr.status === 401) {
        return ('Sorry!! Your session has expired. Please login to continue access.');
    } else if (jqXhr.status === 403) {
        return ('Access Denied – You don’t have permission to access.');
    } else if (jqXhr.status === 500) {
        return ('Internal Server Error.');
    } else if (error === 'parsererror') {
        return ('Requested JSON parse failed.');
    } else if (error === 'timeout') {
        return ('Time out error.');
    } else if (error === 'abort') {
        return ('Ajax request aborted.');
    } else {
        if (error) {
            return error;
        } else {
            return null;
        }
    }
};

/**
* Show toaster message as per type in middle center.
* @param {any} message
* @param {'Allow only success, error, warning and info'} type
* @param {any} headline
*/
var showMessage = function (message, type, headline = null) {

    const options = getToasterOptions();

    switch (type) {
        case 'success':
            toastr.success(
                message,
                headline,
                options
            );
            break;

        case 'error':
            toastr.error(
                message,
                headline,
                options
            );
            break;

        case 'warning':
            toastr.warning(
                message,
                headline,
                options
            );
            break;

        case 'info':
            toastr.info(
                message,
                headline,
                options
            );
            break;
    }
};

function getToasterOptions() {

    const options = {
        closeButton: false,
        debug: false,

        //positionClass: 'toast-middle-center',
        onClick: null,

        showDuration: 300,
        hideDuration: 1000,
        timeOut: 5000,
        extendedTimeOut: 1000,

        showEasing: 'swing',
        hideEasing: 'linear',
        showMethod: 'fadeIn',
        hideMethod: 'fadeOut',
        bodyOutputType: 'trustedHtml',

        progressBar: true,
        preventDuplicates: true,
        newestOnTop: true
    };

    return options;
}

/**
* Show loading icon in specified element Id.
* @param {any} targetEleId - The target element Id or class that will be used for displaying loader.
*/
var showLoader = function ($this) {

    $($this).html(`                
                <div class="loader">
                    <i class="fa fa-circle-o-notch fa-spin text-secondary"></i>
                </div>
            `);
};

/**
* Change button text and disable state.
*/
var changeButtonState = function (eleId, buttonTxt, disable) {

    $(eleId).attr('disabled', disable);
    setBtnTxtWithLoader(eleId, buttonTxt, disable);
};

/**
* Set the button element text with loader.
*/
var setBtnTxtWithLoader = function (eleId, buttonTxt, showLoader) {

    if (showLoader) {
        const iconEle = `<i class="fas fa-circle-notch fa-spin"></i>`;
        const spanEle = `<span class="pl-1">${buttonTxt}</span>`;
        $(eleId).html(`${iconEle} ${spanEle}`);
    } else {
        $(eleId).html(`${buttonTxt}`);
    }
};
