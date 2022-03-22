import { Action, Reducer } from 'redux';
import { ICanvasElement, ICanvasShapeTemplateGroup, ICanvasElementPropertyType, ICanvasShapeTemplate, ICanvasShapeConnectionPoint, ICanvasShape, ICanvasConnector, ICanvasElementType, ICanvasPosition, ICanvasShapeSide } from '../models';
import { v4 as uuidv4 } from 'uuid';
import { CanvasSettings } from '../constants';

export class CanvasService {

    exportAsJson(rootShape: ICanvasShape, fileName: string = "pipeline.json") {
        var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(rootShape));
        console.log(rootShape);
        var downloadAnchorNode = document.createElement('a');
        downloadAnchorNode.setAttribute("href", dataStr);
        downloadAnchorNode.setAttribute("download", fileName);
        document.body.appendChild(downloadAnchorNode); // required for firefox
        downloadAnchorNode.click();
        downloadAnchorNode.remove();
    }

    exportAsDSL(rootShape: ICanvasShape, fileName: string = "pipeline-dsl.txt") {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/export/dsl", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                var data = new Blob([xhr.responseText], { type: 'text/plain' });
                var dataStr = window.URL.createObjectURL(data);

                var downloadAnchorNode = document.createElement('a');                
                downloadAnchorNode.setAttribute("href", dataStr);
                downloadAnchorNode.setAttribute("download", fileName);
                document.body.appendChild(downloadAnchorNode); // required for firefox
                downloadAnchorNode.click();
                downloadAnchorNode.remove();
            }
        };
        var data = JSON.stringify(rootShape);
        xhr.send(data);
    }

    getDistance(pointA: ICanvasPosition, pointB: ICanvasPosition) {
        return Math.sqrt(Math.pow(pointA.x - pointB.x, 2) + Math.pow(pointA.y - pointB.y, 2));
    }

    getPointSideOfShape(point: ICanvasPosition, shape: ICanvasShape): ICanvasShapeSide {
        let distances = {
            left: this.getDistance(point, { x: shape.position.x, y: point.y }),
            up: this.getDistance(point, { x: point.x, y: shape.position.y }),
            right: this.getDistance(point, { x: shape.position.x + shape.width, y: point.y }),
            down: this.getDistance(point, { x: point.x, y: shape.position.y + shape.height })
        }

        let minDistance = Math.min(distances.left, distances.right, distances.up, distances.down);

        if (minDistance === distances.left)
            return ICanvasShapeSide.left;
        if (minDistance === distances.right)
            return ICanvasShapeSide.right;
        if (minDistance === distances.up)
            return ICanvasShapeSide.up;

        return ICanvasShapeSide.down;
    }    

    public pointInCircle(x, y, cx, cy, radius) {
        var distancesquared = (x - cx) * (x - cx) + (y - cy) * (y - cy);
        return distancesquared <= radius * radius;
    }

    public snapToGrid(position: ICanvasPosition) {
        return {
            x: Math.round(position.x / CanvasSettings.gridBlockSize) * CanvasSettings.gridBlockSize,
            y: Math.round(position.y / CanvasSettings.gridBlockSize) * CanvasSettings.gridBlockSize
        }
    }

    public calculateConnectorPoints(connector: ICanvasConnector, srcShape: ICanvasShape, desShape: ICanvasShape) {
        let srcPos = srcShape.connectionPoints.filter(point => point.id === connector.sourceConnectionPointId)[0].position;
        let desPos = desShape.connectionPoints.filter(point => point.id === connector.destConnectionPointId)[0].position;

        srcPos = { x: srcShape.position.x + srcPos.x, y: srcShape.position.y + srcPos.y };
        desPos = { x: desShape.position.x + desPos.x, y: desShape.position.y + desPos.y };

        let points: Array<ICanvasPosition> = [srcPos];
        
        let srcSide = this.getPointSideOfShape(srcPos, srcShape);
        let desSide = this.getPointSideOfShape(desPos, desShape);

        switch (srcSide) {
            case ICanvasShapeSide.left:
                switch (desSide) {
                    case ICanvasShapeSide.left:
                        points.push({
                            x: Math.min(
                                srcPos.x - CanvasSettings.minConnectorLength,
                                desPos.x - CanvasSettings.minConnectorLength),
                            y: srcPos.y
                        })
                        points.push({
                            x: Math.min(
                                srcPos.x - CanvasSettings.minConnectorLength,
                                desPos.x - CanvasSettings.minConnectorLength),
                            y: desPos.y
                        })
                        break;
                    case ICanvasShapeSide.right:
                        points.push({
                            x: Math.min(
                                srcPos.x - CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: srcPos.y
                        });
                        points.push({
                            x: Math.min(
                                srcPos.x - CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.max(
                                desPos.x + CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.max(
                                desPos.x + CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: desPos.y
                        });
                        break;
                    case ICanvasShapeSide.up:
                        if (srcPos.x > desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.min(
                                    srcPos.x - CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.min(
                                    srcPos.x - CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: desPos.y - CanvasSettings.minConnectorLength
                            });
                        }
                        
                        points.push({
                            x: desPos.x,
                            y: desPos.y - CanvasSettings.minConnectorLength
                        });

                        break;
                    case ICanvasShapeSide.down:
                        if (srcPos.x > desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.min(
                                    srcPos.x - CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.min(
                                    srcPos.x - CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: desPos.y + CanvasSettings.minConnectorLength
                            });
                        }
                        
                        points.push({
                            x: desPos.x,
                            y: desPos.y + CanvasSettings.minConnectorLength
                        });
                        break;
                }

                break;
            case ICanvasShapeSide.right:
                switch (desSide) {
                    case ICanvasShapeSide.left:
                        points.push({
                            x: Math.max(
                                srcPos.x + CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: srcPos.y
                        });
                        points.push({
                            x: Math.max(
                                srcPos.x + CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.min(
                                desPos.x - CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: (srcPos.y + desPos.y) / 2
                        });
                        points.push({
                            x: Math.min(
                                desPos.x - CanvasSettings.minConnectorLength,
                                (srcPos.x + desPos.x) / 2),
                            y: desPos.y
                        });

                        break;
                    case ICanvasShapeSide.right:
                        points.push({
                            x: Math.max(
                                srcPos.x + CanvasSettings.minConnectorLength,
                                desPos.x + CanvasSettings.minConnectorLength),
                            y: srcPos.y
                        })
                        points.push({
                            x: Math.max(
                                srcPos.x + CanvasSettings.minConnectorLength,
                                desPos.x + CanvasSettings.minConnectorLength),
                            y: desPos.y
                        })                       
                        break;
                    case ICanvasShapeSide.up:
                        if (srcPos.x < desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.max(
                                    srcPos.x + CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.max(
                                    srcPos.x + CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: desPos.y - CanvasSettings.minConnectorLength
                            });
                        }

                        points.push({
                            x: desPos.x,
                            y: desPos.y - CanvasSettings.minConnectorLength
                        });

                        break;
                    case ICanvasShapeSide.down:
                        if (srcPos.x < desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: desPos.x, y: srcPos.y });
                        }
                        else {
                            points.push({
                                x: Math.max(
                                    srcPos.x + CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: srcPos.y
                            });
                            points.push({
                                x: Math.max(
                                    srcPos.x + CanvasSettings.minConnectorLength,
                                    (srcPos.x + desPos.x) / 2),
                                y: desPos.y + CanvasSettings.minConnectorLength
                            });
                        }

                        points.push({
                            x: desPos.x,
                            y: desPos.y + CanvasSettings.minConnectorLength
                        });
                        break;
                }

                break;
            case ICanvasShapeSide.up:
                switch (desSide) {
                    case ICanvasShapeSide.left:
                        if (srcPos.x < desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.min(
                                    srcPos.y - CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x - CanvasSettings.minConnectorLength,
                                y: Math.min(
                                    srcPos.y - CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                        }

                        points.push({
                            x: desPos.x - CanvasSettings.minConnectorLength,
                            y: desPos.y 
                        });
                        break;
                    case ICanvasShapeSide.right:
                        if (srcPos.x > desPos.x && srcPos.y > desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.min(
                                    srcPos.y - CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x + CanvasSettings.minConnectorLength,
                                y: Math.min(
                                    srcPos.y - CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                        }

                        points.push({
                            x: desPos.x + CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case ICanvasShapeSide.up:
                        points.push({
                            x: srcPos.x,
                            y: Math.min(
                                srcPos.y - CanvasSettings.minConnectorLength,
                                desPos.y - CanvasSettings.minConnectorLength),
                        })
                        points.push({
                            x: desPos.x,
                            y: Math.min(
                                srcPos.y - CanvasSettings.minConnectorLength,
                                desPos.y - CanvasSettings.minConnectorLength),
                        })
                        break;
                    case ICanvasShapeSide.down:
                        points.push({
                            x: srcPos.x,
                            y: Math.min(
                                srcPos.y - CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.min(
                                srcPos.y - CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.max(
                                desPos.y + CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2),
                        });
                        points.push({
                            x: desPos.x,
                            y: Math.max(
                                desPos.y + CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2)
                        });

                        break;
                }
                break;
            case ICanvasShapeSide.down:
                switch (desSide) {
                    case ICanvasShapeSide.left:
                        if (srcPos.x < desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.max(
                                    srcPos.y + CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x - CanvasSettings.minConnectorLength,
                                y: Math.max(
                                    srcPos.y + CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                        }

                        points.push({
                            x: desPos.x - CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case ICanvasShapeSide.right:
                        if (srcPos.x > desPos.x && srcPos.y < desPos.y) {
                            points.push({ x: srcPos.x, y: desPos.y });
                        }
                        else {
                            points.push({
                                x: srcPos.x,
                                y: Math.max(
                                    srcPos.y + CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                            points.push({
                                x: desPos.x - CanvasSettings.minConnectorLength,
                                y: Math.max(
                                    srcPos.y + CanvasSettings.minConnectorLength,
                                    (srcPos.y + desPos.y) / 2)
                            });
                        }

                        points.push({
                            x: desPos.x + CanvasSettings.minConnectorLength,
                            y: desPos.y
                        });
                        break;
                    case ICanvasShapeSide.up:
                        points.push({
                            x: srcPos.x,
                            y: Math.max(
                                srcPos.y + CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.max(
                                srcPos.y + CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2)
                        });
                        points.push({
                            x: (srcPos.x + desPos.x) / 2,
                            y: Math.min(
                                desPos.y - CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2),
                        });
                        points.push({
                            x: desPos.x,
                            y: Math.min(
                                desPos.y - CanvasSettings.minConnectorLength,
                                (srcPos.y + desPos.y) / 2)
                        });

                        break;
                    case ICanvasShapeSide.down:
                        points.push({
                            x: srcPos.x,
                            y: Math.max(
                                srcPos.y + CanvasSettings.minConnectorLength,
                                desPos.y + CanvasSettings.minConnectorLength),
                        })
                        points.push({
                            x: desPos.x,
                            y: Math.max(
                                srcPos.y + CanvasSettings.minConnectorLength,
                                desPos.y + CanvasSettings.minConnectorLength),
                        })
                        break;
                }
                break;
        }

        points.push(desPos);

        return points;
    }
}