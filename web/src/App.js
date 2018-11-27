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
import Logo from './logo/logo';

class App extends Component {
  constructor(props){
    super(props);

    this.state = {
      isLoggedIn: true,
    };
  }

  render() {
    return ( 
      <BrowserRouter>
        <div className ="container">
          <Navbar isLoggedIn={this.state.isLoggedIn}/>
          <div className='content'>
            <Logo show={!this.state.isLoggedIn} size={72} className='logo'/>
            <Route exact path='/' component={() => <SimpleSearch startPrice={1100} startNumberOfRooms={2}/>} />
            <Route exact path='/simple' component={() => <SimpleSearch startPrice={800} startNumberOfRooms={2}/>} />
            <Route exact path='/advanced' component={AdvancedSearch}/>
            <Route path='/searchResults/:maxCost/:numberOfRooms' component={SearchResults}/>
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