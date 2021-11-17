import { Action, Reducer } from 'redux';
import { ICanvasElement, ICanvasShapeTemplateGroup, ICanvasShapeTemplate, ICanvasShapeConnectionPoint, ICanvasShape, ICanvasConnector, ICanvasElementType, ICanvasPosition, ICanvasConnectionPointType, IDSLInfo, ApiResult } from '../models';
import { TemplateService } from '../services/TemplateService';
import { v4 as uuidv4 } from 'uuid';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CanvasState {
    //elements: Array<ICanvasElement>;
    selectedElement?: ICanvasElement | null;
    selectedConnectionPoint?: ICanvasShapeConnectionPoint | null;
    currentMousePosition?: ICanvasPosition | null;

    shapeExpandStack: Array<ICanvasShape>;
    currentRootShape: ICanvasShape;

    //templates: Array<ICanvasShapeTemplate> | null;
    draggedTemplate: ICanvasShapeTemplate | null;
    templates: Array<ICanvasShapeTemplate> | null;
    templateGroups: Array<ICanvasShapeTemplateGroup>;
    selectedTemplate?: ICanvasShapeTemplate | null;
    selectedTab: string | null;

    availableDSLs: Array<IDSLInfo> | null;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface ImportElementsAction { type: 'IMPORT_ELEMENTS', elements: Array<ICanvasElement> }
export interface AddElementAction { type: 'ADD_ELEMENT', element: ICanvasElement }
export interface RemoveElementAction { type: 'REMOVE_ELEMENT', elementId: string }
export interface UpdateElementAction { type: 'UPDATE_ELEMENT', element: ICanvasElement}
export interface SelectElementAction { type: 'SELECT_ELEMENT', element: ICanvasElement }
export interface DeselectElementAction { type: 'DESELECT_ELEMENT' }
export interface SelectConnectionPointAction { type: 'SELECT_CONNECTIONPOINT', element: ICanvasElement, point: ICanvasShapeConnectionPoint }
export interface UpdateMousePosition { type: 'UPDATE_MOUSE_POSITION', position: ICanvasPosition }
export interface SelectTemplateAction { type: 'SELECT_TEMPLATE', template: ICanvasShapeTemplate }
export interface DragTemplateAction { type: 'DRAG_TEMPLATE', template: ICanvasShapeTemplate }
export interface DropTemplateAction { type: 'DROP_TEMPLATE' }
export interface AddTemplateAction { type: 'ADD_TEMPLATE', template: ICanvasShapeTemplate }
export interface UpdateTemplateAction { type: 'UPDATE_TEMPLATE', template: ICanvasShapeTemplate }
export interface RemoveTemplateAction { type: 'REMOVE_TEMPLATE', template: ICanvasShapeTemplate }
export interface FilterTemplatesAction { type: 'FILTER_TEMPLATES', value: string }
export interface ExpandContainer { type: 'EXPAND_CONTAINER', shape: ICanvasShape }
export interface CollapseContainer { type: 'COLLAPSE_CONTAINER', shape: ICanvasShape }
export interface SelectTab { type: 'SELECT_TAB', tabId: string }
export interface RequestDSL { type: 'REQUEST_DSL', dsl: Array<IDSLInfo> }
export interface RequestTemplates { type: 'REQUEST_TEMPLATES', templates: Array<ICanvasShapeTemplate> }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = ImportElementsAction | AddElementAction | RemoveElementAction | SelectElementAction | DeselectElementAction | UpdateElementAction | SelectConnectionPointAction |
    FilterTemplatesAction | SelectTemplateAction | AddTemplateAction | UpdateTemplateAction | RemoveTemplateAction | DragTemplateAction | DropTemplateAction |
    UpdateMousePosition | ExpandContainer | CollapseContainer | SelectTab | RequestDSL | RequestTemplates;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    importElements: (elements: Array<ICanvasElement>) => ({ type: 'IMPORT_ELEMENTS', elements: elements } as ImportElementsAction),
    addElement: (element: ICanvasElement) => ({ type: 'ADD_ELEMENT', element: element } as AddElementAction),
    removeElement: (elementId: string) => ({ type: 'REMOVE_ELEMENT', elementId: elementId } as RemoveElementAction),
    updateElement: (element: ICanvasElement) => ({ type: 'UPDATE_ELEMENT', element: element } as UpdateElementAction),
    selectElement: (element: ICanvasElement) => ({ type: 'SELECT_ELEMENT', element: element } as SelectElementAction),
    deselectElement: () => ({ type: 'DESELECT_ELEMENT' } as DeselectElementAction),
    selectConnectionPoint: (element: ICanvasElement, point: ICanvasShapeConnectionPoint) => ({ type: 'SELECT_CONNECTIONPOINT', element: element, point: point } as SelectConnectionPointAction),
    updateMousePosition: (position: ICanvasPosition) => ({ type: 'UPDATE_MOUSE_POSITION', position: position }),
    
    selectTemplate: (template: ICanvasShapeTemplate) => ({ type: 'SELECT_TEMPLATE', template: template } as SelectTemplateAction),
    dragTemplate: (template: ICanvasShapeTemplate) => ({ type: 'DRAG_TEMPLATE', template: template } as DragTemplateAction),
    dropTemplate: () => ({ type: 'DROP_TEMPLATE' } as DropTemplateAction),
    addTemplate: (template: ICanvasShapeTemplate) => ({ type: 'ADD_TEMPLATE', template: template } as AddTemplateAction),
    updateTemplate: (template: ICanvasShapeTemplate) => ({ type: 'UPDATE_TEMPLATE', template: template } as UpdateTemplateAction),
    removeTemplate: (template: ICanvasShapeTemplate) => ({ type: 'REMOVE_TEMPLATE', template: template } as RemoveTemplateAction),
    filterTemplates: (newValue: string) => ({ type: 'FILTER_TEMPLATES', value: newValue } as FilterTemplatesAction),

    expandContainer: (shape: ICanvasShape) => ({ type: 'EXPAND_CONTAINER', shape: shape }),
    collapseContainer: (shape: ICanvasShape) => ({ type: 'COLLAPSE_CONTAINER', shape: shape }),

    selectTab: (tabId: string) => ({ type: 'SELECT_TAB', tabId: tabId }),

    requestDSLs: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.canvas && !appState.canvas.availableDSLs) {
            fetch(`/api/export/dsl/available`)
                .then(response => response.json() as Promise<Array<IDSLInfo>>)
                .then(data => {
                    dispatch({ type: 'REQUEST_DSL', dsl: data });
                });
        }
    },

     requestTemplates: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
         const appState = getState();
         if (appState && appState.canvas && !appState.canvas.templates) {
            fetch(`/api/templates`)
                .then(response => response.json() as Promise<ApiResult<Array<ICanvasShapeTemplate>>>)
                .then(apiResult => {
                    dispatch({ type: 'REQUEST_TEMPLATES', templates: apiResult.data });
                });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<CanvasState> = (state: CanvasState | undefined, incomingAction: Action): CanvasState => {
    let templateService = new TemplateService();
    //let mockTemplates = templateService.getTemplates();

    if (state === undefined) {
        let rootShape = templateService.createNewRootShape();
        return {
            currentRootShape: rootShape,
            shapeExpandStack: [],
            templates: null,
            templateGroups: templateService.getTemplateGroups([]),
            selectedTemplate: null,
            draggedTemplate: null,
            selectedTab: '1',
            availableDSLs: null,
        };
    }
    
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_DSL':
            return {
                ...state,
                availableDSLs: action.dsl
            }
        case 'REQUEST_TEMPLATES':
            return {
                ...state,
                templates: action.templates,
                templateGroups: templateService.getTemplateGroups(action.templates)
            }
        case 'IMPORT_ELEMENTS':
            let newRootShape = templateService.createNewRootShape();
            newRootShape.elements = action.elements;
            return {
                ...state,
                currentRootShape: newRootShape,
                shapeExpandStack: [],
                selectedConnectionPoint: null,
                selectedElement: null                
            }
        case 'ADD_ELEMENT':
            return {
                ...state,
                currentRootShape: { ...state.currentRootShape, elements: state.currentRootShape.elements.concat(action.element) }
            };
        case 'REMOVE_ELEMENT':
            let elementsToRemove: Array<ICanvasElement> = state.currentRootShape.elements.filter(ele => ele.id === action.elementId);
            if (elementsToRemove[0] && elementsToRemove[0].type === ICanvasElementType.Shape) {
                let shapeToRemove = elementsToRemove[0] as ICanvasShape;
                elementsToRemove = elementsToRemove.concat(
                    state.currentRootShape.elements
                        .filter(ele => ele.type === ICanvasElementType.Connector &&
                            ((ele as ICanvasConnector).destShapeId === shapeToRemove.id || (ele as ICanvasConnector).sourceShapeId === shapeToRemove.id)))
            }

            return {
                ...state,
                currentRootShape: { ...state.currentRootShape, elements: state.currentRootShape.elements.filter(ele => elementsToRemove.indexOf(ele) < 0) },
                selectedElement: null,
                selectedConnectionPoint: null
            };
        case 'UPDATE_ELEMENT':
            let existingElement = state.currentRootShape.elements.filter(x => x.id === action.element.id)[0];
            let indexOfExistingElement = state.currentRootShape.elements.indexOf(existingElement);
            state.currentRootShape.elements[indexOfExistingElement] = action.element;

            return {
                ...state,
                currentRootShape: { ...state.currentRootShape }
            };
        case 'SELECT_ELEMENT':
            return {
                ...state,
                selectedElement: action.element,
                selectedConnectionPoint: null
            };
        case 'DESELECT_ELEMENT':
            return {
                ...state,
                selectedElement: null,
                selectedConnectionPoint: null
            };
        case 'SELECT_CONNECTIONPOINT':
            let newSelectedShape: ICanvasShape = undefined;
            let newSelectedPoint: ICanvasShapeConnectionPoint = undefined;
            let newElements: Array<ICanvasElement> = state.currentRootShape.elements;

            if (state.selectedElement && state.selectedConnectionPoint) {
                if (state.selectedElement.id === action.element.id) {
                    if (state.selectedConnectionPoint.id === action.point.id) {
                        newSelectedPoint = undefined
                    }
                    else if (action.point.type === ICanvasConnectionPointType.output){
                        newSelectedPoint = action.point;
                    }
                }
                else if (action.point.type === ICanvasConnectionPointType.input) {
                    let newConnector: ICanvasConnector = {
                        id: uuidv4(),
                        sourceShapeId: state.selectedElement.id,
                        sourceConnectionPointId: state.selectedConnectionPoint.id,
                        destShapeId: action.element.id,
                        destConnectionPointId: action.point.id,
                        type: ICanvasElementType.Connector
                    };
                    newElements = newElements.concat(newConnector);
                }
            }
            else if (action.point.type === ICanvasConnectionPointType.output) {
                newSelectedShape = action.element as ICanvasShape;
                newSelectedPoint = action.point;
            }
            
            return {
                ...state,
                selectedElement: newSelectedShape,
                selectedConnectionPoint: newSelectedPoint,
                currentRootShape: { ...state.currentRootShape, elements: newElements },
            };
        case 'UPDATE_MOUSE_POSITION':
            return {
                ...state,
                currentMousePosition: action.position
            };
        case 'FILTER_TEMPLATES':
            return {
                ...state,
                templateGroups: templateService.getTemplateGroups(state.templates || [], action.value)
            };
        case 'SELECT_TEMPLATE':
            return {
                ...state,
                selectedTemplate: action.template
            };
        case 'DRAG_TEMPLATE':
            return {
                ...state,
                draggedTemplate: action.template
            };
        case 'DROP_TEMPLATE':
            return {
                ...state,
                draggedTemplate: null
            };
        case 'ADD_TEMPLATE':
            let templateGroup = state.templateGroups.filter(group => group.name === action.template.category)[0];
            if (templateGroup) {
                templateGroup.items = templateGroup.items || [];
                templateGroup.items.push(action.template);
            }
            else {
                templateGroup = {
                    name: action.template.category,
                    items: [action.template]
                }                
            }
            let newTemplateGroups = state.templateGroups.filter(group => group.name !== templateGroup.name).concat(templateGroup);
            let templates: Array<ICanvasShapeTemplate> = [];
            newTemplateGroups.forEach(grp => templates = templates.concat(grp.items));
            templateService.saveTemplate(action.template);
            
            return {
                ...state,
                templateGroups: newTemplateGroups,
                selectedTemplate: action.template
            };
        case 'UPDATE_TEMPLATE':
            let templateGroupToBeUpdated = state.templateGroups.filter(group => group.name === action.template.category)[0];
            if (templateGroupToBeUpdated) {
                let templateToBeUpdated = templateGroupToBeUpdated.items.filter(template => template.id === action.template.id)[0];
                let indexOfExistingTemplate = templateGroupToBeUpdated.items.indexOf(templateToBeUpdated);

                templateToBeUpdated = { ...action.template };
                templateGroupToBeUpdated.items = [...templateGroupToBeUpdated.items.slice(0, indexOfExistingTemplate), ...templateGroupToBeUpdated.items.slice(indexOfExistingTemplate + 1), action.template]
                templateService.saveTemplate(templateToBeUpdated);

                return {
                    ...state,
                    selectedTemplate: templateToBeUpdated
                };
            }
            else {
                let oldTemplateGroup = state.templateGroups.filter(group => group.items.some(template => template.id === action.template.id))[0];
                oldTemplateGroup.items = oldTemplateGroup.items.filter(template => template.id !== action.template.id);
                let newTemplateGroup = {
                    name: action.template.category,
                    items: [action.template]
                }  

                let templateGroups = state.templateGroups;
                if (oldTemplateGroup.items.length === 0) {
                    templateGroups = templateGroups.filter(group => group.name !== oldTemplateGroup.name);
                }
                templateGroups = templateGroups.concat(newTemplateGroup);
                return {
                    ...state,
                    templateGroups: templateGroups,
                    selectedTemplate: action.template
                };
            }
        case 'REMOVE_TEMPLATE':
            
            return {
                ...state
            };
        case 'EXPAND_CONTAINER':
            state.shapeExpandStack.push(state.currentRootShape);
            return {
                ...state,
                currentRootShape: action.shape,
                shapeExpandStack: state.shapeExpandStack,
                selectedConnectionPoint: null,
                selectedElement: null
            };
        case 'COLLAPSE_CONTAINER':
            let rootShape = state.shapeExpandStack.pop();
            rootShape.elements.forEach(e => {
                if (e.id === state.currentRootShape.id) {
                    e["elements"] = state.currentRootShape.elements;
                }
            });

            while (action.shape && rootShape.id !== action.shape.id) {
                let newRootShape = state.shapeExpandStack.pop();
                newRootShape.elements.forEach(e => {
                    if (e.id === rootShape.id) {
                        e["elements"] = rootShape.elements;
                    }
                });
                rootShape = newRootShape;
            }

            return {
                ...state,
                currentRootShape: rootShape,
                shapeExpandStack: state.shapeExpandStack,
                selectedConnectionPoint: null,
                selectedElement: null
            };
        case 'SELECT_TAB':
            return {
                ...state,
                selectedTab: action.tabId
            }
        default:
            return state;
    }
};
