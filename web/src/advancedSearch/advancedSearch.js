import React, {Component} from 'react';
import Paper from '@material-ui/core/Paper';
import Dropdown from '../dropdown/dropdown';
import FormLabel from '@material-ui/core/FormLabel';
import { withStyles, Icon, Button } from '@material-ui/core';
import styles from './styles';
import Link from './../link/link';
import RangeSlider from './../rangeSlider/rangeSlider';

class AdvancedSearch extends Component {
    constructor(props) {
        super(props);

        this.state = {
            maxCost: props.startPrice,
            numberOfRooms: props.startNumberOfRooms
        }

        this.handleMaxCostChange = this.handleMaxCostChange.bind(this);
        this.handleNumberOfRoomsChange = this.handleNumberOfRoomsChange.bind(this);
        this.addPointOfInterest = this.addPointOfInterest.bind(this);
    }

    handleMaxCostChange(maxCost){
        this.setState({maxCost: maxCost});
    }

    handleNumberOfRoomsChange(numberOfRooms){
        this.setState({numberOfRooms:numberOfRooms});
    }

    addPointOfInterest(){
        console.log("qwe");
    }

    render(){
        const {classes} = this.props;
        return (
            <Paper className={classes.searchForm + " advancedSearch"} elevation={1}>
                <div className={classes.firstRow}>
                    <FormLabel className={classes.sliderLabel}>Liczba pokoi</FormLabel>
                    <div className={classes.dropdown}>
                        <Dropdown 
                            value={2} 
                            options={[1,2,3,4,5,6,7,8,9]}
                            onNumberOfRoomsChange={this.handleNumberOfRoomsChange}/>   
                    </div>

                    <Link to='/simple' className={classes.lessOption}>
                        <FormLabel className={classes.link}>Mniej opcji</FormLabel>
                        <Icon className={classes.icon}>
                            arrow_drop_up
                        </Icon> 
                    </Link>
                </div>
                <div className={classes.formRow}>
                    <FormLabel className={classes.sliderLabel}>Cena</FormLabel>
                    <RangeSlider className = {classes.slider}
                        start={[900, 1700]} 
                        min={500} 
                        max={3000}
                        step={100}
                        unit={"zł"} 
                        onPriceChange={this.handleMaxCostChange}/>
                </div>
                <div className={classes.formRow}>
                    <FormLabel className={classes.sliderLabel}>Powierzchnia</FormLabel>
                    <RangeSlider className = {classes.slider}
                        start={[24, 68]} 
                        min={18} 
                        max={120} 
                        step={1}
                        unit={"m²"}
                        onPriceChange={this.handleMaxCostChange}/>
                </div>
                <div className={classes.formRow}>
                    <FormLabel onClick={this.addPointOfInterest} className={classes.pointOfInterest}>
                        <Icon className={classes.iconPoi}>add_circle</Icon>
                        Dodaj punkt zainteresowania
                    </FormLabel>
                </div>
                <div className={classes.formRow}>
                    <Link to={`searchResults/${this.state.maxCost}/${this.state.numberOfRooms}`}
                        className={classes.button} 
                        maxCost={this.state.maxCost}
                        numberOfRooms={this.state.numberOfRooms}>
                            Szukaj
                    </Link>
                </div>
            </Paper>
        );
    }
}

export default withStyles(styles)(AdvancedSearch);