import React, {Component} from 'react';
import './simpleSearch.css';
import SingleSlider from '../singleSlider/singleSlider';
import Paper from '@material-ui/core/Paper';
import Dropdown from '../dropdown/dropdown';
import FormLabel from '@material-ui/core/FormLabel';
import Button from '@material-ui/core/Button';
import { withStyles, Icon } from '@material-ui/core';
import styles from './styles'
import Link from './../link/link'
import HttpService from './../services/httpService'

class SimpleSearch extends Component {
    constructor(props) {
        super(props);

        this.state = {
            maxPrice: props.startPrice,
            numberOfRooms: props.startNumberOfRooms
        }

        this.HttpService = new HttpService();

        this.handleClick = this.handleClick.bind(this);
        this.handleMaxPriceChange = this.handleMaxPriceChange.bind(this);
        this.handleNumberOfRoomsChange = this.handleNumberOfRoomsChange.bind(this);
    }

    handleClick() {
        console.log(this.state);
        this.HttpService.getSerchResults(this.state.maxPrice, this.state.numberOfRooms);
    }

    handleMaxPriceChange(maxPrice){
        this.setState({maxPrice: maxPrice});
    }

    handleNumberOfRoomsChange(numberOfRooms){
        this.setState({numberOfRooms:numberOfRooms});
    }

    render(){
        const {classes} = this.props;
        return (
            <Paper className="simpleSearch" elevation={1}>
                <div className="input-field slider">
                    <FormLabel>Cena</FormLabel>
                    <SingleSlider 
                        start={this.props.startPrice} 
                        min={500} 
                        max={2000} 
                        onPriceChange={this.handleMaxPriceChange}/>
                </div>
                <div className="input-field">
                    <Dropdown 
                        label={'Liczba pokoi'} 
                        value={this.props.startNumberOfRooms} 
                        options={[1,2,3,4,5,6,7,8,9]}
                        onNumberOfRoomsChange={this.handleNumberOfRoomsChange}/>
                </div>
                <div className="input-field">
                <Button variant="contained" color="primary" className={classes.button} onClick={this.handleClick}>
                    Szukaj
                </Button>
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