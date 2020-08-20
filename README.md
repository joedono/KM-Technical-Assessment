# KM Technical Assessment

https://technical-assessment.konicaminoltamarketplace.com/

## Summary
Code is written using .NET Core 3.1 and Microsoft Visual Studio Community 2019.

It is a rather standard Web API project.

`HomeController` handles all the endpoints, validation, and game logic.

`GameBoardService` handles storing which nodes are on the path, which have been clicked, whose turn it is, etc.

The `Models` folder has the various request and response models, as well as a class for the game board itself.

## Running the API
The project can be run on your standard IIS Express instance that is bundled with Visual Studio. It will use the `Http` API model from the client. The project is also hosted on Heroku at https://km-technical-assessment.herokuapp.com, and can be connected to there.

The barest minimum of routing is used. The three endpoints outlined in the spec are:
* https://km-technical-assessment.herokuapp.com/initialize
* https://km-technical-assessment.herokuapp.com/node-clicked
* https://km-technical-assessment.herokuapp.com/error

## Unit Tests
Unit tests were written using the Moq library and can be run using MSTest. Tests are contained in the `KM-Technical-Assessment.Test` project

## Feedback
I found one feature gap that I couldn't figure out. The requirements includes this line:

> The Client is configured for a 4x4 grid, but larger or smaller sizes are possible.

Unless I missed something in the specification, there's no way for the client to tell the API how large the board is. I made it into an optional `boardSize` querystring parameter on the `Initialize` endpoint that defaults to 4, just to show how I would have handled it if I could also edit the client code. Without knowing the board size, the API cannot enforce the win condition, that being there are no more points available to connect the path to. If the board is infinitely large, then the only way to win is to draw a path that surrounds itself. Otherwise, the game could go on indefinitely.
