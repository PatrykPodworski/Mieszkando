import React, { Component } from 'react';
import config from './../config';
import './../../node_modules/leaflet.heat/dist/leaflet-heat';

export default class resultMap extends Component {
  constructor(props){
    super(props)

    this.redIcon = new window.L.Icon({
      iconUrl: 'https://cdn.rawgit.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png',
      shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      shadowSize: [41, 41]
    });

    this.blueIcon = new window.L.Icon({
      iconUrl: 'https://cdn.rawgit.com/pointhi/leaflet-color-markers/master/img/marker-icon-blue.png',
      shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      shadowSize: [41, 41]
    });
    
  }
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
      this.markPointsOfInterest();
    }

    if(this.props.marker){
      this.addMarker(this.props.marker.coordinates);
      this.addRoutes(this.props.marker.routes);
    }
  }

  markPointsOfInterest(){
    if(this.props.offers[0].pointsOfInterest === null) {
      return;
    }
    
    this.props.offers[0].pointsOfInterest.map(x => {
      window.tomtom.L.marker([x.coordinates.latitude, x.coordinates.longitude], {icon: this.redIcon}).addTo(this.map);
    })
  }

  addMarker(coordinates){
    if(this.markerLayer){
      this.map.removeLayer(this.markerLayer);
      this.markPointsOfInterest();
    }
    this.markerLayer = window.tomtom.L.marker([coordinates.latitude, coordinates.longitude], {icon: this.blueIcon}).addTo(this.map);
  }

  addRoutes(routes) {
    if(this.routesLayer){
      this.map.removeLayer(this.routesLayer);
    }

    var geoJson = routes.map(x =>{
      return {
        type: "Feature",
        properties: {},
        geometry: {
          type: "MultiLineString",
          "coordinates": [
            x.geoJson.map(y => [y.latitude, y.longitude])
          ]
        }
      }
    })

    this.routesLayer = window.tomtom.L.geoJson(geoJson).addTo(this.map);
  }

  getOffersCoordinates() {
    return this.props.offers.map(x=> x.offers).flat().map(x => this.getOfferCoordinates(x));
  }

  getOfferCoordinates(offer) {
    return [offer.coordinates.latitude, offer.coordinates.longitude];
  }
}
