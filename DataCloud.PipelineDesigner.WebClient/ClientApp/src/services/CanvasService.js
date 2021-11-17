"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CanvasService = void 0;
var models_1 = require("../models");
var constants_1 = require("../constants");
var CanvasService = /** @class */ (function () {
    function CanvasService() {
    }
    CanvasService.prototype.exportAsJson = function (rootShape, fileName) {
        if (fileName === void 0) { fileName = "pipeline.json"; }
        var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(rootShape));
        var downloadAnchorNode = document.createElement('a');
        downloadAnchorNode.setAttribute("href", dataStr);
        downloadAnchorNode.setAttribute("download", fileName);
        document.body.appendChild(downloadAnchorNode); // required for firefox
        downloadAnchorNode.click();
        downloadAnchorNode.remove();
    };
    CanvasService.prototype.exportAsDSL = function (rootShape, fileName) {
        if (fileName === void 0) { fileName = "pipeline-dsl.txt"; }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/export/dsl", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var data = new Blob([xhr.responseText], { type: 'text/plain' });
                var dataStr = window.URL.createObjectURL(data);
                var downloadAnchorNode = document.createElement('a');
                downloadAnchorNode.setAttribute("href", dataStr);
                downloadAnchorNode.setAttribute("download", fileName);
                document.body.appendChild(downloadAnchorNode); // required for firefox
                downloadAnchorNode.click();
                downloadAnchorNode.remove();
            }
        };
        var data = JSON.stringify(rootShape);
        xhr.send(data);
    };
    CanvasService.prototype.getDistance = function (pointA, pointB) {
        return Math.sqrt(Math.pow(pointA.x - pointB.x, 2) + Math.pow(pointA.y - pointB.y, 2));
    };
    CanvasService.prototype.getPointSideOfShape = function (point, shape) {
        var distances = {
            left: this.getDistance(point, { x: shape.position.x, y: point.y }),
            up: this.getDistance(point, { x: point.x, y: shape.position.y }),
            right: this.getDistance(point, { x: shape.position.x + shape.width, y: point.y }),
            down: this.getDistance(point, { x: point.x, y: shape.position.y + shape.height })
        };
        var minDistance = Math.min(distances.left, distances.right, distances.up, distances.down);
        if (minDistance === distances.left)
            return models_1.ICanvasShapeSide.left;
        if (minDistance === distances.right)
            return models_1.ICanvasShapeSide.right;
        if (minDistance === distances.up)
            return models_1.ICanvasShapeSide.up;
        return models_1.ICanvasShapeSide.down;
    };
    CanvasService.prototype.pointInCircle = function (x, y, cx, cy, radius) {
        var distancesquared = (x - cx) * (x - cx) + (y - cy) * (y - cy);
        return distancesquared <= radius * radius;
    };
    CanvasService.prototype.snapToGrid = function (position) {
        return {
            x: Math.round(position.x / constants_1.CanvasSettings.gridBlockSize) * constants_1.CanvasSettings.gridBlockSize,
            y: Math.round(position.y / constants_1.CanvasSettings.gridBlockSize) * constants_1.CanvasSettings.gridBlockSize
        };
    };
    CanvasService.prototype.calculateConnectorPoints = function (connector, srcShape, desShape) {
        var srcPos = srcShape.connectionPoints.filter(function (point) { return point.id === connector.sourceConnectionPointId; })[0].position;
        var desPos = desShape.connectionPoints.filter(function (point) { return point.id === connector.destConnectionPointId; })[0].position;
        srcPos = { x: srcShape.position.x + srcPos.x, y: srcShape.position.y + srcPos.y };
        desPos = { x: desShape.position.x + desPos.x, y: desShape.position.y + desPos.y };
        var points = [srcPos];
        var srcSide = this.getPointSideOfShape(srcPos, srcShape);
        var desSide = this.getPointSideOfShape(desPos, desShape);
        switch (srcSide) {
            case models_1.ICanvasShapeSide.left:
                switch (desSide) {
                    case models_1.ICanvasShapeSide.left:
                        points.push({
                            x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, desPos.x - constants_1.CanvasSettings.minConnectorLength),
                            y: srcPos.y
                        });
                        points.push({
                            x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, desPos.x - constants_1.CanvasSettings.minConnectorLength),
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.right:
                        points.push({
                            x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: srcPos.y
                        });
                        points.push({
                            x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.max(desPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.max(desPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.up:
                        if (srcPos.x > desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: desPos.y - constants_1.CanvasSettings.minConnectorLength
                            });
                        }
                        points.push({
                            x: desPos.x,
                            y: desPos.y - constants_1.CanvasSettings.minConnectorLength
                        });
                        break;
                    case models_1.ICanvasShapeSide.down:
                        if (srcPos.x > desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.min(srcPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: desPos.y + constants_1.CanvasSettings.minConnectorLength
                            });
                        }
                        points.push({
                            x: desPos.x,
                            y: desPos.y + constants_1.CanvasSettings.minConnectorLength
                        });
                        break;
                }
                break;
            case models_1.ICanvasShapeSide.right:
                switch (desSide) {
                    case models_1.ICanvasShapeSide.left:
                        points.push({
                            x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: srcPos.y
                        });
                        points.push({
                            x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.min(desPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.min(desPos.x - constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.right:
                        points.push({
                            x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, desPos.x + constants_1.CanvasSettings.minConnectorLength),
                            y: srcPos.y
                        });
                        points.push({
                            x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, desPos.x + constants_1.CanvasSettings.minConnectorLength),
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.up:
                        if (srcPos.x < desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: desPos.y - constants_1.CanvasSettings.minConnectorLength
                            });
                        }
                        points.push({
                            x: desPos.x,
                            y: desPos.y - constants_1.CanvasSettings.minConnectorLength
                        });
                        break;
                    case models_1.ICanvasShapeSide.down:
                        if (srcPos.x < desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.max(srcPos.x + constants_1.CanvasSettings.minConnectorLength, (srcPos.x + desPos.x) / 2),
                                y: desPos.y + constants_1.CanvasSettings.minConnectorLength
                            });
                        }
                        points.push({
                            x: desPos.x,
                            y: desPos.y + constants_1.CanvasSettings.minConnectorLength
                        });
                        break;
                }
                break;
            case models_1.ICanvasShapeSide.up:
                switch (desSide) {
                    case models_1.ICanvasShapeSide.left:
                        if (srcPos.x < desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x - constants_1.CanvasSettings.minConnectorLength,
                                y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                        }
                        points.push({
                            x: desPos.x - constants_1.CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.right:
                        if (srcPos.x > desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x + constants_1.CanvasSettings.minConnectorLength,
                                y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                        }
                        points.push({
                            x: desPos.x + constants_1.CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.up:
                        points.push({
                            x: srcPos.x,
                            y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, desPos.y - constants_1.CanvasSettings.minConnectorLength),
                        });
                        points.push({
                            x: desPos.x,
                            y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, desPos.y - constants_1.CanvasSettings.minConnectorLength),
                        });
                        break;
                    case models_1.ICanvasShapeSide.down:
                        points.push({
                            x: srcPos.x,
                            y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.min(srcPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.max(desPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2),
                        });
                        points.push({
                            x: desPos.x,
                            y: Math.max(desPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                        });
                        break;
                }
                break;
            case models_1.ICanvasShapeSide.down:
                switch (desSide) {
                    case models_1.ICanvasShapeSide.left:
                        if (srcPos.x < desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x - constants_1.CanvasSettings.minConnectorLength,
                                y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                        }
                        points.push({
                            x: desPos.x - constants_1.CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.right:
                        if (srcPos.x > desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x - constants_1.CanvasSettings.minConnectorLength,
                                y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                            });
                        }
                        points.push({
                            x: desPos.x + constants_1.CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case models_1.ICanvasShapeSide.up:
                        points.push({
                            x: srcPos.x,
                            y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.min(desPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2),
                        });
                        points.push({
                            x: desPos.x,
                            y: Math.min(desPos.y - constants_1.CanvasSettings.minConnectorLength, (srcPos.y + desPos.y) / 2)
                        });
                        break;
                    case models_1.ICanvasShapeSide.down:
                        points.push({
                            x: srcPos.x,
                            y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, desPos.y + constants_1.CanvasSettings.minConnectorLength),
                        });
                        points.push({
                            x: desPos.x,
                            y: Math.max(srcPos.y + constants_1.CanvasSettings.minConnectorLength, desPos.y + constants_1.CanvasSettings.minConnectorLength),
                        });
                        break;
                }
                break;
        }
        points.push(desPos);
        return points;
    };
    return CanvasService;
}());
exports.CanvasService = CanvasService;
//# sourceMappingURL=CanvasService.js.map