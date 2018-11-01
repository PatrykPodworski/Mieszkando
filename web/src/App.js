import React, {Component} from 'react';
import './App.css';
import Navbar from './navbar/navbar';
import Footer from './footer/footer';
import SimpleSearch from './simpleSearch/simpleSearch';
import {BrowserRouter, Route} from 'react-router-dom'
import Login from './Login/Login';
import Register from './Register/Register';
import AdvancedSearch from './AdvancedSearch/AdvancedSearch';
import SearchResults from './searchResults/SearchResults';
import ReverseSearch from './reverseSearch/ReverseSearch';
import ReverseSearchResults from './reverseSearchResults/ReverseSearchResults';
import MyAccount from './myAccount/MyAccount';
import Export from './export/Export';

class App extends Component {
  render() {
    return ( 
      <BrowserRouter>
        <div class ="container">
          <Navbar/>
          <div className='content'>
            <Route exact path='/' component={SimpleSearch}/>
            <Route exact path='/simple' component={SimpleSearch}/>
            <Route exact path='/advanced' component={AdvancedSearch}/>
            <Route path='/searchResults' component={SearchResults}/>
            <Route path='/reveseSearch' component={ReverseSearch}/>
            <Route path='/reveseSearchResults' component={ReverseSearchResults}/>
            <Route path='/login' component={Login}/>
            <Route path='/register' component={Register}/>
            <Route path='/myaccount' component={MyAccount}/>
            <Route path='/export' component={Export}/>
          </div>
          <Footer/>
        </div>
      </BrowserRouter>
     );
  }
}

export default App;