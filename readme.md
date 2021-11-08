# Basic JWT Auth example for ASP.Net Core

## Token validation / Authentication
To add authentication into your ASP.Net app, check the `Startup` class => `ConfigureServices` method.
Look for `services.AddAuthentication`.
This will tell you app how to validate a token.
You have to enable token validation per route/controller separately. (Below in this readme file. In "Securing your API" section) 

## Swagger
To generate documentation about your authentication setup, check `Startup` class => `ConfigureServices` method
Look for `services.AddSwaggerGen`.

## Token generation
To create a token, check `IdentityController` and `JwtIssuer` classes.

## Securing your API
To add token validation to your controllers and routes, check `GreetingsController` class.
Use attribute `Authorize` on the whole controller or individual routes.

## Using information from a token
To add *claims* to your token check `JwtIssuer` class.
To get those *claims* from the token check `GreetingsController` class => `GreetByName` method.