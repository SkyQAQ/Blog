import Vue from 'vue'
import qs from 'qs';
import RsaHelper from 'jsencrypt';
import config from '../../../vue.config';

! function(factory) {
    if (typeof require === 'function' && typeof exports === 'object' && typeof module === 'object') {
        var target = module['exports'] || exports;
        factory(target);
    } else if (typeof define === 'function' && define['amd']) {
        define(['exports'], factory);
    } else {
        factory(window['wy'] = {});
    }
}(function(utExports) {
    'use strict';
    var wy = typeof utExports !== 'undefined' ? utExports : {};
    /**
     * Get方法
     * @param {string} url 请求地址
     * @param {data} data 请求参数
     * @param {boolean} isFile response是否是文件流
     */
    wy.get = function(url, params, isFile) {
        if (!wy.isNull(params)) {
            var queryString = '';
            for (var key in params) {
                queryString += '&' + key + '=' + encodeURIComponent(params[key]);
            }
            url += "?" + queryString.substr(1);
        }
        var config = {
            responseType: 'json',
        }
        if (isFile) {
            config.responseType = 'blob';
        }
        return Vue.http.get(url, config).then(function(response) {
            if (!wy.isNull(response.data) && !wy.isNull(response.data.code)) {
                if (response.data.code === 1) {
                    return response.data.data;
                } else {
                    return Promise.reject(response.data.mssg);
                }
            }
            return response.data;
        }, function(error) {
            if (error.status === 401) {
                Vue.auth.clearToken();
            }
            return Promise.reject(error.status + ':' + error.data.mssg);
        });
    };
    /**
     * Post方法
     * @param {string} url 请求地址
     * @param {data} params 请求参数
     * @param {boolean} isFile response是否是文件流
     */
    wy.post = function(url, params, isFile) {
        var data;
        if (typeof params === 'object' && !params.toString().contains('FormData')) {
            data = qs.stringify(params);
        } else {
            data = params;
        }
        var config = {
            responseType: 'json',
        }
        if (isFile) {
            config.responseType = 'blob';
        }
        return Vue.http.post(url, data, config).then(function(response) {
            if (!wy.isNull(response.data) && !wy.isNull(response.data.code)) {
                if (response.data.code === 1) {
                    return response.data.data;
                } else {
                    return Promise.reject(response.data.mssg);
                }
            }
            return response.data;
        }, function(error) {
            if (error.status === 401) {
                Vue.prototype.$auth.clearToken();
            }
            return Promise.reject(error.status + ':' + error.data.mssg);
        });
    };
    wy.getSync = function(url, data) {
        //先声明一个请求对象
        var xmlHttpReg = null;
        if (window.ActiveXObject) { //如果是IE
            xmlHttpReg = new ActiveXObject("Microsoft.XMLHTTP");

        } else if (window.XMLHttpRequest) {
            xmlHttpReg = new XMLHttpRequest(); //实例化一个xmlHttpReg
        }
        //如果实例化成功,就调用open()方法,就开始准备向服务器发送请求
        if (xmlHttpReg != null) {
            xmlHttpReg.open("get", url, false);
            if (data != null) {
                xmlHttpReg.send(data);
            } else {
                xmlHttpReg.send();
            }
            //注册相关事件回调处理函数
            if (xmlHttpReg.status == 200 || xmlHttpReg.status == 304) {
                var res = JSON.parse(xmlHttpReg.responseText);
                if (res.code === 1) {
                    return res.data;
                } else {
                    wy.showErrorMssg(res.mssg);
                    return;
                }
            } else {
                if (xmlHttpReg.status == 401) {
                    Vue.auth.clearToken();
                }
                return null;
            }
        }
    };
    wy.postSync = function(url, data) {
        //先声明一个请求对象
        var xmlHttpReg = null;
        if (window.ActiveXObject) { //如果是IE
            xmlHttpReg = new ActiveXObject("Microsoft.XMLHTTP");
        } else if (window.XMLHttpRequest) {
            xmlHttpReg = new XMLHttpRequest(); //实例化一个xmlHttpReg
        }
        //如果实例化成功,就调用open()方法,就开始准备向服务器发送请求
        if (xmlHttpReg != null) {
            xmlHttpReg.open("post", url, true);
            xmlHttpReg.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;"); //缺少这句，后台无法获取参数
            if (data != null) {
                xmlHttpReg.send(data);
            } else {
                xmlHttpReg.send();
            }
            xmlHttpReg.onload = function(e) {};
            xmlHttpReg.ontimeout = function(e) {};
            xmlHttpReg.onerror = function(e) {};
            xmlHttpReg.upload.onprogress = function(e) {};
            if (xmlHttpReg.status == 200 || xmlHttpReg.status == 304) {
                var res = JSON.parse(xmlHttpReg.responseText);
                if (res.code === 1) {
                    return res.data;
                } else {
                    wy.showErrorMssg(res.mssg);
                    return;
                }
            } else {
                if (xmlHttpReg.status == 401 || xmlHttpReg.statusText == 'Unauthorized') {
                    Vue.auth.clearToken();
                }
                return null;
            }
        }
    };
    wy.rsa = function(val, key) {
        if (!val) {
            return '';
        }
        var rsa = new RsaHelper(null);
        rsa.setPublicKey(key);
        return encodeURIComponent(rsa.encrypt(val));
    };
    /**
     * base64String转Blob导出Excel
     */
    wy.downloadExlByBlob = function(buffStr, fileName) {
        fileName = fileName + '.xlsx';
        var raw = window.atob(buffStr);
        var rawLength = raw.length;
        var uInt8Array = new Uint8Array(rawLength); //转换编码为UTF-8
        for (var i = 0; i < rawLength; ++i) {
            uInt8Array[i] = raw.charCodeAt(i);
        }
        var blob = new Blob([uInt8Array], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        if (window.navigator.msSaveOrOpenBlob) {
            navigator.msSaveBlob(blob, fileName);
        } else {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
            window.URL.revokeObjectURL(link.href);
        }
    };
    /**
     * 文件流转Blob下载文件
     * @param  {data} fileStream 文件流
     * @param  {string} fileName 文件名
     */
    wy.downLoad = function(fileStream, fileName) {
        var blob = new Blob([fileStream]);
        if (window.navigator.msSaveOrOpenBlob) {
            navigator.msSaveBlob(blob, fileName);
        } else {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
            window.URL.revokeObjectURL(link.href);
        }
    };
    wy.confirmClick = function(func) {
        Vue.prototype.$confirm('是否确认此操作？', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
        }).then(func).catch(() => {
            Vue.prototype.$message({
                type: 'info',
                message: '操作已取消！'
            });
        });
    };
    wy.getBaseUrl = function() {
        if (process.env.NODE_ENV === 'production') {
            return '';
        }
        var baseUrl = config.devServer.proxy['/api'].target || '';
        if (!baseUrl.endWith('/')) {
            baseUrl += '/';
        }
        return baseUrl;
    }
    wy.GetLongTimeString = function(date) {
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var hour = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        if (hour < 10) {
            hour = '0' + hour;
        }
        if (minutes < 10) {
            minutes = '0' + minutes;
        }
        if (seconds < 10) {
            seconds = '0' + seconds;
        }
        return year + '-' + month + '-' + day + ' ' + hour + ':' + minutes + ':' + seconds;
    };
    wy.ValidPassword = function(val) {
        if (val.length < 8) {
            return '密码长度不能少于8位字符';
        }
        var reg_char = /[A-Za-z]/;
        if (!reg_char.test(val)) {
            return '必须包含英文字符';
        }
        var reg_sympol = /[!@#%&*+.?,]/;
        if (!reg_sympol.test(val)) {
            return '必须包特殊字符';
        }
        var reg_sympol_invalid = /[~^()_<>;$:'"/{[}]}]/;
    };
    wy.isBoolean = function(obj) {
        return typeof obj === "boolean";
    };
    wy.isDate = function(d) {
        if (wy.isNull(d)) {
            return false;
        }
        return d instanceof Date && !isNaN(d.valueOf());
    };
    wy.isEmailAddress = function(str) {
        var regex = /^\w+((-\w+)|(\.\w+))*\[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
        return regex.test(str);
    };
    wy.isFunction = function(f) {
        if (wy.isNull(f)) {
            return false;
        }
        return typeof f === 'function';
    };
    wy.isIDCard = function(str) {
        var regex = /^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/;
        return regex.test(str);
    };
    wy.isInteger = function(str) {
        var regex = /^[-]{0,1}[0-9]{1,}$/;
        return regex.test(str);
    };
    wy.isNull = function(o) {
        return o === undefined || o === null;
    };
    wy.isNullOrWhiteSpace = function(s) {
        return this.isNull(s) || s.trim() === "";
    };
    wy.isNumeric = function(str) {
        var regex = /^(-|\+)?\d+(\.\d+)?$/;
        return regex.test(str);
    };
    wy.isSameGuid = function(guid1, guid2) {
        var isEqual;
        if (guid1 === null || guid2 === null) {
            isEqual = false;
        } else {
            isEqual = guid1
                .replace(/[{}]/g, "")
                .toLowerCase() === guid2
                .replace(/[{}]/g, "")
                .toLowerCase();
        }
        return isEqual;
    };
    wy.isString = function(obj) {
        return typeof obj === "string";
    };
    wy.showSuccessMssg = function(content, isClose) {
        Vue.prototype.$message({
            showClose: isClose,
            message: content,
            type: 'success'
        });
    };
    wy.showWarnMssg = function(content, isClose) {
        Vue.prototype.$message({
            showClose: isClose,
            message: content,
            type: 'warning'
        });
    };
    wy.showErrorMssg = function(content, isClose) {
        Vue.prototype.$message({
            showClose: isClose,
            message: content,
            type: 'error'
        });
    };
    /**
     * 日期格式化
     * @param  {date} date   待格式化日期
     * @param  {string} mask 格式
     * @param  {[type]} utc  [description]
     * @return {string}      格式化后的日期字符串
     */
    wy.formatDateTime = function(date, mask, utc) {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function(val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len)
                    val = "0" + val;
                return val;
            };

        var dateFormat = {
            masks: {},
        };

        // Some common format strings
        dateFormat.masks = {
            "default": "ddd mmm dd yyyy HH:MM:ss",
            shortDate: "m/d/yy",
            mediumDate: "mmm d, yyyy",
            longDate: "mmmm d, yyyy",
            fullDate: "dddd, mmmm d, yyyy",
            shortTime: "h:MM TT",
            mediumTime: "h:MM:ss TT",
            longTime: "h:MM:ss TT Z",
            isoDate: "yyyy-mm-dd",
            isoTime: "HH:MM:ss",
            isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
            isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
        };

        // Regexes and supporting functions are cached through closure
        return function(date, mask, utc) {
            var dF = dateFormat;

            // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
            if (arguments.length === 1 && Object.prototype.toString.call(date) === "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }

            // Passing date through Date applies Date.parse, if necessary
            date = date ?
                new Date(date) :
                new Date();
            if (isNaN(date))
                throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);

            // Allow setting the utc argument via the mask
            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }
            var _ = utc ?
                "getUTC" :
                "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ?
                0 :
                date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    m: m + 1,
                    mm: pad(m + 1),
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ?
                        Math.round(L / 10) :
                        L),
                    t: H < 12 ?
                        "a" : "p",
                    tt: H < 12 ?
                        "am" : "pm",
                    T: H < 12 ?
                        "A" : "P",
                    TT: H < 12 ?
                        "AM" : "PM",
                    Z: utc ?
                        "UTC" :
                        (String(date).match(timezone) || [""])
                        .pop()
                        .replace(timezoneClip, ""),
                    o: (o > 0 ?
                        "-" :
                        "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ?
                        0 :
                        (d % 100 - d % 10 != 10) * d % 10
                    ]
                };
            return mask.replace(token, function($0) {
                return $0 in flags ?
                    flags[$0] :
                    $0.slice(1, $0.length - 1);
            });
        };
    }();
    /**
     * 判断一个字符串是否包含某字符串
     * @param substr 包含的字符串
     * @param ignoreCase 是否忽略大小写
     * @returns {boolean} 如果包含，则返回true，否则返回 false
     */
    String.prototype.contains = function(substr, ignoreCase) {
        if (ignoreCase === null || ignoreCase === undefined) {
            ignoreCase = false;
        }
        if (ignoreCase) {
            return this.search(new RegExp(substr, "i")) > -1;
        } else {
            return this.search(substr) > -1;
        }
    };

    /**
     * 判断一个字符串是不是以某字符串结尾
     * @param s
     * @returns {boolean} 如果是，则返回true，否则返回 false
     */
    String.prototype.endWith = function(s, ignoreCase) {
        if (s === null || s === "" || this.length === 0 || s.length > this.length) {
            return false;
        }
        var ns = this.substring(this.length - s.length);
        if (ignoreCase) {
            return ns.toLowerCase() === s.toLowerCase();
        } else {
            return ns === s;
        }
    };

    /**
     * 字符格式化，同C# String.Format方法
     */
    String.prototype.format = function() {
        var content = this;
        for (var i = 0; i < arguments.length; i++) {
            var replacement = '{' + i + '}';
            content = content.replace(replacement, arguments[i]);
        }
        return content;
    };

    /**
     * 判断一个字符串是不是以某字符串开头
     * @param s
     * @returns {boolean} 如果是，则返回true，否则返回 false
     */
    String.prototype.startWith = function(s, ignoreCase) {
        if (s === null || s === "" || this.length === 0 || s.length > this.length) {
            return false;
        }
        var ns = this.substr(0, s.length);
        if (ignoreCase) {
            return ns.toLowerCase() === s.toLowerCase();
        } else {
            return ns === s;
        }
    };

    /**
     * 移除字符串前后的空格或其它特殊字符，同C#中的Trim方法。
     */
    String.prototype.trim = function(trimChars) {
        var result = this;
        if (typeof trimChars !== "string" || trimChars.length <= 0) {
            trimChars = " ";
        }
        var count = result.length;
        while (count > 0) { //trim the head position
            if (trimChars.indexOf(result[0]) >= 0) {
                result = result.substring(1);
                count--;
            } else {
                break;
            }
        }
        while (count > 0) { //trim the tail position
            if (trimChars.indexOf(result[count - 1]) >= 0) {
                result = result.substring(0, count - 1);
                count--;
            } else {
                break;
            }
        }
        return result;
    };
});