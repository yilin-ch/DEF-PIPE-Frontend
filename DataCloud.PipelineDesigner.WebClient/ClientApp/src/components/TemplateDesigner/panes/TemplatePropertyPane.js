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
var uuid_1 = require("uuid");
var TemplatePropertyPane = /** @class */ (function (_super) {
    __extends(TemplatePropertyPane, _super);
    function TemplatePropertyPane() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplatePropertyPane.prototype.toggle = function (tab) {
        if (this.props.selectedTab !== tab) {
            this.props.selectTab(tab);
        }
    };
    TemplatePropertyPane.prototype.onNameChange = function (e) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.name = e.target.value;
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onCategoryChange = function (e) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.category = e.target.value;
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onWidthChange = function (e) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.width = parseInt(e.target.value);
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onHeightChange = function (e) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.height = parseInt(e.target.value);
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onShapeChange = function (e) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.shape = e.target.value;
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onPropertyNameChange = function (e, prop) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.properties.filter(function (x) { return x.name === prop.name; })[0].name = e.target.value;
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onPropertyTypeChange = function (e, prop) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.properties.filter(function (x) { return x.name === prop.name; })[0].type = parseInt(e.target.value);
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onPropertyValueChange = function (e, prop) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.properties.filter(function (x) { return x.name === prop.name; })[0].value = e.target.value;
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onPropertAllowEditChange = function (e, prop) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.properties.filter(function (x) { return x.name === prop.name; })[0].allowEditing = e.target.checked;
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onConnectionPointChangeX = function (e, point) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        var pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(function (x) { return x.id === point.id; })[0];
        pointToBeUpdated.position.x = parseInt(e.target.value);
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onConnectionPointChangeY = function (e, point) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        var pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(function (x) { return x.id === point.id; })[0];
        pointToBeUpdated.position.y = parseInt(e.target.value);
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.onConnectionPointChangeType = function (e, point) {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        var pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(function (x) { return x.id === point.id; })[0];
        pointToBeUpdated.type = parseInt(e.target.value);
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.addProperty = function () {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.properties.push({ name: 'New property', type: models_1.ICanvasElementPropertyType.singleLineText });
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.addConnectionPoint = function () {
        var updatedTemplate = __assign({}, this.props.selectedTemplate);
        updatedTemplate.canvasTemplate.connectionPoints.push({
            id: (0, uuid_1.v4)(),
            type: models_1.ICanvasConnectionPointType.input,
            position: {
                x: 10,
                y: 10
            }
        });
        this.props.updateTemplate(updatedTemplate);
    };
    TemplatePropertyPane.prototype.renderProperty = function (prop) {
        var _this = this;
        if (!this.props.selectedTemplate)
            return null;
        var propNameUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_name';
        var propTypeUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_type';
        var propValueUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_value';
        var propAllowEditUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_allowedit';
        return React.createElement(reactstrap_1.FormGroup, null,
            React.createElement(reactstrap_1.Label, { for: propNameUniqueId }, "Property Name"),
            React.createElement(reactstrap_1.Input, { type: "text", name: propNameUniqueId, id: propNameUniqueId, value: prop.name, onChange: function (e) { return _this.onPropertyNameChange(e, prop); } }),
            React.createElement(reactstrap_1.Label, { for: propTypeUniqueId }, "Property Type"),
            React.createElement(reactstrap_1.Input, { type: "select", name: propTypeUniqueId, id: propTypeUniqueId, value: prop.type, onChange: function (e) { return _this.onPropertyTypeChange(e, prop); } },
                React.createElement("option", { value: "0" }, "Single line of text"),
                React.createElement("option", { value: "1" }, "Multiple lines of text"),
                React.createElement("option", { value: "2" }, "Options")),
            React.createElement(reactstrap_1.Label, { for: propValueUniqueId }, "Property Value"),
            React.createElement(reactstrap_1.Input, { type: "text", name: propValueUniqueId, id: propValueUniqueId, value: prop.value, onChange: function (e) { return _this.onPropertyValueChange(e, prop); } }),
            React.createElement(reactstrap_1.Label, { for: propAllowEditUniqueId, className: "checkbox-label" },
                React.createElement(reactstrap_1.Input, { type: "checkbox", name: propAllowEditUniqueId, id: propAllowEditUniqueId, checked: prop.allowEditing, onChange: function (e) { return _this.onPropertAllowEditChange(e, prop); } }),
                " ",
                ' ',
                "Editable"));
    };
    TemplatePropertyPane.prototype.renderConnectionPoint = function (point) {
        var _this = this;
        return React.createElement(React.Fragment, null,
            React.createElement("h5", null, point.id),
            React.createElement(reactstrap_1.FormGroup, null,
                React.createElement(reactstrap_1.Label, { for: point.id + '_x' }, "Position X"),
                React.createElement(reactstrap_1.Input, { type: "number", name: point.id + '_x', id: point.id + '_x', value: point.position.x, onChange: function (e) { return _this.onConnectionPointChangeX(e, point); } }),
                React.createElement(reactstrap_1.Label, { for: point.id + '_y' }, "Position Y"),
                React.createElement(reactstrap_1.Input, { type: "number", name: point.id + '_x', id: point.id + '_x', value: point.position.y, onChange: function (e) { return _this.onConnectionPointChangeY(e, point); } }),
                React.createElement(reactstrap_1.Label, { for: point.id + '_type' }, "Type"),
                React.createElement(reactstrap_1.Input, { type: "select", name: point.id + '_type', id: point.id + '_type', value: point.type, onChange: function (e) { return _this.onConnectionPointChangeType(e, point); } },
                    React.createElement("option", { value: "0" }, "Input"),
                    React.createElement("option", { value: "1" }, "Output"))));
    };
    TemplatePropertyPane.prototype.render = function () {
        var _this = this;
        return (React.createElement(React.Fragment, null, this.props.selectedTemplate ?
            React.createElement(React.Fragment, null,
                React.createElement(reactstrap_1.Form, null,
                    React.createElement("h3", { className: "property-pane-header" }, "Template Properties"),
                    React.createElement("p", { className: "property-pane-subheader" },
                        "ID: ",
                        this.props.selectedTemplate.id),
                    React.createElement(reactstrap_1.Nav, { tabs: true },
                        React.createElement(reactstrap_1.NavItem, null,
                            React.createElement(reactstrap_1.NavLink, { className: this.props.selectedTab === '1' ? 'active' : '', onClick: function () { _this.toggle('1'); } }, "Information")),
                        React.createElement(reactstrap_1.NavItem, null,
                            React.createElement(reactstrap_1.NavLink, { className: this.props.selectedTab === '2' ? 'active' : '', onClick: function () { _this.toggle('2'); } }, "Parameters")),
                        React.createElement(reactstrap_1.NavItem, null,
                            React.createElement(reactstrap_1.NavLink, { className: this.props.selectedTab === '3' ? 'active' : '', onClick: function () { _this.toggle('3'); } }, "Connections"))),
                    React.createElement(reactstrap_1.TabContent, { activeTab: this.props.selectedTab },
                        React.createElement(reactstrap_1.TabPane, { tabId: "1" },
                            React.createElement(reactstrap_1.FormGroup, null,
                                React.createElement(reactstrap_1.Label, { for: this.props.selectedTemplate.id + '_name' }, "Name"),
                                React.createElement(reactstrap_1.Input, { type: "text", name: this.props.selectedTemplate.id + '_name', id: this.props.selectedTemplate.id + '_name', value: this.props.selectedTemplate.name, onChange: function (e) { return _this.onNameChange(e); } }),
                                React.createElement(reactstrap_1.Label, { for: this.props.selectedTemplate.id + '_type' }, "Category"),
                                React.createElement(reactstrap_1.Input, { type: "text", name: this.props.selectedTemplate.id + '_type', id: this.props.selectedTemplate.id + '_type', value: this.props.selectedTemplate.category, onChange: function (e) { return _this.onCategoryChange(e); } }),
                                React.createElement(reactstrap_1.Label, { for: this.props.selectedTemplate.id + '_shape' }, "Shape"),
                                React.createElement(reactstrap_1.Input, { type: "select", name: this.props.selectedTemplate.id + '_shape', id: this.props.selectedTemplate.id + '_shape', value: this.props.selectedTemplate.canvasTemplate.shape, onChange: function (e) { return _this.onShapeChange(e); } },
                                    React.createElement("option", null, "Rectangle"),
                                    React.createElement("option", null, "Ellipse"),
                                    React.createElement("option", null, "Diamond"),
                                    React.createElement("option", null, "Database"),
                                    React.createElement("option", null, "Custom")),
                                React.createElement(reactstrap_1.Label, { for: this.props.selectedTemplate.id + '_width' }, "Width"),
                                React.createElement(reactstrap_1.Input, { type: "number", name: this.props.selectedTemplate.id + '_width', id: this.props.selectedTemplate.id + '_width', value: this.props.selectedTemplate.canvasTemplate.width, onChange: function (e) { return _this.onWidthChange(e); } }),
                                React.createElement(reactstrap_1.Label, { for: this.props.selectedTemplate.id + '_height' }, "Height"),
                                React.createElement(reactstrap_1.Input, { type: "number", name: this.props.selectedTemplate.id + '_height', id: this.props.selectedTemplate.id + '_height', value: this.props.selectedTemplate.canvasTemplate.height, onChange: function (e) { return _this.onHeightChange(e); } }))),
                        React.createElement(reactstrap_1.TabPane, { tabId: "2" },
                            this.props.selectedTemplate.canvasTemplate.properties.map(function (prop) { return _this.renderProperty(prop); }),
                            React.createElement(reactstrap_1.Button, { onClick: function (e) { return _this.addProperty(); } }, "Add Property")),
                        React.createElement(reactstrap_1.TabPane, { tabId: "3" },
                            this.props.selectedTemplate.canvasTemplate.connectionPoints.map(function (point) { return _this.renderConnectionPoint(point); }),
                            React.createElement(reactstrap_1.Button, { onClick: function (e) { return _this.addConnectionPoint(); } }, "Add Connection Point"))),
                    React.createElement("td", null,
                        React.createElement("p", { className: "btn btn-success saveButton", onClick: function (e) {
                                _this.props.saveTemplate(_this.props.selectedTemplate);
                            } }, "Save")),
                    React.createElement("td", null,
                        React.createElement("p", { className: "btn btn-danger removeButton", onClick: function (e) {
                                _this.props.removeTemplate(_this.props.selectedTemplate);
                            } }, "Delete"))))
            : null));
    };
    return TemplatePropertyPane;
}(React.PureComponent));
;
exports.default = (0, react_redux_1.connect)(function (state) { return state.canvas; }, CanvasStore.actionCreators)(TemplatePropertyPane);
//# sourceMappingURL=TemplatePropertyPane.js.map