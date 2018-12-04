import React, {Component} from 'react';
import './App.css';
import Navbar from './navbar/navbar';
import Footer from './footer/footer';
import SimpleSearch from './simpleSearch/simpleSearch';
import {BrowserRouter, Route} from 'react-router-dom'
import AdvancedSearch from './advancedSearch/advancedSearch';
import SearchResults from './searchResults/searchResults';
import update from 'immutability-helper';

class App extends Component {
    constructor(props){
        super(props);

        this.state = {
            minCost: 900,
            maxCost: 1600,
            minArea: 28,
            maxArea: 64,
            numberOfRooms: 2,
            pointsOfInterest: []
        }

        this.ranges = {
            minCost: 0,
            maxCost: 3000,
            minArea: 0,
            maxArea: 120,
            rooms: [1,2,3,4,5,6,7,8,9]
        }

        this.handleCostChange = this.handleCostChange.bind(this);
        this.handleAreaChange = this.handleAreaChange.bind(this);
        this.handleNumberOfRoomsChange = this.handleNumberOfRoomsChange.bind(this);
        this.handlePointOfInterestChange = this.handlePointOfInterestChange.bind(this);
        this.addPointOfInterest = this.addPointOfInterest.bind(this);
        this.removePointOfInterest = this.removePointOfInterest.bind(this);
    }

    render() {
        return ( 
        <BrowserRouter>
            <div className ="container">
            <Navbar/>
            <div className='content'>
                <Route exact path='\/(simple)?' render={() => 
                <SimpleSearch 
                    criteria = {this.composeSimpleCriteria()}
                    ranges = {this.ranges}
                    numberOfRoomsChange = {this.handleNumberOfRoomsChange}
                    costChange = {this.handleCostChange}/>
                } />
                <Route exact path='/advanced' render={() => 
                <AdvancedSearch 
                    criteria = {this.state}
                    ranges = {this.ranges}
                    numberOfRoomsChange = {this.handleNumberOfRoomsChange}
                    costChange = {this.handleCostChange}
                    areaChange = {this.handleAreaChange}
                    addPointOfInterest = {this.addPointOfInterest}
                    removePointOfInterest = {this.removePointOfInterest}
                    pointOfInterestChange = {this.handlePointOfInterestChange}/>
                } />
                <Route exact path='/searchResults/:type/:criteria' component={(props) =>    
                <SearchResults type = {props.match.params.type} criteria = {JSON.parse(props.match.params.criteria)}/>
                }/>
            </div>
            <Footer/>
            </div>
        </BrowserRouter>
        );
    }

    handleCostChange(costs){
        if(costs.length === 1){
        this.setState({
            maxCost: costs[0]
        });
        }
        else {
        this.setState({
            minCost: costs[0],
            maxCost: costs[1]
        });
        }
    }

    handleAreaChange(areas){
        this.setState({
            minArea: areas[0],
            maxArea: areas[1]
        });
    }

    handleNumberOfRoomsChange(numberOfRooms){
        this.setState({numberOfRooms:numberOfRooms});
    }

    addPointOfInterest(){
        if(this.state.pointsOfInterest.length > 2){
            return;
        }

        this.setState({
            pointsOfInterest: update(
                this.state.pointsOfInterest, 
                {$push: [{address: '', maxDistanceTo:  0, maxTravelTime: 0}]}
            )
        });
    }

    removePointOfInterest(id){
        this.setState({
            pointsOfInterest: update(
                this.state.pointsOfInterest,
                {$splice: [[id, 1]]}
            )
        });
    }

    handlePointOfInterestChange(id, name, value){
        this.setState({
            pointsOfInterest: update(
                this.state.pointsOfInterest,
                {[id]: {[name]: {$set: value}}})
        });
    }

    composeSimpleCriteria(){
        return {
            maxCost: this.state.maxCost,
            numberOfRooms: this.state.numberOfRooms
        }
    }
}

export default App;