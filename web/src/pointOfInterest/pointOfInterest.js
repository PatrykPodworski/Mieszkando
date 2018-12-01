import React, {Component} from 'react';
import { withStyles, TextField, Icon, InputAdornment } from '@material-ui/core';
import styles from './styles';
class AdvancedSearch extends Component {
    constructor(props){
        super(props);

        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(name, value){
        if ((name === 'distance' || name === 'travelTime') && value < 0){
            return;
        }
        this.props.handleChange(this.props.id, name, value);
    }

    render(){
        const {classes} = this.props;
        return (
            <div className={classes.poiRow}>
                <TextField
                    id="address"
                    label="Adres"
                    className={classes.textField}
                    value={this.props.address}
                    onChange={(e) => this.handleChange('address', e.target.value)}
                    margin="normal"
                    variant="outlined"
                />
                <TextField
                    id="distance"
                    label="Odległość"
                    type="number"
                    className={classes.shortTextField}
                    value={this.props.distance}
                    onChange={(e) => this.handleChange('distance', e.target.value)}
                    margin="normal"
                    variant="outlined"
                    InputProps={{
                        endAdornment: <InputAdornment position="end">km</InputAdornment>
                    }}
                />
                <TextField
                    label="Czas dojazdu"
                    type="number"
                    className={classes.shortTextField}
                    value={this.props.travelTime}
                    onChange={(e) => this.handleChange('travelTime', e.target.value)}
                    margin="normal"
                    variant="outlined"
                    InputProps={{
                        endAdornment: <InputAdornment position="end">min</InputAdornment>
                    }}
                />
                <Icon 
                className={classes.icon} 
                onClick={() => this.props.handleRemove(this.props.id)}>remove_circle</Icon>
            </div>
        );
    }
}

export default withStyles(styles)(AdvancedSearch);