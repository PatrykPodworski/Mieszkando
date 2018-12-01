import React, { Component } from "react";
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';
import styles from './styles';
import PinDrop from '@material-ui/icons/PinDrop';
import { withStyles } from "@material-ui/core";

 class OfferTableRow extends Component {
   constructor(props){
     super(props)

     this.handlePinClick = this.handlePinClick.bind(this);
   }
  render() {
    const { classes } = this.props;
      return(
        <TableRow>
            <TableCell className={classes.cell}>
                {this.props.offer.title}
            </TableCell>
            <TableCell className={classes.cell}>
                {this.props.offer.totalCost} zł
            </TableCell>
            <TableCell className={classes.cell}>
                {this.getRoomsString(this.props.offer.rooms)}
            </TableCell>
            <TableCell className={classes.cell}>
                {this.props.offer.area} m²
            </TableCell>
            <TableCell className={classes.cell}>
                <PinDrop onClick={this.handlePinClick} className={classes.pin}/>
            </TableCell>
            <TableCell classes={{root: classes.cell, lastChild: classes.CellLast}}>
                <a href={this.props.offer.link} className={classes.link}>Ogłoszenie</a>
            </TableCell>
        </TableRow>
      );
  }

  handlePinClick(){
    this.props.onPinClick(this.props.offer);
  }
  getRoomsString(rooms){
    if (rooms === 1){
      return `${rooms} pokój`
    }
    if (rooms < 5){
        return `${rooms} pokoje`
    }
    return `${rooms} pokojów`
  }
}

export default withStyles(styles)(OfferTableRow); 
