/* 拓展方法 把form表单元素的值序列化称Object对象 begin */
serializeObject = function (form) {
    var o = {};
    $.each(form.serializeArray(), function (index) {
        if (o[this['name']]) {
            o[this['name']] = o[this['name']] + "," + this['value'];
        }
        else {
            o[this['name']] = this['value'];
        }
    });
    return o;
};
/* 拓展方法 把form表单元素的值序列化称Object对象 end */

/* 更换主题 begin */
function changeThemeFun(themeName) {
    var $easyuiTheme = $('#easyuiTheme');
    var url = $easyuiTheme.attr('href');
    var href = url.substring(0, url.indexOf('themes')) + 'themes/' + themeName + '/easyui.css';
    $easyuiTheme.attr('href', href);
    //内部窗口主题
    var $iframe = $('iframe');
    if ($iframe.length > 0) {
        for (var i = 0; i < $iframe.length; i++) {
            var ifr = $iframe[i];
            $(ifr).contents().find('#easyuiTheme').attr('href', href);
        }
    }
};
/* 更换主题 end */


/* 当前日期 begin */
//上一个月
function prevMonthDate(da) {
    var d = new Date(da);
    var vYear = d.getFullYear();//年
    var vMon = d.getMonth();//月
    if (vMon < 10) {
        vMon = "0" + vMon;
    }
    var vDay = d.getDate();//日
    if (vDay < 10) {
        vDay = "0" + vDay;
    }
    return vYear + "-" + vMon + "-" + vDay;
}
function parseDate(dateStr) {
    var strArray = dateStr.split("-");
    if (strArray.length == 3) {
        //alert("0"+(strArray[1]-1));
        return new Date(strArray[0], "0" + (strArray[1] - 1), strArray[2]);
    } else {
        return new Date();
    }
}
function prevDate(da) {
    var d = parseDate(da);
    //var d= new Date(e);
    var vYear = d.getFullYear(); //年
    var vMon = d.getMonth(); //月
    if (vMon < 10) {
        vMon = "0" + vMon;
    }
    var vDay = d.getDate(); //日
    if (vDay < 10) {
        vDay = "0" + vDay;
    }
    return vYear + "-" + vMon + "-" + vDay;
}
//前一天
function prevDayDate(da) {
    var d = new Date(da);
    var vYear = d.getFullYear();//年
    var vMon = d.getMonth() + 1;//月
    if (vMon < 10) {
        vMon = "0" + vMon;
    }
    var vDay = d.getDate() - 1;//日
    if (vDay < 10) {
        vDay = "0" + vDay;
    }
    return vYear + "-" + vMon + "-" + vDay;
}
//下一个月
function nextMonthDate(da) {
    var d = new Date(da);
    var vYear = d.getFullYear();//年
    var vMon = d.getMonth() + 2;//月
    if (vMon < 10) {
        vMon = "0" + vMon;
    }
    var vDay = d.getDate();//日
    if (vDay < 10) {
        vDay = "0" + vDay;
    }
    return vYear + "-" + vMon + "-" + vDay;
}
//后一天
function nextDayDate(da) {
    var d = new Date(da);
    var vYear = d.getFullYear();//年
    var vMon = d.getMonth() + 1;//月
    if (vMon < 10) {
        vMon = "0" + vMon;
    }
    var vDay = d.getDate() + 1;//日
    if (vDay < 10) {
        vDay = "0" + vDay;
    }
    return vYear + "-" + vMon + "-" + vDay;
}
//当前日期 年月日
function showDate() {
    var d = new Date();
    var vYear = d.getFullYear();//年
    var vMon = d.getMonth() + 1;//月
    if (vMon < 10) {
        vMon = "0" + vMon;
    }
    var vDay = d.getDate();//日
    if (vDay < 10) {
        vDay = "0" + vDay;
    }
    return vYear + "-" + vMon + "-" + vDay;
}
/* 当前日期 end */

/* 当前时间 begin */
setInterval(show, 1000);
function show() {
    time = new Date();
    var hour = time.getHours();//时
    if (hour < 10) {
        hour = "0" + hour
    }
    var minu = time.getMinutes();//分
    if (minu < 10) {
        minu = "0" + minu
    }
    var sec = time.getSeconds();//秒
    if (sec < 10) {
        sec = "0" + sec
    }
    datetime = hour + ":" + minu + ":" + sec;
    $('.stime').html(datetime);
}
/* 当前时间 end */

/* 日期格式化 begin */

//拓展format方法
// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
// 调用方法
// var time1 = new Date().format("yyyy-MM-dd HH:mm:ss");     
// var time2 = new Date().format("yyyy-MM-dd");
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
/* 日期格式化 end */