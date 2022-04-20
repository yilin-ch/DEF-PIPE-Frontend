"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.reducers = void 0;
var Canvas = require("./Canvas");
// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
exports.reducers = {
    canvas: Canvas.reducer
    //weatherForecasts: WeatherForecasts.reducer
};
//# sourceMappingURL=index.js.map