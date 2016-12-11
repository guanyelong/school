/* HighCharts Data格式 处理函数 */

/* ***************************************** 格式化数据 ***************************************** */

/*柱状图 begin

调用:
$.getJSON(
            "url",//url
            //serializeObject(wlForm), //query params
            function (r) {  //success
                console.info(JSON.stringify(r)); //json对象转化为字符串
                var data = json2ArrayColumn(JSON.stringify(r));//字符串转化为chart要求的格式
                showChart(data);//柱状图加载函数
            }
        ); 
*/
function json2ArrayColumn(jsonData) {
    //console.info('json2ArrayColumn');
    var jsonsString = jsonData.slice(1, jsonData.length - 1);
    var jsonStrings = jsonsString.split("},");
    var length = jsonStrings.length;
    var jsons = [];
    for (var i = 0; i != length - 1; ++i) {
        jsonStrings[i] += '}';
    }
    var source = [[]];
    for (var i = 0; i < length; i++) {
        jsons[i] = eval('(' + jsonStrings[i] + ')');
        var data = [];
        for (var key in jsons[i]) {
            if (i == 0) {
                source[0].push(key);
            }
            data.push(jsons[i][key]);
        }
        source.push(data);
    }
    return source.slice(1, source.length);
}

/*柱状图 end*/

/* ***************************************** show chart ***************************************** */

//show chart 折线图
function showChartLine(data1, cate1, title1, subtitle1, ytitle1) {
    $('.dchart').highcharts({
        chart: {
            type: 'line'
        },
        title: {
            text: title1 //'Monthly Average Temperature'
        },
        subtitle: {
            text: subtitle1 //'Source: WorldClimate.com'
        },
        xAxis: {
            categories: cate1
        },
        yAxis: {
            title: {
                text: ytitle1 //'Temperature (°C)'
            }
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
        series: data1 //chart data
    });
}

//show chart 饼状图
function showChartPie(data1, title1, sname1) {
    // Build the chart
    $('.dchart').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: title1 //'Browser market shares at a specific website, 2014'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: sname1, //'Browser share',
            data: data1 //chart数据
        }]
    });
}


//show chart 横向柱状图
function showChartBar(data1, cate1, title1, subtitle1, ytitle1, tooltip1) {
    $('.dchart').highcharts({
        chart: {
            type: 'bar'
        },
        title: {
            text: title1 //'Historic World Population by Region' //chart标题
        },
        subtitle: {
            text: subtitle1 //'Source: Wikipedia.org'
        },
        xAxis: {
            categories: cate1, // 左侧标题 Y
            title: {
                text: null
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: ytitle1, // 'Population (millions)',// 水平单位 X
                align: 'high'
            },
            labels: {
                overflow: 'justify'
            }
        },
        tooltip: {
            valueSuffix: tooltip1 //' millions' //提示文字
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        legend: { //说明
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'top',
            x: -40,
            y: 100,
            floating: true,
            borderWidth: 1,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor || '#FFFFFF'),
            shadow: true
        },
        credits: {
            enabled: true
        },
        series: data1 //chart数据
    });
}

//show chart 柱状图
function showChartColumn(data1, title1, subtitle1, ytitle1, tooltip1, sname1) {
    $('.dchart').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: title1 //'工作时长统计'
        },
        subtitle: {
            text: subtitle1 //date1.datebox('getValue') + ' 至 ' + date2.datebox('getValue')
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: -45,
                align: 'right',
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: ytitle1 //'工作时长 (分钟)'
            }
        },
        legend: {
            enabled: true //说明
        },
        tooltip: {
            pointFormat: tooltip1 //'工作时长: <b>{point.y:.1f} 分钟</b>'
        },
        series: [{
            name: sname1, //'Population'
            data: data1, //data
            dataLabels: {
                enabled: true,
                rotation: -90,
                color: '#FFFFFF',
                align: 'right',
                x: 4,
                y: 10,
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif',
                    textShadow: '0 0 3px black'
                }
            }
        }]
    });
}


/* ***************************************** ***************************************** */

//设置有选中项的Pie数据
function SetSelectedPieData(data) {
    //console.info('SetSelectedPieData'); 
    //声明对象 赋值
    var s = {};
    s.name = data[0][0];
    s.y = data[0][1];
    s.sliced = true;
    s.selected = true;
    //s对象替换0索引的数据
    var d = data.slice(0, data.length);
    d[0] = s;
    return d;
}


/* ***************************************** */

/* json 转化为 二维数组 begin */

function json2Array1() {
    var jsonData = [{ id: '0', name: '工厂', size: 13, parentID: '' }, { id: '1', name: '分销A', size: 13, parentID: '0' }, { id: '2', name: '分销B', size: 13, parentID: '0' }];
    var source = [];
    source[0] = ['ID', 'name', 'size', 'parentID'];
    for (var i = 0, j = jsonData.length; i < j; i++)
        source[i + 1] = [jsonData[i].id, jsonData[i].name, jsonData[i].size, jsonData[i].parentID];

    alert(source.join('|'))
}

function json2Array2() {
    var jsonData = [{ id: '0', name: '工厂', size: 13, parentID: '' }, { id: '1', name: '分销A', size: 13, parentID: '0' }, { id: '2', name: '分销B', size: 13, parentID: '0' }];
    var arr1 = "['ID','name','size','parentID']";
    var arr2 = '[';
    for (i = 0; i < jsonData.length; i++) {
        var temp = "['{0}','{1}','{2}','{3}']";
        arr2 += temp.replace("{0}", jsonData[i].id).replace("{1}", jsonData[i].size).replace("{2}", jsonData[i].name).replace("{3}", jsonData[i].parentID)
    }
    arr2 += ']'
    var source = [arr1, arr2];
    alert(source);
}

function json2Array(jsonData) {
    console.info('----json2Array----');
    console.info(jsonData);
    var jsonsString = jsonData.slice(1, jsonData.length - 1);
    //console.info('jsonsString:' + jsonsString);
    var jsonStrings = jsonsString.split("},");
    //console.info('jsonStrings:' + jsonStrings);
    var length = jsonStrings.length;
    //console.info('length:' + length);
    var jsons = [];
    for (var i = 0; i != length - 1; ++i) {
        jsonStrings[i] += '}';
    }
    //console.info('jsonStrings2:' + jsonStrings);
    //console.info(jsonString[0]);
    //console.info(jsonStrings[0]);
    var source = [[]];
    //for (var i = 0; i != length; ++i) {
    for (var i = 0; i < length; i++) {
        jsons[i] = eval('(' + jsonStrings[i] + ')');
        var data = [];
        for (var key in jsons[i]) {
            if (i == 0) {
                source[0].push(key);
            }
            data.push(jsons[i][key]);
        }
        source.push(data);
    }
    return source.slice(1, source.length);
}

/* json 转化为 二维数组 end */