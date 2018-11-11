import React, {Component} from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Logo from '../logo/logo'

export default class Navbar extends Component {
    render() {
       return (
        <div className="navbar">
        <AppBar position="static" color="primary">
          <Toolbar>
            <Logo show={this.props.isLoggedIn} size={32}/>
          </Toolbar>
        </AppBar>
      </div>
        );
    }
}
