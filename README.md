# MERLOT PDF Generation Service
The MERLOT PDF Service is a microservice for generating PDF files for a given JSON input.

Currently this service supports the generation of a contract PDF given the provided details using the REST API.


## Development

To start development for the MERLOT marketplace, please refer to [this document](https://github.com/merlot-education/.github/blob/main/Docs/DevEnv.md)
to set up a local WSL development environment of all relevant services.
This is by far the easiest way to get everything up and running locally.

## Structure

```
├── PdfService
│   ├── Controllers   # external REST API controllers
│   ├── Documents     # Templates for generation of PDFs
│   ├── Models        # internal data models of pdf-generation-related data
│   ├── Services      # internal services for processing data from the controller layer
```

## Dependencies
None

## Build

    dotnet build PdfService

## Run

    dotnet run --project PdfService

## Deploy (Docker)

This microservice can be deployed as part of the full MERLOT docker stack at
[localdeployment](https://github.com/merlot-education/localdeployment).

## Deploy (Helm)
TODO