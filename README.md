# Lokalise

##  A C# standard wrapper for Lokalise 

## Getting started
Install the package with `dotnet add package Lokalise` or other ways mentioned on https://www.nuget.org/packages/Lokalise/

### Using the Cli2Lokalise implementation
**IMPORTANT!** Download and add the Lokalise2 CLI tool to your PATH or download it to a place and refer to it from your code.


```cs
var lokalise = new Cli2Lokalise(); // or new Cli2Lokalise(path_to_lokalise2_file);
var token = _configuration["Token"];
var projectId = _configuration["ProjectId"];
await lokalise.Download(token, projectId, DownloadFormat.Json);
```


### Using the HttpLokalise implementation
```cs
var lokalise = new HttpLokalise();
var token = _configuration["Token"];
var projectId = _configuration["ProjectId"];
await lokalise.Download(token, projectId, DownloadFormat.Json);
```
