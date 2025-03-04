# DataLineage.Tracking

## Overview

**DataLineage.Tracking** is a powerful and extensible **data lineage tracking** library for .NET. It allows structured tracking of data transformations across various mapping solutions, including **AutoMapper, Mapster, custom mappers**, or any other mapping strategy.

## Features 🚀

✅ **Generic & Flexible** – Works with any mapping framework or custom implementations  
✅ **Tracks Data Transformations** – Captures how data flows from source to target  
✅ **Dependency Injection (DI) Ready** – Easily integrates into .NET applications  
✅ **Extensible Sink System** – Store lineage data in memory, JSON files, databases, etc.  
✅ **Asynchronous Support** – Efficient non-blocking lineage storage  

## Installation 📦

To install **DataLineage.Tracking**, run:

```sh
dotnet add package DataLineage.Tracking
```

## Getting Started 🛠️

### **1️⃣ Setting Up Dependency Injection**

Register **DataLineage.Tracking** in your **.NET DI container**:

```csharp
using Microsoft.Extensions.DependencyInjection;
using DataLineage.Tracking;
using DataLineage.Tracking.Sinks;

// Configure DI
var serviceProvider = new ServiceCollection()
    .AddDataLineageTracking(typeof(Program).Assembly, sinks: new FileLineageSink("lineage.json"))
    .BuildServiceProvider();

// Resolve lineage tracker
var lineageTracker = serviceProvider.GetRequiredService<IDataLineageTracker>();
```

### **2️⃣ Tracking Data Transformations**

Use the **lineage tracker** to monitor how data moves from a source field to a target field:

```csharp
lineageTracker.Track(
    sourceName: "OrderSystem",
    sourceEntity: "Order",
    sourceField: "TotalPrice",
    sourceValidated: true,
    sourceDescription: "The total price of an order",
    transformationRule: "Converted from decimal to string",
    targetName: "ReportingDB",
    targetEntity: "FinancialReport",
    targetField: "OrderTotal",
    targetValidated: true,
    targetDescription: "The order total in reporting currency"
);
```

### **3️⃣ Using a Generic Mapper with Lineage**

You can implement a **trackable entity mapper** to **automate lineage tracking**:

```csharp
using DataLineage.Tracking.Mapping;
using DataLineage.Tracking.Interfaces;

public class OrderToReportMapper : TrackableEntityMapper<Order, FinancialReport>
{
    public OrderToReportMapper(IDataLineageTracker tracker) : base(tracker) {}

    public override FinancialReport Map(Order order)
    {
        var report = new FinancialReport
        {
            OrderId = order.Id,
            OrderTotal = order.TotalPrice.ToString("F2")
        };

        _lineageTracker.Track(
            "OrderSystem", nameof(Order), nameof(Order.TotalPrice), true, "Order price",
            "Converted to string",
            "ReportingDB", nameof(FinancialReport), nameof(FinancialReport.OrderTotal), true, "Formatted order total"
        );

        return report;
    }
}
```

### **4️⃣ Using Lineage Sinks (File, Database, etc.)**

By default, lineage data is **stored in memory**, but you can store it in **JSON files** or a **relational database**.

#### **📂 File-based Storage**

```csharp
var fileSink = new FileLineageSink("lineage.json", deleteOnStartup: true);
var lineageTracker = new DataLineageTracker(fileSink);
```

#### **🗄️ Database Storage (SQL)**

```csharp
using System.Data.SqlClient;
using DataLineage.Tracking.Sinks;

// Configure a database lineage sink
var sqlSink = new SqlLineageSink(() => new SqlConnection("Your_Connection_String"));

// Register in DI
var serviceProvider = new ServiceCollection()
    .AddDataLineageTracking(typeof(Program).Assembly, sinks: sqlSink)
    .BuildServiceProvider();
```

### **5️⃣ Retrieving Lineage History**

```csharp
var history = lineageTracker.GetLineage();
foreach (var entry in history)
{
    Console.WriteLine(entry);
}
```

---

## **Advanced Features**

### ✅ **Multiple Sinks**

You can configure multiple **lineage sinks** at once, for example, storing lineage in **both a file and a database**:

```csharp
var fileSink = new FileLineageSink("lineage.json");
var sqlSink = new SqlLineageSink(() => new SqlConnection("Your_Connection_String"));

var serviceProvider = new ServiceCollection()
    .AddDataLineageTracking(typeof(Program).Assembly, sinks: fileSink, sqlSink)
    .BuildServiceProvider();
```

### ✅ **Asynchronous Storage**

All lineage operations **support async storage**, ensuring efficiency in high-performance applications.

---

## **Contributing**

Contributions are welcome! If you find a bug, want a feature, or have an idea, feel free to open an **issue** or **pull request** on GitHub.

## **License**

This project is licensed under the **MIT License**.
