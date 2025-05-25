Project has: gRPC, c#, microservice architecture, dates business logic that contains 3 cases:
   1 DayOfMonth is < that maxDaysInNextMonth -> next month with DayOfMonth value
   2 DayOfMonth is > maxDaysInNextMonth and adjust is true -> the nearest last day of the next month
   3 DayOfMonth is > maxDaysInNextMonth and adjust is false -> the nearest day (in many cases is the first day of +2 month)

Simple way to build the gRPC server is command in terminal: dotnet run
WebApi: http is built by developer interface as usual

Project has proto files that maps the C# classes

Exception middleware was included,
Validation was included
