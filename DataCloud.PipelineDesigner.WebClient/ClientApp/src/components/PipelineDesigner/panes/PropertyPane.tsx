import * as React from 'react';
import {connect} from 'react-redux';
import {Form, FormGroup, Input, Label} from 'reactstrap';
import '../../select2.min.css';
import {
    IAPiTemplate, ICanvasConnectionPointType,
    ICanvasElementProperty,
    ICanvasElementPropertyType,
    ICanvasElementType,
    ICanvasShape, ICanvasShapeTemplate, IResourceProvider
} from '../../../models';
import {ApplicationState} from '../../../store';
import * as CanvasStore from '../../../store/Canvas';
import {JSONEditor, Schema} from "react-schema-based-json-editor";
import schemas from '../../schemas.json';

interface MyState {
    initialValue: Object,
    updateValue: Object,
}

type PropertyPaneProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;


class PropertyPane extends React.Component<PropertyPaneProps, MyState> {
    constructor(props) {
        super(props);
        this.state = {
            initialValue: {},
            updateValue: '',
        }
    }

    static getSchema(providers: IResourceProvider[]): Schema {
        var schema: Schema = schemas["params"] as Schema;
        schema["properties"]["resourceProvider"]["enum"] = providers.map(provider => provider.name);

        return schema;
    };

    static getProviderSchema(): Schema {
        var schema: Schema = schemas["providers"] as Schema;
        return schema;
    };

    static getLoopConditionSchema(): Schema {
        var schema: Schema = schemas["loopconditions"] as Schema;
        return schema;
    };

    private updatePropertyValue = (value: any, isValid: boolean) => {
        this.setState({
            updateValue: value
        })
    };

    private updateProviderValue = (value: any, isValid: boolean) => {
        console.log(isValid)
        console.log(value)
        if(isValid){
            this.props.updateProviders(value.providers as Array<IResourceProvider>);
        }

    };

    private savePropertyValue = () => {
        const template = this.props.repo.find(repo => repo.id === this.props.currentRootShape.templateId);
        const index = template.canvasTemplate.elements.findIndex(e => e.id === this.props.selectedElement.id);
        (template.canvasTemplate.elements[index] as ICanvasShape).parameters = this.state.updateValue;

        this.props.addRepo(template);
    };

    onPropertyChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedElement = {...this.props.selectedElement} as ICanvasShape;
        updatedElement.properties.filter(x => x.name === prop.name)[0].value = e.target.value;

        this.props.updateElement(updatedElement);
    }

    private updateStepName(name: string) {
        var element = (this.props.selectedElement as ICanvasShape);
        element.name = name;
        this.props.updateElement(element)
    }

    public render() {
        let selectedShape = this.props.selectedElement && this.props.selectedElement.type === ICanvasElementType.Shape ? (this.props.selectedElement as ICanvasShape) : null;
        console.log(selectedShape?.elements)
        return (
            <React.Fragment>
                {selectedShape ?
                    <React.Fragment>
                        <h3 className="property-pane-header">
                            <div contentEditable="true" onKeyDown={e => e.keyCode == 13 ?? e.currentTarget.blur} onBlur={e => this.updateStepName(e.currentTarget.textContent)}>{selectedShape.name}</div>
                        </h3>
                        <p className="property-pane-subheader">ID: {selectedShape.id}</p>
                        <Label>Loop Pipeline</Label>
                        <Input type="select">
                            <option value="0">No</option>
                            <option value="1">Yes</option>
                        </Input>
                        <Label>Exit Condition</Label>
                        <Input type="text" />

                        { selectedShape.elements?.length > 0 ?
                            null
                            : <JSONEditor
                                key={selectedShape.id}
                                schema={PropertyPane.getSchema(this.props.providers)}
                                initialValue={(selectedShape as ICanvasShape).parameters}
                                updateValue={this.updatePropertyValue}
                                theme="bootstrap5"
                                icon="bootstrap-icons"
                                
                                />}
                    </React.Fragment>
                    :
                    <React.Fragment                    >
                        {this.props.providers && this.props.currentRootShape.elements.length > 0 ?
                            <JSONEditor
                                schema={PropertyPane.getProviderSchema()}
                                initialValue={{providers: this.props.providers}}
                                updateValue={this.updateProviderValue}
                                theme="bootstrap5"
                                icon="bootstrap-icons"/>
                            : null}
                    </React.Fragment>}
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(PropertyPane);
