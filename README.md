# ShamWow Document Scrubber

Built for processing production files and removing any sensitive data.

This functions by adding data annotations/attributes to your transform POCOs.


### Getting Started

#### Step One

Make sure all packages and dependencies are installed
* ShamWow
* Rock.TSI.Ambassador.Contracts
* Faker.Net
* Gridsum.DataflowEx
* System.Data.HashFunction.xxHash

> If you're using this with a Nexsys specific transform, then skip this step

#### Step Two

Create a transform object that maps all properties from your file to the object and annotate for scrubbing

``` CSharp
public class TestTransform 
{
    public int Id {get; set;}
    [Scrub]
    [StringAtr("Email")]
    public string Email {get; set;}
}
```
> for a list of available [Attributes](#attributes)

#### Step Three

Transform File to object and begin processing

``` CSharp
    //Gets instance of processor
    IShamWow process = ShamWowEngine.GetFactory().Create(t, ShamWow.Constants.ScrubTypes.Marked);
    //Starts Scrubbing
    process.Scrub();
    //Gets the clean data back
    var clean = process.CleanData();
    //Returns if the Scrub was successful
    var manifestValue = process.CheckManifest();
```




### Scrub Modes

**Full** - Every property that has a compatible type will be scrubbed with the most basic method, unless annotated otherwise

**Marked** - Will only scrub the marked properties in the method described in the annotation


### Attributes

* Scrubber

> This is the main attribute required for marking data for scrubbing

### Scrubbing Types

#### StringAtr

> Available String Scrub Types

* Address
* AddressTwo
* City
* State
* Zip
* Phone
* SSN
* Email
* DOB
* FullName
* LastName
* FirstName
* MiddleName
* UserName



#### DoubleAtr

> Available Double Types

**Coming Soon**

#### DecimalAtr

> Available Decimal Types

**Coming Soon**

#### IntAtr

> Available Integer Types

* Phone
* Zip
* VIN (Coming Soon)
* PIN (Coming Soon)


Example for scrubbing Email
``` CSharp
[Scrub]
[StringAtr("Email")]
public string str {get; set;}
```


## How it Works

Through reflection this app is able to parse a POCO by properties and find data marked for scrubbing through custom attributes.

> If you have a POCO then you're all set to start scrubbing personally identifiable information (PII)

> The POCO for translating the file to an object will always be the user's responsibility


### Future

* XML and JSON file Readers
* TPL Dataflow Pipeline for process flow
* Extensive Scrub Types

### **Still a work in progress**