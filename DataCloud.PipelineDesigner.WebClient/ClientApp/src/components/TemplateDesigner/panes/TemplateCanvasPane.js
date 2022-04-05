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
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
var CanvasStore = require("../../../store/Canvas");
var react_konva_1 = require("react-konva");
var models_1 = require("../../../models");
var CanvasRenderer_1 = require("../../../services/CanvasRenderer");
var TemplateCanvasPane = /** @class */ (function (_super) {
    __extends(TemplateCanvasPane, _super);
    function TemplateCanvasPane() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplateCanvasPane.prototype.onKeyDown = function (e) {
        if (e.keyCode === 46 && this.props.selectedElement) {
            this.props.removeElement(this.props.selectedElement.id);
        }
    };
    TemplateCanvasPane.prototype.onShapeClick = function (e, shape) {
        e.cancelBubble = true;
        if (!this.props.selectedElement || this.props.selectedElement.id !== shape.id) {
            this.props.selectElement(shape);
        }
        else
            this.props.deselectElement();
    };
    TemplateCanvasPane.prototype.onConnectorClick = function (e, connector) {
        e.cancelBubble = true;
        if (!this.props.selectedElement || this.props.selectedElement.id !== connector.id) {
            this.props.selectElement(connector);
        }
        else
            this.props.deselectElement();
    };
    TemplateCanvasPane.prototype.onConnectionPointClick = function (e, shape, point) {
        e.cancelBubble = true;
        this.props.selectConnectionPoint(shape, point);
    };
    TemplateCanvasPane.prototype.renderTemplate = function (template) {
        if (!template)
            return null;
        var shape = {
            id: '',
            name: template.name,
            type: models_1.ICanvasElementType.Shape,
            connectionPoints: template.canvasTemplate.connectionPoints,
            properties: template.canvasTemplate.properties,
            width: template.canvasTemplate.width,
            height: template.canvasTemplate.height,
            shape: template.canvasTemplate.shape,
            position: { x: 700, y: 500 }
        };
        return this.renderCanvasShape(shape);
    };
    TemplateCanvasPane.prototype.renderCanvasShape = function (shape) {
        var _this = this;
        var isSelectedShape = false;
        if (this.props.selectedElement && this.props.selectedElement.id === shape.id) {
            isSelectedShape = true;
        }
        return React.createElement(react_konva_1.Group, { x: shape.position.x, y: shape.position.y, onClick: function (e) { return _this.onShapeClick(e, shape); } },
            CanvasRenderer_1.CanvasRenderer.renderShapeComponent(shape, isSelectedShape, function () { }),
            CanvasRenderer_1.CanvasRenderer.renderShapeName(shape),
            shape.connectionPoints.map(function (p) {
                var isSelectedConnectionPoint = false;
                if (isSelectedShape && _this.props.selectedConnectionPoint && _this.props.selectedConnectionPoint.id === p.id) {
                    isSelectedConnectionPoint = true;
                }
                return CanvasRenderer_1.CanvasRenderer.renderConnectionPoint(p, shape, isSelectedConnectionPoint, function (e, shape, point) { return _this.onConnectionPointClick(e, shape, point); });
            }));
    };
    TemplateCanvasPane.prototype.render = function () {
        var _this = this;
        return (React.createElement(React.Fragment, null,
            React.createElement("div", { id: "canvas-container", className: "canvas-container", tabIndex: 1, onKeyDown: function (e) { return _this.onKeyDown(e); }, onDragOver: function (e) { return e.preventDefault(); } },
                React.createElement(react_konva_1.Stage, { width: window.innerWidth, height: window.innerHeight, onClick: function (e) { return _this.props.deselectElement(); } },
                    React.createElement(react_konva_1.Layer, null, CanvasRenderer_1.CanvasRenderer.renderGrid()),
                    React.createElement(react_konva_1.Layer, null, this.renderTemplate(this.props.selectedTemplate))))));
    };
    return TemplateCanvasPane;
}(React.PureComponent));
;
exports.default = react_redux_1.connect(function (state) { return state.canvas; }, CanvasStore.actionCreators)(TemplateCanvasPane);
//# sourceMappingURL=TemplateCanvasPane.js.map