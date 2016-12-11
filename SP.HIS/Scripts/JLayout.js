var cc; //main Layout
var lbhelp; //top linkbutton help
var lblogout; //top linkbutton logout
var lbhome; //top linkbutton home
var lbadmin; //top linkbutton admin
var lDialog; //login dialog
var lForm; //login form

$(function () {
    //link buttons
    lbhelp = $('#lbhelp').linkbutton({
        plain: true,
        iconCls: 'icon-jhelp',
        onClick: function () {
            console.info('help linkbutton');
        }
    });

    lblogout = $('#lblogout').linkbutton({
        plain: true,
        iconCls: 'icon-logout',
        onClick: function () {
            $.messager.confirm('温馨提示', '确定要退出系统吗?', function (r) {
                if (r) {
                    $.ajax({
                        url: '/Home/Logout',
                        type: "post",
                        cache: false,
                        dataType: 'json',
                        success: function (r) {
                            if (r.ifOk) {
                                window.location.reload();
                            }
                        }
                    });
                }
            });
        }
    });

    lbhome = $('#lbhome').linkbutton({
        plain: true,
        iconCls: 'icon-home',
        onClick: function () {
            centerTabs.tabs('select', '首页');
        }
    });

    lbadmin = $('#lbadmin').linkbutton({
        plain: true,
        iconCls: 'icon-smile',
        onClick: function () {
            centerTabs.tabs('select', '首页');
        }
    });
    cc = $('#cc').layout();
})

var loadTree = function (menuId) {

    //首页默认选中
    if (menuId == "01") {
        centerTabs.tabs('select', '首页');
    }

    $('#ulTree').tree({
        url: '/Home/LoadUserTree?id=' + menuId,
        lines: true,
        onClick: function (node) {
            if (node) {
                var isLeaf = $(this).tree('isLeaf', node.target);
                if (isLeaf) {
                    addTab(node);
                }
            }
        },
        onDblClick: function (node) {
            var isLeaf = $(this).tree('isLeaf', node.target);
            if (node) {
                if (!isLeaf) {
                    if (node.state == 'closed') {
                        $(this).tree('expand', node.target);
                    } else {
                        $(this).tree('collapse', node.target);
                    }
                }
            }
        }
        //        onLoadSuccess: function (node, data) {
        //            var t = $(this);
        //            if (data) {
        //                $(data).each(function (index, d) {
        //                    if (this.state == 'closed') {
        //                        t.tree('expandAll');
        //                    }
        //                });
        //            }
        //        }
    });
};