import React, {Component} from 'react';
import './simpleSearch.css'
import SingleSlider from '../singleSlider/singleSlider';
import Paper from '@material-ui/core/Paper'

export default class SimpleSearch extends Component {
    render(){
        return (
            <Paper className="simpleSearch" elevation={1}>
                <label>Cena</label>
                <SingleSlider start={800} min={500} max={2000}/>
                <label>Liczba pokoi</label>
                <div>dropdown</div>
                <button>Szukaj</button>
                <div>Więcej opcji</div>
            </Paper>
        );
    }
}