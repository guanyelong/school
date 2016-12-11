var hrp;//home right panel
var hrdg;//home right datagrid
var centerTabs;//center tabs
var ctabsMenu;//center tabs context menu

$(function () {
    //tabs menu
    ctabsMenu = $('#ctabsMenu').menu({
        onClick: function (item) {
            //console.info('ctabsMenu 点击事件');
            //console.info(item);//右键菜单object
            console.info($(this));
            console.info(item);
            var curTabTitle = $(this).data('tabTitle');//当前tab页标题
                if (item.text == "刷新Tab页") {
                    tabMRefresh(curTabTitle);
                }
                else
                    if (item.text == "关闭Tab页") { 
                        tabMClose(curTabTitle);
                    }
                    else
                        if (item.text == "关闭其他Tab页") { 
                            var tlen = centerTabs.tabs('tabs').length;
                            //var tab = centerTabs.tabs('getSelected'); //当前选中tab
                            var tab = centerTabs.tabs('getTab', curTabTitle); //当前点击tab
                            var index = centerTabs.tabs('getTabIndex', tab); //获取指定tab的索引
                            console.info(index);
                            tabMCloseOthers(tlen, index);
                        }
                        else
                            if (item.text == "关闭所有Tab页") { 
                                var tlen = centerTabs.tabs('tabs').length;
                                tabMCloseAll(tlen);
                            }
        }
    });
    
    //tabs
    centerTabs = $('#ctabs').tabs({
        fit: true,
        border: false,
        onContextMenu: function (e, title) {
            //右键操作
            e.preventDefault();
            ctabsMenu.menu('show', {
                left: e.pageX,
                top: e.pageY
            }).data('tabTitle', title);
        }
        /*onBeforeClose: function (title, index) {
            //关闭tab页之前触发事件
            console.info(title);
            console.info(index);
        }*/
    });

    //datagrid
//    hrdg = $('#hrDG').datagrid({
//        fit: true,
//        fitColumns: true,
//        border: false,
//        url: '/MASQuery/HRDG',
//        method: 'post',
//        pagination: true,
//        /*showPageList: false,//页码选择列表
//        showRefresh: false,//刷新按钮
//        layout: ['first', 'prev', 'next', 'last'],*/
//        columns: [[{
//            title: '登录名',
//            field: 'lName'
//        }, {
//            title: 'IP',
//            field: 'IP'
//        }, {
//            title: '登陆时间',
//            field: 'lDate'
//        }]]
//    });
    //datagrid 分页选择器 隐藏
    $('.pagination-page-list').hide();
    //分页输入框
    //$('.pagination-num').hide();
    //刷新按钮
    //$('.pagination-load').hide();
     

    //panel
//    hrp = $('#hrP').panel({
//        fit: true,
//        border: false,
//        title: '登陆日志',
//        tools: [{
//            iconCls: 'icon-reload',
//            handler: function () {
//                //重新加载hrdg
//                hrdg.datagrid('reload');
//            }
//        }]
//    });

});

