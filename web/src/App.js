import React, {Component} from 'react';
import './App.css';
import Navbar from './navbar/navbar';
import Footer from './footer/footer';
import SimpleSearch from './simpleSearch/simpleSearch';
import {BrowserRouter, Route} from 'react-router-dom'
import Login from './login/login';
import Register from './register/register';
import AdvancedSearch from './advancedSearch/advancedSearch';
import SearchResults from './searchResults/searchResults';
import ReverseSearch from './reverseSearch/reverseSearch';
import ReverseSearchResults from './reverseSearchResults/reverseSearchResults';
import MyAccount from './myAccount/myAccount';
import Export from './export/export';

class App extends Component {
  render() {
    return ( 
      <BrowserRouter>
        <div className ="container">
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