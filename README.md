# Surveys

Hello, Compass users!
Welcome to your new Survey App.

* Use API to do basic CRUD operations on entities : Survey, Questions, Options, Users etc.
* Explore new Surveys in the survey collection.
* Submit the Survey at your will. Many times, multiple times - no bars.
* This is initial POC only and is not a full fledged product.

To make this product go high in quality, we would need the following things implemented :
* Proper user authorization and authentication system
* JWT based react app authentication
* Create more layers in the client app, namely - data modeling, service layers, secure routing etc.
* Replace SQLLite with something worthy of load handling - RDS, Azure SQL etc
* Based on the project structure, a doc7ument DB would make a good choice as well - CosmosDB anyone ?

We are using a fixed user (well, fixed User ID) for this POC.

## Swagger 
Swagger has been implemented to go through the available APIs. Please visit https://localhost:44369/swagger/index.html to have a look. Make sure you change the base URL to the correct one.


## The API

There is an API controller "Surveys" which serves as a sole back-end to handle the data flow in and out of DB:

* There is no prefix or separate route been used for api controller as it is only a single page POC
* It is recommended to use API versioning in future
* It is recommended to add a separate api route to distinguish iwht any future developments
* A Postman collection has been added for your reference
* It is recommended to implement CDC testing for production

## Entities

To keep all the data in relational form, a very broad approach has been used here where the main entity is Survey
* Survey has Questions in it with One-to-Many relationship.
* Survey has Survey-Responses in it with one-to-Many relationship.
* Questions have further Options in them with one-to-Many relationship.
* Question Type has not been used in this POC and would need discussion with team to understand the logic for it.
  * One logical implementation would be to have another Entity QuestionType which would help deciding the rendering type of the option controls on UI
  * Text box, Radio button, check boxes etc
  * Based on this, we would need to amend the responses expected from the user as it could be single answered or multiple choices.
* Question Options are the options a user has to make choice for.
* All IDs are being used as GUIDs rather than integer based implementations.

## Create Survey
<img src="https://github.com/User972/Surveys/blob/master/Comp.Survey.App/New-controller.png" />
