import * as React from 'react';
import { connect } from 'react-redux';
import { Col, Row } from 'reactstrap';
import TemplatePalettePane from './panes/TemplatePalettePane';
import TemplateCanvasPane from './panes/TemplateCanvasPane';
import TemplatePropertyPane from './panes/TemplatePropertyPane';
import "./../../style/common.css";

const TemplateDesigner = () => (
    <Row className="designer-container">
        <Col md="2" className="palette-pane-container designer-pane">
            <TemplatePalettePane/>
        </Col>
        <Col className="canvas-pane-container designer-pane">
            <TemplateCanvasPane/>
        </Col>
        <Col md="2" className="property-pane-container designer-pane">
            <TemplatePropertyPane/>
        </Col>
    </Row>
);

export default connect()(TemplateDesigner);
