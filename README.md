# ShamWow Document Scrubber

[![Build Status](https://dev.azure.com/dss-gh/ShamWow/_apis/build/status/dills122.ShamWow?branchName=master)](https://dev.azure.com/dss-gh/ShamWow/_build/latest?definitionId=1?branchName=master)

ShamWow is a document scrubber to remove personally identifiable information while still retaining the same data type(Email, Phone, etc.). 

## How it Works

ShamWow uses reflection to gain access to all of the properties within a POCO (Plain Old C# Object) and iterates through all the properties while performing scrubbing specific to the type described within the POCO. You describe your type of scrubbing by annotating a property with its specific typed scrubber (ScrubDouble, ScrubInteger, etc.) and then add in the argument corresponding to the type of data you want your PII replaced with.

Example:
``` csharp
//How you would tell ShamWow to scrub an Address
[ScrubString("Address")]
public string StreetAddress1 { get; set; }
```

> In this example ShamWow as long as the value isn't null, then this property will have its value replaced with a fake Address

## Full Workflow Example

Once your POCOs are annotated and you have installed the ShamWow NuGet package you're all set you begin scrubbing documents.

#### Initial Setup

> Install needed NuGet packages along with their dependencies
1. [ShamWow](https://www.nuget.org/packages/ShamWow)
2. [ShamWow.Interfaces](https://www.nuget.org/packages/ShamWow.Interfaces/)

#### Step 1 Get Requirements

> Get object to scrub and get instance of ShamWow

``` csharp
//Fake object
var obj = new object();
//Get instance of ShamWow and run in marked only mode
IShamWow processor = ShamWowEngine.GetFactory().Create(obj, ScrubMode.Marked);
```

#### Step 2 Scrubbing

> Start scrubbing and get clean values

``` csharp
//Start scrubbing
processor.Scrub();
//Retrieve clean data
var cleanData = processor.CleanData();
```

#### Step 3 Verify Scrubbing

> Get Manifest and verify the scrubbing was successful

``` csharp
//Get Manifest for Auditing purposes
var manifest = processor.GetManifest();
//Check the manifest for errors
var IsSuccess = processor.CheckManifest();
```

## Special Attributes

> **PreserveValue** ensure the that this property's value will not be scrubbed or changed

> **StatefulScrub** allows you to define a variable name that then can be used throughout your POCO transform to ensure the value's of each decorated property will be the same.

``` csharp
//Example of Preserve attribute
[PreserveValue]
public string str {get; set;}
//Examaple of a stateful scrub
[StatefulScrub("StateOne")]
public int id {get; set;}
```


## Wrap Up 

Thats the basics of  ShamWow, it is meant to be very painless and require the least amount of intervention by the user. The three major items you need before scrubbing is 

1. POCOs for data to mapping
2. ShamWow NuGet package
3. PII to scrub
