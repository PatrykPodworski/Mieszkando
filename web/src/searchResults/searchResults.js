import React, { Component } from 'react';
import Paper from '@material-ui/core/Paper';
import HttpService from './../services/httpService';
import ResultMap from './../resultMap/resultMap'
import ListOfOffers from './../listOfOffers/listOfOffers'

class SearchResults extends Component {
  constructor(props) {
    super(props);

    this.state = {
      maxCost: props.match.params.maxCost,
      numberOfRooms: props.match.params.numberOfRooms,
      offers: [],
    };
  }

  async componentDidMount(){
    const httpService = new HttpService();

    const results = await httpService.getSerchResultsAsync(
      this.state.maxCost, 
      this.state.numberOfRooms
    );

    this.setState({offers: results});
    console.log(this.state);
  }

  render() {
    return (
      <Paper>
      <ResultMap/>
      <ListOfOffers offers={this.state.offers}/>
      </Paper>
    )
  }
}

export default SearchResults; 