var ulTree;//left Main Tree 
var pulTree;//left permission Main Tree 

$(function () {
    ulTree = $('#ulTree').tree({
        url: '/Home/LoadUserTree',
        lines: true,
        onClick: function (node) {
            if (node) {
                var isLeaf = $(this).tree('isLeaf', node.target);
                if (isLeaf) {
                    addTab(node);
                    console.info(node);
                }
            }
            
        },
        onDblClick: function(node) {
            //如果不是子节点
            //双击 展开or折叠节点
            var isLeaf = $(this).tree('isLeaf', node.target);
            if (!isLeaf) {
                if (node.state == 'closed') {
                    $(this).tree('expand', node.target);
                } else {
                    $(this).tree('collapse', node.target);
                }
            }
        },
        onLoadSuccess: function(node, data) {
            var t = $(this);
            if (data) {
                $(data).each(function (index, d) {
                   
                    //全部展开
                    if (this.state == 'closed') {
                        t.tree('expandAll');
                    }
                });
            }
        }
    });


    pulTree = $('#pulTree').tree({
        url: '/MASQuery/LoadPTree',
        lines: true,
        onClick: function(node) {
            //1.判断是子节点
            //2.打开or选中窗口
            var isLeaf = $(this).tree('isLeaf', node.target);
            if (isLeaf) {
                //console.info(node.text + '#' + node.attributes.url);
                addTab(node); // JCenter.js
            }
        },
        onDblClick: function(node) {
            //如果不是子节点
            //双击 展开or折叠节点
            var isLeaf = $(this).tree('isLeaf', node.target);
            if (!isLeaf) {
                if (node.state == 'closed') {
                    $(this).tree('expand', node.target);
                } else {
                    $(this).tree('collapse', node.target);
                }
            }
        },
        onLoadSuccess: function(node, data) {
            //加载到2级菜单
            //console.info(node);
            //console.info(data);
          
            var t = $(this);
            if (data) {
                
                $(data).each(function(index, d) {
                    //全部展开
                    if (this.state == 'closed') {
                        t.tree('expandAll');
                    }
                    //设置1级菜单 展开
                    /*if (this.state == 'closed' && this.id.length == 2) {
                        t.tree('expandAll');
                    }*/
                });
            }
        }
    });

});