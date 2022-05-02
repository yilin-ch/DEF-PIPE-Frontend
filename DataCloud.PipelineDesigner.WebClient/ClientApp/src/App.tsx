import * as React from 'react';
import {Route, Router, Routes} from 'react-router';
import PipelineDesigner from './components/PipelineDesigner/PipelineDesigner';
import TemplateDesigner from './components/TemplateDesigner/TemplateDesigner';

import './custom.css'
import NavMenu from "./components/NavMenu";
import {Container} from "reactstrap";


export default () => {

    return (
        <React.Fragment>
            <NavMenu/>
            <Container fluid={true} className="app-container">
                <Routes>
                    <Route path='/' element={<PipelineDesigner/>}/>
                    <Route path='/repo' element={<PipelineDesigner/>}/>
                    <Route path='/template-designer' element={<TemplateDesigner/>}/>
                </Routes>
            </Container>
        </React.Fragment>
    )
};
