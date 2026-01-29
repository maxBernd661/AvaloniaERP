# AvaloniaERP - [Download](https://github.com/maxBernd661/AvaloniaERP/releases/download/Stable/AvaloniaERP_1.0.0_Stable.zip)

![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/maxBernd661/AvaloniaERP/total) ![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/maxBernd661/AvaloniaERP/tests.yml) ![GitHub Repo stars](https://img.shields.io/github/stars/maxBernd661/AvaloniaERP)



## Überblick
Das Programm stellt ein ERP System für das erstellen von Kunden, Artikeln und Bestellungen dar. 
* AvaloniaERP.Win - Frontend basierend auf Avalonia MVVM
* AvaloniaERP.Core - Backend, Entity Framework Core mit SQLite

## ER-Modell
* docs/erModel.pdf
* Enthält Tabellenstruktur und Beziehungen

## SQL
* docs/tables.sql
* Enthält Befehle zum erstellen der Tabellen

## Datenbankschnittstelle
* **Framework / ORM:** EntityFramework Core (EFCore)
* **Anbindung:** EFCore verwendet die Entitäten in `AvaloniaERP.Core/Entity` und erzeugt die Tabellen über Migrations
* **Speicherort:** Lokale SQLite Datenbank, Speicherort festgelegt über `AvaloniaERP.Win/appsettings.json`

## Quellcodebeschreibung

### Backend
* **Entitäten:** `AvaloniaERP.Core/Entity/*.cs` definieren die Domänenobjekte und die Konfigurationen für EFCore
* **DbContext:** `AvaloniaERP.Core/EntityContext.cs` stellt CRUD Zugriff bereit, verwaltet Zeitstempel und Soft-Deletes
* **Businesslogik:**
    * `RowBase<T>`: DTOs zur darstellung der Entites im Frontend
    * `IQueryProfile<T>`: Definiert Ladeverhalten vom Objekt aus der Datenbank
    * `IGraphMerger<T>`: Erlaubt ordentliches speichern von bestehenden komplexen Objektgraphen

### Frontend
* **Avalonia MVVM im .NET Generic Host**
* Servicebasierte Architektur
* Model = Entitäten
* Views über XML definiert
* `EntityDetailViewModel<T>` und `ListViewModel<TEntity, TEntityRow>` als Viewmodel
    * Erstellung über `IViewModelFactory`, Navigation über `INavigationService`
    * CRUD über `IDataManipulationService`

## Unit Tests
* Was: Setzen von Zeitstempeln und Soft-Delete bei EntityContext.SaveChanges()
* Warum: Bilded die Logik für Auditing und konsistente Datenhaltung
* Wie: `dotnet test AvaloniaERP.Core.Test/AvaloniaERP.Core.Test.csproj`
