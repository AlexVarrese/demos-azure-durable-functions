# demos-azure-durable-functions

This repo contains a Function App which is part of a blog post series about Azure Durable Functions and it also serves as demo material some for my presentations and [Youtube videos]().

## Update 2018-06-15

I've restructured the solution and removed the functions which depended on 3rd party APIs (Meetup, Google Directions) which require additional accounts in order to use them. This code is still in the repo but I will move it to another one.

Now the chaining and fan-out & fan-in examples use the excellent (and free to use) [SWAPI](https://swapi.co/) which does't require any authentication and makes the demo easier to use for newcomers.

## Demos

The Function App solution consists of the following demos which can be executed independently.

### 01-Basic

#### HelloWorld

Shows how an orchestration function ([`HelloWorld`](\src\DurableFunctions.Demo.DotNetCore\01-Basics\Orchestrations\HelloWorld.cs)) calls an activity function ([`HelloWorldActivity`](\src\DurableFunctions.Demo.DotNetCore\01-Basics\Activities\HelloWorldActivity.cs)).

Run the solution and use the requests in [HelloWorld.http](\api-tests\orchestrations\01-Basic\HelloWorld.http) to start the orchestration.

#### HelloName

Shows how an orchestration function ([`HelloName`](\src\DurableFunctions.Demo.DotNetCore\01-Basics\Orchestrations\HelloName.cs)) calls an activity function ([`HelloNameActivity`](\src\DurableFunctions.Demo.DotNetCore\01-Basics\Activities\HelloWorldActivity.cs)) and passing a some data to the activity.

Run the solution and use the requests in [HelloName.http](\api-tests\orchestrations\01-Basic\HelloName.http) to start the orchestration.

### 02-Chaining

Shows how an orchestration function ([`GetSwCharacterInfo`](\src\DurableFunctions.Demo.DotNetCore\02-Chaining\Orchestrations\GetSwCharacterInfo.cs)) calls two activity functions in a chain ([`SearchCharacter`](src\DurableFunctions.Demo.DotNetCore\02-Chaining\Activities\SearchCharacter.cs) -> [`GetPlanet`](\src\DurableFunctions.Demo.DotNetCore\02-Chaining\Activities\GetPlanet.cs)).

The orchestration function requires a (partial) name of a Star Wars character. This character is searched in the `SearchCharacter` activity which uses the `swapi.co` API. When a character is returned the `GetPlanet` activity is called (also uses `swapi.co` again) to return the name of the home planet of the character. The full name of the character and the planet are returned from the orchestration.

Run the solution and use the requests in [GetSwCharacterInfo.http](\api-tests\orchestrations\02-Chaining\GetSwCharacterInfo.http) to start the orchestration.

### 03-FanOutFanIn

Shows how an orchestration function ([`GetSwPlanetResidents`](\src\DurableFunctions.Demo.DotNetCore\03-FanOutFanIn\Orchestrations\GetSwPlanetResidents.cs)) calls activity functions using the fan-out/fan-in pattern ([`SearchPlanet`](\src\DurableFunctions.Demo.DotNetCore\03-FanOutFanIn\Activities\SearchPlanet.cs) -> n * [`GetCharacter`](\src\DurableFunctions.Demo.DotNetCore\03-FanOutFanIn\Activities\GetCharacter.cs)).

The orchestration function requires a (partial) name of a Star Wars planet. This planet is searched in the `SearchPlanet` activity which uses the `swapi.co` API. When a planet is returned the `GetCharacter` activity is called for each of the residents returned found in the `SearchPlanet` result. The full name of the planet and the names of the planet residents are returned from the orchestration.

Run the solution and use the requests in [GetSwPlanetResidents.http](\api-tests\orchestrations\03-FanOutFanIn\GetSwPlanetResidents.http) to start the orchestration.

## Using the VS Code REST Client

I strongly recommend to the excellent [REST client](https://github.com/Huachao/vscode-restclient) for VS Code so you can use the http files located in the [`api-tests`](\api-tests) folder to start the orchestration functions.

## My blog posts & videos with more info about Durable Functions.

[Azure Durable Functions - Stateful function orchestrations (part 1)](http://blog.marcduiker.nl/2017/11/05/durable-azure-functions-stateful-orchestrations.html)

[Azure Durable Functions - Stateful function orchestrations (part 2)](http://blog.marcduiker.nl/2017/11/07/durable-azure-functions-stateful-orchestrations-part2.html)

[Durable Functions on YouTube (part 1)](https://blog.marcduiker.nl/2017/11/15/durable-functions-youtube-part1.html)

[Durable Functions on YouTube (part 2) - Eternal orchestrations & external events](https://blog.marcduiker.nl/2017/12/01/durable-functions-youtube-part2.html)

[Durable Functions on YouTube (part 3) - The Function Chaining Pattern](https://blog.marcduiker.nl/2018/03/06/durable-functions-youtube-part3.html)