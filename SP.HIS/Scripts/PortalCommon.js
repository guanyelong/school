/*******************************************begin自定义验证规则**********************************************/
$(function () {
    $.extend($.fn.validatebox.defaults.rules, {
        equals: {
            validator: function (value, param) {
                return value == $(param[0]).val();
            },
            message: '两次输入不一致'
        },
        phone: {
            validator: function (value, param) {
                var telReg = /^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
                return telReg.test(value);
            },
            message: '请输入有效的手机号码格式'
        },
        Unique: {
            validator: function (value, param) {
                if (param == undefined) param = {};
                var isok = false;
                $.ajax({
                    type: "POST",
                    async: false,
                    data: { value: value, name: param[1], id: $(param[2]).val() },
                    url: param[0],
                    success: function (result) {
                        if (result == "True") isok = true;
                    }
                });
                return isok;
            },
            message: '信息已存在，请重新输入'
        },
        NameUnique: {
            validator: function (value, param) {
                if (param == undefined) param = {};
                param[4] = "信息已存在，请重新输入";
                var parent = $(param[2]).val();
                var isok = false;
                if (parent != "") {
                    $.ajax({
                        type: "POST",
                        async: false,
                        data: { value: value, name: param[1], id: $(param[3]).val(), parent: parent },
                        url: param[0],
                        success: function (result) {
                            if (result == "True") isok = true;
                        }
                    });
                } else {
                    param[4] = "请选择所属类别";
                }
                return isok;
            },
            message: '{4}'
        }
    });
});
/*******************************************end自定义验证规则**********************************************/
/*******************************************begin分页条操作**********************************************/
//绑定分页条
function DataGridPagination(pages, obj, form) {
    pages.pagination({
        total: obj.total,
        pageNumber: obj.pageNumber,
        pageSize: obj.pageSize,
        pageList: [10, 15, 20, 30, 40, 50],
        layout: ['list', 'sep', 'first', 'prev', 'links', 'next', 'last', 'sep'],
        onSelectPage: function (pageNumber, pageSize) {
            $(form).find("input[name='pageNumber']").val(pageNumber);
            $(form).find("input[name='pageSize']").val(pageSize);
            Select();
        }
    });
}
/*******************************************end分页条操**********************************************/


/*******************************************begin列表内容转换**********************************************/
function PublishedState(value) {
    if (typeof (value) == "boolean" && value) value = "True";
    if (value == "True") {
        return "已发布";
    } else {
        return "未发布";
    }
}
function AuditState(value) {
    if (typeof (value) == "boolean" && value) value = "True";
    if (value == "True" || value == "1") {
        return "已通过";
    } else {
        return "未通过";
    }
}
function MenuState(value) {
    if (typeof (value) == "boolean" && value) value = "True";
    if (value == "True") {
        return "显示";
    } else {
        return "隐藏";
    }
}
function ContentState(value) {
    if (typeof (value) == "boolean" && value) value = "True";
    if (value == "True") {
        return "有";
    } else {
        return "无";
    }
}
function SexState(value) {
    if (typeof (value) == "boolean" && value) value = "True";
    if (value == "True") {
        return "男";
    } else {
        return "女";
    }
}
function ConvertPrice(value) {
    if (!isNaN(value)) {
        return parseFloat(value).toFixed(2);
    } else {
        return 0.00;
    }
}
function DateFormat(val) {
    if (val != null && val != "") {
        var date = new Date(parseInt(val.replace("/Date(", "").replace(")/", ""), 10));
        //月份为0-11，所以+1，月份小于10时补个0
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hours + ":" + minutes + ":" + seconds;
    }
    else {
        return "";
    }
}
function DateFormatYMD(val) {
    if (val != null && val != "") {
        var date = new Date(parseInt(val.replace("/Date(", "").replace(")/", ""), 10));
        //月份为0-11，所以+1，月份小于10时补个0
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
    else {
        return "";
    }
}
function ContentPrompt() {
    var table = $("table.datagrid-btable div.datagrid-cell");
    table.each(function () {
        var content = $(this).text();
        $(this).attr("title", content);
    });
}
function ConvertCoding(value) {
    if (value != "") {
        return decodeURI(value)
    } else {
        return value;
    }
}
/*******************************************end列表内容转换**********************************************/
