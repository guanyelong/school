var MaskUtil = (function () {

    var $mask, $maskMsg;

    var defMsg = '正在处理，请稍待。。。';

    function init(id) {
        //if (!$mask) {
        if (id == '' || id == null) {
            id = "body";
        }
        $mask = $("<div class=\"datagrid-mask mymask\"></div>").appendTo(id);
        //}
        //if (!$maskMsg) {
        $maskMsg = $("<div class=\"datagrid-mask-msg mymask\">" + defMsg + "</div>")
                .appendTo(id).css({ 'font-size': '12px' });
        //}
        $mask.css({ width: "100%", height: "100%" });

        $maskMsg.css({
            left: ($mask.width() - 190) / 2, top: ($mask.height() - 45) / 2
        });

    }

    return {
        mask: function (id) {
            init(id);
            $mask.show();
            $maskMsg.html(defMsg).show();
        }
        , unmask: function () {
            $mask.hide();
            $maskMsg.hide();
            $mask.remove();
            $maskMsg.remove();
        }
    }

} ());