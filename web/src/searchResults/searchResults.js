import React, { Component } from 'react';
import Paper from '@material-ui/core/Paper';
import HttpService from './../services/httpService';
import ResultMap from './../resultMap/resultMap'
import ListOfOffers from './../listOfOffers/listOfOffers'
import { withStyles } from '@material-ui/core';
import styles from './styles';

class SearchResults extends Component {
  constructor(props) {
    super(props);

    this.state = {
      offers: [],
      marker: null,
    };

    this.addMarker = this.addMarker.bind(this);
  }

  async componentDidMount(){
    const httpService = new HttpService();

    if(this.props.type === "simple"){
      const results = await httpService.getSerchResultsAsync(
        this.props.criteria.maxCost, 
        this.props.criteria.numberOfRooms
      );
      this.setState({offers: results});
    }
    else {
      const results = await httpService.getAdvancedSearchResultsAsync(
        this.props.criteria
      );
      this.setState({offers: results});
    }

  }

  render() {
    const { classes } = this.props;
    return (
      <Paper className={classes.paper}>
        <ResultMap offers={this.state.offers} marker={this.state.marker} className={classes.map}/>
        <ListOfOffers offers={this.state.offers} onPinClick={this.addMarker}/>
      </Paper>
    )
  }

  addMarker(offer){
    this.setState({marker: offer});
  }
}
export default withStyles(styles)(SearchResults); 