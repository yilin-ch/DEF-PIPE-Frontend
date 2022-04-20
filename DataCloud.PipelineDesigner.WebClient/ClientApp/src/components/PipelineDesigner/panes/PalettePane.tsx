import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { connect } from 'react-redux';
import { Input } from 'reactstrap';
import * as CanvasStore from '../../../store/Canvas';
import { ApplicationState } from '../../../store';
import { ICanvasElementType, ICanvasShapeTemplate, ICanvasShape, IAPiTemplate } from '../../../models';
import { v4 as uuidv4 } from 'uuid';

type PaletteProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators &
    RouteComponentProps<{}>;

class PalettePane extends React.PureComponent<PaletteProps> {


    onFilterChanged(e: React.ChangeEvent<HTMLInputElement>) {
        this.props.filterTemplates(e.target.value);
    }

    onTemplateClicked(template: IAPiTemplate) {
        let newShape: ICanvasShape = {
            ...template.canvasTemplate,
            name: template.name,
            properties: template.canvasTemplate.properties.map(p => ({ ...p })),
            id: uuidv4(),
            templateId: template.id,
            type: ICanvasElementType.Shape,
            width: template.canvasTemplate.width,
            height: template.canvasTemplate.height,
            shape: template.canvasTemplate.shape,
            position: { x: 500, y: 500 },
            canHaveChildren: template.canvasTemplate.isContainer,
            elements: template.canvasTemplate.elements || []
        };

        this.props.addElement(newShape);
    }

    onTemplateDragStarted(template: IAPiTemplate) {
        this.props.dragTemplate(template);
    }

    onTemplateDragEnded(template: IAPiTemplate) {

    }

    public render() {
        return (
            <React.Fragment>
                <Input className="palette-searchbox" type="text" placeholder="Search components..." onChange={this.onFilterChanged.bind(this)} />
                {this.props.templateGroups ? this.props.templateGroups.map(group =>
                    <React.Fragment>
                        <p className="palette-group-header">{group.name}</p>
                        {group.items.map(item =>
                            <p className="palette-group-item" onClick={() => this.onTemplateClicked(item)} draggable onDragStart={(e) => this.onTemplateDragStarted(item)} onDragEnd={(e) => this.onTemplateDragEnded(item)}>{item.name}</p>
                        )}
                    </React.Fragment>
                ) : null}
                {this.props.repoGroups ? this.props.repoGroups.map(group =>
                    <React.Fragment>
                        <p className="palette-group-header">{group.name}</p>
                        {group.items.map(item =>
                            <p className="palette-group-item" onClick={() => this.onTemplateClicked(item)} draggable onDragStart={(e) => this.onTemplateDragStarted(item)} onDragEnd={(e) => this.onTemplateDragEnded(item)}>{item.name}</p>
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
)(PalettePane);
