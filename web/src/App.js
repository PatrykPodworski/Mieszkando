import React, {Component} from 'react';
import './App.css';
import Navbar from './navbar/navbar';
import Footer from './footer/footer';
import Content from './content/content';

class App extends Component {
  render() {
    return ( 
      <div>
        <Navbar/>
        <Content/>
        <Footer/>
      </div>
     );
  }
}

export default App;