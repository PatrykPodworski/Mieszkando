import React, { Component } from 'react'
import { Link as RouterLink }  from 'react-router-dom'
import styles from './styles'
import { withStyles } from '@material-ui/core';

class Link extends Component {
  render() {
      const { classes } = this.props;
    return (
      <RouterLink to={this.props.to} className={`${classes.link} ${this.props.className}`}>
        {this.props.children}
      </RouterLink>
    )
  }
}

export default withStyles(styles)(Link);