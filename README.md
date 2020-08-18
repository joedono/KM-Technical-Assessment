# KM Technical Assessment

https://technical-assessment.konicaminoltamarketplace.com/

## Summary
Code is written using .NET Core 3.1 and Microsoft Visual Studio Community 2019.

It is a rather standard Web API project with a `HomeController` handling all the endpoints, validation, game logic, and tracking moves across the board, and several model classes to handle request and response data. The application uses a `MemoryCache` class to store game data directly from within `HomeController`.

## Running the API
The project can be run on your standard IIS Express instance that is bundled with Visual Studio. It will use the `Http` API model from the client. The project is also hosted on Heroku at https://km-technical-assessment.herokuapp.com, and can be connected to there.

The barest minimum of routing is used. For instance, the call to initialize is:

    https://km-technical-assessment.herokuapp.com/initialize

And the call for `/node-clicked` is simply:

    https://km-technical-assessment.herokuapp.com/node-clicked
