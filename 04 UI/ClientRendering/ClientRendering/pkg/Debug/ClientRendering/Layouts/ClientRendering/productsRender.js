(function () {

    var renderer = {};
    renderer.Templates = {};

    renderer.bindDropDown = function (ctx) {
        var formCtx = SPClientTemplates.Utility.GetFormContextForCurrentField(ctx);
        var fld = formCtx.fieldName;
        formCtx.registerGetValueCallback(fld, renderer.getFieldValue.bind(null, fld));

        var items = new Array("Category 1", "Category 2", "Category 3");
        var returnHtml = "<div><input id='input" + fld + "' type='text' list='lst" + fld + "' style='width:386px'><datalist id='lst" + fld + "'>";

        for (var i = 0; i < items.length; i++) {
            returnHtml += "<option";
            returnHtml += ">" + items[i] + "</option>";
        }

        returnHtml += "</datalist></div>";
        return returnHtml;
    };

    renderer.getFieldValue = function (fieldName) {
        var result = $("#input" + fieldName).val();
        return result;
    };

    renderer.Templates.Fields = {
        'Category': { 'NewForm': renderer.bindDropDown }
    };

    SPClientTemplates.TemplateManager.RegisterTemplateOverrides(renderer);
})();