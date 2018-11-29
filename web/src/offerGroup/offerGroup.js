import React, { Component } from 'react'
import { withStyles } from '@material-ui/core/styles';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import styles from './styles';
import OffersTable from '../offersTable/offersTable';

class SearchResults extends Component {
    render() {
    const { classes } = this.props;
    return (
        <ExpansionPanel>
        <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
          <Typography className={classes.heading}>{this.props.group.district}, {this.getLenghtString(this.props.group.offers.length)}</Typography>
        </ExpansionPanelSummary>
        <ExpansionPanelDetails>
            <OffersTable offers={this.props.group.offers}/>
        </ExpansionPanelDetails>
      </ExpansionPanel>
    )
  }

  getLenghtString(length){
    if (length === 1){
        return `${length} oferta`
    }
    if (length < 5){
        return `${length} oferty`
    }
    return `${length} ofert`
  }
}
export default withStyles(styles)(SearchResults); 