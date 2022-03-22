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
var models_1 = require("../../../models");
var CanvasStore = require("../../../store/Canvas");
var PropertyPane = /** @class */ (function (_super) {
    __extends(PropertyPane, _super);
    function PropertyPane() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PropertyPane.prototype.onPropertyChange = function (e, prop) {
        var updatedElement = __assign({}, this.props.selectedElement);
        updatedElement.properties.filter(function (x) { return x.name === prop.name; })[0].value = e.target.value;
        this.props.updateElement(updatedElement);
    };
    PropertyPane.prototype.renderProperty = function (prop) {
        var _this = this;
        if (!this.props.selectedElement)
            return null;
        var propUniqueId = this.props.selectedElement.id + "-" + prop.name;
        switch (prop.type) {
            case models_1.ICanvasElementPropertyType.singleLineText:
                return React.createElement(reactstrap_1.FormGroup, null,
                    React.createElement(reactstrap_1.Label, { for: propUniqueId }, prop.name),
                    React.createElement(reactstrap_1.Input, { type: "text", name: propUniqueId, id: propUniqueId, value: prop.value, onChange: function (e) { return _this.onPropertyChange(e, prop); } }));
            case models_1.ICanvasElementPropertyType.multiLineText:
                return React.createElement(reactstrap_1.FormGroup, null,
                    React.createElement(reactstrap_1.Label, { for: propUniqueId }, prop.name),
                    React.createElement(reactstrap_1.Input, { type: "textarea", name: propUniqueId, id: propUniqueId, value: prop.value, onChange: function (e) { return _this.onPropertyChange(e, prop); }, size: 7 }));
            case models_1.ICanvasElementPropertyType.select:
                return React.createElement(reactstrap_1.FormGroup, null,
                    React.createElement(reactstrap_1.Label, { for: propUniqueId }, prop.name),
                    React.createElement(reactstrap_1.Input, { type: "select", name: propUniqueId, id: propUniqueId, value: prop.value, onChange: function (e) { return _this.onPropertyChange(e, prop); } }, prop.options ? prop.options.map(function (op) { return React.createElement("option", null, op); }) : null));
        }
    };
    PropertyPane.prototype.render = function () {
        var _this = this;
        var selectedShape = this.props.selectedElement && this.props.selectedElement.type === models_1.ICanvasElementType.Shape ? this.props.selectedElement : null;
        return (React.createElement(React.Fragment, null, selectedShape ?
            React.createElement(React.Fragment, null,
                React.createElement("h3", { className: "property-pane-header" }, selectedShape.name),
                React.createElement("p", { className: "property-pane-subheader" },
                    "ID: ",
                    selectedShape.id),
                React.createElement(reactstrap_1.Form, null, selectedShape.properties.filter(function (p) { return p.allowEditing; }).map(function (prop) { return _this.renderProperty(prop); })))
            : null));
    };
    return PropertyPane;
}(React.PureComponent));
;
exports.default = (0, react_redux_1.connect)(function (state) { return state.canvas; }, CanvasStore.actionCreators)(PropertyPane);
//# sourceMappingURL=PropertyPane.js.map