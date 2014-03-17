$(function () {

    var ModestMethods = {};

    ModestMethods.GridManager = {};

    /* * * Posts Grid * * */
    ModestMethods.GridManager.postsGrid = function (gridName, pagerName) {

        var afterclickPgButtons = function (whichbutton, formid, rowid) {
            tinyMCE.get("Description").setContent(formid[0]["Description"].value);
            tinyMCE.get("Content").setContent(formid[0]["Content"].value);
        };

        var afterShowForm = function (form) {
            tinyMCE.execCommand('mceAddControl', false, "Description");
            tinyMCE.execCommand('mceAddControl', false, "Content");
        };

        var onClose = function (form) {
            tinyMCE.execCommand('mceRemoveControl', false, "Description");
            tinyMCE.execCommand('mceRemoveControl', false, "Content");
        };

        var beforeSubmitHandler = function (postdata, form) {
            var selRowData = $(gridName).getRowData($(gridName).getGridParam('selrow'));
            if (selRowData["PostedOn"])
                postdata.PostedOn = selRowData["PostedOn"];
            postdata.Description = tinyMCE.get("Description").getContent();
            postdata.Content = tinyMCE.get("Content").getContent();

            return [true];
        };

        // Post columns
        var colNames = [
            'PostId',
            'Title',
            'Description',
            'Content',
            'Category',
            'CategoryId',
            'Tags',
            'Author',
            'Url Slug',
            'Published',
            'Posted On',
            'Modified'
        ];

        var columns = [];

        columns.push({
            name: 'PostId',
            editable: true,
            hidden: true,
            key: true
        });

        columns.push({
            name: 'Title',
            index: 'Title',
            width: 250,
            editable: true,
            editoptions: {
                size: 43,
                maxlength: 500
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'Description',
            hidden: true,
            width: 250,
            sortable: false,
            editable: true,
            edittype: 'textarea',
            editoptions: {
                rows: "20",
                cols: "100"
            },
            editrules: {
                custom: true,

                custom_func: function (val, colname) {
                    val = tinyMCE.get("Description").getContent();
                    if (val) return [true, ""];
                    return [false, "The " + colname + " field is required"];
                },

                edithidden: true
            }
        });


        columns.push({
            name: 'Content',
            hidden: true,
            width: 250,
            sortable: false,
            editable: true,
            edittype: 'textarea',
            editoptions: {
                rows: "40",
                cols: "100"
            },
            editrules: {
                custom: true,

                custom_func: function (val, colname) {
                    val = tinyMCE.get("Content").getContent();
                    if (val) return [true, ""];
                    return [false, "The " + colname + " field is required"];
                },

                edithidden: true
            }
        });

        columns.push({
            name: 'CategoryId',
            hidden: true,
            editable: true,
            edittype: 'select',
            editoptions: {
                style: 'width:250px;',
                dataUrl: '/Admin/GetCategoriesHtml'
            },
            editrules: {
                required: true,
                edithidden: true
            }
        });

        columns.push({
            name: 'Category.Name',
            index: 'Category',
            width: 150
        });

        columns.push({
            name: 'Tags',
            width: 150,
            editable: true,
            edittype: 'select',
            editoptions: {
                style: 'width:250px;',
                dataUrl: '/Admin/GetTagsHtml',
                multiple: true
            },
            editrules: {
                required: true
            }
        });


        columns.push({
            name: 'Author',
            width: 125,
            sortable: false,
            editable: true,
            editoptions: {
                size: 43,
                maxlength: 200
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'UrlSlug',
            width: 200,
            sortable: false,
            editable: true,
            editoptions: {
                size: 43,
                maxlength: 200
            },
            editrules: {
                custom: true,

                custom_func: function (val, colname) {

                    var pattern = /^[a-zA-Z]+$/;

                    if (val) {
                        if (val.match(pattern)) {
                            return [true, ""];
                        }
                        else {
                            return [false, "The " + colname + " can only contain alpha characters with no spaces."];
                        }
                    }
                    else
                        return [false, "The " + colname + " field is required"];
                }
            }
        });

        columns.push({
            name: 'Published',
            index: 'Published',
            width: 100,
            align: 'center',
            editable: true,
            edittype: 'checkbox',
            editoptions: {
                value: "true:false",
                defaultValue: 'false'
            }
        });

        columns.push({
            name: 'PostedOn',
            index: 'PostedOn',
            width: 150,
            align: 'center',
            sorttype: 'date',
            datefmt: 'm/d/Y'
        });

        columns.push({
            name: 'Modified',
            index: 'Modified',
            width: 150,
            align: 'center',
            sorttype: 'date',
            datefmt: 'm/d/Y'
        });

        // create the grid
        $(gridName).jqGrid({
            // server url and other ajax stuff 
            url: '/Admin/Posts',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',

            // columns
            colNames: colNames,
            colModel: columns,

            // pagination options
            toppager: true,
            pager: pagerName,
            rowNum: 10,
            rowList: [10, 20, 30],

            // row number column
            rownumbers: true,
            rownumWidth: 40,

            // default sorting
            sortname: 'PostedOn',
            sortorder: 'desc',

            // display the no. of records message
            viewrecords: true,

            jsonReader: { repeatitems: false }

        });

        // configuring add options
        var addOptions = {
            url: '/Admin/AddPost',
            addCaption: 'Add Post',
            processData: "Saving...",
            width: 900,
            closeAfterAdd: true,
            closeOnEscape: true,
            afterShowForm: afterShowForm,
            onClose: onClose,
            afterSubmit: ModestMethods.GridManager.afterSubmitHandler,
            beforeSubmit: beforeSubmitHandler,
        };

        var editOptions = {
            url: '/Admin/EditPost',
            editCaption: 'Edit Post',
            processData: "Saving...",
            width: 900,
            closeAfterEdit: true,
            closeOnEscape: true,
            afterclickPgButtons: afterclickPgButtons,
            afterShowForm: afterShowForm,
            onClose: onClose,
            afterSubmit: ModestMethods.GridManager.afterSubmitHandler,
            beforeSubmit: beforeSubmitHandler
        };

        var deleteOptions = {
            url: '/Admin/DeletePost',
            caption: 'Delete Post',
            processData: "Saving...",
            msg: "Delete the Post?",
            closeOnEscape: true,
            afterSubmit: ModestMethods.GridManager.afterSubmitHandler
        };

        $(gridName).navGrid(pagerName, { cloneToTop: true, search: false }, editOptions, addOptions, deleteOptions);


        /* * * Categories Grid * * */
        ModestMethods.GridManager.categoriesGrid = function (gridName, pagerName) {
            var colNames = ['CategoryId', 'Name', 'Url Slug', 'Description'];

            var columns = [];

            // Category columns
            columns.push({
                name: 'CategoryId',
                editable: true,
                hidden: true,
                key: true
            });

            columns.push({
                name: 'Name',
                index: 'Name',
                width: 200,
                editable: true,
                edittype: 'text',
                editoptions: {
                    size: 30,
                    maxlength: 50
                },
                editrules: {
                    required: true
                }
            });

            columns.push({
                name: 'UrlSlug',
                index: 'UrlSlug',
                width: 200,
                editable: true,
                edittype: 'text',
                sortable: false,
                editoptions: {
                    size: 30,
                    maxlength: 50
                },
                editrules: {
                    custom: true,

                    custom_func: function (val, colname) {

                        var pattern = /^[a-zA-Z]+$/;

                        if (val) {
                            if (val.match(pattern)) {
                                return [true, ""];
                            }
                            else {
                                return [false, "The " + colname + " can only contain alpha characters with no spaces."];
                            }
                        }
                        else
                            return [false, "The " + colname + " field is required"];
                    }
                }
            });

            columns.push({
                name: 'Description',
                index: 'Description',
                width: 200,
                editable: true,
                edittype: 'textarea',
                sortable: false,
                editoptions: {
                    rows: "4",
                    cols: "28"
                }
            });

            $(gridName).jqGrid({
                url: '/Admin/Categories',
                datatype: 'json',
                mtype: 'GET',
                height: 'auto',
                toppager: true,
                colNames: colNames,
                colModel: columns,
                pager: pagerName,
                rownumbers: true,
                rownumWidth: 40,
                rowNum: 500,
                sortname: 'Name',
                loadonce: true,
                jsonReader: {
                    repeatitems: false
                }
            });

            var addOptions = {
                url: '/Admin/AddCategory',
                width: 400,
                addCaption: 'Add Category',
                processData: "Saving...",
                closeAfterAdd: true,
                closeOnEscape: true,
                afterSubmit: function (response, postdata) {
                    var json = $.parseJSON(response.responseText);

                    if (json) {
                        // since the data is in the client-side, reload the grid.
                        $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                        return [json.success, json.message, json.id];
                    }

                    return [false, "Failed to get result from server.", null];
                }
            };

            var editOptions = {
                url: '/Admin/EditCategory',
                width: 400,
                editCaption: 'Edit Category',
                processData: "Saving...",
                closeAfterEdit: true,
                closeOnEscape: true,
                afterSubmit: function (response, postdata) {
                    var json = $.parseJSON(response.responseText);

                    if (json) {
                        $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                        return [json.success, json.message, json.id];
                    }

                    return [false, "Failed to get result from server.", null];
                }
            };

            var deleteOptions = {
                url: '/Admin/DeleteCategory',
                caption: 'Delete Category',
                processData: "Saving...",
                width: 500,
                msg: "Delete the category? This will delete all the posts belongs to this category as well.",
                closeOnEscape: true,
                afterSubmit: ModestMethods.GridManager.afterSubmitHandler
            };

            // configuring the navigation toolbar.
            $(gridName).jqGrid('navGrid', pagerName,
            {
                cloneToTop: true,
                search: false
            },
            editOptions, addOptions, deleteOptions);
        };
    };

    /* * * Tags Grid * * */
    ModestMethods.GridManager.tagsGrid = function (gridName, pagerName) {
        var colNames = ['TagId', 'Name', 'Url Slug', 'Description'];

        var columns = [];

        columns.push({
            name: 'TagId',
            editable: true,
            hidden: true,
            key: true
        });

        columns.push({
            name: 'Name',
            index: 'Name',
            width: 200,
            editable: true,
            edittype: 'text',
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'UrlSlug',
            index: 'UrlSlug',
            width: 200,
            editable: true,
            edittype: 'text',
            sortable: false,
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'Description',
            index: 'Description',
            width: 200,
            editable: true,
            edittype: 'textarea',
            sortable: false,
            editoptions: {
                rows: "4",
                cols: "28"
            }
        });

        $(gridName).jqGrid({
            url: '/Admin/Tags',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',
            toppager: true,
            colNames: colNames,
            colModel: columns,
            pager: pagerName,
            rownumbers: true,
            rownumWidth: 40,
            rowNum: 500,
            sortname: 'Name',
            loadonce: true,
            jsonReader: {
                repeatitems: false
            }
        });

        var addOptions = {
            url: '/Admin/AddTag',
            width: 400,
            addCaption: 'Add Tag',
            processData: "Saving...",
            closeAfterAdd: true,
            closeOnEscape: true,
            afterSubmit: function (response, postdata) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    // since the data is in the client-side, reload the grid.
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Failed to get result from server.", null];
            }
        };

        var editOptions = {
            url: '/Admin/EditTag',
            width: 400,
            editCaption: 'Edit Tag',
            processData: "Saving...",
            closeAfterEdit: true,
            closeOnEscape: true,
            afterSubmit: function (response, postdata) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Failed to get result from server.", null];
            }
        };

        var deleteOptions = {
            url: '/Admin/DeleteTag',
            caption: 'Delete Tag',
            processData: "Saving...",
            width: 500,
            msg: "Delete the tag? This will delete the connection to all the posts that belongs to this tag as well.",
            closeOnEscape: true,
            afterSubmit: ModestMethods.GridManager.afterSubmitHandler
        };

        $(gridName).jqGrid('navGrid', pagerName,
        {
            cloneToTop: true,
            search: false
        },

        editOptions, addOptions, deleteOptions);
    };

    ModestMethods.GridManager.afterSubmitHandler = function (response, postdata) {

        var json = $.parseJSON(response.responseText);

        if (json) return [json.success, json.message, json.id];

        return [false, "Failed to get result from server.", null];
    }

    $("#tabs").tabs({
        show: function (event, ui) {

            if (!ui.tab.isLoaded) {

                var gdMgr = ModestMethods.GridManager,
                      fn, gridName, pagerName;

                switch (ui.index) {
                    case 0:
                        fn = gdMgr.postsGrid;
                        gridName = "#tablePosts";
                        pagerName = "#pagerPosts";
                        break;
                    case 1:
                        fn = gdMgr.categoriesGrid;
                        gridName = "#tableCats";
                        pagerName = "#pagerCats";
                        break;
                    case 2:
                        fn = gdMgr.tagsGrid;
                        gridName = "#tableTags";
                        pagerName = "#pagerTags";
                        break;
                };

                fn(gridName, pagerName);
                ui.tab.isLoaded = true;
            }
        }
    });
});