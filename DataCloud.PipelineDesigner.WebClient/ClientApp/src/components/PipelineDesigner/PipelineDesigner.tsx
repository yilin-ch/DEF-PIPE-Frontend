import * as React from 'react';
import { connect } from 'react-redux';
import { Col, Row } from 'reactstrap';
import PalettePane from './panes/PalettePane';
import CanvasPane from './panes/CanvasPane';
import PropertyPane from './panes/PropertyPane';
import "./../../style/common.css";

const PipelineDesigner = () => (
    <Row className="designer-container">
        <Col md="2" className="palette-pane-container designer-pane">
            <PalettePane location={null} history={null} match={null}></PalettePane>
        </Col>
        <Col className="canvas-pane-container designer-pane">
            <CanvasPane location={null} history={null} match={null}></CanvasPane>
        </Col>
        <Col md="2" className="property-pane-container designer-pane">
            <PropertyPane location={null} history={null} match={null}></PropertyPane>
        </Col>
    </Row>
);

export default connect()(PipelineDesigner);
