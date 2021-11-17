"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CanvasRenderer = void 0;
var React = require("react");
var models_1 = require("../models");
var react_konva_1 = require("react-konva");
var constants_1 = require("../constants");
var CanvasRenderer = /** @class */ (function () {
    function CanvasRenderer() {
    }
    CanvasRenderer.renderGrid = function () {
        var lines = [];
        var canvasContainer = document.getElementById('canvas-container');
        if (canvasContainer) {
            var canvasWidth = canvasContainer.scrollWidth;
            var canvasHeight = canvasContainer.scrollHeight;
            var x = 0;
            while (x < canvasWidth) {
                lines.push([x, 0, x, canvasHeight]);
                x += constants_1.CanvasSettings.gridBlockSize;
            }
            var y = 0;
            while (y < canvasHeight) {
                lines.push([0, y, canvasWidth, y]);
                y += constants_1.CanvasSettings.gridBlockSize;
            }
        }
        return lines.map(function (line, index) {
            var strokeColor = index % 5 === 0 ? '#ccc' : '#ddd';
            return React.createElement(react_konva_1.Line, { points: line, stroke: strokeColor });
        });
    };
    CanvasRenderer.renderShapeComponent = function (shape, isSelectedShape, containerExpandCallback) {
        switch (shape.shape) {
            case "Container":
                return [
                    React.createElement(react_konva_1.Rect, { width: shape.width, height: shape.height, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground }),
                    React.createElement(react_konva_1.Rect, { x: shape.width - 50, y: -20, width: 50, height: 20, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground }),
                    React.createElement(react_konva_1.Text, { x: shape.width - 50, y: -20, width: 50, height: 20, text: "...", align: "center", verticalAlign: "middle", fontSize: 18, fontFamily: "Calibri", fill: constants_1.CanvasSettings.textColor, onClick: function (e) { return containerExpandCallback(e, shape); } })
                ];
            case "Rectangle":
                return React.createElement(react_konva_1.Rect, { width: shape.width, height: shape.height, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground });
            case "Ellipse":
                var radiusX = shape.width / 2;
                var radiusY = shape.height / 2;
                return React.createElement(react_konva_1.Ellipse, { x: radiusX, y: radiusY, radiusX: radiusX, radiusY: radiusY, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground });
            case "Diamond":
                var radius = shape.width / 2;
                return React.createElement(react_konva_1.RegularPolygon, { x: radius, y: radius, sides: 4, radius: radius, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground });
            case "Database":
                var ellipseRadiusX = shape.width / 2;
                var ellipseRadiusY = shape.height / 8;
                return [
                    React.createElement(react_konva_1.Ellipse, { listening: false, x: ellipseRadiusX, y: shape.height - ellipseRadiusY, radiusX: ellipseRadiusX, radiusY: ellipseRadiusY, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground }),
                    React.createElement(react_konva_1.Rect, { listening: false, x: 0, y: ellipseRadiusY, width: shape.width, height: shape.height - (ellipseRadiusY * 2), stroke: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground }),
                    React.createElement(react_konva_1.Line, { listening: false, points: [0, ellipseRadiusY, 0, shape.height - ellipseRadiusY], stroke: constants_1.CanvasSettings.shapeStroke }),
                    React.createElement(react_konva_1.Line, { listening: false, points: [shape.width, ellipseRadiusY, shape.width, shape.height - ellipseRadiusY], stroke: constants_1.CanvasSettings.shapeStroke }),
                    React.createElement(react_konva_1.Ellipse, { listening: false, x: ellipseRadiusX, y: ellipseRadiusY, radiusX: ellipseRadiusX, radiusY: ellipseRadiusY, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedShape ? constants_1.CanvasSettings.selectedShapeBackground : constants_1.CanvasSettings.shapeBackground })
                ];
        }
    };
    CanvasRenderer.renderShapeName = function (shape) {
        return React.createElement(react_konva_1.Text, { width: shape.width, height: shape.height, text: shape.name, align: "center", verticalAlign: "middle", fontSize: 18, fontFamily: constants_1.CanvasSettings.fontFamily, fill: constants_1.CanvasSettings.textColor });
    };
    CanvasRenderer.renderConnectionPoint = function (point, shape, isSelectedConnectionPoint, onConnectionPointClick) {
        if (point.type === models_1.ICanvasConnectionPointType.input) {
            return React.createElement(react_konva_1.Circle, { x: point.position.x, y: point.position.y, radius: 5, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedConnectionPoint ? constants_1.CanvasSettings.selectedConnectionPointBackground : constants_1.CanvasSettings.connectionPointBackground, onClick: function (e) { return onConnectionPointClick(e, shape, point); } });
        }
        else {
            return [
                React.createElement(react_konva_1.Circle, { x: point.position.x, y: point.position.y, radius: 5, stroke: constants_1.CanvasSettings.shapeStroke, fill: isSelectedConnectionPoint ? constants_1.CanvasSettings.selectedConnectionPointBackground : constants_1.CanvasSettings.connectionPointBackground, onClick: function (e) { return onConnectionPointClick(e, shape, point); } }),
                React.createElement(react_konva_1.Circle, { listening: false, x: point.position.x, y: point.position.y, radius: 2, stroke: constants_1.CanvasSettings.shapeStroke, fill: constants_1.CanvasSettings.shapeStroke, onClick: function (e) { return onConnectionPointClick(e, shape, point); } })
            ];
        }
    };
    return CanvasRenderer;
}());
exports.CanvasRenderer = CanvasRenderer;
//# sourceMappingURL=CanvasRenderer.js.map