# ShamWow Document Scrubber

Built for processing production files and removing any sensitive data.

This functions by adding data annotations/attributes to your transform POCOs.

### Scrub Modes

**Full** - Every property that has a compatible type will be scrubbed with the most basic method, unless annotated otherwise

**Marked** - Will only scrub the marked properties in the method described in the annotation


### Attributes

* Scrubber

> This is the main attribute required for marking data for scrubbing

### Scrubbing Types

* StringAtr
* DoubleAtr (Coming)
* DecimalAtr (Coming)
* IntAtr (Coming)


Example for scrubbing Email
```
[Scrub]
[StringAtr("Email")]
public string str {get; set;}
```


## How it Works

Through reflection this app is able to parse a POCO by properties and find data marked for scrubbing through custom attributes.

> If you have a POCO to map an XML file to you're ready to mark it for scrubbing.

### **Still a work in progress**