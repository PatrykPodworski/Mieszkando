import React, { Component } from 'react'

export default class listOfOffers extends Component {
  render() {
    return (
      <div>
        <ul>
          {this.props.offers.map((offer, i) => {
            return (<li key={i}>{offer.title}</li>)
          })}
        </ul>
      </div>
    )
  }
}
