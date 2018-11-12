/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="DataTables-1.10.2/jquery.dataTables.js" />
/// <reference path="tinymce/tinymce.min.js" />
/// <reference path="~/Scripts/bootstrap-dialog.js" />
/// <reference path="~/Scripts/modernizr-2.8.3.js" />
/// <reference path="~/Scripts/ace/ace.js" />
/// <reference path="~/Scripts/ckeditor/ckeditor.js"/>
/// <reference path="~/Scripts/ckeditor/plugins/markdown/js/to-markdown.js"/>
/// <reference path="~/Scripts/ckeditor/plugins/markdown/js/marked.js"/>
/// <reference path="~/Scripts/nanospell/autoload.js"/>
(function($) {

    // Unobtrusive UI initialisation code goes here...

    // Wire-up Bootstrap sub-menus
    $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function(event) {
        event.preventDefault();
        event.stopPropagation();
        $(this).parent().siblings().removeClass('open');
        $(this).parent().toggleClass('open');
    });

    // Wire-up any TinyMCE editors
    if (typeof tinymce !== 'undefined') {
        tinymce.init({
            selector: "textarea.html-editor",
            theme: "modern",
            plugins: [
                // removed: fullscreen
                "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                "searchreplace wordcount visualblocks visualchars code",
                "insertdatetime media nonbreaking save table contextmenu directionality",
                "emoticons template paste textcolor colorpicker textpattern tinyfilemanager.net"
            ],
            toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
            toolbar2: "print preview media | forecolor backcolor emoticons",
            image_advtab: true,
            templates: [
                //{ title: 'Test template 1', content: 'Test 1' },
                //{ title: 'Test template 2', content: 'Test 2' }
            ],
            height: "300px",
            convert_urls: false,
        });
        window.tfm_path = '/WebForms/FileManager/dialog.aspx';
    }

    if (typeof ace !== 'undefined') {
        // https://gist.github.com/duncansmart/5267653
        $('textarea.text-editor').each(function() {
            var textarea = $(this);
            var mode = textarea.data('editor');
            var editDiv = $('<div>', {
                position: 'absolute',
                width: textarea.width(),
                height: textarea.height(),
                'class': textarea.attr('class')
            }).insertBefore(textarea);
            textarea.css('display', 'none');
            var editor = ace.edit(editDiv[0]);
            //editor.renderer.setShowGutter(false);
            editor.getSession().setValue(textarea.val());
            editor.getSession().setMode("ace/mode/" + mode);
            editor.setTheme("ace/theme/chrome");

            // copy back to textarea on form submit...
            textarea.closest('form').submit(function() {
                textarea.val(editor.getSession().getValue());
            });
        });
    }

    if (typeof CKEDITOR !== 'undefined') {
        $("textarea.markdown-editor").each(function() {
            var $textarea = $(this);
            var id = $textarea.attr("id");
            if (id !== undefined) {
                convertTextAreaToMarkdown(id);
            }
        });
    }

    function convertTextAreaToMarkdown(id) {
        // Convert markdown to HTML before invoking CKEditor
        var $textarea = $("#" + id);
        var markdown = $textarea.val();
        // Invoke CKEditor
        CKEDITOR.replace(id, {
            toolbarGroups: [
                { name: 'tools' },
                { name: 'links' },
                { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
                { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
                { name: 'styles' },
                { name: 'others' }
            ]
        });
        // Update the editor with the HTML version of the markdown
        if (typeof (markdown) === "string" && markdown != "") {
            var html = marked(markdown, { langPrefix: 'language-' });
            CKEDITOR.instances[id].setData(html);
        }
        // Force CKEditor to convert to markdown when updating the textarea
        CKEDITOR.instances[id].updateElement = function () {
            var html = CKEDITOR.instances[id].getData();
            // Reinstate this line if the markdown button is present in the editor
            //var markdown = $("#cke_" + id + " a.cke_button__markdown").hasClass("cke_button_off")
            //    ? toMarkdown(html)
            //    : html;
            // No markdown button so the user only has HTML
            var markdown = toMarkdown(html);
            $textarea.html(markdown);
        };
        nanospell.ckeditor(id, {
            dictionary: "en_uk",
            server: "asp.net"
        });

        // Handle disabled markdown editor
        if ($textarea.hasClass("markdown-editor-disabled")) {
            CKEDITOR.instances[id].config.bodyClass = "cke_disabled";
            CKEDITOR.instances[id].config.readOnly = true;
        }
    }

    // Wire up of confirmations: data-confirm="Message to display to the user"
    $('[data-confirm]').click(function() {
        //return BootstrapDialog.confirm($(this).data('confirm'));
        return confirm($(this).data('confirm'));
    });

    // Wire up toggles
    //            data-toggle="jQuery selector for item(s) to toggle"
    $('[data-toggle]').click(function(e) {
        $($(this).data('toggle')).toggle();
        e.preventDefault();
    });

    // Bind context menu to the top of the page if it would scroll off the top
    $(window).bind('scroll', function() {
        var $main = $('.navbar-main'),
            $top = $('.navbar-inverse'),
            $window = $(window);

        // But only if the context menu is on the page, i.e. the user is logged in
        if ($main.length > 0) {
            if ($window.scrollTop() > 130) {
                $main.addClass('navbar-fixed-top');
                $top.removeClass('navbar-fixed-top');
            } else {
                $main.removeClass('navbar-fixed-top');
                $top.addClass('navbar-fixed-top');
            }
        }
    });

    // Bind tooltips
    $('.glyphicon-question-sign').tooltip({ placement: "auto" });

    // Check date picker availability
    if (!Modernizr.inputtypes.date) {
        $('input[type=date]').datepicker({
            dateFormat: 'dd/mm/yy'
        });
    }

    // Chrome expects dates to be in wire-format yyyy-mm-dd 
    if (!!window.chrome) {
        var regEx = /(\d+)\/(\d+)\/(\d+)/;
        $('input[type=date]').each(function(i) {
            var date = $(this).attr('value');
            if (date.match(regEx)) {
                var newDate = date.replace(regEx, "$3-$2-$1");
                $(this).val(newDate);
            }
        });
    }

    // Add character counts
    $('.character-count').each(function (index, input) {
        // http://stackoverflow.com/a/14010497
        function wordCount(val) {
            var wom = val.match(/\S+/g);
            return {
                charactersNoSpaces: val.replace(/\s+/g, '').length,
                characters: val.length,
                words: wom ? wom.length : 0,
                lines: val.split(/\r*\n/).length
            };
        }

        var $this = $(input);
        $this.on('keyup', function () {
            var info = wordCount(input.value);
            var showLines = $this.prop('tagName') === 'textarea';
            $(this).next().replaceWith('<div class="text-right small">' + (showLines ? 'Lines: ' + info.lines + ', ' : '') + 'Words: ' + info.words + ', Characters: ' + info.characters + '</div>');
        }).keyup();
    });

    // Show any session message in the appropriate location
    $(".session-message")
        .each(function (i, item) {
            // Find the first header           
            var header = $("h1:first");
            if (header.length === 0) {
                header = $("h2:first");
            }
            if (header.length === 0) {
                header = $("h3:first");
            }
            if (header.length === 0) {
                header = $("h4:first");
            }
            if (header.length === 0) {
                header = $("h5:first");
            }
            if (header.length === 0) {
                header = $("h6:first");
            }
            // No headers, put the message as the first child of the body
            if (header.length === 0) {
                $(".body-content:first-child").before($(this).show());
                return;
            }
            // Found a header, find the last header or hr in this block, terminating at the first hr
            while (header.next()[0] !== undefined && -1 !== $.inArray(header.next()[0].tagName, ['h1', 'H1', 'h2', 'H2', 'h3', 'H3', 'h4', 'H4', 'h5', 'H5', 'h6', 'H6', 'hr', 'HR'])) {
                console.log("Found ", header.next()[0].tagName);
                header = header.next();
                if (-1 === $.inArray(header[0].tagName, 'hr', 'HR')) {
                    break;
                }
            }
            header.after($(this).show());
        });

    // Wire-up DataTables
    if (typeof ($.fn.DataTable) == 'function') {
        var columnDefs = [];
        $('.dataTable')
            .addClass('dt-responsive')
            .addClass('table')
            .addClass('table-striped')
            .addClass('table-bordered')
            .each(function(tableIndex, table) {
                var $table = $(table),
                    html = "<tfoot><tr role='row'>",
                    ajax = $table.data("ajax"),
                    createdRow = $table.data("created-row");
                columnDefs[tableIndex] = [];
                $table.find("thead tr:first th").each(function(thIndex, th) {
                    var $th = $(th),
                        label = $.trim($th.text()),
                        // Enable or disable sorting on this column.
                        sortable = $th.data("sort") !== false,
                        // Enable or disable the table footer filter on this column.
                        filterable = $th.data("filter") !== false,
                        // Enable or disable searching on this column.
                        searchable = $th.data("search") !== false,
                        // Specify the data type of this column for searching.
                        dataType = $th.data("type"),
                        // Enable or disable the display of this column.
                        visible = $th.data("visible") !== false;
                    columnDefs[tableIndex][thIndex] = {
                        "bSortable": sortable,
                        "bSearchable": searchable,
                        "sType": dataType === undefined ? "dom-text" : dataType,
                        "bVisible": visible
                    };
                    html += "<td scope='col'>";
                    if (filterable) {
                        html += "<input type='text' placeholder='" + label + "' data-index='" + thIndex + "'/>";
                    }
                    html += "</td>";
                });
                html += "</tr></tfoot>";
                $table.append($(html));
                $table.DataTable({
                    "aaSorting": [],
                    "sPaginationType": 'full_numbers',
                    "dom": '<"pull-right"C T><"clearfix">Rlfrtip',
                    "tableTools": {
                        "sSwfPath": "/Content/DataTables-1.10.2/swf/copy_csv_xls.swf",
                        "aButtons": [
                            "copy",
                            "csv",
                            "xls",
                            //{
                            //    "sExtends": "pdf",
                            //    "sPdfOrientation": "landscape"/*,
                            //    "sPdfMessage": "Your custom message would go here."*/
                            //},
                            "print"
                        ]
                    },
                    "aoColumns": columnDefs[tableIndex],
                    "ajax": ajax,
                    "deferRender": true,
                    "colVis": {
                        //restore: "Restore",
                        showAll: "Show all",
                        showNone: "Show none"
                    },
                    "createdRow": window[createdRow]
                });
            });

        // Helper to fix table width and update column visibity selector
        var fixTablesTimer;

        function fixTables($table) {
            clearTimeout(fixTablesTimer);
            fixTablesTimer = setTimeout(function() {
                $table.css('width', '100%');
                $table.each(function() {
                    var dt = $(this).dataTable();
                    $.fn.dataTable.ColVis.fnRebuild(dt);
                });
            }, 50);
        }

        // Make the table nicer, this $('.dataTable') is not the same as the one above
        var $dataTable = $('.dataTable');
        $dataTable.each(function() {
            var dt = $(this).dataTable();
            $(this)
                .css('width', '100%')
                .on('draw.dt', function() {
                    setTimeout(function() {
                        fixTables($dataTable);
                    }, 50);
                })
                .on("init.dt", function(e, settings, json) {
                    var init = $(dt).data('init');
                    if (init !== undefined && typeof (window[init]) == "function") {
                        window[init](e, settings, json);
                    }
                })
                .closest('.dataTables_wrapper')
                .find('div.ColVis').addClass("DTTT").find("button").removeClass().addClass('btn btn-default').end().end()
                .find('div[id$=_filter] input').attr('placeholder', 'Search').addClass('form-control input-sm').end()
                .find('div[id$=_length] select').addClass('form-control input-sm').end()
                .bind('page', function(e) {
                    window.console && console.log('pagination event:', e); //this event must be fired whenever you paginate
                })
                .find("tfoot input")
                .keyup(function() {
                    /* Filter on the column (the index) of this element */
                    dt.fnFilter($(this).val(), parseInt($(this).data("index")));
                }).end();
        });
        // Prevent issues caused by explicit widths added for responsiveness
        if ($dataTable.length > 0) {
            $(window).bind('resize', function() {
                fixTables($dataTable);
            });
        }
    }

})(jQuery);

// Usage: "{0} world".format("hello")
String.prototype.format = String.prototype.f = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};