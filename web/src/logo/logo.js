import React, { Component } from 'react';
import Link from './../link/link'
import Icon from '@material-ui/core/Icon'
import { withStyles } from '@material-ui/core';
import styles from './styles';

class Logo extends Component {
  render() {
    var { classes } = this.props;
      return (
        <Link to='/'>
          <div className={classes.logo} style={{fontSize: this.props.size + 'px'}}>
            <Icon className={classes.icon}>home</Icon>
          <div>Mieszkando</div>
        </div>
      </Link>
    );
  }
}

export default withStyles(styles)(Logo);