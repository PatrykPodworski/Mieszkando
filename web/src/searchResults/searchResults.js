import React, { Component } from 'react';
import Paper from '@material-ui/core/Paper';
import { withRouter } from 'react-router-dom';

class SearchResults extends Component {
  constructor(props) {
    super(props);
    this.state = {
      offers: this.props.location.state.offers,
    };
  }
  render() {
    return (
      <Paper>
        {this.state.offers}
      </Paper>
    )
  }
}

export default SearchResults; 