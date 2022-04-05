"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var models_1 = require("../models");
var uuid_1 = require("uuid");
var TemplateService = /** @class */ (function () {
    function TemplateService() {
    }
    TemplateService.prototype.createNewRootShape = function () {
        var rootShape = {
            id: uuid_1.v4(),
            name: "Root",
            type: models_1.ICanvasElementType.Shape,
            canHaveChildren: true,
            elements: [],
            position: null,
            properties: [],
            connectionPoints: [],
            width: 0,
            height: 0
        };
        return rootShape;
    };
    TemplateService.prototype.getTemplateGroups = function (templates, filterText) {
        filterText = (filterText || '').toLowerCase();
        templates = templates.filter(function (template) { return !filterText || template.name.toLowerCase().indexOf(filterText) > -1; });
        var categories = templates
            .map(function (x) { return x.category; })
            .filter(function (val, index, self) { return self.indexOf(val) === index; }); // get unique values
        var templateGroups = categories.map(function (x) {
            return { name: x, items: templates.filter(function (template) { return template.category === x; }) };
        });
        return templateGroups;
    };
    TemplateService.prototype.saveTemplate = function (template) {
        console.log(template);
        TemplateService.saveTemplateTimeoutHandle = setTimeout(function () {
            TemplateService.saveTemplateTimeoutHandle = null;
            fetch("/api/templates", {
                method: "POST",
                body: JSON.stringify(template),
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        }, 500);
    };
    TemplateService.prototype.deleteTemplate = function (templateId) {
        TemplateService.saveTemplateTimeoutHandle = setTimeout(function () {
            TemplateService.saveTemplateTimeoutHandle = null;
            fetch("/api/templates/" + templateId, {
                method: "DELETE",
            });
        }, 500);
    };
    TemplateService.prototype.persistTemplate = function (templates) {
        // localStorage.setItem("datacloud-templates", JSON.stringify(templates));
    };
    TemplateService.saveTemplateTimeoutHandle = null;
    return TemplateService;
}());
exports.TemplateService = TemplateService;
//# sourceMappingURL=TemplateService.js.map