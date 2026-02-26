# Solio Parser
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Cross--platform-lightgrey)
![License](https://img.shields.io/badge/License-Portfolio%20%2F%20Educational-blue)


A command-line tool that converts team strength data from Elevenify to Solio Analytics CSV format.

## Overview

This parser automates the weekly task of transferring Premier League team strength data from [Elevenify](https://www.elevenify.com/p/premier-league-data) to [Solio Analytics FPL](https://fpl.solioanalytics.com/). Instead of manually entering values, the tool reads raw data and generates a clean CSV file ready for import.

## Features

- Parses messy input with inconsistent whitespace and tabs
- Extracts offensive and defensive strength values
- Filters out duplicate separator lines
- Maps team names to 3-letter codes (ARS, LIV, MCI, etc.)
- Formats decimals to 3 decimal places
- Outputs clean CSV in Solio's expected format

## Setup

```powershell
git clone <repository-url>
cd solio-parser
dotnet build
```

## Usage

1. Copy the team strength table from Elevenify
2. Paste into a `.txt` file in the `InputFiles` folder
3. Run the parser:

```powershell
dotnet run elevenify_ts_feb1_26.txt
```

Output is automatically generated in `OutputFiles` with `.csv` extension:
- Input: `InputFiles/elevenify_ts_feb1_26.txt`
- Output: `OutputFiles/elevenify_ts_feb1_26.csv`

You can also specify a custom output path:
```powershell
dotnet run <input_file> <output_file>
```

## Input Format

Expected data format from Elevenify:
```
1	Arsenal		1.84	0.81	1.03
1.031.03
2	Liverpool	1.81	1.22	0.59
0.590.59
...
```

Each team row contains:
- Position (1-20)
- Team name
- Offensive value
- Defensive value
- Overall value

Separator lines with duplicate overall values are automatically filtered out.

## Output Format

Generated CSV format for Solio Analytics:
```csv
Team,Goals,Goals Conceded
ARS,1.840,0.810
LIV,1.810,1.220
MCI,1.970,1.120
...
```

## Team Codes

Team names are automatically converted to 3-letter codes:

| Team Name | Code |
|-----------|------|
| Arsenal | ARS |
| Aston Villa | AVL |
| Bournemouth | BOU |
| Brentford | BRE |
| Brighton | BHA |
| Chelsea | CHE |
| Crystal Palace | CRY |
| Everton | EVE |
| Liverpool | LIV |
| Man City | MCI |
| Man Utd | MUN |
| Newcastle | NEW |
| Spurs | TOT |
| West Ham | WHU |
| Wolves | WOL |

*Additional teams are supported with fallback conversion.*

## Project Structure

```
solio-parser/
├── InputFiles/          # Place .txt files from Elevenify here
├── OutputFiles/         # Generated CSV files
├── Program.cs           # Main parser logic
└── README.md           
```

## Technical Details

- **Platform**: .NET 10.0 / C#
- **Parsing**: Regex-based with whitespace handling
- **Formatting**: 3 decimal places, period separator

## Example Output

```
Successfully parsed 20 teams:
ARS: Goals=1.840, Goals Conceded=0.810
AVL: Goals=1.470, Goals Conceded=1.380
BOU: Goals=1.550, Goals Conceded=1.480
...

CSV file written to: OutputFiles\elevenify_ts_feb1_26.csv
```

