import React, {Component} from 'react';
import './App.css';
import SimpleSearch from './simpleSearch/simpleSearch';

class App extends Component {
  render() {
    return ( 
      <div className='container'>
        <SimpleSearch/>
      </div>
     );
  }
}

export default App;