"use strict";
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
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.reducer = exports.actionCreators = void 0;
var models_1 = require("../models");
var TemplateService_1 = require("../services/TemplateService");
var uuid_1 = require("uuid");
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    importElements: function (elements) { return ({ type: 'IMPORT_ELEMENTS', elements: elements }); },
    addElement: function (element) { return ({ type: 'ADD_ELEMENT', element: element }); },
    removeElement: function (elementId) { return ({ type: 'REMOVE_ELEMENT', elementId: elementId }); },
    updateElement: function (element) { return ({ type: 'UPDATE_ELEMENT', element: element }); },
    selectElement: function (element) { return ({ type: 'SELECT_ELEMENT', element: element }); },
    deselectElement: function () { return ({ type: 'DESELECT_ELEMENT' }); },
    selectConnectionPoint: function (element, point) { return ({ type: 'SELECT_CONNECTIONPOINT', element: element, point: point }); },
    updateMousePosition: function (position) { return ({ type: 'UPDATE_MOUSE_POSITION', position: position }); },
    selectTemplate: function (template) { return ({ type: 'SELECT_TEMPLATE', template: template }); },
    dragTemplate: function (template) { return ({ type: 'DRAG_TEMPLATE', template: template }); },
    dropTemplate: function () { return ({ type: 'DROP_TEMPLATE' }); },
    addTemplate: function (template) { return ({ type: 'ADD_TEMPLATE', template: template }); },
    addRepo: function (template) { return ({ type: 'ADD_REPO', repo: template }); },
    updateTemplate: function (template) { return ({ type: 'UPDATE_TEMPLATE', template: template }); },
    saveTemplate: function (template) { return ({ type: 'SAVE_TEMPLATE', template: template }); },
    removeTemplate: function (template) { return ({ type: 'REMOVE_TEMPLATE', template: template }); },
    filterTemplates: function (newValue) { return ({ type: 'FILTER_TEMPLATES', value: newValue }); },
    expandContainer: function (shape) { return ({ type: 'EXPAND_CONTAINER', shape: shape }); },
    collapseContainer: function (shape) { return ({ type: 'COLLAPSE_CONTAINER', shape: shape }); },
    selectTab: function (tabId) { return ({ type: 'SELECT_TAB', tabId: tabId }); },
    requestDSLs: function () { return function (dispatch, getState) {
        // Only load data if it's something we don't already have (and are not already loading)
        var appState = getState();
        if (appState && appState.canvas && !appState.canvas.availableDSLs) {
            fetch("/api/export/dsl/available")
                .then(function (response) { return response.json(); })
                .then(function (data) {
                dispatch({ type: 'REQUEST_DSL', dsl: data });
            });
        }
    }; },
    requestTemplates: function () { return function (dispatch, getState) {
        var appState = getState();
        if (appState && appState.canvas && !appState.canvas.templates) {
            fetch("/api/templates")
                .then(function (response) { return response.json(); })
                .then(function (apiResult) {
                dispatch({ type: 'REQUEST_TEMPLATES', templates: apiResult.data });
            });
        }
    }; },
    requestRepo: function (username) { return function (dispatch, getState) {
        var appState = getState();
        if (username && appState && appState.canvas && !appState.canvas.repo) {
            fetch("/api/templates/" + username)
                .then(function (response) { return response.json(); })
                .then(function (apiResult) {
                dispatch({ type: 'REQUEST_REPO', repo: apiResult.data, username: username });
            });
        }
    }; },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var reducer = function (state, incomingAction) {
    var templateService = new TemplateService_1.TemplateService();
    //let mockTemplates = templateService.getTemplates();
    if (state === undefined) {
        var rootShape = templateService.createNewRootShape();
        return {
            currentRootShape: rootShape,
            shapeExpandStack: [],
            templates: null,
            repo: null,
            username: null,
            templateGroups: templateService.getTemplateGroups([]),
            repoGroups: templateService.getTemplateGroups([]),
            selectedTemplate: null,
            draggedTemplate: null,
            selectedTab: '1',
            availableDSLs: null,
        };
    }
    var action = incomingAction;
    switch (action.type) {
        case 'REQUEST_DSL':
            return __assign(__assign({}, state), { availableDSLs: action.dsl });
        case 'REQUEST_TEMPLATES':
            return __assign(__assign({}, state), { templates: action.templates, templateGroups: templateService.getTemplateGroups(action.templates) });
        case 'REQUEST_REPO':
            return __assign(__assign({}, state), { repo: action.repo, repoGroups: templateService.getTemplateGroups(action.repo), username: action.username });
        case 'IMPORT_ELEMENTS':
            var newRootShape = templateService.createNewRootShape();
            newRootShape.elements = action.elements;
            return __assign(__assign({}, state), { currentRootShape: newRootShape, shapeExpandStack: [], selectedConnectionPoint: null, selectedElement: null });
        case 'ADD_ELEMENT':
            return __assign(__assign({}, state), { currentRootShape: __assign(__assign({}, state.currentRootShape), { elements: state.currentRootShape.elements.concat(action.element) }) });
        case 'REMOVE_ELEMENT':
            var elementsToRemove_1 = state.currentRootShape.elements.filter(function (ele) { return ele.id === action.elementId; });
            if (elementsToRemove_1[0] && elementsToRemove_1[0].type === models_1.ICanvasElementType.Shape) {
                var shapeToRemove_1 = elementsToRemove_1[0];
                elementsToRemove_1 = elementsToRemove_1.concat(state.currentRootShape.elements
                    .filter(function (ele) { return ele.type === models_1.ICanvasElementType.Connector &&
                    (ele.destShapeId === shapeToRemove_1.id || ele.sourceShapeId === shapeToRemove_1.id); }));
            }
            return __assign(__assign({}, state), { currentRootShape: __assign(__assign({}, state.currentRootShape), { elements: state.currentRootShape.elements.filter(function (ele) { return elementsToRemove_1.indexOf(ele) < 0; }) }), selectedElement: null, selectedConnectionPoint: null });
        case 'UPDATE_ELEMENT':
            var existingElement = state.currentRootShape.elements.filter(function (x) { return x.id === action.element.id; })[0];
            var indexOfExistingElement = state.currentRootShape.elements.indexOf(existingElement);
            state.currentRootShape.elements[indexOfExistingElement] = action.element;
            return __assign(__assign({}, state), { currentRootShape: __assign({}, state.currentRootShape) });
        case 'SELECT_ELEMENT':
            return __assign(__assign({}, state), { selectedElement: action.element, selectedConnectionPoint: null });
        case 'DESELECT_ELEMENT':
            return __assign(__assign({}, state), { selectedElement: null, selectedConnectionPoint: null });
        case 'SELECT_CONNECTIONPOINT':
            var newSelectedShape = undefined;
            var newSelectedPoint = undefined;
            var newElements = state.currentRootShape.elements;
            if (state.selectedElement && state.selectedConnectionPoint) {
                if (state.selectedElement.id === action.element.id) {
                    if (state.selectedConnectionPoint.id === action.point.id) {
                        newSelectedPoint = undefined;
                    }
                    else if (action.point.type === models_1.ICanvasConnectionPointType.output) {
                        newSelectedPoint = action.point;
                    }
                }
                else if (action.point.type === models_1.ICanvasConnectionPointType.input) {
                    var newConnector = {
                        id: (0, uuid_1.v4)(),
                        sourceShapeId: state.selectedElement.id,
                        sourceConnectionPointId: state.selectedConnectionPoint.id,
                        destShapeId: action.element.id,
                        destConnectionPointId: action.point.id,
                        type: models_1.ICanvasElementType.Connector
                    };
                    newElements = newElements.concat(newConnector);
                }
            }
            else if (action.point.type === models_1.ICanvasConnectionPointType.output) {
                newSelectedShape = action.element;
                newSelectedPoint = action.point;
            }
            return __assign(__assign({}, state), { selectedElement: newSelectedShape, selectedConnectionPoint: newSelectedPoint, currentRootShape: __assign(__assign({}, state.currentRootShape), { elements: newElements }) });
        case 'UPDATE_MOUSE_POSITION':
            return __assign(__assign({}, state), { currentMousePosition: action.position });
        case 'FILTER_TEMPLATES':
            return __assign(__assign({}, state), { templateGroups: templateService.getTemplateGroups(state.templates || [], action.value) });
        case 'SELECT_TEMPLATE':
            return __assign(__assign({}, state), { selectedTemplate: action.template });
        case 'DRAG_TEMPLATE':
            return __assign(__assign({}, state), { draggedTemplate: action.template });
        case 'DROP_TEMPLATE':
            return __assign(__assign({}, state), { draggedTemplate: null });
        case 'ADD_TEMPLATE':
            var templateGroup_1 = state.templateGroups.filter(function (group) { return group.name === action.template.category; })[0];
            if (templateGroup_1) {
                templateGroup_1.items = templateGroup_1.items || [];
                templateGroup_1.items.push(action.template);
            }
            else {
                templateGroup_1 = {
                    name: action.template.category,
                    items: [action.template]
                };
            }
            var groupIndex = state.templateGroups.findIndex(function (group) { return group.name === templateGroup_1.name; });
            state.templateGroups[groupIndex] = templateGroup_1;
            templateService.saveTemplate(action.template);
            return __assign(__assign({}, state), { templateGroups: state.templateGroups, selectedTemplate: action.template });
        case 'ADD_REPO':
            var repoGroup_1 = state.repoGroups.filter(function (group) { return group.name === action.repo.category; })[0];
            if (repoGroup_1) {
                repoGroup_1.items = repoGroup_1.items || [];
                repoGroup_1.items.push(action.repo);
            }
            else {
                repoGroup_1 = {
                    name: action.repo.category,
                    items: [action.repo]
                };
            }
            var repoIndex = state.repoGroups.findIndex(function (group) { return group.name === repoGroup_1.name; });
            state.repoGroups[repoIndex] = repoGroup_1;
            templateService.saveRepo(action.repo, state.username);
            return __assign(__assign({}, state), { repoGroups: state.repoGroups, selectedTemplate: action.repo });
        case 'UPDATE_TEMPLATE':
            var templateGroupToBeUpdated = state.templateGroups.filter(function (group) { return group.name === action.template.category; })[0];
            if (templateGroupToBeUpdated) {
                var templateToBeUpdated = templateGroupToBeUpdated.items.filter(function (template) { return template.id === action.template.id; })[0];
                var indexOfExistingTemplate = templateGroupToBeUpdated.items.indexOf(templateToBeUpdated);
                templateToBeUpdated = __assign({}, action.template);
                templateGroupToBeUpdated.items = __spreadArray(__spreadArray(__spreadArray([], templateGroupToBeUpdated.items.slice(0, indexOfExistingTemplate), true), templateGroupToBeUpdated.items.slice(indexOfExistingTemplate + 1), true), [action.template], false);
                //templateService.saveTemplate(templateToBeUpdated);
                return __assign(__assign({}, state), { selectedTemplate: templateToBeUpdated });
            }
            else {
                var oldTemplateGroup_1 = state.templateGroups.filter(function (group) { return group.items.some(function (template) { return template.id === action.template.id; }); })[0];
                oldTemplateGroup_1.items = oldTemplateGroup_1.items.filter(function (template) { return template.id !== action.template.id; });
                var newTemplateGroup = {
                    name: action.template.category,
                    items: [action.template]
                };
                var templateGroups = state.templateGroups;
                if (oldTemplateGroup_1.items.length === 0) {
                    templateGroups = templateGroups.filter(function (group) { return group.name !== oldTemplateGroup_1.name; });
                }
                templateGroups = templateGroups.concat(newTemplateGroup);
                return __assign(__assign({}, state), { templateGroups: templateGroups, selectedTemplate: action.template });
            }
        case 'SAVE_TEMPLATE':
            templateService.saveTemplate(state.selectedTemplate);
            return state;
        case 'REMOVE_TEMPLATE':
            templateGroup_1 = state.templateGroups.filter(function (group) { return group.name === action.template.category; })[0];
            console.log(templateGroup_1.items.length);
            templateGroup_1.items = templateGroup_1.items.filter(function (obj) { return obj !== action.template; });
            var index = state.templateGroups.findIndex(function (group) { return group.name === templateGroup_1.name; });
            state.templateGroups[index] = templateGroup_1;
            templateService.deleteTemplate(action.template.id);
            console.log(state.templateGroups[index].items.length);
            return __assign(__assign({}, state), { templateGroups: state.templateGroups, selectedTemplate: null, selectedElement: null });
        case 'EXPAND_CONTAINER':
            state.shapeExpandStack.push(state.currentRootShape);
            return __assign(__assign({}, state), { currentRootShape: action.shape, shapeExpandStack: state.shapeExpandStack, selectedConnectionPoint: null, selectedElement: null });
        case 'COLLAPSE_CONTAINER':
            var rootShape_1 = state.shapeExpandStack.pop();
            rootShape_1.elements.forEach(function (e) {
                if (e.id === state.currentRootShape.id) {
                    e["elements"] = state.currentRootShape.elements;
                }
            });
            while (action.shape && rootShape_1.id !== action.shape.id) {
                var newRootShape_1 = state.shapeExpandStack.pop();
                newRootShape_1.elements.forEach(function (e) {
                    if (e.id === rootShape_1.id) {
                        e["elements"] = rootShape_1.elements;
                    }
                });
                rootShape_1 = newRootShape_1;
            }
            return __assign(__assign({}, state), { currentRootShape: rootShape_1, shapeExpandStack: state.shapeExpandStack, selectedConnectionPoint: null, selectedElement: null });
        case 'SELECT_TAB':
            return __assign(__assign({}, state), { selectedTab: action.tabId });
        default:
            return state;
    }
};
exports.reducer = reducer;
//# sourceMappingURL=Canvas.js.map