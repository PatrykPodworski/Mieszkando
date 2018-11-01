import React, {Component} from 'react';
import SimpleSearch from './../simpleSearch/simpleSearch';

export default class Content extends Component {
    render() {
       return (
        <div className='container'>
            <SimpleSearch/>
        </div>
        );
    }
}
