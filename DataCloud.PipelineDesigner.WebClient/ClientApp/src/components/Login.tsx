import * as React from 'react'
import {connect} from "react-redux";
import { useCallback } from 'react'
import { useNavigate , useLocation } from 'react-router'
import { useKeycloak } from '@react-keycloak/web'
import KeycloakService from "../services/KeycloakService";


const Login = () => {

    return (

        <div>
            <button type="button" onClick={()=> KeycloakService.doLogin()}>
                Login
            </button>
        </div>
    )
};

export default connect()(Login);
