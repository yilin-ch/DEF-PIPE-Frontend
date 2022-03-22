import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import PipelineDesigner from './components/PipelineDesigner/PipelineDesigner';
import TemplateDesigner from './components/TemplateDesigner/TemplateDesigner';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={PipelineDesigner} />
        <Route exact ath='/template-designer' component={TemplateDesigner} />
    </Layout>
);
