import * as React from 'react';
import { connect } from 'react-redux';
import { Button, Input } from 'reactstrap';
import * as CanvasStore from '../../../store/Canvas';
import { ApplicationState } from '../../../store';
import { ICanvasElementType, ICanvasShapeTemplate, ICanvasShape, IAPiTemplate } from '../../../models';
import { v4 as uuidv4 } from 'uuid';


type TemplatePaletteProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;

class TemplatePalettePane extends React.PureComponent<TemplatePaletteProps> {

    onFilterChanged(e: React.ChangeEvent<HTMLInputElement>) {
        this.props.filterTemplates(e.target.value);
    }

    onTemplateClicked(template: IAPiTemplate) {
        console.log(this.props.templateGroups)
        this.props.selectTemplate(template);
    }

    onTemplateDeleted(template: IAPiTemplate) {
        this.props.removeTemplate(template);
    }

    onAddNewTemplate(group: string) {
        let template: IAPiTemplate = {
            id: uuidv4(),
            name: "New template",
            description: "",
            category: group,
            canvasTemplate: {
                width: 150,
                height: 100,
                shape: "Rectangle",
                conditional: "Other",
                properties: [],
                connectionPoints: [
                    { id: uuidv4(), position: { x: 0, y: 50 }, type: 0, condition: null },
                    { id: uuidv4(), position: { x: 150, y: 50 }, type: 1, condition: null }
                ]
            },
            resourceProviders: []
        };
        this.props.addTemplate(template);
    }

    public render() {
        return (
            <React.Fragment>
                <Input className="palette-searchbox" type="text" placeholder="Search components..." onChange={this.onFilterChanged.bind(this)} />
                <Button className="addButton" block={true} onClick={(e) => this.onAddNewTemplate("New category")}>Add</Button>
                {this.props.templateGroups ? this.props.templateGroups.map(group =>
                    <React.Fragment>
                        <p className="palette-group-header">{group.name}</p>
                        <Button className="addButton"  onClick={(e) => this.onAddNewTemplate(group.name)}>Add</Button>
                        {group.items.map(item =>
                            <p className="palette-group-item" onClick={() => this.onTemplateClicked(item)}>
                                {item.name} {this.props.selectedTemplate?.id == item.id ? <i className="bi bi-pencil" style={{ padding: 5 }} /> : ""}
                            </p>
                        )}
                    </React.Fragment>
                ) : null}
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(TemplatePalettePane);
