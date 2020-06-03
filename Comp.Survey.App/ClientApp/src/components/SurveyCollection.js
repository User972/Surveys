import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class SurveyCollection extends Component {
    static displayName = SurveyCollection.name;

    constructor(props) {
        super(props);
        this.state = { surveys: [], loading: true };
    }

    componentDidMount() {
        this.populateSurveys();
    }

    static renderSurveysTable(surveys) {
        return (
            <table className='table' aria-labelledby="tabelLabel">
                <tbody>
                    {surveys.map(survey =>
                        <tr key={survey.id}>
                            <td>
                                <Link className="btn btn-light btn-block" to={{
                                    pathname: '/survey/' + survey.id + '/' + survey.name,
                                    state: {
                                        surveyId: survey.id,
                                        surveyName: survey.name
                                    }
                                }}>{survey.name}</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : SurveyCollection.renderSurveysTable(this.state.surveys);

        return (
            <div>
                <h1 id="tabelLabel" >Compass Surveys</h1>
                <br />
                {contents}
            </div>
        );
    }

    async populateSurveys() {
        const response = await fetch('surveys');
        const data = await response.json();
        this.setState({ surveys: data, loading: false });
    }
}