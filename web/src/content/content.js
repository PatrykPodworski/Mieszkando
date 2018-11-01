import React, {Component} from 'react';
import SimpleSearch from './../simpleSearch/simpleSearch';
import './content.css';

export default class Content extends Component {
    render() {
       return (
        <div className='content'>
            <SimpleSearch/>
        </div>
        );
    }
}
