# 🛩️ Aircraft Type Designator Scraper

This project scrapes the latest list of aircraft type designators from Wikipedia and exports them to a clean, normalized CSV file. It is designed to run automatically on a schedule, ensuring your repo always has the most up-to-date aircraft codes and model information — including Wikipedia links for reference.

## 📦 What It Does

- Scrapes from: [Wikipedia: List of aircraft type designators](https://en.wikipedia.org/wiki/List_of_aircraft_type_designators)
- Extracts:
  - ICAO Code
  - IATA Type Code
  - Aircraft Model
  - Wikipedia Link (from the model's hyperlink)
- Normalizes data:
  - Removes reference markers (e.g., `[4]`, `&#91;note&#93;`)
  - Converts `"-"` or `"—"` values to empty strings
- Outputs to: `aircraft_type_designators.csv`
- Automatically commits and pushes changes to the `main` branch (if data has changed)

## ⚙️ GitHub Actions Automation

This repo includes a GitHub Action that runs the scraper regularly via **CRON** and also supports manual or event-based runs.

### 🧾 Trigger Schedule

```yaml
on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]
  workflow_dispatch:  # 🚀 Manual trigger
  schedule:
    - cron: '0 0 * * 3'  # ⏰ Every Wednesday at 00:00 UTC
```

## 🔁 What Happens

1. Action checks out the main branch
1. Installs the latest .NET 9 SDK
1. Builds and runs the AircraftTypeDesignatorScraper project
1. Detects changes to the CSV file
1. Commits and pushes only if the CSV was updated


## 🏃‍♂️ Run It Locally

You can run the scraper manually on your machine using:

```bash
dotnet run --project AircraftTypeDesignatorScraper.csproj
```

It will output a fresh `aircraft_type_designators.csv` in the current working directory.

## 📁 Output Format

```csv
"ICAO Code","IATA Type Code","Model","Wikipedia Link"
"A306","ABY","Airbus A300-600 Freighter","https://en.wikipedia.org/wiki/Airbus_A300"
"A337","","Airbus A330-700 ""BelugaXL""","https://en.wikipedia.org/wiki/Airbus_Beluga_XL"
...
```

## 🤝 Contributing

PRs and improvements welcome! Especially if you want to:

- Expand scraper coverage (other aircraft tables, IATA codes, etc.)
- Add unit tests or CI validation
- Integrate with a frontend for displaying CSV contents

## 🪪 License

This project is licensed under the [MIT License](LICENSE).

You are free to use, modify, and distribute this software in personal, commercial, or educational projects, as long as the original license and copyright
notice are included. Contributions are welcome and encouraged under the same license.
