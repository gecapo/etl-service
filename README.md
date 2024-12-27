# ETL Service

A flexible and extensible ETL (Extract, Transform, Load) service built with .NET, designed to handle a variety of data processing scenarios with ease. 

## Features

- **Pluggable Data Providers**: 
  - Register different data providers or use preexisting ones with your own configuration.
- **Customizable Parser Strategies**: 
  - Register various parser strategies or implement your own to handle specific data parsing needs.
- **Generic Processing Framework**: 
  - All data is processed through a generic processor strategy. 
  - Optionally, you can create and use your own processing logic.
- **Logging with Serilog**: 
  - Built-in support for Serilog, allowing configurable and structured logging.
- **Pluggable Logging Sinks**: 
  - Flexibly register different sinks to send logs.
