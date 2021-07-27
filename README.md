## Table of contents
* [General Info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Swagger](#swagger)
* [Features](#features)

## General Info
### LocationAPI
Used to get Location information for from end validation.

#### Status
In development.

## Technologies

### 3rd party libraries
* libphonenumber-csharp: 8.12.28 : https://www.nuget.org/packages/libphonenumber-csharp
* Postal Codes: https://gist.github.com/matthewbednarski/4d15c7f50258b82e2d7e#file-postal-codes-json

### Core libraries
* ASP.NET Core: 3.1
* C#
* AutoMapper: 10.1.1
* AutoMapper.Extensions.Microsoft.DependencyInjection: 8.1.1
* Microsoft.NET.Test.Sdk: 16.10.0
* Moq: 4.16.1
* NBuilder: 6.1.0
* NUnit: 3.13.2
* NUnit3TestAdapter: 4.0.0
* Swashbuckle.AspNetCore: 6.1.4

## Setup
To run this project install and run using Visual Studio or Visual Studio Code.

## Swagger
Swagger is used to expose endpoints and provide a testing interface.

## Features
* Get regex patterns for Location information for input validation
  * Phone numbers
    * Possible number types such as Mobile/Cell phone or VOIP
    * Regex patterns
    * Example number formats

  * Post Codes/Zip Codes.
    * Notes
    * TerritoryOrCountry
    * ISO
    * Format
    * Regex
* Get list of Location ISO codes and names.

Ideally to be used in SPA or front end applications.
