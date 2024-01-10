function BaseApiService() {
    var _this = this;
    this.baseUrl = "/api";
    this.apiGet = function (resourceUri, skipBaseUrl) {
      //  var _baseUrl = hasValue(skipBaseUrl) && skipBaseUrl == true ? "" : _this.baseUrl;
        var _baseUrl = "";
        var d = $.Deferred();
        $.ajax({
            url: _baseUrl + resourceUri,
            dataType: 'json',
            beforeSend: function (xhr) {
                var token = sessionStorage.getItem("security_token");
                if (token != null && token.length > 0) {
                    xhr.setRequestHeader('security_token', token);
                }
            },
            success: d.resolve,
            error: d.reject
        });
        return d.promise();
    }
    this.apiGetSync = function (resourceUri, successCallback, errorCallback) {
        if ((typeof errorCallback == 'function') == false) {
            errorCallback = function (err) {
                console.log(err);
            };
        }
        $.ajax({
            url: resourceUri,
            dataType: 'json',
            success: successCallback,
            error: errorCallback,
            async: false
        });
    }
    this.apiPost = function (resourceUri, requestObject, skipBaseUrl) {
        //var _baseUrl = hasValue(skipBaseUrl) && skipBaseUrl == true ? "" : _this.baseUrl;
        var _baseUrl = "";
        var d = $.Deferred();
        $.ajax({
            url: _baseUrl + resourceUri,
            dataType: "json",
            beforeSend: function (xhr) {
                var token = sessionStorage.getItem("security_token");
                if (token != null && token.length > 0) {
                    xhr.setRequestHeader('security_token', token);
                }
            },
            type: 'POST',
            data: requestObject,
            success: d.resolve,
            error: d.reject
        });
        return d.promise();
    }
    this.apiPostSync = function (resourceUri, requestObject, successCallback, errorCallback) {
        if ((typeof errorCallback == 'function') == false) {
            errorCallback = function (err) {
                console.log(err);
            };
        }
        $.ajax({
            url: resourceUri,
            dataType: "json",
            type: 'POST',
            data: requestObject,
            success: successCallback,
            error: errorCallback,
            async: false
        });
    }
    this.apiPut = function (resourceUri, requestObject, skipBaseUrl) {
        var _baseUrl = hasValue(skipBaseUrl) && skipBaseUrl == true ? "" : _this.baseUrl;
        var d = $.Deferred();
        $.ajax({
            url: _baseUrl + resourceUri,
            dataType: "json",
            type: 'PUT',
            data: requestObject,
            success: d.resolve,
            error: d.reject
        });
        return d.promise();
    }
    this.apiDelete = function (resourceUri, skipBaseUrl) {
        var _baseUrl = hasValue(skipBaseUrl) && skipBaseUrl == true ? "" : _this.baseUrl;
        var d = $.Deferred();
        $.ajax({
            url: _baseUrl + resourceUri,
            dataType: "json",
            type: 'DELETE',
            success: d.resolve,
            error: d.reject
        });
        return d.promise();
    }
    this.html = function (pageUrl, cacheRequired) {
        cacheRequired = hasValue(cacheRequired) ? cacheRequired : false;
        var d = $.Deferred();
        $.ajax({
            url: pageUrl,
            dataType: 'html',
            cache: cacheRequired, //true or fales
            success: d.resolve,
            error: d.reject
        });
        return d.promise();
    }
}

window.baseApiService = new BaseApiService();