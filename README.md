# Solio Parser

Ett verktyg för att automatiskt konvertera team strength-data från Elevenify till Solio Analytics CSV-format.

## Syfte

Detta projekt löser problemet med att manuellt mata in team strength-data varje vecka. Istället för att knacka in alla värden för hand från [Elevenify Premier League Data](https://www.elevenify.com/p/premier-league-data) till [Solio Analytics FPL](https://fpl.solioanalytics.com/), kan du nu kopiera tabellen till en fil och automatiskt generera en färdig CSV-fil som kan importeras direkt.

## Hur det fungerar

Programmet läser smutsig data från Elevenify (med mycket whitespace, tabbar och extra rader) och:
- Extraherar lagnamn, offensiva värden (Goals) och defensiva värden (Goals Conceded)
- Ignorerar mellanliggande rader som innehåller duplicerade "overall"-värden
- Konverterar lagnamn till 3-bokstavskoder (ARS, LIV, MCI, etc.)
- Formaterar decimaler till 3 decimaler med punkt som decimalseparator
- Skapar en ren CSV-fil i exakt det format som Solio behöver

## Installation

```powershell
git clone <repository-url>
cd solio-parser
dotnet build
```

## Användning

### Grundläggande användning

1. Kopiera team strength-tabellen från Elevenify
2. Klistra in i en .txt-fil i `InputFiles`-mappen
3. Kör programmet:

```powershell
dotnet run <filnamn.txt>
```

Exempel:
```powershell
dotnet run elevenify_ts_feb1_26.txt
```

Output genereras automatiskt i `OutputFiles`-mappen med samma namn men .csv-ändelse:
- Input: `InputFiles/elevenify_ts_feb1_26.txt`
- Output: `OutputFiles/elevenify_ts_feb1_26.csv`

### Med anpassad output-fil

```powershell
dotnet run <input_fil> <output_fil>
```

Exempel:
```powershell
dotnet run elevenify_ts_feb1_26.txt my_custom_output.csv
```

## Input-format

Programmet förväntar sig data från Elevenify i följande format:
```
1	Arsenal		1.84	0.81	1.03
1.031.03
2	Liverpool	1.81	1.22	0.59
0.590.59
...
```

Varje lag har:
- Indexering (1-20)
- Lagnamn
- Offensive värde
- Defensive värde
- Overall värde

Mellan varje lag finns en rad med duplicerat overall-värde som ignoreras.

## Output-format

Genererad CSV-fil har exakt det format som Solio förväntar sig:
```csv
Team,Goals,Goals Conceded
ARS,1.840,0.810
LIV,1.810,1.220
MCI,1.970,1.120
...
```

## Lagkoder

Programmet konverterar automatiskt lagnamn till 3-bokstavskoder:

| Elevenify Namn | Solio Kod |
|---------------|-----------|
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
| ... och fler |

## Projektstruktur

```
solio-parser/
├── InputFiles/          # Lägg dina .txt-filer från Elevenify här
├── OutputFiles/         # Genererade CSV-filer hamnar här
├── Program.cs           # Huvudprogrammet
└── README.md           # Denna fil
```

## Teknisk information

- **Språk**: C# (.NET 10.0)
- **Parsing**: Regex-baserad parsing som hanterar olika whitespace-format
- **Decimal-formattering**: 3 decimaler, punkt som separator
- **Felhantering**: Kontrollerar att filer finns och ger tydliga felmeddelanden

## Exempel på körning

```powershell
PS V:\RiderProjects\solio-parser\solio-parser> dotnet run elevenify_ts_feb1_26.txt

Successfully parsed 20 teams:
ARS: Goals=1.840, Goals Conceded=0.810
AVL: Goals=1.470, Goals Conceded=1.380
BOU: Goals=1.550, Goals Conceded=1.480
...

CSV file written to: OutputFiles\elevenify_ts_feb1_26.csv
```

## Licens

Detta är ett personligt verktyg för att underlätta datahantering mellan Elevenify och Solio Analytics.
