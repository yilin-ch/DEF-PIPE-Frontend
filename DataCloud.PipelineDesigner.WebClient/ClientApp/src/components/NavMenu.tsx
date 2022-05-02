import * as React from 'react';
import {
    Collapse,
    Container, DropdownItem, DropdownMenu, DropdownToggle,
    Navbar,
    NavbarBrand,
    NavbarToggler,
    NavItem,
    NavLink,
    UncontrolledDropdown
} from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import KeycloakService from "../services/KeycloakService";

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    public render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>

                        <NavbarBrand tag={Link} to="/">DataCloud</NavbarBrand>
                        <NavbarToggler onClick={this.toggle} className="mr-2"/>
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark global-nav-link" to="/" active >Pipeline Designer</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark global-nav-link" to="/template-designer" activeClassName="global-nav-selected">Template Designer</NavLink>
                                </NavItem>
                                <UncontrolledDropdown
                                    inNavbar
                                    nav
                                >
                                    <DropdownToggle
                                        caret
                                        nav
                                    >
                                        <i className="bi bi-person-fill" style={{padding: 5}}/>
                                        {KeycloakService.isLoggedIn() ?
                                            <a>{KeycloakService.getUsername()}</a>
                                            : null}
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem onClick={()=> KeycloakService.isLoggedIn() ? KeycloakService.doLogout(): KeycloakService.doLogin()}>
                                            {KeycloakService.isLoggedIn() ?
                                                "Logout"
                                                : "Login"}
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>
                            </ul>
                        </Collapse>
                </Navbar>
            </header>
        );
    }

    private toggle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
}
