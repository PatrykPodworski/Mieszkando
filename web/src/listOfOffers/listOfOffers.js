import React, { Component } from 'react'
import OfferGroup from '../offerGroup/offerGroup';

export default class ListOfOffers extends Component {
  render() {
    return (
      <div>
          {this.props.offers
            .sort((x,y) => y.offers.length - x.offers.length )
            .map((group, i) => {
            return (
              <OfferGroup group={group} key={i}/>)
          })}
      </div>
    )
  }
}
