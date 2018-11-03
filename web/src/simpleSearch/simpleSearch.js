import React, {Component} from 'react';
import './simpleSearch.css';
import SingleSlider from '../singleSlider/singleSlider';
import Paper from '@material-ui/core/Paper';
import Dropdown from '../dropdown/dropdown';

export default class SimpleSearch extends Component {
    render(){
        return (
            <Paper className="simpleSearch" elevation={1}>
                <div className="input-field slider">
                    <label>Cena</label>
                    <SingleSlider start={800} min={500} max={2000}/>
                </div>
                <div className="input-field">
                    <Dropdown label={'Liczba pokoi'} value={2} options={[1,2,3,4,5]}/>
                </div>
                <div className="input-field">
                <button>Szukaj</button>
                <div>Więcej opcji</div>
                </div>
            </Paper>
        );
    }
}