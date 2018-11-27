import React, {Component} from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Logo from '../logo/logo'
import styles from './styles';
import { withStyles } from '@material-ui/core';

class Navbar extends Component {
    render() {
      const { classes } = this.props;

       return (
        <AppBar position="static" className={classes.navbar}>
          <Toolbar>
            <Logo show={this.props.isLoggedIn} size={32}/>
          </Toolbar>
        </AppBar>
        );
    }
}

export default withStyles(styles)(Navbar);