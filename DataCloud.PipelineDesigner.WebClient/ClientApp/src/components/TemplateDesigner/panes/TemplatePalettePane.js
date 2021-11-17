"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
var reactstrap_1 = require("reactstrap");
var CanvasStore = require("../../../store/Canvas");
var uuid_1 = require("uuid");
var TemplatePalettePane = /** @class */ (function (_super) {
    __extends(TemplatePalettePane, _super);
    function TemplatePalettePane() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplatePalettePane.prototype.onFilterChanged = function (e) {
        this.props.filterTemplates(e.target.value);
    };
    TemplatePalettePane.prototype.onTemplateClicked = function (template) {
        this.props.selectTemplate(template);
    };
    TemplatePalettePane.prototype.onAddNewTemplate = function (group) {
        var template = {
            id: uuid_1.v4(),
            name: "New template",
            description: "",
            width: 300,
            height: 200,
            category: group,
            shape: "Rectangle",
            properties: [],
            connectionPoints: []
        };
        this.props.addTemplate(template);
    };
    TemplatePalettePane.prototype.render = function () {
        var _this = this;
        return (React.createElement(React.Fragment, null,
            React.createElement(reactstrap_1.Input, { className: "palette-searchbox", type: "text", placeholder: "Search components...", onChange: this.onFilterChanged.bind(this) }),
            this.props.templateGroups ? this.props.templateGroups.map(function (group) {
                return React.createElement(React.Fragment, null,
                    React.createElement("p", { className: "palette-group-header" },
                        group.name,
                        " ",
                        React.createElement(reactstrap_1.Button, { className: "addButton", onClick: function (e) { return _this.onAddNewTemplate(group.name); } }, "Add")),
                    group.items.map(function (item) {
                        return React.createElement("p", { className: "palette-group-item", onClick: function () { return _this.onTemplateClicked(item); } }, item.name);
                    }));
            }) : null));
    };
    return TemplatePalettePane;
}(React.PureComponent));
;
exports.default = react_redux_1.connect(function (state) { return state.canvas; }, CanvasStore.actionCreators)(TemplatePalettePane);
//# sourceMappingURL=TemplatePalettePane.js.map