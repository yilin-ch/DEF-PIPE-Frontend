"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ICanvasShapeSide = exports.ICanvasConnectionPointType = exports.ICanvasElementPropertyType = exports.ICanvasElementType = void 0;
var ICanvasElementType;
(function (ICanvasElementType) {
    ICanvasElementType[ICanvasElementType["Shape"] = 0] = "Shape";
    ICanvasElementType[ICanvasElementType["Connector"] = 1] = "Connector";
})(ICanvasElementType = exports.ICanvasElementType || (exports.ICanvasElementType = {}));
var ICanvasElementPropertyType;
(function (ICanvasElementPropertyType) {
    ICanvasElementPropertyType[ICanvasElementPropertyType["singleLineText"] = 0] = "singleLineText";
    ICanvasElementPropertyType[ICanvasElementPropertyType["multiLineText"] = 1] = "multiLineText";
    ICanvasElementPropertyType[ICanvasElementPropertyType["select"] = 2] = "select";
})(ICanvasElementPropertyType = exports.ICanvasElementPropertyType || (exports.ICanvasElementPropertyType = {}));
var ICanvasConnectionPointType;
(function (ICanvasConnectionPointType) {
    ICanvasConnectionPointType[ICanvasConnectionPointType["input"] = 0] = "input";
    ICanvasConnectionPointType[ICanvasConnectionPointType["output"] = 1] = "output";
})(ICanvasConnectionPointType = exports.ICanvasConnectionPointType || (exports.ICanvasConnectionPointType = {}));
var ICanvasShapeSide;
(function (ICanvasShapeSide) {
    ICanvasShapeSide[ICanvasShapeSide["left"] = 0] = "left";
    ICanvasShapeSide[ICanvasShapeSide["right"] = 1] = "right";
    ICanvasShapeSide[ICanvasShapeSide["up"] = 2] = "up";
    ICanvasShapeSide[ICanvasShapeSide["down"] = 3] = "down";
})(ICanvasShapeSide = exports.ICanvasShapeSide || (exports.ICanvasShapeSide = {}));
//# sourceMappingURL=index.js.map