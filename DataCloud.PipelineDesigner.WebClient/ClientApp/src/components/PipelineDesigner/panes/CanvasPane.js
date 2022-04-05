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
var CanvasStore = require("../../../store/Canvas");
var react_konva_1 = require("react-konva");
var models_1 = require("../../../models");
var CanvasService_1 = require("../../../services/CanvasService");
var CanvasRenderer_1 = require("../../../services/CanvasRenderer");
var reactstrap_1 = require("reactstrap");
var uuid_1 = require("uuid");
var CanvasPane = /** @class */ (function (_super) {
    __extends(CanvasPane, _super);
    function CanvasPane() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.canvasService = new CanvasService_1.CanvasService();
        _this.stageStyle = {
            cursor: 'default'
        };
        _this.saveAsTemplateModal = false;
        _this.exportDSLModal = false;
        _this.saveAsTemplateName = "";
        _this.saveAsTemplateDescription = "";
        _this.saveAsTemplateCategory = "";
        _this.selectedDSLToExport = "";
        return _this;
    }
    CanvasPane.prototype.toggleSaveAsTemplateModal = function () {
        this.saveAsTemplateModal = !this.saveAsTemplateModal;
    };
    CanvasPane.prototype.toggleExportDSLModal = function () {
        this.exportDSLModal = !this.exportDSLModal;
    };
    CanvasPane.prototype.saveAsTemplate = function () {
        this.props.addTemplate({
            id: uuid_1.v4(),
            name: this.saveAsTemplateName,
            description: "",
            category: this.saveAsTemplateCategory,
            canvasTemplate: {
                shape: "Container",
                isContainer: true,
                elements: (this.props.shapeExpandStack[0] || this.props.currentRootShape).elements,
                connectionPoints: [{
                        id: '1',
                        position: { x: 0, y: 50 },
                        type: models_1.ICanvasConnectionPointType.input
                    },
                    {
                        id: '2',
                        position: { x: 200, y: 50 },
                        type: models_1.ICanvasConnectionPointType.output
                    }],
                properties: [],
                width: 200,
                height: 100
            }
        });
        this.saveAsTemplateName = "";
        this.saveAsTemplateDescription = "";
        this.saveAsTemplateCategory = "";
        this.saveAsTemplateModal = false;
    };
    CanvasPane.prototype.onKeyDown = function (e) {
        if (e.keyCode === 46 && this.props.selectedElement) {
            this.props.removeElement(this.props.selectedElement.id);
        }
    };
    CanvasPane.prototype.onShapeClick = function (e, shape) {
        e.cancelBubble = true;
        if (!this.props.selectedElement || this.props.selectedElement.id !== shape.id) {
            this.props.selectElement(shape);
        }
        else
            this.props.deselectElement();
    };
    CanvasPane.prototype.onConnectorClick = function (e, connector) {
        e.cancelBubble = true;
        if (!this.props.selectedElement || this.props.selectedElement.id !== connector.id) {
            this.props.selectElement(connector);
        }
        else
            this.props.deselectElement();
    };
    CanvasPane.prototype.onConnectionPointClick = function (e, shape, point) {
        e.cancelBubble = true;
        this.props.selectConnectionPoint(shape, point);
    };
    CanvasPane.prototype.onShapeDragEnd = function (e, shape) {
        var newPosition = this.canvasService.snapToGrid(e.target.position());
        e.target.setPosition(newPosition);
        var newShape = __assign(__assign({}, shape), { position: newPosition });
        this.props.updateElement(newShape);
    };
    CanvasPane.prototype.onShapeDragMove = function (e, shape) {
        var newPosition = this.canvasService.snapToGrid(e.target.position());
        e.target.setPosition(newPosition);
        var newShape = __assign(__assign({}, shape), { position: newPosition });
        this.props.updateElement(newShape);
    };
    CanvasPane.prototype.onContainerExpand = function (e, shape) {
        e.cancelBubble = true;
        this.props.expandContainer(shape);
    };
    CanvasPane.prototype.onCollapseContainer = function (shape) {
        this.props.collapseContainer(shape);
    };
    CanvasPane.prototype.onMouseMove = function (e) {
        var canvasContainer = document.getElementById('canvas-container').getBoundingClientRect();
        this.props.updateMousePosition({ x: e.clientX - canvasContainer.left, y: e.clientY - canvasContainer.top });
    };
    CanvasPane.prototype.exportCanvasAsJson = function () {
        this.canvasService.exportAsJson(this.props.currentRootShape);
    };
    CanvasPane.prototype.exportCanvasAsDSL = function () {
        this.canvasService.exportAsDSL(this.props.currentRootShape);
    };
    CanvasPane.prototype.onDrop = function (e) {
        e.preventDefault();
        if (this.props.draggedTemplate) {
            this.onTemplateDrop(e);
        }
        else if (e.dataTransfer.files.length > 0) {
            this.onJsonDrop(e);
        }
    };
    CanvasPane.prototype.onTemplateDrop = function (e) {
        var template = this.props.draggedTemplate;
        var canvasContainer = document.getElementById('canvas-container');
        var dropPosition = this.canvasService.snapToGrid({
            x: e.clientX - canvasContainer.getBoundingClientRect().left,
            y: e.clientY - canvasContainer.getBoundingClientRect().top
        });
        var newShape = __assign(__assign({}, template.canvasTemplate), { properties: template.canvasTemplate.properties.map(function (p) { return (__assign({}, p)); }), id: uuid_1.v4(), type: models_1.ICanvasElementType.Shape, width: template.canvasTemplate.width, height: template.canvasTemplate.height, shape: template.canvasTemplate.shape, position: dropPosition });
        this.props.addElement(newShape);
        this.props.dropTemplate();
    };
    CanvasPane.prototype.onJsonDrop = function (e) {
        var _this = this;
        var file = e.dataTransfer.files[0];
        var reader = new FileReader();
        reader.onload = function (event) {
            if (event.target && event.target.result) {
                var json = event.target.result;
                var elements = JSON.parse(json);
                _this.props.importElements(elements);
            }
        };
        reader.readAsText(file);
    };
    CanvasPane.prototype.renderTemporaryConnector = function () {
        var _this = this;
        var points = [];
        if (this.props.selectedConnectionPoint) {
            this.props.currentRootShape.elements.forEach(function (ele) {
                if (ele.type === models_1.ICanvasElementType.Shape && ele.id != _this.props.selectedElement.id) {
                    var shape_1 = ele;
                    var point = shape_1.connectionPoints.filter(function (p) {
                        return p.type === models_1.ICanvasConnectionPointType.input &&
                            _this.canvasService.pointInCircle(shape_1.position.x + p.position.x, shape_1.position.y + p.position.y, _this.props.currentMousePosition.x, _this.props.currentMousePosition.y, 10);
                    })[0];
                    if (point) {
                        var connector = {
                            sourceConnectionPointId: _this.props.selectedConnectionPoint.id,
                            sourceShapeId: _this.props.selectedElement.id,
                            destConnectionPointId: point.id,
                            destShapeId: ele.id,
                            id: '',
                            type: models_1.ICanvasElementType.Connector
                        };
                        _this.canvasService.calculateConnectorPoints(connector, _this.props.selectedElement, shape_1).forEach(function (p) {
                            points.push(p.x);
                            points.push(p.y);
                        });
                    }
                }
            });
            if (points.length === 0) {
                var destShape = {
                    id: '',
                    name: '',
                    position: {
                        x: this.props.currentMousePosition.x - 10,
                        y: this.props.currentMousePosition.y
                    },
                    width: 20,
                    height: 10,
                    connectionPoints: [{ id: '0', position: { x: 10, y: 0 }, type: models_1.ICanvasConnectionPointType.input }],
                    type: models_1.ICanvasElementType.Shape,
                    properties: []
                };
                var connector = {
                    sourceConnectionPointId: this.props.selectedConnectionPoint.id,
                    sourceShapeId: this.props.selectedElement.id,
                    destConnectionPointId: '0',
                    destShapeId: destShape.id,
                    id: '',
                    type: models_1.ICanvasElementType.Connector
                };
                this.canvasService.calculateConnectorPoints(connector, this.props.selectedElement, destShape).forEach(function (p) {
                    points.push(p.x);
                    points.push(p.y);
                });
            }
        }
        if (points.length > 0) {
            return React.createElement(react_konva_1.Arrow, { points: points, dash: [10, 5], stroke: "black" });
        }
        else
            return null;
    };
    CanvasPane.prototype.renderCanvasElement = function (element) {
        if (element.type === models_1.ICanvasElementType.Shape) {
            return this.renderCanvasShape(element);
        }
        else {
            return this.renderCanvasConnector(element);
        }
    };
    CanvasPane.prototype.renderCanvasShape = function (shape) {
        var _this = this;
        var isSelectedShape = false;
        if (this.props.selectedElement && this.props.selectedElement.id === shape.id) {
            isSelectedShape = true;
        }
        return React.createElement(react_konva_1.Group, { x: shape.position.x, y: shape.position.y, draggable: true, onClick: function (e) { return _this.onShapeClick(e, shape); }, onDragMove: function (e) { return _this.onShapeDragMove(e, shape); }, onDragEnd: function (e) { return _this.onShapeDragEnd(e, shape); } },
            CanvasRenderer_1.CanvasRenderer.renderShapeComponent(shape, isSelectedShape, function (e, shape) { return _this.onContainerExpand(e, shape); }),
            CanvasRenderer_1.CanvasRenderer.renderShapeName(shape),
            shape.connectionPoints.map(function (p) {
                var isSelectedConnectionPoint = false;
                if (isSelectedShape && _this.props.selectedConnectionPoint && _this.props.selectedConnectionPoint.id === p.id) {
                    isSelectedConnectionPoint = true;
                }
                return CanvasRenderer_1.CanvasRenderer.renderConnectionPoint(p, shape, isSelectedConnectionPoint, function (e, shape, point) { return _this.onConnectionPointClick(e, shape, point); });
            }));
    };
    CanvasPane.prototype.renderCanvasConnector = function (connector) {
        var _this = this;
        var isSelectedConnector = false;
        if (this.props.selectedElement && this.props.selectedElement.id === connector.id) {
            isSelectedConnector = true;
        }
        var srcShape = this.props.currentRootShape.elements.filter(function (x) { return x.id === connector.sourceShapeId; })[0];
        var destShape = this.props.currentRootShape.elements.filter(function (x) { return x.id === connector.destShapeId; })[0];
        var points = [];
        this.canvasService.calculateConnectorPoints(connector, srcShape, destShape).forEach(function (p) {
            points.push(p.x);
            points.push(p.y);
        });
        return React.createElement(react_konva_1.Arrow, { points: points, stroke: isSelectedConnector ? "blue" : "black", onClick: function (e) { return _this.onConnectorClick(e, connector); } });
    };
    CanvasPane.prototype.render = function () {
        var _this = this;
        this.props.requestDSLs();
        this.props.requestTemplates();
        return (React.createElement(React.Fragment, null,
            React.createElement("div", { id: "canvas-container", className: "canvas-container", tabIndex: 1, onKeyDown: function (e) { return _this.onKeyDown(e); }, onDrop: function (e) { return _this.onDrop(e); }, onDragOver: function (e) { return e.preventDefault(); }, onMouseMove: function (e) { return _this.onMouseMove(e); } },
                React.createElement(react_konva_1.Stage, { width: window.innerWidth, height: window.innerHeight, onClick: function (e) { return _this.props.deselectElement(); }, style: this.stageStyle, onMou: true },
                    React.createElement(react_konva_1.Layer, { listening: false }, CanvasRenderer_1.CanvasRenderer.renderGrid()),
                    React.createElement(react_konva_1.Layer, null,
                        this.renderTemporaryConnector(),
                        this.props.currentRootShape.elements.map(function (x) { return _this.renderCanvasElement(x); }))),
                React.createElement(reactstrap_1.ButtonGroup, { className: "canvas-top-toolbar" },
                    React.createElement(reactstrap_1.Button, { onClick: function () { return _this.exportCanvasAsJson(); } }, "Export JSON"),
                    React.createElement(reactstrap_1.Button, { onClick: function () { return _this.toggleExportDSLModal(); } }, "Export DSL"),
                    React.createElement(reactstrap_1.Button, { onClick: function () { return _this.toggleSaveAsTemplateModal(); } }, "Save as Template")),
                React.createElement(reactstrap_1.Breadcrumb, { className: "canvas-breadcrumb" },
                    this.props.shapeExpandStack.map(function (shape) {
                        return React.createElement(reactstrap_1.BreadcrumbItem, { onClick: function () { return _this.onCollapseContainer(shape); } }, shape.name);
                    }),
                    React.createElement(reactstrap_1.BreadcrumbItem, { active: true }, this.props.currentRootShape.name)),
                React.createElement(reactstrap_1.Modal, { isOpen: this.saveAsTemplateModal, toggle: function (e) { return _this.toggleSaveAsTemplateModal(); } },
                    React.createElement(reactstrap_1.ModalHeader, { toggle: function (e) { return _this.toggleSaveAsTemplateModal(); } }, "Save pipeline as template"),
                    React.createElement(reactstrap_1.ModalBody, null,
                        React.createElement(reactstrap_1.FormGroup, null,
                            React.createElement(reactstrap_1.Label, { for: "txt-template-category" }, "Category"),
                            React.createElement(reactstrap_1.Input, { type: "text", name: "txt-template-category", id: "txt-template-category", onChange: function (e) { _this.saveAsTemplateCategory = e.target.value; } })),
                        React.createElement(reactstrap_1.FormGroup, null,
                            React.createElement(reactstrap_1.Label, { for: "txt-template-name" }, "Name"),
                            React.createElement(reactstrap_1.Input, { type: "text", name: "txt-template-name", id: "txt-template-name", onChange: function (e) { _this.saveAsTemplateName = e.target.value; } })),
                        React.createElement(reactstrap_1.FormGroup, null,
                            React.createElement(reactstrap_1.Label, { for: "txt-template-description" }, "Description"),
                            React.createElement(reactstrap_1.Input, { type: "textarea", name: "txt-template-description", id: "txt-template-description", onChange: function (e) { _this.saveAsTemplateDescription = e.target.value; } }))),
                    React.createElement(reactstrap_1.ModalFooter, null,
                        React.createElement(reactstrap_1.Button, { color: "primary", onClick: function (e) { return _this.saveAsTemplate(); } }, "Save"),
                        React.createElement(reactstrap_1.Button, { color: "secondary", onClick: function (e) { return _this.toggleSaveAsTemplateModal(); } }, "Cancel"))),
                React.createElement(reactstrap_1.Modal, { isOpen: this.exportDSLModal, toggle: function (e) { return _this.toggleExportDSLModal(); } },
                    React.createElement(reactstrap_1.ModalHeader, { toggle: function (e) { return _this.toggleExportDSLModal(); } }, "Export pipeline as DSL"),
                    React.createElement(reactstrap_1.ModalBody, null,
                        React.createElement(reactstrap_1.FormGroup, null,
                            React.createElement(reactstrap_1.Label, { for: "txt-export-dsl-name" }, "DSL to export"),
                            React.createElement(reactstrap_1.Input, { type: "select", name: "txt-export-dsl-name", id: "txt-export-dsl-name", onChange: function (e) { _this.selectedDSLToExport = e.target.value; } }, this.props.availableDSLs ? this.props.availableDSLs.map(function (dsl) { return React.createElement("option", { value: dsl.name }, dsl.name); }) : null))),
                    React.createElement(reactstrap_1.ModalFooter, null,
                        React.createElement(reactstrap_1.Button, { color: "primary", onClick: function (e) { return _this.exportCanvasAsDSL(); } }, "Export"),
                        React.createElement(reactstrap_1.Button, { color: "secondary", onClick: function (e) { return _this.toggleExportDSLModal(); } }, "Cancel"))))));
    };
    return CanvasPane;
}(React.PureComponent));
;
exports.default = react_redux_1.connect(function (state) { return state.canvas; }, CanvasStore.actionCreators)(CanvasPane);
//# sourceMappingURL=CanvasPane.js.map