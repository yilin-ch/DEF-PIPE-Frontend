import * as React from 'react';
import { connect } from 'react-redux';
import * as CanvasStore from '../../../store/Canvas';
import { ApplicationState } from '../../../store';
import { useParams } from 'react-router-dom';
import { Group, Layer, Rect, Stage, Text, Arrow, Circle, RegularPolygon, Ellipse, Line } from 'react-konva';
import { ICanvasElementPropertyType, ICanvasShape, ICanvasElementType, ICanvasConnector, ICanvasShapeConnectionPoint, ICanvasElement, ICanvasConnectionPointType } from '../../../models';
import { CanvasService } from '../../../services/CanvasService';
import { CanvasRenderer } from '../../../services/CanvasRenderer';
import { CanvasSettings } from '../../../constants';
import { Breadcrumb, BreadcrumbItem, Button, ButtonGroup, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Tooltip } from 'reactstrap';
import * as Autosuggest from "react-autosuggest";
import { v4 as uuidv4 } from 'uuid';
import Konva from "konva";

 interface MyState {
     value: string,
    suggestions: Array<any>
}

type CanvasProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;


class CanvasPane extends React.PureComponent<CanvasProps, MyState> {
    componentDidMount() {
        // const params: any = useParams();
        // const username = params.username;
    }
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            suggestions: []
        }
    }
    canvasService: CanvasService = new CanvasService();

    stageStyle = {
        cursor: 'default'
    }



    saveAsTemplateModal = false;
    saveAsRepoModal = false;
    exportDSLModal = false;
    findRepoModal = false;

    saveAsName = "";
    saveAsDescription = "";
    saveAsCategory = "";
    saveAsRepoPublic = false;

    selectedDSLToExport = "";

    toggleSaveAsTemplateModal() {
        this.saveAsTemplateModal = !this.saveAsTemplateModal;
    }
    toggleSaveAsRepoModal() {
        this.saveAsRepoModal = !this.saveAsRepoModal;
    }
    toggleExportDSLModal() {
        this.exportDSLModal = !this.exportDSLModal;
    }
    toggleFindRepoModal() {
        this.findRepoModal = !this.findRepoModal;
    }

    testing
    repos = [
        {
            user: 'vladom',
            workflow: "DEF-PIP"
        },
        {
            user: 'user',
            workflow: 'TELLU'
        }
        ];
    getRepoSuggestions = value => {
        const inputValue = value.trim().toLowerCase();
        const inputLength = inputValue.length;

        return inputLength === 0 ? [] : this.repos.filter(repo =>
            repo.user.toLowerCase().slice(0, inputLength) === inputValue
        );
    };
    renderSuggestion = suggestion => (
        <div>
            {suggestion.user + ' - ' + suggestion.workflow}
        </div>
    );

    onChange = (event, { newValue }) => {

        this.setState({
            value: newValue
        });
    };

    onSuggestionsFetchRequested = ({ value }) => {
        this.setState({
            suggestions: this.getRepoSuggestions(value)
        });
    };

    onSuggestionsClearRequested = () => {
        this.setState({
            suggestions: []
        });
    };



    getSuggestionValue = suggestion => suggestion.name;

    saveAsTemplate() {
        this.props.addTemplate({
            id: uuidv4(),
            name: this.saveAsName,
            description: "",
            category: this.saveAsCategory,
            canvasTemplate: {
                shape: "Container",
                isContainer: true,
                elements: (this.props.shapeExpandStack[0] || this.props.currentRootShape).elements,
                connectionPoints: [{
                    id: '1',
                    position: { x: 0, y: 50 },
                    type: ICanvasConnectionPointType.input
                },
                {
                    id: '2',
                    position: { x: 200, y: 50 },
                    type: ICanvasConnectionPointType.output
                }],
                properties: [],
                width: 200,
                height: 100
            }
        });

        this.saveAsName = "";
        this.saveAsDescription = "";
        this.saveAsCategory = "";
        this.saveAsTemplateModal = false;
    }

    saveAsRepo() {
        console.log(this.saveAsRepoPublic)
        this.props.addRepo({
            id: uuidv4(),
            name: this.saveAsName,
            description: "",
            category: this.saveAsCategory,
            canvasTemplate: {
                shape: "Container",
                isContainer: true,
                elements: (this.props.shapeExpandStack[0] || this.props.currentRootShape).elements,
                connectionPoints: [{
                    id: '1',
                    position: { x: 0, y: 50 },
                    type: ICanvasConnectionPointType.input
                },
                {
                    id: '2',
                    position: { x: 200, y: 50 },
                    type: ICanvasConnectionPointType.output
                }],
                properties: [],
                width: 200,
                height: 100
            },
            public: this.saveAsRepoPublic
        });

        this.saveAsName = "";
        this.saveAsDescription = "";
        this.saveAsCategory = "";
        this.saveAsRepoModal = false;
        this.saveAsRepoPublic = false;
    }

    onKeyDown(e: React.KeyboardEvent) {
        if (e.keyCode === 46 && this.props.selectedElement) {
            this.props.removeElement(this.props.selectedElement.id);
        }
    }

    onShapeClick(e: Konva.KonvaEventObject<MouseEvent>, shape: ICanvasShape) {
        e.cancelBubble = true;
        if (!this.props.selectedElement || this.props.selectedElement.id !== shape.id) {
            this.props.selectElement(shape);
        }
        else
            this.props.deselectElement();
    }

    onConnectorClick(e: Konva.KonvaEventObject<MouseEvent>, connector: ICanvasConnector) {
        e.cancelBubble = true;

        if (!this.props.selectedElement || this.props.selectedElement.id !== connector.id) {
            this.props.selectElement(connector);
        }
        else
            this.props.deselectElement();
    }

    onConnectionPointClick(e: Konva.KonvaEventObject<MouseEvent>, shape: ICanvasShape, point: ICanvasShapeConnectionPoint) {
        e.cancelBubble = true;
        this.props.selectConnectionPoint(shape, point);
    }

    onShapeDragEnd(e: Konva.KonvaEventObject<DragEvent>, shape: ICanvasShape) {
        let newPosition = this.canvasService.snapToGrid(e.target.position());
        e.target.setPosition(newPosition);

        let newShape: ICanvasShape = {
            ...shape,
            position: newPosition
        };
        this.props.updateElement(newShape);
    }

    onShapeDragMove(e: Konva.KonvaEventObject<DragEvent>, shape: ICanvasShape) {
        let newPosition = this.canvasService.snapToGrid(e.target.position());
        e.target.setPosition(newPosition);

        let newShape: ICanvasShape = {
            ...shape,
            position: newPosition
        };

        this.props.updateElement(newShape);
    }

    onContainerExpand(e: Konva.KonvaEventObject<MouseEvent>, shape: ICanvasShape) {
        e.cancelBubble = true;
        this.props.expandContainer(shape);
    }

    onCollapseContainer(shape: ICanvasShape) {
        this.props.collapseContainer(shape);
    }



    onMouseMove(e: React.MouseEvent) {
        let canvasContainer = document.getElementById('canvas-container').getBoundingClientRect();
        this.props.updateMousePosition({ x: e.clientX - canvasContainer.left, y: e.clientY - canvasContainer.top });
    }

    exportCanvasAsJson() {
        this.canvasService.exportAsJson(this.props.currentRootShape);
    }

    exportCanvasAsDSL() {
        this.canvasService.exportAsDSL(this.props.currentRootShape);
    }

    onDrop(e: React.DragEvent) {
        e.preventDefault();
        if (this.props.draggedTemplate) {
            this.onTemplateDrop(e);
        } else if (e.dataTransfer.files.length > 0) {
            this.onJsonDrop(e);
        }
    }

    onTemplateDrop(e: React.DragEvent) {
        let template = this.props.draggedTemplate;
        let canvasContainer = document.getElementById('canvas-container');
        let dropPosition = this.canvasService.snapToGrid({
            x: e.clientX - canvasContainer.getBoundingClientRect().left,
            y: e.clientY - canvasContainer.getBoundingClientRect().top
        });

        let newShape: ICanvasShape = {
            ...template.canvasTemplate,
            properties: template.canvasTemplate.properties.map(p => ({ ...p })),
            id: uuidv4(),
            type: ICanvasElementType.Shape,
            width: template.canvasTemplate.width,
            height: template.canvasTemplate.height,
            shape: template.canvasTemplate.shape,
            position: dropPosition
        };

        this.props.addElement(newShape);
        this.props.dropTemplate();
    }

    onJsonDrop(e: React.DragEvent) {
        let file = e.dataTransfer.files[0];
        let reader = new FileReader();
        reader.onload = (event) => {
            if (event.target && event.target.result) {
                let json = event.target.result as string;
                let elements: Array<ICanvasElement> = JSON.parse(json);
                this.props.importElements(elements);
            }
        };
        reader.readAsText(file);
    }

    renderTemporaryConnector() {
        let points: Array<number> = [];
        if (this.props.selectedConnectionPoint) {
            this.props.currentRootShape.elements.forEach(ele => {
                if (ele.type === ICanvasElementType.Shape && ele.id != this.props.selectedElement.id) {
                    let shape = ele as ICanvasShape;
                    let point = shape.connectionPoints.filter(p =>
                        p.type === ICanvasConnectionPointType.input &&
                        this.canvasService.pointInCircle(
                            shape.position.x + p.position.x,
                            shape.position.y + p.position.y,
                            this.props.currentMousePosition.x,
                            this.props.currentMousePosition.y, 10)
                    )[0];

                    if (point) {
                        let connector: ICanvasConnector = {
                            sourceConnectionPointId: this.props.selectedConnectionPoint.id,
                            sourceShapeId: this.props.selectedElement.id,
                            destConnectionPointId: point.id,
                            destShapeId: ele.id,
                            id: '',
                            type: ICanvasElementType.Connector
                        }
                        this.canvasService.calculateConnectorPoints(connector, this.props.selectedElement as ICanvasShape, shape).forEach(p => {
                            points.push(p.x);
                            points.push(p.y)
                        });
                    }
                }
            });

            if (points.length === 0) {
                let destShape: ICanvasShape = {
                    id: '',
                    name: '',
                    position: {
                        x: this.props.currentMousePosition.x - 10,
                        y: this.props.currentMousePosition.y
                    },
                    width: 20,
                    height: 10,
                    connectionPoints: [{ id: '0', position: { x: 10, y: 0 }, type: ICanvasConnectionPointType.input }],
                    type: ICanvasElementType.Shape,
                    properties: []
                }
                let connector: ICanvasConnector = {
                    sourceConnectionPointId: this.props.selectedConnectionPoint.id,
                    sourceShapeId: this.props.selectedElement.id,
                    destConnectionPointId: '0',
                    destShapeId: destShape.id,
                    id: '',
                    type: ICanvasElementType.Connector
                }
                this.canvasService.calculateConnectorPoints(connector, this.props.selectedElement as ICanvasShape, destShape).forEach(p => {
                    points.push(p.x);
                    points.push(p.y)
                });
            }
        }

        if (points.length > 0) {
            return <Arrow points={points} dash={[10, 5]} stroke={"black"}></ Arrow>
        }
        else
            return null;
    }

    renderCanvasElement(element: ICanvasElement) {
        if (element.type === ICanvasElementType.Shape) {
            return this.renderCanvasShape(element as ICanvasShape);
        }
        else {
            return this.renderCanvasConnector(element as ICanvasConnector);
        }
    }

    renderCanvasShape(shape: ICanvasShape) {
        let isSelectedShape = false;
        if (this.props.selectedElement && this.props.selectedElement.id === shape.id) {
            isSelectedShape = true;
        }

        return <Group x={shape.position.x} y={shape.position.y} draggable={true}
            onClick={(e) => this.onShapeClick(e, shape)}
            onDragMove={(e) => this.onShapeDragMove(e, shape)}
            onDragEnd={(e) => this.onShapeDragEnd(e, shape)}>

            {CanvasRenderer.renderShapeComponent(shape, isSelectedShape, (e, shape) => this.onContainerExpand(e, shape))}
            {CanvasRenderer.renderShapeName(shape)}

            {shape.connectionPoints.map(p => {
                let isSelectedConnectionPoint = false;
                if (isSelectedShape && this.props.selectedConnectionPoint && this.props.selectedConnectionPoint.id === p.id) {
                    isSelectedConnectionPoint = true;
                }

                return CanvasRenderer.renderConnectionPoint(p, shape, isSelectedConnectionPoint, (e, shape, point) => this.onConnectionPointClick(e, shape, point));
            })}
        </Group>
    }

    renderCanvasConnector(connector: ICanvasConnector) {
        let isSelectedConnector = false;
        if (this.props.selectedElement && this.props.selectedElement.id === connector.id) {
            isSelectedConnector = true;
        }

        let srcShape = this.props.currentRootShape.elements.filter(x => x.id === connector.sourceShapeId)[0] as ICanvasShape;
        let destShape = this.props.currentRootShape.elements.filter(x => x.id === connector.destShapeId)[0] as ICanvasShape;

        let points: Array<number> = [];
        this.canvasService.calculateConnectorPoints(connector, srcShape, destShape).forEach(p => {
            points.push(p.x);
            points.push(p.y)
        });

        return <Arrow points={points} stroke={isSelectedConnector ? "blue" : "black"} onClick={(e) => this.onConnectorClick(e, connector)} ></ Arrow>
    }

    public render() {
        var username = 'vladom'
        this.props.requestDSLs();
        this.props.requestTemplates();
        this.props.requestRepo(username);

        const { value, suggestions } = this.state;

        const inputProps = {
            placeholder: 'Search for a repo, workflow',
            value,
            onChange: this.onChange
        };

        return (
            <React.Fragment>
                <div id="canvas-container" className="canvas-container" tabIndex={1} onKeyDown={(e: React.KeyboardEvent) => this.onKeyDown(e)} onDrop={(e) => this.onDrop(e)} onDragOver={(e) => e.preventDefault()} onMouseMove={(e) => this.onMouseMove(e)}>
                    <Stage width={window.innerWidth} height={window.innerHeight} onClick={(e) => this.props.deselectElement()} style={this.stageStyle} onMou >
                        <Layer listening={false}>
                            {CanvasRenderer.renderGrid()}
                        </Layer>
                        <Layer >
                            {this.renderTemporaryConnector()}
                            {this.props.currentRootShape.elements.map(x => this.renderCanvasElement(x))}
                        </Layer>
                    </Stage>
                    <ButtonGroup className="canvas-top-toolbar">
                        <Button onClick={() => this.toggleFindRepoModal()}><i className="bi bi-search" style={{ padding: 5 }}></i></Button>
                        <Button onClick={() => this.exportCanvasAsJson()}>Export JSON</Button>
                        <Button onClick={() => this.toggleExportDSLModal()}>Export DSL</Button>
                        <Button onClick={() => this.toggleSaveAsTemplateModal()}>Save as Template</Button>
                        {username  && <Button onClick={() => this.toggleSaveAsRepoModal()}>Save in repo</Button> }
                    </ButtonGroup>

                    <Breadcrumb className="canvas-breadcrumb">
                        {this.props.shapeExpandStack.map(shape =>
                            <BreadcrumbItem onClick={() => this.onCollapseContainer(shape)}>{shape.name}</BreadcrumbItem>
                        )}
                        <BreadcrumbItem active>{this.props.currentRootShape.name}</BreadcrumbItem>
                    </Breadcrumb>

                    {/* Find repo modal */}
                    <Modal isOpen={this.findRepoModal} toggle={(e) => this.toggleFindRepoModal()}>
                        <ModalHeader toggle={(e) => this.toggleFindRepoModal()}>Find external public repo</ModalHeader>
                        <ModalBody>
                            <Autosuggest
                                suggestions={suggestions}
                                onSuggestionsFetchRequested={this.onSuggestionsFetchRequested}
                                onSuggestionsClearRequested={this.onSuggestionsClearRequested}
                                getSuggestionValue={this.getSuggestionValue}
                                renderSuggestion={this.renderSuggestion}
                                inputProps={inputProps}
                            />
                        </ModalBody>
                        <ModalFooter>
                            <Button color="primary" onClick={(e) => this.toggleFindRepoModal()}>Import repo</Button>
                            <Button color="secondary" onClick={(e) => this.toggleFindRepoModal()}>Cancel</Button>
                        </ModalFooter>
                    </Modal>

                    {/* Save template modal */}
                    <Modal isOpen={this.saveAsTemplateModal} toggle={(e) => this.toggleSaveAsTemplateModal()}>
                        <ModalHeader toggle={(e) => this.toggleSaveAsTemplateModal()}>Save pipeline as template</ModalHeader>
                        <ModalBody>
                            <FormGroup>
                                <Label for={"txt-template-category"}>Category</Label>
                                <Input type="text" name={"txt-template-category"} id={"txt-template-category"} onChange={(e) => { this.saveAsCategory = e.target.value }} ></Input>
                            </FormGroup>
                            <FormGroup>
                                <Label for={"txt-template-name"}>Name</Label>
                                <Input type="text" name={"txt-template-name"} id={"txt-template-name"} onChange={(e) => { this.saveAsName = e.target.value }} ></Input>
                            </FormGroup>
                            <FormGroup>
                                <Label for={"txt-template-description"}>Description</Label>
                                <Input type="textarea" name={"txt-template-description"} id={"txt-template-description"} onChange={(e) => { this.saveAsDescription = e.target.value }} ></Input>
                            </FormGroup>
                        </ModalBody>
                        <ModalFooter>
                            <Button color="primary" onClick={(e) => this.saveAsTemplate()}>Save</Button>
                            <Button color="secondary" onClick={(e) => this.toggleSaveAsTemplateModal()}>Cancel</Button>
                        </ModalFooter>
                    </Modal>

                    {/* Save repo modal */}
                    <Modal isOpen={this.saveAsRepoModal} toggle={(e) => this.toggleSaveAsRepoModal()}>
                        <ModalHeader toggle={(e) => this.toggleSaveAsRepoModal()}>Save pipeline in repository</ModalHeader>
                        <ModalBody>
                            <FormGroup>
                                <Label for={"txt-template-category"}>Category</Label>
                                <Input type="text" name={"txt-template-category"} id={"txt-template-category"} onChange={(e) => { this.saveAsCategory = e.target.value }} ></Input>
                            </FormGroup>
                            <FormGroup>
                                <Label for={"txt-template-name"}>Name</Label>
                                <Input type="text" name={"txt-template-name"} id={"txt-template-name"} onChange={(e) => { this.saveAsName = e.target.value }} ></Input>
                            </FormGroup>
                            <FormGroup>
                                <Label for={"txt-template-description"}>Description</Label>
                                <Input type="textarea" name={"txt-template-description"} id={"txt-template-description"} onChange={(e) => { this.saveAsDescription = e.target.value }} ></Input>
                            </FormGroup>
                            <FormGroup check inline>
                                <Input type="checkbox" id={"txt-repo-public"} checked={this.saveAsRepoPublic} onChange={(e) => { this.saveAsRepoPublic = e.target.checked, console.log(this.saveAsRepoPublic) }} ></Input>
                                <Label check>
                                    Public repository
                                </Label>

                            </FormGroup>
                        </ModalBody>
                        <ModalFooter>
                            <Button color="primary" onClick={(e) => this.saveAsRepo()}>Save</Button>
                            <Button color="secondary" onClick={(e) => this.toggleSaveAsRepoModal()}>Cancel</Button>
                        </ModalFooter>
                    </Modal>

                    {/* Export Dsl modal */}
                    <Modal isOpen={this.exportDSLModal} toggle={(e) => this.toggleExportDSLModal()}>
                        <ModalHeader toggle={(e) => this.toggleExportDSLModal()}>Export pipeline as DSL</ModalHeader>
                        <ModalBody>
                            <FormGroup>
                                <Label for={"txt-export-dsl-name"}>DSL to export</Label>
                                <Input type="select" name={"txt-export-dsl-name"} id={"txt-export-dsl-name"} onChange={(e) => { this.selectedDSLToExport = e.target.value }}>
                                    {this.props.availableDSLs ? this.props.availableDSLs.map(dsl => <option value={dsl.name}>{dsl.name}</option>) : null}
                                </Input>
                            </FormGroup>
                        </ModalBody>
                        <ModalFooter>
                            <Button color="primary" onClick={(e) => this.exportCanvasAsDSL()}>Export</Button>
                            <Button color="secondary" onClick={(e) => this.toggleExportDSLModal()}>Cancel</Button>
                        </ModalFooter>
                    </Modal>
                </div>
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(CanvasPane);
