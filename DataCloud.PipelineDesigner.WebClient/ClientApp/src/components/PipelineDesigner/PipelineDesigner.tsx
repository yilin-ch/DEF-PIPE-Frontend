import * as React from 'react';
import { connect } from 'react-redux';
import PalettePane from './panes/PalettePane';
import CanvasPane from './panes/CanvasPane';
import PropertyPane from './panes/PropertyPane';
import "./../../style/common.css";
import {Col, Row} from "reactstrap";

const PipelineDesigner = () => {
    return (
        <Row className="designer-container">
            <Col md="2" className="palette-pane-container designer-pane">
                <PalettePane/>
            </Col>
            <Col className="canvas-pane-container designer-pane">
                <CanvasPane/>
            </Col>
            <Col md="3" className="property-pane-container designer-pane">
                <PropertyPane/>
            </Col>
        </Row>
    )
};

export default connect()(PipelineDesigner);
