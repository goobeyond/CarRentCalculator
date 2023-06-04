# CarRentCalculator
## Intro
This project calculates the rent of a car based on the subscription type, car type, estimated time and distance the car is used.

## Usage
Once the program has been started, we have an endpoint under the rout `POST Rent/Estimate` that accepts the following body as request:
```
    {
        "subscriptionType": "Occasional",
        "carType": "Small",
        "from": "2023-06-04T15:23:16.643Z",
        "to": "2023-06-04T15:23:16.643Z",
        "estimatedDistanceKm": 0
    }
```

The following validations are implemented:
- The From and To date must be valid, although there are no limits set.
- From date must be before To date.
- From date and To date can't be equal.
- Estimated distance cannot be negative.

The information regarding rates has been stored in a JSON file that acts as our database.

For each request, we load the file and assuming the request is valid, we return the estimated cost of renting the car.

## Assumptions and Limitations
There are many ways this program could be improved or updated depending on the usage.
### Updating rates


At the moment we assume the rates do not get changed often.

If the rates need to be updated, we can adjust the application in 1 of 2 ways:

1- If the rates get updated by a person and only occasionally, we can add an endpoint that allows us to upload a new JSON file and overwrite the old file. This approach is simple, but unsafe as it is prone to human error in the JSON file.

2- If the rates are updated automatically, we can create an Azure funtion to trigger at the appropriate intervals and update the rates for us.

We could also implement some caching mechanism to prevent reading from the file too many times.

### Caching
Again depending on how frequently the rates change, we could implement a cache to prevent reading the same data over and over again from the file.

1- If files get uploaded manually we can invalidate the cache everytime a new version is uploaded.

2- If the rates are adjusted automatically in fixed intervals, we can invalidate the cache as the new values are calculated

### UI
Unfortunately, due to my lack of experience in UI development, the app has only a swagger page to interact with.

