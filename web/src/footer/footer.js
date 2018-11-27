import React, {Component} from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import { withStyles } from '@material-ui/core';
import styles from './styles';

class Footer extends Component {
    render() {
      const { classes } = this.props;

       return (
        <AppBar position="static" className={classes.footer}>
          <Toolbar>
          <div className="copyright">
            Copyright © {(new Date().getFullYear())} Mieszkando
          </div>
          </Toolbar>
        </AppBar>
        );
    }
}

export default withStyles(styles)(Footer);