# Description

This project provides a reusable model class which can be used to read a HAL formatted response as a statically typed object.

More description to follow...

# Examples

Example HAL Response:
```json
{
    "_links": {
        "self": {
            "href": "http://example.org/api/user/matthew"
        }
    },
    "id": "matthew",
    "name": "Matthew Weier O'Phinney",
    "_embedded": {
        "contacts": [
            {
                "_links": {
                    "self": {
                        "href": "http://example.org/api/user/mac_nibblet"
                    }
                },
                "id": "mac_nibblet",
                "name": "Antoine Hedgecock"
            },
            {
                "_links": {
                    "self": {
                        "href": "http://example.org/api/user/spiffyjr"
                    }
                },
                "id": "spiffyjr",
                "name": "Kyle Spraggs"
            }
        ],
        "website": {
            "_links": {
                "self": {
                    "href": "http://example.org/api/locations/mwop"
                }
            },
            "id": "mwop",
            "url": "http://www.mwop.net"
        },
    }
}
```

Model Classes
```csharp
public abstract class BaseResource
{
    [JsonProperty("id")]
    public string Id { get; set; }
}

public class WebsiteResource : BaseResource
{
    [JsonProperty("url")]
    public string Url { get; set; }
}

public class ContactResource : BaseResource
{
    [JsonProperty("name")]
    public string Name { get; set; }
}
```


## Deserializing a HAL response
```csharp
var httpClient = new HttpClient();
var resource = await httpClient.GetAsync("/url/to/my/resource");
var resourceContent = await resource.Content.ReadAsStringAsync();

var halResource = JsonConvert.DeserializeObject<HalcyonResponseModel<ContactResource>>(resourceContent, new HalcyonJsonConverter<ContactResource>());

var resourceName = halResource.Model.Name;
var resourceId = halResource.Model.Id;
```

## Retrieving an embedded item

```csharp
var httpClient = new HttpClient();
var resource = await httpClient.GetAsync("/url/to/my/resource");
var resourceContent = await resource.Content.ReadAsStringAsync();

var halResource = JsonConvert.DeserializeObject<HalcyonResponseModel<ContactResource>>(resourceContent, new HalcyonJsonConverter<ContactResource>());

// Fetches the embedded website resource, returns HalcyonResponseModel<Website>
var embeddedWebsite = halResource.GetEmbeddedResource<WebsiteResource>("website");

var websiteUrl = embeddedWebsite.Model.Url;
```

## Retrieving an embedded collection

```csharp
var httpClient = new HttpClient();
var resource = await httpClient.GetAsync("/url/to/my/resource");
var resourceContent = await resource.Content.ReadAsStringAsync();

var halResource = JsonConvert.DeserializeObject<HalcyonResponseModel<ContactResource>>(resourceContent, new HalcyonJsonConverter<ContactResource>());

// Fetches the embedded contacts resources, returns an array of HalcyonResponseModel<Website>
var embeddedContacts = halResource.GetEmbeddedResources<WebsiteResource>("contacts");

foreach(var contact in embeddedContacts) 
{
    var contactName = contact.Model.Name;
}
```

## Checking if a link exists
```csharp
var httpClient = new HttpClient();
var resource = await httpClient.GetAsync("/url/to/my/resource");
var resourceContent = await resource.Content.ReadAsStringAsync();

var halResource = JsonConvert.DeserializeObject<HalcyonResponseModel<ContactResource>>(resourceContent, new HalcyonJsonConverter<ContactResource>());

var selfLinkExists = halResource.ContainsLink("self");
```

## Checking if a link exists, and the link value matches
```csharp
var httpClient = new HttpClient();
var resource = await httpClient.GetAsync("/url/to/my/resource");
var resourceContent = await resource.Content.ReadAsStringAsync();

var halResource = JsonConvert.DeserializeObject<HalcyonResponseModel<ContactResource>>(resourceContent, new HalcyonJsonConverter<ContactResource>());

var selfLinkExistsAndValueMatches = halResource.ContainsLink("self", "http://example.org/api/user/matthew");
```