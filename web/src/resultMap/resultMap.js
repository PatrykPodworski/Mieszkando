import React, { Component } from 'react';
import config from './../config';
import './../../node_modules/leaflet.heat/dist/leaflet-heat';

export default class resultMap extends Component {
  render() {
    if(this.loaded){
      this.refreshMap();
    }
    return (
      <div id = 'map' className={this.props.className}></div>
    )
  }

  componentDidMount() {
    this.loaded = true;
    this.loadMap();
  }

  loadMap() {
    const apiKey = config.tomtomApiKey;
    this.map = window.tomtom.map('map', {
        key: apiKey,
        basePath: 'https://api.tomtom.com/maps-sdk-js/4.43.11/examples/sdk',
        center: [54.3553994, 18.6422703],
        zoom: 11
    });
  }

  refreshMap(){
    if(!this.heatLayer){
      this.heatLayer = window.L.heatLayer(this.getOffersCoordinates(), {minOpacity: 0.4, radius: 40, blur: 60}).addTo(this.map);
    }

    if(this.props.marker){
      this.markerLayer = this.addMarker(this.props.marker);
    }
  }

  addMarker(marker){
    window.tomtom.L.marker(marker).addTo(this.map);
  }

  getOffersCoordinates() {
    return this.props.offers.map(x=> x.offers).flat().map(x => this.getOfferCoordinates(x));
  }

  getOfferCoordinates(offer) {
    return [offer.latitude, offer.longitude];
  }
}
