import React, {Component} from 'react';
import Nouislider from "nouislider-react";
import "nouislider/distribute/nouislider.css";
import "./styles.css";
import wNumb from "wnumb";

export default class SingleSlider extends Component {
    constructor(props){
        super(props);

        this.handleUpdate = this.handleUpdate.bind(this);
    }

    handleUpdate(value){
        this.props.onPriceChange(parseInt(value[0].split(' ')[0]));
    }

    render(){
        return (
            <Nouislider className= {this.props.className}
            range={{ min: this.props.min, max: this.props.max }} 
            start={this.props.start} 
            step={this.props.step}
            connect={[false, true, false]}
            tooltips={true}
            format={wNumb({
                decimals: 0,
                suffix: ' '+this.props.unit
            })}
            onUpdate = {this.handleUpdate}
            />
        );
    }
}