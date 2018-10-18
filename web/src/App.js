import React, { Component } from 'react';
import './App.css';
import config from './config';

const apiKey = config.tomtomApiKey;
var cityRegions;

class App extends Component {
  render() {
    return (
      <div id = 'map'></div>
    );
  }

  componentDidMount() { 
    console.log('');
    fetch("http://localhost:62003/api/cityregions")
    .then((response) =>  response.json())
    .then((result) => this.cityRegions = result)
    .then(() => this.loadMap());
    }

    loadMap() {
      var map = window.tomtom.map('map', {
        source: 'vector',
        key: apiKey,
        basePath: '/tomtomWebSdk',
      });

      var polygon = {
        'type': 'FeatureCollection',
        'features': [
            {
                'type': 'Feature',  
                'properties': {},
                'geometry': {
                    'type': 'Polygon',
                    'coordinates': this.getRegionsCoordinates(this.cityRegions)
                }
            }
        ]
    };
    var geoJson = window.tomtom.L.geoJson(polygon, { style: { color: '#FFC312', opacity: 0.5, } }).addTo(map);
    map.fitBounds(geoJson.getBounds(), { padding: [5, 5] });
    }

    getRegionsCoordinates(regions) {
     return regions.map(x => this.getRegionCoordinates(x));
    }

    getRegionCoordinates(region) {
      return [
        [parseFloat(region.longitude), parseFloat(region.latitude)],
        [parseFloat(region.longitude) + parseFloat(region.longitudeSize), parseFloat(region.latitude)],
        [parseFloat(region.longitude) + parseFloat(region.longitudeSize), parseFloat(region.latitude) + parseFloat(region.latitudeSize)],
        [parseFloat(region.longitude), parseFloat(region.latitude) + parseFloat(region.latitudeSize)]
      ]
    }
  }

export default App;
