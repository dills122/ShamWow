# Working with Scrubbers and Attributes

### Contents

1. [Strings](#Strings)
2. [Decimal](#Decimal)
3. [Double](#Double)
4. [Integer](#Integer)
5. [Long](#Long)
6. [Short](#Short)
7. [Misc](#Miscellaneous)
8. [Other Attributes](#Other-Scrubbers)
   1. [Predefined Value Atr.](#PredefinedValue)
   2. [Preserve Value Atr.](#PreserveValue)
   3. [Stateful Scrub Atr.](#StatefulScrub)

This document will go over the available attributes and scrub types ShamWow allows users to leverage when removing PII. 

ShamWow supports all primitive types such as

1. Strings
2. Decimal
3. Double
4. Integer
5. Long
6. Short
7. Byte (Only Arrays)
8. Arrays of any type above


## Strings

#### Constructors

``` csharp
//Basic scrub that will default to lorem ipsum text
public ScrubString() { }

//Allows user to select the string scrubber to use
public ScrubString(StringScrubber scrubber) { }

//Allows a user to replace scrubbed text with a redacted string
public ScrubString(StringScrubber scrubber, bool IsRedacted) { }

//Allows a user to define the length for lorem ipsum text
public ScrubString(StringScrubber scrubber, int length) { }

//Alows a user to generate an id or number as a string
public ScrubString(StringScrubber, int minValue, int maxValue) { }
```

#### Scrubbers

1. Address
2. AddressTwo
3. City
4. State
5. StateCode
6. Zip
7. CountyFIPS
8. CountyName
9. Phone
10. Email
11. DOB
12. SSN
13. FullName
14. FirstName
15. MiddleName
16. LastName
17. UserName
18. Number


## Decimal

#### Constructors

``` csharp
//Uses default scrub to generate random number
public ScrubDecimal() { }

//Defines a scrubber to use
public ScrubDecimal(DecimalScrubber scrubber) { }
```

#### Scrubbers

> Currently Decimal scrubbing only supports default at the moment, so no specific scrubbers are defined as of right now.

1. Default (Currently generates random number)


## Double

#### Constructors

``` csharp
//Uses default scrub to generate random number
public ScrubDouble() { }

//Defines a scrubber to use
public ScrubDouble(DoubleScrubber scrubber) { }
```

#### Scrubbers

> Currently Double scrubbing only supports default at the moment, so no specific scrubbers are defined as of right now.

1. Default (Currently generates random number)


## Integer 

#### Constructor

``` csharp 
//Uses default scrub to generate random number
public ScrubInteger() { }

//Defines a scrubber to use
public ScrubInteger(IntegerScrubber scrubber) { }
```

#### Scrubbers

1. Zip (First 5 only)
2. Phone
3. VIN


## Long 

#### Constructor

``` csharp 
//Uses default scrub to generate random number
public ScrubLong() { }

//Defines a scrubber to use
public ScrubLong(LongScrubber scrubber) { }
```

#### Scrubbers

1. Phone

## Short

#### Constructors

``` csharp
//Uses default scrub to generate random number
public ScrubShort() { }

//Defines a scrubber to use
public ScrubShort(ShortScrubber scrubber) { }
```

#### Scrubbers

> Currently Short scrubbing only supports default at the moment, so no specific scrubbers are defined as of right now.

1. Default (Currently generates random number)

## Miscellaneous

#### Byte
> The Byte format currently does not support custom scrubbing and does not have an attribute for itself. Currently only Byte Arrays work, which will always be treated as a document.

#### Arrays
> Arrays currently work as treating each of their items as its own individual property and scrubbing them individually then joining them back into an array

## Other Scrubbers

#### PredefinedValue

``` csharp
//Allows a user to define the scrub value before-hand
public PredefinedValue(object value) { }
```

This scrubber allows a user to create a scrub value to replace the original value within the object.

> ***Note type must be the same as original type***


#### PreserveValue 

``` csharp
//Marks the property as do not scrub, leaving the original value intact
public PreserveValue() { }
```

#### StatefulScrub

``` csharp
//Allows a user to define a variable name to store scrub state 
public StatefulScrub(string value) { }

```

This attribute allows a user to define a key that will allow all other properties using the same key to have consistent state across the entire object.