import React, { Component } from 'react';
import './App.css';
import config from './config';

const apiKey = config.tomtomApiKey;

class App extends Component {
  render() {
    return (
      <div id = 'map'></div>
    );
  }

  componentDidMount() { 
    console.log('');
    const script = document.createElement('script');
    script.src = process.env.PUBLIC_URL + '/tomtomWebSdk/tomtom.min.js';
    document.body.appendChild(script);
    script.async = true;
    script.onload = function () {
      window.tomtom.L.map('map', {
        source: 'vector',
        key: apiKey,
        center: [54.3565, 18.6461],
        basePath: '/tomtomWebSdk',
        zoom: 15
      });
    }
  }
}

export default App;
