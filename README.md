# recognizers-service

This is Microsoft's [Recognizers-Text](https://github.com/microsoft/Recognizers-Text) ([.NET Nuget Package](https://www.nuget.org/profiles/Recognizers.Text)) packaged into a REST service. It is useful to extract information, such as numbers and time, from a text sentence. Being in a REST service makes it easy to consume in a cloud native environment, or chatbot development.

To spin up the development environment

```shell
./setup.sh -r
```
To shut down:

```shell
./setup.sh -d
```

There is a swagger UI for API specifications at http://localhost:5000/swagger/index.html

## Features

Currently, it supports

- Number
- Currency
- Dimensions
- Number Range
- Currency Range (Not yet implemented)
- Dimension Range (Not yet implemented)

# About Dotnet 6
- [.NET documentation](https://docs.microsoft.com/en-us/dotnet/fundamentals/)
- [What's new in Dotnet 6](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6)
- [Official .NET Docker images](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/net-core-net-framework-containers/official-net-docker-images)