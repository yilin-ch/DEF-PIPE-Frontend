import * as React from 'react';
import { ICanvasShapeConnectionPoint, ICanvasShape, ICanvasConnectionPointType } from '../models';
import { Rect, Text, Arrow, Circle, Ellipse, RegularPolygon, Line } from 'react-konva';
import { CanvasSettings } from '../constants';

export class CanvasRenderer {
    public static renderGrid() {
        let lines: Array<Array<number>> = [];
        let canvasContainer = document.getElementById('canvas-container');
        if (canvasContainer) {
            let canvasWidth = canvasContainer.scrollWidth;
            let canvasHeight = canvasContainer.scrollHeight;
            let x = 0;
            while (x < canvasWidth) {
                lines.push([x, 0, x, canvasHeight]);
                x += CanvasSettings.gridBlockSize;
            }

            let y = 0;
            while (y < canvasHeight) {
                lines.push([0, y, canvasWidth, y]);
                y += CanvasSettings.gridBlockSize;
            }
        }


        return lines.map((line, index) => {
            let strokeColor = index % 5 === 0 ? '#ccc' : '#ddd';
            return <Line points={line} stroke={strokeColor}></Line>
        });
    }

    public static renderShapeComponent(shape: ICanvasShape, isSelectedShape: boolean, containerExpandCallback: (e, shape: ICanvasShape) => void) {
        switch (shape.shape) {
            case "Container":
                return [
                    <Rect width={shape.width} height={shape.height} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground} ></Rect>,
                    <Rect x={shape.width - 50} y={-20} width={50} height={20} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground}></Rect>,
                    <Text x={shape.width - 50} y={-20} width={50} height={20} text={"..."} align="center" verticalAlign="middle" fontSize={18} fontFamily="Calibri" fill={CanvasSettings.textColor} onClick={(e) => containerExpandCallback(e, shape)}></Text>
                ];

            case "Rectangle":
                return <Rect width={shape.width} height={shape.height} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground} ></Rect>

            case "Ellipse":
                let radiusX = shape.width / 2;
                let radiusY = shape.height / 2;
                return <Ellipse x={radiusX} y={radiusY} radiusX={radiusX} radiusY={radiusY} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground}></Ellipse>

            case "Diamond":
                let radius = shape.width / 2;
                return <RegularPolygon x={radius} y={radius} sides={4} radius={radius} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground}></RegularPolygon>;

            case "Database":
                let ellipseRadiusX = shape.width / 2;
                let ellipseRadiusY = shape.height / 8;
                return [
                    <Ellipse listening={false} x={ellipseRadiusX} y={shape.height - ellipseRadiusY} radiusX={ellipseRadiusX} radiusY={ellipseRadiusY} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground}></Ellipse>,
                    <Rect listening={false} x={0} y={ellipseRadiusY} width={shape.width} height={shape.height - (ellipseRadiusY * 2)} stroke={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground} ></Rect>,
                    <Line listening={false} points={[0, ellipseRadiusY, 0, shape.height - ellipseRadiusY]} stroke={CanvasSettings.shapeStroke}></Line>,
                    <Line listening={false} points={[shape.width, ellipseRadiusY, shape.width, shape.height - ellipseRadiusY]} stroke={CanvasSettings.shapeStroke}></Line>,
                    <Ellipse listening={false} x={ellipseRadiusX} y={ellipseRadiusY} radiusX={ellipseRadiusX} radiusY={ellipseRadiusY} stroke={CanvasSettings.shapeStroke} fill={isSelectedShape ? CanvasSettings.selectedShapeBackground : CanvasSettings.shapeBackground}></Ellipse>
                ];
        }
    }

    public static renderShapeName(shape: ICanvasShape) {
        return <Text width={shape.width} height={shape.height} text={shape.name} align="center" verticalAlign="middle" fontSize={18} fontFamily={CanvasSettings.fontFamily} fill={CanvasSettings.textColor} ></Text>
    }

    public static renderConnectionPoint(
        point: ICanvasShapeConnectionPoint,
        shape: ICanvasShape,
        isSelectedConnectionPoint: boolean,
        onConnectionPointClick: (e, shape: ICanvasShape, point: ICanvasShapeConnectionPoint) => void) {

        if (point.type === ICanvasConnectionPointType.input) {
            return <Circle x={point.position.x} y={point.position.y} radius={5} stroke={CanvasSettings.shapeStroke} fill={isSelectedConnectionPoint ? CanvasSettings.selectedConnectionPointBackground : CanvasSettings.connectionPointBackground} onClick={(e) => onConnectionPointClick(e, shape, point)}></Circle>
        }
        else {
            return [
                <Circle x={point.position.x} y={point.position.y} radius={5} stroke={CanvasSettings.shapeStroke} fill={isSelectedConnectionPoint ? CanvasSettings.selectedConnectionPointBackground : CanvasSettings.connectionPointBackground} onClick={(e) => onConnectionPointClick(e, shape, point)}></Circle>,
                <Circle listening={false} x={point.position.x} y={point.position.y} radius={2} stroke={CanvasSettings.shapeStroke} fill={CanvasSettings.shapeStroke} onClick={(e) => onConnectionPointClick(e, shape, point)}></Circle>
            ];
        }
    }
}