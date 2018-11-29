import React, { Component } from "react";
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableRow from '@material-ui/core/TableRow';
import styles from './styles';
import { withStyles, TableFooter, TablePagination } from "@material-ui/core";
import PinDrop from '@material-ui/icons/PinDrop';

 class OffersTable extends Component {
   constructor(props){
     super(props)

     this.state = {
       page: 0,
       rowsPerPage: 5
     };

     this.handlePinClick = this.handlePinClick.bind(this);
   }
  render() {
    const { classes } = this.props;
    return (
      <Table>
        <TableBody>
          {this.props.offers.slice( this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage)
            .map((row, i) => {
            return (
              <TableRow key={i}>
                <TableCell component="th" scope="row" className={classes.cell}>
                  {row.title}
                </TableCell>
                <TableCell className={classes.cell}>
                  {row.totalCost} zł
                </TableCell>
                <TableCell className={classes.cell}>
                  {this.getRoomsString(row.rooms)}
                </TableCell>
                <TableCell className={classes.cell}>
                  {row.area} m2
                </TableCell>
                <TableCell className={classes.cell}>
                    <PinDrop onClick={() => this.handlePinClick(row.latitude, row.longitude)} className={classes.pin}/>
                </TableCell>
                <TableCell className={classes.cell}>
                <a href={row.link}>Oferta</a>
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
        <TableFooter>
              <TableRow>
                <TablePagination
                  rowsPerPageOptions={[5, 10, 25]}
                  colSpan={3}
                  count={this.props.offers.length}
                  rowsPerPage={this.state.rowsPerPage}
                  page={this.state.page}
                  onChangePage={this.handleChangePage}
                  onChangeRowsPerPage={this.handleChangeRowsPerPage}
                />
              </TableRow>
            </TableFooter>
      </Table>
    );
  }

  handleChangePage = (event, page) => {
    this.setState({ page });
  };

  handleChangeRowsPerPage = event => {
    this.setState({ rowsPerPage: event.target.value });
  };

  handlePinClick(lat, lon){
    console.log(lat);
    console.log(lon);
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

export default withStyles(styles)(OffersTable); 
