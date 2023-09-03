import * as React from 'react';
import { connect } from 'react-redux';
import * as CanvasStore from '../../../store/Canvas';
import { ApplicationState } from '../../../store';
import { Group, Layer, Rect, Stage, Text, Arrow, Circle, KonvaNodeComponent, Ellipse, RegularPolygon, Line } from 'react-konva';
import { ICanvasElementPropertyType, ICanvasShape, ICanvasElementType, ICanvasConnector, ICanvasShapeConnectionPoint, ICanvasElement, ICanvasShapeTemplate, IAPiTemplate } from '../../../models';
import Konva from 'konva';
import { Button, ButtonGroup } from 'reactstrap';
import { CanvasSettings } from '../../../constants';
import { CanvasRenderer } from '../../../services/CanvasRenderer';

type TemplateCanvasProps =
    CanvasStore.CanvasState &
    typeof CanvasStore.actionCreators;

class TemplateCanvasPane extends React.PureComponent<TemplateCanvasProps> {

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

    renderTemplate(template: IAPiTemplate) {
        if (!template) return null;

        let shape: ICanvasShape = {
            id: '',
            name: template.name,
            type: ICanvasElementType.Shape,
            connectionPoints: template.canvasTemplate.connectionPoints,
            properties: template.canvasTemplate.properties,
            width: template.canvasTemplate.width,
            height: template.canvasTemplate.height,
            shape: template.canvasTemplate.shape,
            position: { x: 300, y: 300 }
        };

        return this.renderCanvasShape(shape);
    }

    renderCanvasShape(shape: ICanvasShape) {
        let isSelectedShape = false;
        if (this.props.selectedElement && this.props.selectedElement.id === shape.id) {
            isSelectedShape = true;
        }

        return <Group x={shape.position.x} y={shape.position.y} onClick={(e) => this.onShapeClick(e, shape)}>
            {CanvasRenderer.renderShapeComponent(shape, isSelectedShape, () => { })}
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

    public render() {

        return (
            <React.Fragment>
                <div id="canvas-container" className="canvas-container" tabIndex={1} onKeyDown={(e: React.KeyboardEvent) => this.onKeyDown(e)} onDragOver={(e) => e.preventDefault()}>
                    <Stage width={window.innerWidth} height={window.innerHeight} onClick={(e) => this.props.deselectElement()}>
                        <Layer>
                            {CanvasRenderer.renderGrid()}
                        </Layer>
                        <Layer>
                            {this.renderTemplate(this.props.selectedTemplate)}
                        </Layer>
                    </Stage>
                </div>
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.canvas,
    CanvasStore.actionCreators
)(TemplateCanvasPane);
