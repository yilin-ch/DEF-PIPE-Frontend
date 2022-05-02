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
                width: 300,
                height: 200,
                shape: "Rectangle",
                properties: [],
                connectionPoints: []
            }
        };
        this.props.addTemplate(template);
    }

    public render() {
        return (
            <React.Fragment>
                <Input className="palette-searchbox" type="text" placeholder="Search components..." onChange={this.onFilterChanged.bind(this)} />

                {this.props.templateGroups ? this.props.templateGroups.map(group =>
                    <React.Fragment>
                        <p className="palette-group-header">{group.name} <Button className="addButton" onClick={(e) => this.onAddNewTemplate(group.name)}>Add</Button></p>
                        {group.items.map(item =>
                            <p className="palette-group-item" onClick={() => this.onTemplateClicked(item)}>
                                {item.name}
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
