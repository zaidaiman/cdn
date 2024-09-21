# Complete Developer Network

## Description

This project consists of multiple components to handle different aspects of the Complete Developer Network:

- **CDNAPI**: Handles API requests and responses.
- **CDNDomain**: A library used by CDNAPI to manage domain definitions.
- **Terraform**: Manages EC2 deployment and infrastructure as code.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Components](#components)
  - [CDNAPI](#cdnapi)
  - [CDNDomain](#cdndomain)
  - [Terraform](#terraform)
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
   cd complete-developer-network
   ```

2. Restore and build solution:
   ```sh
   dotnet restore
   dotnet build
   ```

## Usage
1.  Run the API:
    ```sh
    dotnet run --project CDNAPI/CDNAPI.csproj
    ```
2.  Navigate to http://localhost:5007/swagger/index.html to view the API documentation.

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
├── docker-compose.yml     # Docker compose file
├── Dockerfile             # Dockerfile for CDNAPI build
├── README.md              # Project documentation
└── LICENSE                # License file
```

## Components

### CDNAPI
- **Description**: Handles API requests and responses.
- **Technologies**: .NET Core, Entity Framework Core, PostgreSQL, Swagger

### CDNDomain
- **Description**: A DLL library used by CDNAPI to manage domain definitions.
- **Technologies**: .NET Core, Entity Framework Core, PostgreSQL

### Terraform
- **Description**: Manages EC2 deployment and infrastructure as code.
- **Technologies**: Terraform, AWS

## Testing
1.  Run the tests:
    ```sh
    dotnet test
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
