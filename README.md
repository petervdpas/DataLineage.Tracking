# DataLineage.Tracking

## Overview

DataLineage.Tracking is a .NET library that enables structured data lineage tracking for any mapping solution, including AutoMapper, Mapster, and custom mappers.

## Features

- Generic lineage tracking for any mapper
- Tracks source and target entities, fields, and transformation rules
- Decoupled and reusable via NuGet

## Installation

To install the package, run:

```sh
dotnet add package DataLineage.Tracking
```

## Usage

```csharp
using DataLineage.Tracking.Interfaces;
using DataLineage.Tracking.Lineage;

var lineageTracker = new DataLineageTracker();
```

## Contributing

Contributions are welcome! Please submit an issue or pull request.

## License

This project is licensed under the MIT License.
