import * as React from 'react';
import {connect} from 'react-redux';
import {Button, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import * as CanvasStore from '../../../store/Canvas';
import {ApplicationState} from '../../../store';
import {ICanvasElementType, ICanvasShapeTemplate, ICanvasShape, IAPiTemplate} from '../../../models';
import {v4 as uuidv4} from 'uuid';
import {Menu, Item, Separator, Submenu, useContextMenu, ItemParams} from 'react-contexify';
import 'react-contexify/dist/ReactContexify.css';

type PaletteProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;

class PalettePane extends React.PureComponent<PaletteProps> {


    onFilterChanged(e: React.ChangeEvent<HTMLInputElement>) {
        this.props.filterTemplates(e.target.value);
    }

    onTemplateClicked(template: IAPiTemplate) {
        let newShape: ICanvasShape = {
            ...template.canvasTemplate,
            name: template.name,
            properties: template.canvasTemplate.properties.map(p => ({...p})),
            id: uuidv4(),
            templateId: template.id,
            type: ICanvasElementType.Shape,
            width: template.canvasTemplate.width,
            height: template.canvasTemplate.height,
            shape: template.canvasTemplate.shape,
            position: {x: 500, y: 500},
            canHaveChildren: template.canvasTemplate.isContainer,
            elements: template.canvasTemplate.elements || []
        };

        this.props.addElement(newShape, template.resourceProviders);
    }

    onTemplateEdit(template: IAPiTemplate) {
        this.props.editRepo(template);
        //template.canvasTemplate.elements.forEach((e) => this.props.addElement(e))
    }

    onTemplateDragStarted(template: IAPiTemplate) {
        this.props.dragTemplate(template);
    }

    onTemplateDragEnded(template: IAPiTemplate) {

    }


    rightClick = (e, template: IAPiTemplate) => {
        e.preventDefault();
        const { show } = useContextMenu({
            id: template.id,
        });
        show(e)
    };

    deleteRepo = (params: ItemParams) => {
        this.props.removeRepo(params.data.repo);

    };

    public render() {
        return (
            <React.Fragment>
                <Input className="palette-searchbox" type="text" placeholder="Search components..."
                       onChange={this.onFilterChanged.bind(this)}/>
                {this.props.templateGroups ? this.props.templateGroups.map(group =>
                    <React.Fragment>
                        <p className="palette-group-header">{group.name}</p>
                        {group.items.map(item =>
                           <p className="palette-group-item" onClick={() => this.onTemplateClicked(item)} draggable
                               onDragStart={(e) => this.onTemplateDragStarted(item)}
                               onDragEnd={(e) => this.onTemplateDragEnded(item)}>{item.name}</p>
                        )}
                    </React.Fragment>
                ) : null}

                {this.props.username ?
                    <React.Fragment>
                        <p className="palette-group-username">
                            My repository
                        </p>

                    </React.Fragment> : null}
                {this.props.repoGroups ? this.props.repoGroups.map(group =>
                    <React.Fragment>
                        <p className="palette-group-header">{group.name}</p>
                        {group.items.map(item =>
                            <div><p className="palette-group-item" onContextMenu={(e) => this.rightClick(e, item)}
                                    onClick={() => this.onTemplateClicked(item)} draggable
                                    onDragStart={(e) => this.onTemplateDragStarted(item)}
                                    onDragEnd={(e) => this.onTemplateDragEnded(item)}>
                                {item.public ? <i className="bi bi-unlock" style={{padding: 5, color: '#FF0000'}}/> :
                                    <i className="bi bi-lock-fill" style={{padding: 5}}/>}
                                {item.name}
                            </p>
                                <Menu id={item.id}>
                                    <Item disabled>{item.name}</Item>
                                    <Separator />
                                    <Item onClick={() => this.onTemplateClicked(item)} style={{fontWeight: "bold"}}><i className="bi bi-plus-circle-fill" style={{padding: 5}}/>Add to workflow</Item>
                                    <Item disabled={this.props.currentRepoEdit != null} onClick={() => this.onTemplateEdit(item)}><i className="bi bi-pencil" style={{padding: 5}}/>Edit</Item>
                                    <Item onClick={this.deleteRepo} data={{ repo: item }}><i className="bi bi-trash" style={{padding: 5, color: "red"}}/>Delete</Item>
                                </Menu>
                            </div>
                        )}
                    </React.Fragment>
                ) : null}

            </React.Fragment>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(PalettePane);
