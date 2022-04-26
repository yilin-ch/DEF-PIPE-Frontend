import * as React from 'react';
import {connect} from 'react-redux';
import {Form, FormGroup, Input, Label} from 'reactstrap';
import {
    IAPiTemplate, ICanvasConnectionPointType,
    ICanvasElementProperty,
    ICanvasElementPropertyType,
    ICanvasElementType,
    ICanvasShape, ICanvasShapeTemplate
} from '../../../models';
import {ApplicationState} from '../../../store';
import * as CanvasStore from '../../../store/Canvas';
import {JSONEditor, Schema} from "react-schema-based-json-editor";

interface MyState {
    initialValue: Object,
    updateValue: Object,
    selectedShape: ICanvasShape
}

type PropertyPaneProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;


class PropertyPane extends React.Component<PropertyPaneProps, MyState> {
    constructor(props) {
        super(props);
        var selectedShape = this.props.selectedElement && this.props.selectedElement.type === ICanvasElementType.Shape ? (this.props.selectedElement as ICanvasShape) : null;
        this.state = {
            initialValue: {},
            updateValue: '',
            selectedShape: selectedShape
        }
    }

    schema: Schema = {
        type: "object",
        properties: {
            "data_source_step": {
                "type": "string"
            },
            "implementation": {
                "type": "string"
            },
            "image": {
                "type": "string"
            },
            "environmentParameters": {
                "type": "array",
                "items": {
                    "type": "object",
                    "properties": {
                        "key": {
                            "type": "string"
                        },
                        "value": {
                            "type": "string"
                        }
                    },
                    "required": [
                        "key",
                        "value"
                    ]
                }
            },
            "resourceProvider": {
                "type": "string"
            },
        },
        "required": [
            "data_source_step",
            "implementation",
            "image",
            "environmentParameters",
            "resourceProvider"
        ]
    }

    private updatePropertyValue = (value: any, isValid: boolean) => {
        this.setState({
            updateValue: value
        })
    };

    private savePropertyValue = () => {
        console.log(this.props.repo)
        var template = this.props.repo.filter(repo => repo.id == this.props.currentRootShape.templateId)[0];
        console.log(template)
        var index = template.canvasTemplate.elements.findIndex(e => e.id = this.props.selectedElement.id);
        (template.canvasTemplate.elements[index] as ICanvasShape).parameters = this.state.updateValue

        this.props.addRepo(template)
    };

    onPropertyChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedElement = {...this.props.selectedElement} as ICanvasShape;
        updatedElement.properties.filter(x => x.name === prop.name)[0].value = e.target.value;

        this.props.updateElement(updatedElement);
    }

    renderProperty(prop: ICanvasElementProperty) {
        if (!this.props.selectedElement) return null;
        let propUniqueId = this.props.selectedElement.id + "-" + prop.name;

        switch (prop.type) {
            case ICanvasElementPropertyType.singleLineText:
                return <FormGroup>
                    <Label for={propUniqueId}>{prop.name}</Label>
                    <Input type="text" name={propUniqueId} id={propUniqueId} value={prop.value}
                           onChange={(e) => this.onPropertyChange(e, prop)}/>
                </FormGroup>;
            case ICanvasElementPropertyType.multiLineText:
                return <FormGroup>
                    <Label for={propUniqueId}>{prop.name}</Label>
                    <Input type="textarea" name={propUniqueId} id={propUniqueId} value={prop.value}
                           onChange={(e) => this.onPropertyChange(e, prop)} size={7}/>
                </FormGroup>;
            case ICanvasElementPropertyType.select:
                return <FormGroup>
                    <Label for={propUniqueId}>{prop.name}</Label>
                    <Input type="select" name={propUniqueId} id={propUniqueId} value={prop.value}
                           onChange={(e) => this.onPropertyChange(e, prop)}>
                        {prop.options ? prop.options.map(op => <option>{op}</option>) : null}
                    </Input>
                </FormGroup>;
        }
    }

    public render() {
        let selectedShape = this.props.selectedElement && this.props.selectedElement.type === ICanvasElementType.Shape ? (this.props.selectedElement as ICanvasShape) : null;
        return (
            <React.Fragment>
                {selectedShape ?
                    <React.Fragment>
                        <h3 className="property-pane-header">{selectedShape.name}</h3>
                        <p className="property-pane-subheader">ID: {selectedShape.id}</p>
                        <Form>
                            {selectedShape.properties.filter(p => p.allowEditing).map(prop => this.renderProperty(prop))}
                        </Form>
                        <td>
                            <p className="btn btn-success saveButton" onClick={(e) => {
                                this.savePropertyValue()
                            }}>
                                Save
                            </p>
                        </td>
                        <td>
                            <p className="btn btn-danger removeButton" onClick={(e) => {
                                this.setState({
                                    updateValue: this.state.initialValue
                                });
                            }}>
                                Reset
                            </p>
                        </td>
                        <div className="from-group">
                            <JSONEditor
                                schema={this.schema}
                                initialValue={(selectedShape as ICanvasShape).parameters}
                                updateValue={this.updatePropertyValue}
                                theme="bootstrap5"
                                icon="bootstrap-icons"/>
                        </div>
                    </React.Fragment>
                    : null}
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(PropertyPane);
