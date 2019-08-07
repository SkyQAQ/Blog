var wy = function () {
    function Get(url, data) {
        //先声明一个异步请求对象
        var xmlHttpReg = null;
        var response = '';
        if (window.ActiveXObject) {//如果是IE
            xmlHttpReg = new ActiveXObject("Microsoft.XMLHTTP");

        } else if (window.XMLHttpRequest) {

            xmlHttpReg = new XMLHttpRequest(); //实例化一个xmlHttpReg
        }
        //如果实例化成功,就调用open()方法,就开始准备向服务器发送请求
        if (xmlHttpReg != null) {
            xmlHttpReg.open("get", url, false);
            xmlHttpReg.send(data);
            response = xmlHttpReg.responseText;
        }
        return response;
    }
    function syncGet(url, data) {
        //先声明一个异步请求对象
        var xmlHttpReg = null;
        var response = '';
        if (window.ActiveXObject) {//如果是IE
            xmlHttpReg = new ActiveXObject("Microsoft.XMLHTTP");

        } else if (window.XMLHttpRequest) {

            xmlHttpReg = new XMLHttpRequest(); //实例化一个xmlHttpReg
        }
        //如果实例化成功,就调用open()方法,就开始准备向服务器发送请求
        if (xmlHttpReg != null) {
            xmlHttpReg.open("get", url, true);
            if (data != null) {
                xmlHttpReg.send(data);
            }
            xmlHttpReg.onreadystatechange = function () {
                if (xmlHttpReg.readyState == 4 && xmlHttpReg.status == 200) {//4代表执行完成 200代表执行成功
                    response = xmlHttpReg.responseText;
                }
            }; //设置回调函数
        }
        return response;
    }
}
function syncPost(url, data) {
    //先声明一个异步请求对象
    var xmlHttpReg = null;
    var response;
    if (window.ActiveXObject) {//如果是IE
        xmlHttpReg = new ActiveXObject("Microsoft.XMLHTTP");

    } else if (window.XMLHttpRequest) {

        xmlHttpReg = new XMLHttpRequest(); //实例化一个xmlHttpReg
    }
    //如果实例化成功,就调用open()方法,就开始准备向服务器发送请求
    if (xmlHttpReg != null) {
        xmlHttpReg.open("post", url, true);
        xmlHttpReg.setRequestHeader("Content-Type", "application/x-www-form-urlencoded;");//缺少这句，后台无法获取参数
        if (data != null) {
            xmlHttpReg.send(data);
        }
        xmlHttpReg.onreadystatechange = doResult; //设置回调函数
    }
    //回调函数
    //一旦readyState的值改变,将会调用这个函数,readyState=4表示完成相应
    //设定函数doResult()
    function doResult() {
        if (xmlHttpReg.readyState == 4 && xmlHttpReg.status == 200) {//4代表执行完成 200代表执行成功
            response = xmlHttpReg.responseText;
        }
        response = '';
    }
    return response;
}

UrlParam = function () { // url参数 
    　　var data, index;
    　　(function init() {
        　　　　data = []; //值，如[["1","2"],["zhangsan"],["lisi"]] 
        　　　　index = {}; //键:索引，如{a:0,b:1,c:2} 
        　　　　var u = window.location.search.substr(1);
        　　　　if (u != '') {
            　　　　　　var params = decodeURIComponent(u).split('&');
            　　　　　　for (var i = 0, len = params.length; i < len; i++) {
                　　　　　　　　if (params[i] != '') {
                    　　　　　　　　　　var p = params[i].split("=");
                    　　　　　　　　　　if (p.length == 1 || (p.length == 2 && p[1] == '')) {// p | p= | = 
                        　　　　　　　　　　　　data.push(['']);
                        　　　　　　　　　　　　index[p[0]] = data.length - 1;
                    　　　　　　　　　　} else if (typeof (p[0]) == 'undefined' || p[0] == '') { // =c 舍弃 
                        　　　　　　　　　　　　continue;
                    　　　　　　　　　　} else if (typeof (index[p[0]]) == 'undefined') { // c=aaa 
                        　　　　　　　　　　　　data.push([p[1]]);
                        　　　　　　　　　　　　index[p[0]] = data.length - 1;
                    　　　　　　　　　　} else {// c=aaa 
                        　　　　　　　　　　　　data[index[p[0]]].push(p[1]);
                    　　　　　　　　　　}
                　　　　　　　　}
            　　　　　　}
        　　　　}
    　　})();
    　　return {
        　　　　// 获得参数,类似request.getParameter() 
        　　　　param: function (o) { // o: 参数名或者参数次序 
            　　　　　　try {
                　　　　　　　　return (typeof (o) == 'number' ? data[o][0] : data[index[o]][0]);
            　　　　　　} catch (e) {
            　　　　　　}
        　　　　},
        　　　　//获得参数组, 类似request.getParameterValues() 
        　　　　paramValues: function (o) { // o: 参数名或者参数次序 
            　　　　　　try {
                　　　　　　　　return (typeof (o) == 'number' ? data[o] : data[index[o]]);
            　　　　　　} catch (e) { }
        　　　　},
        　　　　//是否含有paramName参数 
        　　　　hasParam: function (paramName) {
            　　　　　　return typeof (paramName) == 'string' ? typeof (index[paramName]) != 'undefined' : false;
        　　　　},
        　　　　// 获得参数Map ,类似request.getParameterMap() 
        　　　　paramMap: function () {
            　　　　　　var map = {};
            　　　　　　try {
                　　　　　　　　for (var p in index) { map[p] = data[index[p]]; }
            　　　　　　} catch (e) { }
            　　　　　　return map;
        　　　　}
    }
}();