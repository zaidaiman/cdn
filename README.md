# Complete Developer Network

## Description

This project consists of multiple components to handle different aspects of the Complete Developer Network:

- **CDNAPI**: Handles API requests and responses.
- **CDNDomain**: A library used by CDNAPI to manage domain definitions.
- **Terraform**: Manages EC2 deployment and infrastructure as code.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Notes](#notes)
- [Usage](#usage)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Testing](#testing)
- [Deployment](#deployment)
- [License](#license)

## Prerequisites

- .NET SDK 8.0 or later
- PostgreSQL
- Terraform
- AWS CLI (for deployment)

## Installation

1. Clone the repository:
   ```sh
   git clone git@github.com:zaidaiman/cdn.git
   cd cdn
   ```

2. Restore and build solution:
   ```sh
   dotnet restore
   dotnet build
   ```

3. Build webapp:
   ```sh
   cd webapp
   npm i
   ```

## Notes

1. With the recent release of Apple Sequoia OS, Apple has changed how it handle certificates, which resulted failure when perform `dotnet dev-certs https --trust`. The team at Microsoft is working on it and scheduled to release by Oct 2024: https://github.com/dotnet/announcements/issues/324.

2. I have made the changes so that https redirection only happens when it is not in development environment.

## Usage

1.  Run the API in project root directory:
    ```sh
    dotnet run --project CDNAPI/CDNAPI.csproj
    ```

2.  Navigate to http://localhost:5007/swagger/index.html to view the API documentation.

3.  Run the webapp in webapp directory:
    ```sh
    npm start
    # default url should be http://localhost:4200
    ```

## Configuration

1.  For development, create `appSettings.Development.json`
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=freelancer;Username=user;Password=password"
      },
      "Jwt": {
        "Key": "<generate-issuer-key>",
        "Issuer": "Complete Developer Network",
        "Audience": "Complete Developer Network API"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "None",
          "Microsoft.Hosting.Lifetime": "None"
        }
      }
    }
    ```

2.  To generate issuer key, run the following command and replace the value in `appSettings.Development.json`:
    ```sh
    openssl rand -base64 32
    ```

## Project Structure
```
complete-developer-network/
├── CDNAPI/                # API project
├── CDNDomain/             # Domain library
├── terraform/             # Terraform scripts for deployment
├── webapp/                # Simple web app demo
├── docker-compose.yml     # Docker compose file
├── Dockerfile             # Dockerfile for CDNAPI build
├── README.md              # Project documentation
└── LICENSE                # License file
```

## Testing
1.  Run the tests:
    ```sh
    dotnet test --logger "console;verbosity=detailed"
    ```

## Deployment
1.  Initialize Terraform:
    ```sh
    cd terraform
    terraform init
    ```
2.  Plan the deployment and verify the changes to be made (double confirm, as this will create/delete resources in AWS!):
    ```sh
    terraform plan
    ```
3.  Apply the changes:
    ```sh
    terraform apply
    ```

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
