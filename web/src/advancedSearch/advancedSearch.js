import React, {Component} from 'react';
import Paper from '@material-ui/core/Paper';
import Dropdown from '../dropdown/dropdown';
import FormLabel from '@material-ui/core/FormLabel';
import { withStyles, Icon } from '@material-ui/core';
import styles from './styles';
import Link from './../link/link';
import RangeSlider from './../rangeSlider/rangeSlider';
import PointOfInterest from './../pointOfInterest/pointOfInterest';

class AdvancedSearch extends Component {
    render(){
        const {classes} = this.props;
        return (
            <Paper className={classes.searchForm + " advancedSearch"} elevation={1}>
                <div className={classes.firstRow}>
                    <FormLabel className={classes.sliderLabel}>Liczba pokoi</FormLabel>
                    <div className={classes.dropdown}>
                        <Dropdown 
                            value={this.props.criteria.numberOfRooms} 
                            options={[1,2,3,4,5,6,7,8,9]}
                            onNumberOfRoomsChange={this.props.numberOfRoomsChange}/>   
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
                        start={[this.props.criteria.minCost, this.props.criteria.maxCost]} 
                        min={0} 
                        max={3000}
                        step={100}
                        unit={"zł"} 
                        onValueChange={this.props.costChange}/>
                </div>
                <div className={classes.formRow}>
                    <FormLabel className={classes.sliderLabel}>Powierzchnia</FormLabel>
                    <RangeSlider className = {classes.slider}
                        start={[this.props.criteria.minArea, this.props.criteria.maxArea]} 
                        min={0} 
                        max={120} 
                        step={1}
                        unit={"m²"}
                        onValueChange={this.props.areaChange}/>
                </div>
                <div className={classes.formRow}>
                    <FormLabel onClick={this.props.addPointOfInterest} className={classes.pointOfInterest}>
                        <Icon className={classes.iconPoi}>add_circle</Icon>
                        Dodaj punkt zainteresowania
                    </FormLabel>
                </div>
                {this.props.criteria.pointsOfInterest.map((x, i) => {
                    return(
                    <PointOfInterest 
                    key={`poi{i}`}
                    id = {i}
                    address = {x.address}
                    distance = {x.maxDistanceTo}
                    travelTime = {x.maxTravelTime}
                    handleChange ={this.props.pointOfInterestChange} 
                    handleRemove = {this.props.removePointOfInterest}
                    className={classes.formRow}/>
                    )
                })}
                <div className={classes.formRow}>
                    <Link to={'/searchResults/advanced'}
                        className={classes.button}>
                            Szukaj
                    </Link>
                </div>
            </Paper>
        );
    }
}

export default withStyles(styles)(AdvancedSearch);