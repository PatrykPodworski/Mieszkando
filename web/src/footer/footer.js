import React, {Component} from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';

export default class Footer extends Component {
    render() {
       return (
        <div className="footer">
        <AppBar position="static" color="secondary">
          <Toolbar>
          <div className="copyright">
            Copyright © {(new Date().getFullYear())} Mieszkando
          </div>
          </Toolbar>
        </AppBar>
      </div>
        );
    }
}
