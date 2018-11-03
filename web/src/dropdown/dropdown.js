import React, { Component } from 'react';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import './dropdown.css';

export default class Dropdown extends Component {

  constructor(props){
    super(props);
    this.handleChange = this.handleChange.bind(this);

    this.state = {value: props.value};
  }

  render() {
    return (
      <FormControl variant="outlined" className='dropdown'>
        <InputLabel
          ref={ref => {
            this.InputLabelRef = ref;
          }}
          htmlFor="outlined-label-simple"
        >
          {this.props.label}
        </InputLabel>
        <Select
          label={this.props.label}
          value={this.state.value}
          onChange={this.handleChange}
          input={
              <OutlinedInput
                labelWidth={90}
                name="label"
                id="outlined-label-simple"
              />
            }
        >
          <MenuItem value={1}>1</MenuItem>
          <MenuItem value={2}>2</MenuItem>
          <MenuItem value={3}>3</MenuItem>
        </Select>
      </FormControl>
    )
  }

  handleChange(e){
    console.log(e.target.value);
    this.setState({value: e.target.value});
  }
}
