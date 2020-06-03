This is a sample application designed for Compass assignment where it was requested to have a simple UI to show Surveys and a user can select to fill that survey and submit.

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
** One logical implementation would be to have another Entity QuestionType which would help deciding the rendering type of the option controls on UI
** Text box, Radio button, check boxes etc
** Based on this, we would need to amend the responses expected from the user as it could be single answered or multiple choices.
* Question Options are the options a user has to make choice for.
* All IDs are being used as GUIDs rather than integer based implementations.

