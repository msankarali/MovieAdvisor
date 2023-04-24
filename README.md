
# Movie Advisor

Movie advisor app is a web application that collects and provides movie recommendations using .NET 7 and Clean Architecture.

## Built With

- .NET 7
- MsSQL
- Docker
- RabbitMq
- Redis
- Hangfire
## Installation and Running

### Using Docker Compose

To run the application with Docker Compose, follow these steps:

1. Install Docker Compose if you haven't already by running the following command:

    ```bash
    sudo apt install docker-compose
    ```

2. Navigate to the project's root directory and run the following command to start the application:

    ```bash
    docker-compose up
    ```

This will start the application and all its dependencies (RabbitMQ, Redis, MsSQL, and Hangfire) in separate containers.


### .NET 7

To run the application using .NET 7, follow these steps:

1. Install the [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet/7.0) if you haven't already.

2. Navigate to the project's root directory and run the following command to build the application:

    ```bash
    dotnet build
    ```

3. Run the following command to start the application:

    ```bash
    dotnet restore
    dotnet build
    dotnet run
    ```

This will start the application and listen on `https://localhost:5270`.
## Roadmap

- [ ] Create API for movie list, requiring authentication with Auth0 or JWT login method
    - [ ] Auth operations
        - [ ] Register
        - [ ] Login
    - [x] Get paginated list of all movies
        - [x] Cache paged data in Redis using page size and number as strategy for 7 days
    - [x] Get movie details, including average rating, user rating, and user notes
        - [x] Cache data in Redis using movie as strategy
    - [x] Add note and rating to a selected movie
        - [x] Remove cache using selected movie as strategy
    - [x] Send movie recommendation to a given email address using RabbitMQ
- [x] themoviedb.org API data collector in an hourly scheduled job
- [x] Redis for caching
- [x] RabbitMQ for message queuing
- [x] Hangfire for scheduled job operations
- [x] Use Docker to containerize
- [ ] Document the API
- [ ] Unit and integration tests
