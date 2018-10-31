import React, {Component} from 'react';
import Nouislider from "nouislider-react";
import "nouislider/distribute/nouislider.css";

export default class SingleSlider extends Component {
    render(){
        return (
            <Nouislider 
            range={{ min: this.props.min, max: this.props.max }} 
            start={[this.props.start]} 
            connect 
            tooltips/>
        );
    }
}