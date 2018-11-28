import React, { Component } from 'react'
import OfferGroup from '../offerGroup/offerGroup';

export default class listOfOffers extends Component {
  render() {
    return (
      <div>
          {this.props.offers
            .sort((x,y) => y.offers.length - x.offers.length )
            .map((group) => {
            return (
              <OfferGroup group={group}/>)
          })}
      </div>
    )
  }
}
