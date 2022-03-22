import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Form, FormGroup, Input, Label } from 'reactstrap';
import { ICanvasElementProperty, ICanvasElementPropertyType, ICanvasElementType, ICanvasShape } from '../../../models';
import { ApplicationState } from '../../../store';
import * as CanvasStore from '../../../store/Canvas';

type PropertyPaneProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators &
    RouteComponentProps<{}>;

class PropertyPane extends React.PureComponent<PropertyPaneProps> {
    onPropertyChange(e: React.ChangeEvent<HTMLInputElement>, prop: ICanvasElementProperty) {
        let updatedElement = { ...this.props.selectedElement } as ICanvasShape;
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
                    <Input type="text" name={propUniqueId} id={propUniqueId} value={prop.value} onChange={(e) => this.onPropertyChange(e, prop)} />
                        </FormGroup>;
            case ICanvasElementPropertyType.multiLineText:
                return <FormGroup>
                    <Label for={propUniqueId}>{prop.name}</Label>
                    <Input type="textarea" name={propUniqueId} id={propUniqueId} value={prop.value} onChange={(e) => this.onPropertyChange(e, prop)} size={7}/>
                </FormGroup>;
            case ICanvasElementPropertyType.select:
                return <FormGroup>
                    <Label for={propUniqueId}>{prop.name}</Label>
                    <Input type="select" name={propUniqueId} id={propUniqueId} value={prop.value} onChange={(e) => this.onPropertyChange(e, prop)} >
                        {prop.options ? prop.options.map(op => <option>{op}</option>) : null}
                    </Input>
                </FormGroup>;
        }
    }

    public render() {
        let selectedShape = this.props.selectedElement && this.props.selectedElement.type === ICanvasElementType.Shape ? (this.props.selectedElement as ICanvasShape) : null;
        return (
            <React.Fragment>
                { selectedShape ? 
                    <React.Fragment>
                        <h3 className="property-pane-header">{selectedShape.name}</h3>
                        <p className="property-pane-subheader">ID: {selectedShape.id}</p>
                        <Form>
                            {selectedShape.properties.filter(p => p.allowEditing).map(prop => this.renderProperty(prop))}
                        </Form>
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
