import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Survey extends Component {
    static displayName = Survey.name;

    constructor(props) {
        super(props);
        this.state = {
            questions: {},
            loading: true,
            id: 0,
            name: "",
            compUserSurveyDetails: []
        };
    }
    onQuestionOptionChanged(e, questionId, optionId, optionValue) {
        var items = this.state.compUserSurveyDetails.filter(item => item.SelectedOptionId !== optionId);
        if (e.target.checked) {
            items.push({
                //TODO : Remove this field as it is a placeholder in case some notes or useful information needs to be sent across.
                "CompUserSurvey": "12",
                "SurveyQuestionId": questionId,
                "SelectedOptionId": optionId,
                "SelectedOptionValue": optionValue
            });
        }
        this.setState({ compUserSurveyDetails: items });
    }

    async handleSubmit(self) {
        debugger;
        var objectData = {
            "SubmissionTitle": self.state.name,
            "CompUserId": "b719e6ad-d0a0-403c-828b-9487259e01cb", // Fixed as of now
            compUserSurveyDetails: self.state.compUserSurveyDetails
        };


        const settings = {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(objectData)
        };
        try {
            const fetchResponse = await fetch('/surveys/' + self.state.id +'/submission', settings);
            const data = await fetchResponse.json();
            alert("Congrats ! The survey has been completed. Thanks for your responses.");
        } catch (e) {
            alert("Apologies ! The survey couldn't be completed. please try again later.");
        }

    }

    componentDidMount() {
        console.log(this.props.match.params);
        this.populateSurveyQuestions(this.props.match.params.surveyId, this.props.match.params.surveyName);
    }

    static renderSurveyQuestionsTable(questions, self) {
        return (
            <div>
                <table className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {questions.map(question =>
                            <tr key={question.id}>
                                <td>
                                    <div className="border rounded p-3 bg-light">
                                        <h4>{question.title}</h4>
                                        <span>{question.subTitle}</span>

                                        <div
                                            className="radio-row" >
                                            {question.questionOptions.map(option =>
                                                <div className="input-row" key={option.id}>
                                                    <input type="radio" name={option.id} value={option.id} onChange={(e) => self.onQuestionOptionChanged(e, question.id, option.id, option.text)} />
                                                    <label htmlFor={option.id} className="p-2">{option.text}</label>
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
                <div>
                    <Link className="btn btn-light" to={{ pathname: '/survey-collection' }}>Back</Link>
                    <button className="btn btn-primary float-right" onClick={() => self.handleSubmit(self)} >Submit</button>
                </div>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Survey.renderSurveyQuestionsTable(this.state.questions, this);

        return (
            <div>
                <h1 id="tabelLabel" >{this.state.name}</h1>
                <br />
                {contents}
            </div>
        );
    }

    async populateSurveyQuestions(surveyId, surveyName) {
        const response = await fetch('surveys/' + surveyId + '/questions');
        const data = await response.json();
        this.setState({ questions: data, loading: false, id: surveyId, name: surveyName });
    }
}