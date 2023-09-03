﻿import * as React from 'react';
import { connect } from 'react-redux';
import { Button, Form, FormGroup, Input, Label, Nav, NavItem, NavLink, TabContent, TabPane } from 'reactstrap';
import { ICanvasConnectionPointType, ICanvasElementProperty, ICanvasElementPropertyType, ICanvasElementType, ICanvasShape, ICanvasShapeConnectionPoint, ICanvasShapeTemplate, IAPiTemplate } from '../../../models';
import { ApplicationState } from '../../../store';
import * as CanvasStore from '../../../store/Canvas';
import { v4 as uuidv4 } from 'uuid';
import { JSONEditor, Schema } from "react-schema-based-json-editor";
import schemas from '../../schemas.json'

type TemplatePropertyPaneProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;


class TemplatePropertyPane extends React.PureComponent<TemplatePropertyPaneProps> {


    toggle(tab: string) {
        if (this.props.selectedTab !== tab) {
            this.props.selectTab(tab);
        }
    }

    onNameChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate: IAPiTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.name = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }
    onCategoryChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.category = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }
    onWidthChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.canvasTemplate.width = parseInt(e.target.value);

        this.props.updateTemplate(updatedTemplate);
    }
    onHeightChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.canvasTemplate.height = parseInt(e.target.value);

        this.props.updateTemplate(updatedTemplate);
    }
    onShapeChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.canvasTemplate.shape = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }
    onTypeChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.canvasTemplate.conditional = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }
    onConditionChange(e: React.ChangeEvent<HTMLInputElement>) {
        let updatedTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.canvasTemplate.condition = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }

    onPropertyNameChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        updatedTemplate.canvasTemplate.properties.filter(x => x.name === prop.name)[0].name = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }
    onPropertyTypeChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        updatedTemplate.canvasTemplate.properties.filter(x => x.name === prop.name)[0].type = parseInt(e.target.value) as ICanvasElementPropertyType;

        this.props.updateTemplate(updatedTemplate);
    }
    onPropertyValueChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        updatedTemplate.canvasTemplate.properties.filter(x => x.name === prop.name)[0].value = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }
    onPropertAllowEditChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        updatedTemplate.canvasTemplate.properties.filter(x => x.name === prop.name)[0].allowEditing = e.target.checked;

        this.props.updateTemplate(updatedTemplate);
    }

    onConnectionPointChangeX(e: React.ChangeEvent<HTMLInputElement>, point: ICanvasShapeConnectionPoint) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        let pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(x => x.id === point.id)[0]
        pointToBeUpdated.position.x = parseInt(e.target.value);

        this.props.updateTemplate(updatedTemplate);
    }
    onConnectionPointChangeY(e: React.ChangeEvent<HTMLInputElement>, point: ICanvasShapeConnectionPoint) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        let pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(x => x.id === point.id)[0]
        pointToBeUpdated.position.y = parseInt(e.target.value);

        this.props.updateTemplate(updatedTemplate);
    }

    onConnectionPointChangeType(e: React.ChangeEvent<HTMLInputElement>, point: ICanvasShapeConnectionPoint) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        let pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(x => x.id === point.id)[0]
        pointToBeUpdated.type = parseInt(e.target.value);

        this.props.updateTemplate(updatedTemplate);
    }

    onConnectionPointChangeCondition(e: React.ChangeEvent<HTMLInputElement>, point: ICanvasShapeConnectionPoint) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        let pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(x => x.id === point.id)[0]
        pointToBeUpdated.condition = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }

    onConnectionPointChangeCase(e: React.ChangeEvent<HTMLInputElement>, point: ICanvasShapeConnectionPoint) {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        let pointToBeUpdated = updatedTemplate.canvasTemplate.connectionPoints.filter(x => x.id === point.id)[0]
        pointToBeUpdated.case = e.target.value;

        this.props.updateTemplate(updatedTemplate);
    }

    addProperty() {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        updatedTemplate.canvasTemplate.properties.push({ name: 'New property', type: ICanvasElementPropertyType.singleLineText });

        this.props.updateTemplate(updatedTemplate);
    }

    onParametersUpdate = (value: any, isValid: boolean) => {
        let updatedTemplate: IAPiTemplate = { ...this.props.selectedTemplate };
        updatedTemplate.canvasTemplate.parameters = value;
        this.props.updateTemplate(updatedTemplate);
    };

    addConnectionPoint() {
        let updatedTemplate = { ...this.props.selectedTemplate } as IAPiTemplate;
        updatedTemplate.canvasTemplate.connectionPoints.push({
            id: uuidv4(),
            type: ICanvasConnectionPointType.input,
            position: {
                x: 10,
                y: 10
            }
        })

        this.props.updateTemplate(updatedTemplate);
    }

    renderProperty(prop: ICanvasElementProperty) {
        if (!this.props.selectedTemplate) return null;
        let propNameUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_name';
        let propTypeUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_type';
        let propValueUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_value';
        let propAllowEditUniqueId = this.props.selectedTemplate.id + "-" + prop.name + '_allowedit';

        return <FormGroup>
            <Label for={propNameUniqueId}>Property Name</Label>
            <Input type="text" name={propNameUniqueId} id={propNameUniqueId} value={prop.name} onChange={(e) => this.onPropertyNameChange(e, prop)} />
            <Label for={propTypeUniqueId}>Property Type</Label>
            <Input type="select" name={propTypeUniqueId} id={propTypeUniqueId} value={prop.type} onChange={(e) => this.onPropertyTypeChange(e, prop)}>
                <option value="0">Single line of text</option>
                <option value="1">Multiple lines of text</option>
                <option value="2">Options</option>
            </Input>
            <Label for={propValueUniqueId}>Property Value</Label>
            <Input type="text" name={propValueUniqueId} id={propValueUniqueId} value={prop.value} onChange={(e) => this.onPropertyValueChange(e, prop)} />

            <Label for={propAllowEditUniqueId} className="checkbox-label">
                <Input type="checkbox" name={propAllowEditUniqueId} id={propAllowEditUniqueId} checked={prop.allowEditing} onChange={(e) => this.onPropertAllowEditChange(e, prop)} /> {' '}
                Editable
            </Label>
        </FormGroup>;
    }

    renderConnectionPoint(point: ICanvasShapeConnectionPoint) {
        return <React.Fragment>
            <h5>{point.id}</h5>
            <FormGroup>
                <Label for={point.id + '_x'}>Position X</Label>
                <Input type="number" name={point.id + '_x'} id={point.id + '_x'} value={point.position.x} onChange={(e) => this.onConnectionPointChangeX(e, point)} />

                <Label for={point.id + '_y'}>Position Y</Label>
                <Input type="number" name={point.id + '_x'} id={point.id + '_x'} value={point.position.y} onChange={(e) => this.onConnectionPointChangeY(e, point)} />

                <Label for={point.id + '_type'}>Type</Label>
                <Input type="select" name={point.id + '_type'} id={point.id + '_type'} value={point.type} onChange={(e) => this.onConnectionPointChangeType(e, point)}>
                    <option value="0">Input</option>
                    <option value="1">Output</option>
                </Input>

                <Label for={point.id + '_condition'}>Condition</Label>
                <Input type="text" name={point.id + '_condition'} id={point.id + '_condition'} value={point.condition} onChange={(e) => this.onConnectionPointChangeCondition(e, point)} />

                <Label for={point.id + '_case'}>Case</Label>
                <Input type="select" name={point.id + '_case'} id={point.id + '_case'} value={point.case} onChange={(e) => this.onConnectionPointChangeCase(e, point)}>
                    <option value="1">Case-If</option>
                    <option value="0">Case-Else</option>
                </Input>
            </FormGroup>
        </React.Fragment >
    }

    static getSchema(): Schema {
        var schema: Schema = schemas["params"] as Schema;
        schema["properties"]["resourceProvider"]["readonly"] = true;

        return schema;
    };

    public render() {
        let selectedShape = (this.props.selectedTemplate?.canvasTemplate as ICanvasShape);
        return (
            <React.Fragment>
                {this.props.selectedTemplate ?
                    <React.Fragment>
                            <h3 className="property-pane-header">
                                Step Properties
                            </h3>
                            <p className="property-pane-subheader">ID: {this.props.selectedTemplate.id}</p>
                            <Nav tabs>
                                <NavItem>
                                    <NavLink className={this.props.selectedTab === '1' ? 'active' : ''} onClick={() => { this.toggle('1'); }}>Information</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink className={this.props.selectedTab === '2' ? 'active' : ''} onClick={() => { this.toggle('2'); }}>Parameters</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink className={this.props.selectedTab === '3' ? 'active' : ''} onClick={() => { this.toggle('3'); }}>Connections</NavLink>
                                </NavItem>
                            </Nav>
                            <TabContent activeTab={this.props.selectedTab}>
                                <TabPane tabId="1">
                                    <FormGroup>
                                        <Label for={this.props.selectedTemplate.id + '_name'}>Name</Label>
                                        <Input type="text" name={this.props.selectedTemplate.id + '_name'} id={this.props.selectedTemplate.id + '_name'} value={this.props.selectedTemplate.name} onChange={(e) => this.onNameChange(e)} />
                                        <Label for={this.props.selectedTemplate.id + '_type'}>Category</Label>
                                        <Input type="text" name={this.props.selectedTemplate.id + '_type'} id={this.props.selectedTemplate.id + '_type'} value={this.props.selectedTemplate.category} onChange={(e) => this.onCategoryChange(e)} />
                                        <Label for={this.props.selectedTemplate.id + '_functiontype'}>Type</Label>
                                        <Input type="select" name={this.props.selectedTemplate.id + '_functiontype'} id={this.props.selectedTemplate.id + '_functiontype'} value={this.props.selectedTemplate.canvasTemplate.conditional} onChange={(e) => this.onTypeChange(e)}>
                                            <option>Other</option>
                                            <option>Conditional</option>
                                        </Input>
                                        <Label for={this.props.selectedTemplate.id + '_condition'}>Condition</Label>
                                    <Input type="text" name={this.props.selectedTemplate.id + '_condition'} id={this.props.selectedTemplate.id + '_condition'} value={this.props.selectedTemplate.canvasTemplate.condition} onChange={(e) => this.onConditionChange(e)} />
                                        <Label for={this.props.selectedTemplate.id + '_shape'}>Shape</Label>
                                        <Input type="select" name={this.props.selectedTemplate.id + '_shape'} id={this.props.selectedTemplate.id + '_shape'} value={this.props.selectedTemplate.canvasTemplate.shape} onChange={(e) => this.onShapeChange(e)}>
                                            <option>Rectangle</option>
                                            <option>Ellipse</option>
                                            <option>Diamond</option>
                                            <option>Database</option>
                                            <option>Custom</option>
                                        </Input>
                                        <Label for={this.props.selectedTemplate.id + '_width'}>Width</Label>
                                        <Input type="number" name={this.props.selectedTemplate.id + '_width'} id={this.props.selectedTemplate.id + '_width'} value={this.props.selectedTemplate.canvasTemplate.width} onChange={(e) => this.onWidthChange(e)} />
                                        <Label for={this.props.selectedTemplate.id + '_height'}>Height</Label>
                                        <Input type="number" name={this.props.selectedTemplate.id + '_height'} id={this.props.selectedTemplate.id + '_height'} value={this.props.selectedTemplate.canvasTemplate.height} onChange={(e) => this.onHeightChange(e)} />
                                    </FormGroup>
                                </TabPane>
                                <TabPane tabId="2">
                                    {<JSONEditor
                                        key={this.props.selectedTemplate.id}
                                        schema={TemplatePropertyPane.getSchema()}
                                        initialValue={selectedShape.parameters}
                                        updateValue={this.onParametersUpdate}
                                        theme="bootstrap5"
                                        icon="bootstrap-icons"
                                    />}
                                </TabPane>
                                <TabPane tabId="3">
                                    {this.props.selectedTemplate.canvasTemplate.connectionPoints.map(point => this.renderConnectionPoint(point))}
                                    <Button onClick={e => this.addConnectionPoint()}>Add Connection Point</Button>
                                </TabPane>
                            </TabContent>
                            <td>
                                <p className="btn btn-success saveButton" onClick={(e) => {
                                    this.props.saveTemplate(this.props.selectedTemplate)
                                }}>
                                    Save
                                </p>
                            </td>
                            <td>
                                <p className="btn btn-danger removeButton" onClick={(e) => {
                                    this.props.removeTemplate(this.props.selectedTemplate)
                                }}>
                                    Delete
                                </p>
                            </td>
                    </React.Fragment>
                    : null}
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(TemplatePropertyPane);
