We are creating a WebAPI in ASP.Net Core and our Angular App seperately to ensure that each application run's seperately on different machines. 
Our Angular app request to ASP.Net Core API via Http and gain data in response from API.
To create web API using ASP .Net Core we need to insatll .Net Core SDK. Here I am working on .Net Core Version: 3.1.1 and angular version 9
To create the ASP .net core webAPI we need to run "dotnet new webapi -n DatingApp.API" command which create ASP .net core webAPI project in our folder with "WeatherForecastController" pre-initialize controller


    How ASP .Net Core App run...?
->  Every Asp.net core app have Program.cs class. which contain's single method i.e. main method.
->  So when we started a Asp.net core app to search for program class and it run's main method of this program class
-> Main method called "CreateHostBuilder" method which set's some default to run our applications. This method tell our application  to use Kestrel server to run our application which provided by .net core which hosting our web api. this methopd is also configureSome Defaults like some logging information's from appsetting.json and so on... which are logged down on console.
it also read from configuration setting file which is there in oue project. it also initialize the startup class

Startup class 
Sone configurations are injected into our starup class by "Configuration" property this allow's us to asscess setting's in our appsettings.jason file required for our application. we can add addition's setting
"ConfigureServices" method is our dependencies injection container. so whenever we added in our project and we want the other can consume that as a service then we need to add them into ConfigureServices method
"Configure" this method we use to configure out HTTP pipeline