import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { SurveyCollection } from './components/SurveyCollection';
import { Survey } from './components/Survey';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/survey-collection' component={SurveyCollection} />
                <Route path='/survey/:surveyId/:surveyName' component={Survey} />
            </Layout>
        );
    }
}
