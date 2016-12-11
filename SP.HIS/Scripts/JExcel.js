//form sheetname
function export2Excel(queryParams, euidg, sheetName, module) {

    //定制DataGrid的columns信息,只返回{field:,title:}
    var columns = new Array();

    var ffields = euidg.datagrid('getColumnFields', true);//冻结列
    //冻结列
    for (var i = 0; i < ffields.length; i++) {
        var opts = euidg.datagrid('getColumnOption', ffields[i]);
        var column = new Object();
        column.field = opts.field;
        column.title = opts.title;
        columns.push(column);
    }

    var fields = euidg.datagrid('getColumnFields'); //未冻结列
    //未冻结列
    for (var i = 0; i < fields.length; i++) {
        var opts = euidg.datagrid('getColumnOption', fields[i]);
        var column = new Object();
        column.field = opts.field;
        column.title = opts.title;
        columns.push(column);
    }

    //excel object
    var excelWorkSheet = new Object();
    excelWorkSheet.queryparams = queryParams;//查询参数
    excelWorkSheet.columns = columns;//列名
    excelWorkSheet.module = module;//模块
    excelWorkSheet.sheetName = sheetName;//excel文件名
    //console.info(JSON.stringify(excelWorkSheet));
    //href
    var filename = "/HttpHandlers/ExcelHandler.ashx?action=export2excel&excel=" + JSON.stringify(excelWorkSheet);
    location.href = filename;
    //window.open(filename, '_blank');
}

/***********************************************************************************************************************/

//导出easyui datagrid的数据 sheetName
function exportData(euidg, sheetName) {
    //datagrid rows 
    var rows = euidg.datagrid("getRows");
    if (rows.length == 0) {
        alert("没有数据可供导出!");
        console.info("没有数据可供导出!");
        return;
    }
    //定制DataGrid的columns信息,只返回{field:,title:}
    var columns = new Array();
    var fields = euidg.datagrid('getColumnFields');

    //列
    for (var i = 0; i < fields.length; i++) {
        var opts = euidg.datagrid('getColumnOption', fields[i]);
        var column = new Object();
        column.field = opts.field;
        column.title = opts.title;
        columns.push(column);
    }

    //excel object
    var excelWorkSheet = new Object();
    excelWorkSheet.rows = rows;
    excelWorkSheet.columns = columns;
    //excelWorkSheet.sheetName = "工作时长统计(" + date1.datebox('getValue') + "至" + date2.datebox('getValue') + ")";
    //excelWorkSheet.sheetName = "工作时长统计";
    excelWorkSheet.sheetName = sheetName;

    //post
    //$.post("/HttpHandlers/FileHandler.ashx", "{action:'exportexcel',d:'" + new Date() + "',excelWorkSheet:'" + JSON.stringify(excelWorkSheet) + "'}");

    /*
    var excelObj = {}; // param object
    excelObj.action = 'exportexcel';
    excelObj.excelWorkSheet = JSON.stringify(excelWorkSheet);
    console.info($.toJSON(excelObj)); //自动拼接
    */

    /*
    //ajax post 
    $.ajax({
        url: '/HttpHandlers/FileHandler.ashx',
        //拼接Json字符串，字符型参数的值要添加引号，而且对于用户输入的文本字段要对',/等进行特殊处理
        data: "{action:'exportexcel',d:'" + new Date() + "',excelWorkSheet:'" + JSON.stringify(excelWorkSheet) + "'}",//jquery params 值必须用''或者"" 否则出错 
        //data: $.toJSON(excelObj), //jquery json plugin 自动拼接 方便！ 引用jquery.json.js文件
        type: 'post',
        dataType: 'html',
        contentType: 'application/html;charset=utf-8',
        cache: false,
        success: function (data) {
            //根据返回值data.d判断是不是成功
            console.info('success');
            //window.open(data, '_blank');
            //console.info(data);
        },
        error: function (xhr) {
            //中间发生异常，查看xhr.responseText
            console.info('error');
            console.info(xhr.responseText);
        }
    });
    */

    /*
    // ajax get
    $.ajax({
        url: '/HttpHandlers/FileHandler.ashx',
        data: "action=exportexcel&excelWorkSheet=" + $.toJSON(excelWorkSheet), //jquery params
        type: 'get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        cache: false,
        success: function (data) {
            //根据返回值data.d判断是不是成功
            console.info('success');
            console.info(data);
        },
        error: function (xhr) {
            //中间发生异常，查看xhr.responseText
            console.info('error');
            console.info(xhr.responseText);
        }
    });
    */

    /*
    //get 当数据量太大的时候 url太长 参数不完整 IE存在错误！url长度在2048之内时 没问题！
    //console.info(JSON.stringify(excelWorkSheet).length);
    var filename = "/HttpHandlers/FileHandler.ashx?action=exportexcel&excelWorkSheet=" + JSON.stringify(excelWorkSheet);
    console.info(filename);
    //var filename = "/HttpHandlers/FileHandler.ashx?action=exportexcel&a=1&excelWorkSheet=" + $.toJSON(excelWorkSheet);
    //location.href = filename;
    window.open(filename, '_blank');
    */


}
