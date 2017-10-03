(function(){
    var overrideCtx = {};
    overrideCtx.Templates = {};
    overrideCtx.Templates.Fields = {  'Description': { 'View' : '<i><#=ctx.CurrentItem.Description#></i>'}};
    SPClientTemplates.TemplateManager.RegisterTemplateOverrides(overrideCtx);
})()