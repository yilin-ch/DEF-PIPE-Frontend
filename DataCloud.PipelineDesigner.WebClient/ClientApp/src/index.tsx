import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom/client';
import {Provider} from 'react-redux';
import {ConnectedRouter} from 'connected-react-router';
import {createBrowserHistory} from 'history';
import configureStore from './store/configureStore';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { BrowserRouter as Router } from 'react-router-dom';
import KeycloakService from "./services/KeycloakService";

// Create browser history to use in the Redux store
const history = createBrowserHistory();

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore(history);
const root = ReactDOM.createRoot(document.getElementById("root"));



const renderApp = () => root.render(
    <React.StrictMode>
        <Provider store={store}>
            <Router>
                <App/>
            </Router>
        </Provider>
    </React.StrictMode>,
);

KeycloakService.initKeycloak(renderApp);

registerServiceWorker();
