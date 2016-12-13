/*自定义文件导入组件*/
(function ($) {
    /*相关函数*/
    function initUploadFile(target) {
        var state = $.data(target, 'uploadFile');
        var opts = state.options;

        initUploader(target);
    }

    function deleteFile(target, file, fileId, fileName) {
        if (!fileId) {
            return;
        }

        var state = $.data(target, 'uploadFile');
        var opts = state.options;

        $.ajax({
            url: "/WebUpload/DeleteFile",
            data: { fileName: fileName },
            dataType: "json",
            success: function (returnData) {
                //failed
                if (returnData.result == "error") {
                    $.messager.alert("提示", "删除错误 " + returnData.message);
                    $("#grid").treegrid("load");
                    return;
                }
                $("#grid").treegrid("load");
            },
            error: function (returnData) {
                $.messager.alert("提示", "删除失败 ");
            }

        });

        filePath = $("#" + fileId).attr("filePath");

        if (opts.deleteFile != null) {
            opts.deleteFile(fileName, filePath);
        }

        $("#" + fileId).remove();

    }

    //默认索引文件
    function initDefaultFile(target) {
        var state = $.data(target, 'uploadFile');
        var opts = state.options;

        var filePath = opts.defaultFileSrc;

        var $list = $("#fileList");
        var $li = $('<div id="existFile" class="file-item thumbnail" filePath="' + filePath + '">' +
                        '<img>' +
                        '<div class="delete">删除</div>' +
                    '</div>'),
            $img = $li.find('img');

        // $list为容器jQuery实例
        $list.append($li);

        $("#existFile .delete").on("click", function () {

            var filePath = $(this).parent().attr("filePath");
            var fileName = filePath.substring(filePath.lastIndexOf("/") + 1);
            deleteFile(target, null, "existFile", fileName);
        });

        // 创建缩略图
        // 如果为非图片文件，可以不用调用此方法。
        // thumbnailWidth x thumbnailHeight 为 100 x 100
        $img.attr('src', filePath);
        $img.height(opts.width);
        $img.width(opts.height);

    }
    //开始上传
    function initUploader(target) {
        var state = $.data(target, 'uploadFile');
        var opts = state.options;

        var uploader = state.uploader;
        if (uploader) {
            state.uploader = null;
            uploader = null;
        }

        $(target).empty().append($.fn.uploadFile.templateHtml);
        if (opts.title != "") {
            $(target).find("label[sid='title']").val(opts.title);
        }
        else {
            $(target).find("label[sid='title']").remove();
        }


        var thumbWidth = opts.width;
        var thumbHeight = opts.height;

        //初始化uploader 组件
        uploader = WebUploader.create({
            auto: true,
            // swf文件路径
            swf: '/Scripts/WebUploader/Uploader.swf',
            // 文件接收服务端。
            server: '/WebUpload/UploadFile',
            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: {
                id: '#picker',
                multiple: false
            },
            // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
            resize: false,
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',
                mimeTypes: 'image/*'
            }
        });

        uploader.on('beforeFileQueued', function (file) {

            //只能上传一个文件
            var files = uploader.getFiles();

            if (opts.singleFile && files.length > 0) {
                $.each(files, function (index, file) {
                    uploader.removeFile(file);
                });
            }

            if (opts.singleFile) {
                var $list = $("#fileList").empty();
            }
        });

        uploader.on('fileQueued', function (file) {

            var $list = $("#fileList");
            var $li = $(
            '<div id="' + file.id + '" class="file-item thumbnail">' +
                '<img>' +
                '<div class="info">' + file.name + '</div>' +
                '<div class="delete">删除</div>' +
            '</div>'),
            $img = $li.find('img');

            // $list为容器jQuery实例
            $list.append($li);

            $("#" + file.id + " .delete").on("click", function () {
                uploader.removeFile(file);
                var filePath = $(this).parent().attr("fileName");
                deleteFile(target, file, file.id, filePath);
            });

            // 创建缩略图
            // 如果为非图片文件，可以不用调用此方法。
            // thumbnailWidth x thumbnailHeight 为 100 x 100
            uploader.makeThumb(file, function (error, src) {
                if (error) {
                    $img.replaceWith('<span>不能预览</span>');
                    return;
                }

                $img.attr('src', src);
            }, thumbWidth, thumbHeight);
        });

        uploader.on('uploadProgress', function (file, percentage) {
            var $li = $('#' + file.id),
        $percent = $li.find('.progress span');

            // 避免重复创建
            if (!$percent.length) {
                $percent = $('<p class="progress"><span></span></p>')
                .appendTo($li)
                .find('span');
            }

            $percent.css('width', percentage * 100 + '%');
        });

        uploader.on('uploadSuccess', function (file, response) {
            //$('#' + file.id).find('p.state').text('已上传');
            $('#' + file.id).addClass('upload-state-done').attr("fileSrc", response.filePath);
            $('#' + file.id).attr("fileName", response.fileName);
            if (opts.uploadSucess) {
                opts.uploadSucess(file, response, uploader);
            }
        });

        uploader.on('uploadError', function (file) {
            var $li = $('#' + file.id),
                $error = $li.find('div.error');

            // 避免重复创建
            if (!$error.length) {
                $error = $('<div class="error"></div>').appendTo($li);
            }

            $error.text('上传失败');
        });

        uploader.on('uploadComplete', function (file, response) {
            //文件上传完成
            //$('#' + file.id).find('.progress').remove();
            $.messager.alert("文件上传提示", "文件上传成功");
        });

        if (opts.defaultFileSrc && opts.defaultFileSrc != "") {
            initDefaultFile(target);
        }

        state.uploader = uploader;
    }
    //文件上传
    function uploadFile(target) {
        var state = $.data(target, 'uploadFile');
        var opts = state.options;

        var uploader = state.uploader;
        if (uploader == undefined) {
            return;
        }
        uploader.upload();
    }

    //构造函数
    $.fn.uploadFile = function (options) {
        if (typeof options == 'string') {
            var method = $.fn.uploadFile.methods[options];
            if (method) {
                return method(this);
            }
            else {
                return '无效方法';
            }
        }

        options = options || {};

        return this.each(function () {
            var state = $.data(this, 'uploadFile');
            if (state) {
                $.extend(state.options, options);
                initUploadFile(this);
            }
            else {
                state = $.data(this, 'uploadFile', {
                    options: $.extend({}, $.fn.uploadFile.Defaults, options),
                    uploader: null //上传组建
                })
                initUploadFile(this);
            }
        })
    }
    //默认属性
    $.fn.uploadFile.Defaults = {
        title: "选择文件：",
        width: 150,
        height: 150,
        uploader: {
            onuploadcomplet: function (importUrl) {
                //文件上传完成事件
            }
        }
    };
    $.fn.uploadFile.methods = {

    };
    //模版Html
    $.fn.uploadFile.templateHtml = '<div id="uploader" class="wu-example">' +
                                        '<div>' +
                                            '<label sid="title" for="exampleInputFile"></label>' +
                                            '<div id="picker">选择文件</div>' +
                                        '</div>' +
                                        '<!--用来存放文件信息-->' +
                                        '<div id="fileList" class="uploader-list"></div>' +
                                    '</div>' +
                                '</div>';
} (jQuery));