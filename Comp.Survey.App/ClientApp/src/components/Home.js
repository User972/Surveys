import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>Hello, Compass users!</h1>
                <p>Welcome to your new Survey App.</p>
                <ul>
                    <li>Use API to do basic CRUD operations on entities : Survey, Questions, Options, Users etc.</li>
                    <li>Explore new Surveys in the survey collection.</li>
                    <li>Submit the Survey at your will. Many times, multiple times - no bars.</li>
                    <li>This is initial POC only and is not a full fledged product.</li>
                </ul>
                <p>To make this product go high in quality, we would need the following things implemented :</p>
                <ul>

                    <li>Proper user authorization and authentication system</li>
                    <li>JWT based react app authentication</li>
                    <li>Create more layers in the client app, namely - data modeling, service layers, secure routing etc.</li>
                    <li>Replace SQLLite with something worthy of load handling - RDS, Azure SQL etc</li>
                    <li>Based on the project structure, a doc7ument DB would make a good choice as well - CosmosDB anyone ?</li>
                </ul>
            <p>We are using a fixed user (well, fixed User ID) for this POC.</p>
            </div>
        );
    }
}
