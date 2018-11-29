import React, { Component } from "react";
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableRow from '@material-ui/core/TableRow';
import {TableFooter, TablePagination } from "@material-ui/core";
import OfferTableRow from './offerTableRow';

 export default class OffersTable extends Component {
   constructor(props){
     super(props)

     this.state = {
       page: 0,
       rowsPerPage: 5
     };
   }

  render() {
    return (
      <Table>
        <TableBody>
          {this.props.offers.slice( this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage)
            .map((offer, i) => {
            return (
              <OfferTableRow offer={offer} key={i} onPinClick={this.props.onPinClick}/>
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
}
