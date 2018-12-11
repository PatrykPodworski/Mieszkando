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
      error: null
    };


    this.addMarker = this.addMarker.bind(this);
  }

  async componentDidMount(){
    const httpService = new HttpService();

    try {
      if(this.props.type === "simple"){
        const results = await httpService.getSerchResultsAsync(
          this.props.criteria.maxCost, 
          this.props.criteria.numberOfRooms
        );
        this.setState({offers: results, error: false});
      }
      else {
        const results = await httpService.getAdvancedSearchResultsAsync(
          this.props.criteria
        );
        this.setState({offers: results, error: false});
      }
    }
    catch {
      this.setState({error: true});
    }


  }

  render() {
    const { classes } = this.props;

    if(this.state.error === true){
      return(
        <Paper className={classes.paper}>
          <p className={classes.info}>
            Coś poszło nie tak. Spróbuj ponownie za kilka minut lub zmień kryteria wyszukiwania.
          </p>
        </Paper>
      )
    }

    if(this.state.error === null){
      return(
        <Paper className={classes.paper}>
          <p className={classes.info}>
            Trwa wyszukiwanie. Prosimy o cierpliwość.
          </p>
        </Paper>
      )
    }

    if(this.state.offers.length === 0) {
      return(
        <Paper className={classes.paper}>
          <ResultMap offers={this.state.offers} marker={this.state.marker} className={classes.map}/>
          <p className={classes.info}>
            Brak pasujących wyników, spróbuj zmienić kryteria wyszukiwania.
          </p>
        </Paper>
      )
    }

    return(
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