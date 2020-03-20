# Json.Extensions

Awesome Extension Library to help map `JObject`/`JArray` to JPaths.  As well as allowing you to set values using said JPath

- [Json to JPaths with Values](https://github.com/brandonhein/Json.Extensions/blob/master/README.md#json-to-jpaths)
- [Json to JPaths](https://github.com/brandonhein/Json.Extensions/blob/master/README.md#json-paths-alone)
- [JPaths with Values to Json](https://github.com/brandonhein/Json.Extensions/blob/master/README.md#paths-with-values-to-json)
- [JPaths to Json Shell](https://github.com/brandonhein/Json.Extensions/blob/master/README.md#paths-to-json-shell)

---

## Json to JPaths

#### Json Paths with Values
When doing mapping, you're going to want to know the path to get a value in the json.  This will allow you to parse thru the json, and return a `IDictionary<string, object>` result for you.  Where the Key of the dictionary is the string JPath, and the Value is the object value from the json

Sample Input
```json
{
 "OfferId": "4E77902B-3A23-4A13-8699-90B23B58EE21",
 "SubscriptionId": "844a7c65-5523-4aaf-9e17-6e223178ee51",
 "EventType": "Purchase",
 "SubscriptionStartDate": "2016-01-01T00:00:00Z",
 "SubscriptionEndDate": "9999-12-31T23:59:59.9999999",  
 "SubscriptionLicenseCount": "25",
 "IsTrial": "True",
 "EmailRecipients": [{
  "EmailAddresses": ["8IQ46/U36757n1apJJGILv4qjWyYV+1vR/GbJYx3QLg="],
  "UserPrincipalName": "bI+KE5twJoPHZegYiJTFiQSMgeQ1+e0+vzw+ZmWE4SaTHXdxs/uqeY6hEWnCCEOk",  
  "FirstName": "pjUjsiidR28eBKH0NA7yEA==",
  "LastName": "863qxqfQTSWpU3KJ/MlfMQ==",
  "DisplayName": "Andrew S",
  "UserCountry": "UK",
  "OfferName": "30-Day Trial"
 }]
}
```
C# Code to call
```csharp
IDictionary<string, object> result = JsonParser.PathsAndValues(json);
```
Output
```json
{
  "OfferId": "4E77902B-3A23-4A13-8699-90B23B58EE21",
  "SubscriptionId": "844a7c65-5523-4aaf-9e17-6e223178ee51",
  "EventType": "Purchase",
  "SubscriptionStartDate": "2016-01-01T00:00:00Z",
  "SubscriptionEndDate": "9999-12-31T23:59:59.9999999",
  "SubscriptionLicenseCount": "25",
  "IsTrial": "True",
  "EmailRecipients[0].EmailAddresses[0]": "8IQ46/U36757n1apJJGILv4qjWyYV+1vR/GbJYx3QLg=",
  "EmailRecipients[0].UserPrincipalName": "bI+KE5twJoPHZegYiJTFiQSMgeQ1+e0+vzw+ZmWE4SaTHXdxs/uqeY6hEWnCCEOk",
  "EmailRecipients[0].FirstName": "pjUjsiidR28eBKH0NA7yEA==",
  "EmailRecipients[0].LastName": "863qxqfQTSWpU3KJ/MlfMQ==",
  "EmailRecipients[0].DisplayName": "Andrew S",
  "EmailRecipients[0].UserCountry": "UK",
  "EmailRecipients[0].OfferName": "30-Day Trial"
}
```

#### Json Paths alone
If you just want to parse thru any json object to get the JPaths (no values)... you can do so by parsing it and getting all the JPaths in an `IEnumerable<string>`

Sample Input
```json
{
  "squadName": "Super hero squad",
  "homeTown": "Metro City",
  "formed": 2016,
  "secretBase": "Super tower",
  "active": true,
  "members": [
    {
      "name": "Molecule Man",
      "age": 29,
      "secretIdentity": "Dan Jukes",
      "powers": [
        "Radiation resistance",
        "Turning tiny",
        "Radiation blast"
      ]
    }
  ]
}
```
C# Code to call
```csharp
IEnumerable<string> result = JsonParser.Paths(json);
```
Output
```json
[
  "squadName",
  "homeTown",
  "formed",
  "secretBase",
  "active",
  "members[0].name",
  "members[0].age",
  "members[0].secretIdentity",
  "members[0].powers[0]",
  "members[0].powers[1]",
  "members[0].powers[2]"
]
```

---

## JPaths to Json

#### Paths with Values to Json
When you have an `IDictionary<string, object>` of your paths and the values.  You can call `JsonSimplifier` to create a JSON we all know and love

Sample Input
```json
{
    "[0].zipCode": "90210",
    "[0].city": "Beverly Hills",
    "[0].state": "CA",
    "[0].region": "West",
    "[0].division": "Pacific",
    "[0].latitude": 34.088808,
    "[0].longitude": -118.40612,
    "[0].timezone": "PST",
    "[0].utcOffset": "-08:00",
    "[0].observesDaylightSavings": true,
    "[0].areaCode": "310",
    "[0].countyFIPS": "06037",
    "[0].countyName": "Los Angeles",
    "[0].isAwake": true,
    "[0].utcTime": "2020-01-25T23:07:01.6789983Z",
    "[0].localTime": "2020-01-25T18:07:01.679-05:00",
    "[0].readableTime": "2020-01-25 03:07:01 PM",
    "[1].zipCode": "12345",
    "[1].city": "Schenectady",
    "[1].state": "NY",
    "[1].region": "Northeast",
    "[1].division": "Mid-Atlantic",
    "[1].latitude": 42.833261,
    "[1].longitude": -74.058015,
    "[1].timezone": "EST",
    "[1].utcOffset": "-05:00",
    "[1].observesDaylightSavings": true,
    "[1].areaCode": "518",
    "[1].countyFIPS": "36093",
    "[1].countyName": "Schenectady",
    "[1].isAwake": true,
    "[1].utcTime": "2020-01-25T23:07:01.6793892Z",
    "[1].localTime": "2020-01-25T18:07:01.679-05:00",
    "[1].readableTime": "2020-01-25 06:07:01 PM"
}
```
C# Code to call
```csharp
var json = JsonSimplifier.Simplify(pathsAndValues);
```
Output
```json
[
  {
    "zipCode": "90210",
    "city": "Beverly Hills",
    "state": "CA",
    "region": "West",
    "division": "Pacific",
    "latitude": 34.088808,
    "longitude": -118.40612,
    "timezone": "PST",
    "utcOffset": "-08:00",
    "observesDaylightSavings": true,
    "areaCode": "310",
    "countyFIPS": "06037",
    "countyName": "Los Angeles",
    "isAwake": true,
    "utcTime": "2020-01-25T23:07:01.6789983Z",
    "localTime": "2020-01-25T18:07:01.679-05:00",
    "readableTime": "2020-01-25 03:07:01 PM"
  },
  {
    "zipCode": "12345",
    "city": "Schenectady",
    "state": "NY",
    "region": "Northeast",
    "division": "Mid-Atlantic",
    "latitude": 42.833261,
    "longitude": -74.058015,
    "timezone": "EST",
    "utcOffset": "-05:00",
    "observesDaylightSavings": true,
    "areaCode": "518",
    "countyFIPS": "36093",
    "countyName": "Schenectady",
    "isAwake": true,
    "utcTime": "2020-01-25T23:07:01.6793892Z",
    "localTime": "2020-01-25T18:07:01.679-05:00",
    "readableTime": "2020-01-25 06:07:01 PM"
  }
]
```

#### Paths to Json Shell
If you wanted to create a shell Json Object from the paths you have... you can do so calling the `JsonShell` class

Sample Input
```json
[
  "[0].zipCode",
  "[0].city",
  "[0].state",
  "[0].region",
  "[0].division",
  "[0].latitude",
  "[0].longitude",
  "[0].timezone",
  "[0].utcOffset",
  "[0].observesDaylightSavings",
  "[0].areaCode",
  "[0].countyFIPS",
  "[0].countyName",
  "[0].isAwake",
  "[0].utcTime",
  "[0].localTime",
  "[0].readableTime"
]
```
C# Code to call
```csharp
var jsonShell = JsonShell.GenerateFromPaths(paths);
```
Output
```json
[
  {
    "zipCode": {},
    "city": {},
    "state": {},
    "region": {},
    "division": {},
    "latitude": {},
    "longitude": {},
    "timezone": {},
    "utcOffset": {},
    "observesDaylightSavings": {},
    "areaCode": {},
    "countyFIPS": {},
    "countyName": {},
    "isAwake": {},
    "utcTime": {},
    "localTime": {},
    "readableTime": {}
  }
]
```
