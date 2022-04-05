"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
var reactstrap_1 = require("reactstrap");
var CanvasStore = require("../../../store/Canvas");
var models_1 = require("../../../models");
var uuid_1 = require("uuid");
var PalettePane = /** @class */ (function (_super) {
    __extends(PalettePane, _super);
    function PalettePane() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PalettePane.prototype.onFilterChanged = function (e) {
        this.props.filterTemplates(e.target.value);
    };
    PalettePane.prototype.onTemplateClicked = function (template) {
        console.log(template.canvasTemplate);
        var newShape = __assign(__assign({}, template.canvasTemplate), { name: template.name, properties: template.canvasTemplate.properties.map(function (p) { return (__assign({}, p)); }), id: uuid_1.v4(), templateId: template.id, type: models_1.ICanvasElementType.Shape, width: template.canvasTemplate.width, height: template.canvasTemplate.height, shape: template.canvasTemplate.shape, position: { x: 500, y: 500 }, canHaveChildren: template.canvasTemplate.isContainer, elements: template.canvasTemplate.elements || [] });
        this.props.addElement(newShape);
    };
    PalettePane.prototype.onTemplateDragStarted = function (template) {
        this.props.dragTemplate(template);
    };
    PalettePane.prototype.onTemplateDragEnded = function (template) {
    };
    PalettePane.prototype.render = function () {
        var _this = this;
        return (React.createElement(React.Fragment, null,
            React.createElement(reactstrap_1.Input, { className: "palette-searchbox", type: "text", placeholder: "Search components...", onChange: this.onFilterChanged.bind(this) }),
            this.props.templateGroups ? this.props.templateGroups.map(function (group) {
                return React.createElement(React.Fragment, null,
                    React.createElement("p", { className: "palette-group-header" }, group.name),
                    group.items.map(function (item) {
                        return React.createElement("p", { className: "palette-group-item", onClick: function () { return _this.onTemplateClicked(item); }, draggable: true, onDragStart: function (e) { return _this.onTemplateDragStarted(item); }, onDragEnd: function (e) { return _this.onTemplateDragEnded(item); } }, item.name);
                    }));
            }) : null));
    };
    return PalettePane;
}(React.PureComponent));
;
exports.default = react_redux_1.connect(function (state) { return state.canvas; }, CanvasStore.actionCreators)(PalettePane);
//# sourceMappingURL=PalettePane.js.map