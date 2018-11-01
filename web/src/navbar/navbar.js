import React, {Component} from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import HomeIcon from '@material-ui/icons/Home'

export default class Navbar extends Component {
    render() {
       return (
        <div className="navbar">
        <AppBar position="static" color="primary">
          <Toolbar>
            <HomeIcon/>
            <Typography variant="h6" color="inherit">
              Mieszkando
            </Typography>
          </Toolbar>
        </AppBar>
      </div>
        );
    }
}
