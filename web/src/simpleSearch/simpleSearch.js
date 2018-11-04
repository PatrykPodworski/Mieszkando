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

class SimpleSearch extends Component {
    render(){
        const {classes} = this.props;
        return (
            <Paper className="simpleSearch" elevation={1}>
                <div className="input-field slider">
                    <FormLabel>Cena</FormLabel>
                    <SingleSlider start={800} min={500} max={2000}/>
                </div>
                <div className="input-field">
                    <Dropdown label={'Liczba pokoi'} value={2} options={[1,2,3,4,5]}/>
                </div>
                <div className="input-field">
                <Button variant="contained" color="primary" className={classes.button}>
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