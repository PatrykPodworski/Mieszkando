import React, {Component} from 'react';
import './App.css';
import Navbar from './navbar/navbar';
import Footer from './footer/footer';
import SimpleSearch from './simpleSearch/simpleSearch';
import {BrowserRouter, Route} from 'react-router-dom'
import AdvancedSearch from './advancedSearch/advancedSearch';
import SearchResults from './searchResults/searchResults';
import Logo from './logo/logo';

class App extends Component {
  render() {
    return ( 
      <BrowserRouter>
        <div className ="container">
          <Navbar/>
          <div className='content'>
            <Route exact path='/' component={() => <SimpleSearch startPrice={1100} startNumberOfRooms={2}/>} />
            <Route exact path='/simple' component={() => <SimpleSearch startPrice={800} startNumberOfRooms={2}/>} />
            <Route exact path='/advanced' component={AdvancedSearch}/>
            <Route path='/searchResults/:maxCost/:numberOfRooms' component={SearchResults}/>
          </div>
          <Footer/>
        </div>
      </BrowserRouter>
     );
  }
}

export default App;