import React, {Component} from 'react';
import './simpleSearch.css'
import SingleSlider from '../singleSlider/singleSlider';

export default class SimpleSearch extends Component {
    render(){
        return (
            <div className="simpleSearch">
                <label>Cena</label>
                <SingleSlider start={800} min={500} max={2000}/>
                <label>Liczba pokoi</label>
                <div>dropdown</div>
                <button>Szukaj</button>
                <div>Więcej opcji</div>
            </div>
        );
    }
}