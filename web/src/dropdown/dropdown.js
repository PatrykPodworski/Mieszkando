import React, { Component } from 'react';
import Select from '@material-ui/core/Select';
import MenuItem from '@material-ui/core/MenuItem';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import styles from './styles';
import { withStyles } from '@material-ui/core';
import ReactDOM from 'react-dom';

class Dropdown extends Component {

  constructor(props){
    super(props);
    this.handleChange = this.handleChange.bind(this);

    this.state = {
      value: props.value,
      labelWidth: 0
    };
  }

  componentDidMount(){
    this.setState({
      labelWidth: ReactDOM.findDOMNode(this.InputLabelRef).offsetWidth
    })
  }

  render() {
    const { classes } = this.props;
    
    return (
      <FormControl variant="outlined" className={classes.formControl}>
        <InputLabel htmlFor="dropdown-simple"
          ref={ref => {
                this.InputLabelRef = ref;
              }}
        >
          {this.props.label}
        </InputLabel>
        <Select
          label={this.props.label}
          value={this.state.value}
          onChange={this.handleChange}
          input={
              <OutlinedInput
                className={classes.outlinedInput} 
                labelWidth={this.state.labelWidth}
                name="dropdown-simple"
                id="dropdown-simple"
              />
            }
        >
        {this.props.options.map(x => (
          <MenuItem value={x} key={x}>{x}</MenuItem>
        ))}
        </Select>
      </FormControl>
    )
  }

  handleChange(e){
    this.props.onNumberOfRoomsChange(e.target.value);
    this.setState({value: e.target.value});
  }
}

export default withStyles(styles)(Dropdown);