/* tab页右键菜单 */
//刷新tab页
function tabMRefresh(titleOrIndex) {
    //console.info('tabMRefresh:' + titleOrIndex);
    refreshTab(titleOrIndex);
}
//关闭
function tabMClose(titleOrIndex) {
    //console.info('tabMClose:' + titleOrIndex);
    centerTabs.tabs('close', titleOrIndex);
}
//关闭其他
function tabMCloseOthers(tLength, index) {
    //console.info('tabMCloseAll:' + tLength);
    var ti = 0; //要删除tab页的索引
    for (var i = 0; i < tLength; i++) {
        if (i == index) {
            ti = 1;
        }
        tabMClose(ti); //关闭其他所有
    }
}
//关闭全部
function tabMCloseAll(tLength) {
    //console.info('tabMCloseAll:' + tLength);
    for (var i = 0; i < tLength; i++) {
        tabMClose(0); //关闭所有 首页tab也关闭
    }
}
/* center */
//刷新tab页
function refreshTab(title) {
    var tab = centerTabs.tabs('getTab', title);
    if (tab != null) {
        centerTabs.tabs('update', {
            tab: tab,
            options: tab.panel('options')
        });
    }
}

//新增tab页
function addTab(node) {
    //console.info('addTab');
    //console.info(node);
    //如果存在tab页 选中tab页
    if (centerTabs.tabs('exists', node.text)) {
        centerTabs.tabs('select', node.text);
    }
    else {
        //不存在的话 新增Tab页
        //加载Tab页 
        TabLoad(node);
    }
}
//tab页加载
function TabLoad(node) {
    /*//进度条
    $.messager.progress({
    title: 'JulieCT提示：',
    text: '页面加载中...',
    interval: 60
    });
    //600ms之后 进度条关闭
    setTimeout(function () {
    $.messager.progress('close');
    }, 600);*/
    //新增tab页
    centerTabs.tabs('add', {
        title: node.text,
        closable: true,
        content: '<iframe src="' + node.attributes.url + '" frameborder="0" style="border:0;width:100%;height:99.4%;"></iframe>',
        tools: [{
            iconCls: 'icon-mini-refresh', //刷新按钮
            handler: function () {
                refreshTab(node.text);
            }
        }]
    });
}

function addTab1(subtitle, url, icon) {
    if (!centerTabs.tabs('exists', subtitle)) {
        centerTabs.tabs('add', {
            title: subtitle,
            content: '<iframe src="' + url + '" frameborder="0" style="border:0;width:100%;height:99.4%;"></iframe>',
            closable: true,
            tools: [{
                iconCls: 'icon-mini-refresh', //刷新按钮
                handler: function () {
                    refreshTab(subtitle);
                }
            }],
            icon: icon
        });
    } else {
        centerTabs.tabs('select', subtitle);
    }
}

function addTabContent(subtitle, content, icon) {
    if (!centerTabs.tabs('exists', subtitle)) {
        centerTabs.tabs('add', {
            title: subtitle,
            content: content,
            closable: true,
            tools: [{
                iconCls: 'icon-mini-refresh', //刷新按钮
                handler: function () {
                    refreshTab(subtitle);
                }
            }],
            icon: icon
        });
    } else {
        centerTabs.tabs('select', subtitle);
    }
}
