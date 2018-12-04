import React, {Component} from 'react';
import './simpleSearch.css';
import SingleSlider from '../singleSlider/singleSlider';
import Paper from '@material-ui/core/Paper';
import Dropdown from '../dropdown/dropdown';
import FormLabel from '@material-ui/core/FormLabel';
import { withStyles, Icon } from '@material-ui/core';
import styles from './styles';
import Link from './../link/link';


class SimpleSearch extends Component {
    render(){
        const {classes} = this.props;
        return (
            <Paper className="simpleSearch" elevation={1}>
                <div className="input-field slider">
                    <FormLabel>Cena</FormLabel>
                    <SingleSlider 
                        start={this.props.criteria.maxCost} 
                        min={this.props.ranges.minCost} 
                        max={this.props.ranges.maxCost} 
                        onValueChange={this.props.costChange}/>
                </div>
                <div className="input-field">
                    <Dropdown 
                        label={'Liczba pokoi'} 
                        value={this.props.criteria.numberOfRooms} 
                        options={this.props.ranges.rooms}
                        onNumberOfRoomsChange={this.props.numberOfRoomsChange}/>
                </div>
                <div className="input-field">
                    <Link to={`/searchResults/simple/${JSON.stringify(this.props.criteria)}`}
                    className={classes.button}>
                        Szukaj
                    </Link>
                    <Link to='/advanced'>
                        <div className={classes.link}>
                            <FormLabel>Więcej opcji</FormLabel>
                            <Icon className={classes.icon}>
                                arrow_drop_down
                            </Icon> 
                        </div>
                    </Link>
                </div>
            </Paper>
        );
    }
}

export default withStyles(styles)(SimpleSearch);