using System.Globalization;
using System.Text.RegularExpressions;

namespace solio_parser;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: solio_parser <input_file> [output_file]");
            return;
        }

        string inputFile = args[0];

        // Check if file exists in current directory, otherwise check InputFiles folder
        if (!File.Exists(inputFile))
        {
            string inputFilesPath = Path.Combine("InputFiles", inputFile);
            if (File.Exists(inputFilesPath))
            {
                inputFile = inputFilesPath;
            }
            else
            {
                Console.WriteLine($"File not found: {inputFile}");
                Console.WriteLine($"Also checked: {inputFilesPath}");
                return;
            }
        }

        string outputFile;
        if (args.Length > 1)
        {
            outputFile = args[1];
        }
        else
        {
            // Create OutputFiles directory if it doesn't exist
            Directory.CreateDirectory("OutputFiles");

            // Get filename without path and replace .txt with .csv
            string fileName = Path.GetFileNameWithoutExtension(inputFile);
            outputFile = Path.Combine("OutputFiles", $"{fileName}.csv");
        }

        try
        {
            var teams = ParseElevenifyData(inputFile);

            if (teams.Count > 0)
            {
                Console.WriteLine($"Successfully parsed {teams.Count} teams:");
                foreach (var team in teams)
                {
                    Console.WriteLine(
                        $"{team.ShortName}: Goals={team.Attack}, Goals Conceded={team.Defence}"
                    );
                }

                WriteSolioCsv(teams, outputFile);
                Console.WriteLine($"\nCSV file written to: {outputFile}");
            }
            else
            {
                Console.WriteLine("No teams found in the input file.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static List<SolioData> ParseElevenifyData(string inputFile)
    {
        var teams = new List<SolioData>();
        var lines = File.ReadAllLines(inputFile);

        foreach (var line in lines)
        {
            // Skip empty lines and lines that don't start with a digit
            if (string.IsNullOrWhiteSpace(line) || !char.IsDigit(line.Trim()[0]))
            {
                continue;
            }

            // Check if line contains duplicate values (the separator lines we want to skip)
            // These lines look like: "1.031.03" or "0.100.10"
            var trimmedLine = line.Trim();
            if (Regex.IsMatch(trimmedLine, @"^[\-\d\.]+[\-\d\.]+$") && !trimmedLine.Contains('\t'))
            {
                continue; // Skip the duplicate value lines
            }

            // Split by tabs and whitespace
            var parts = Regex
                .Split(line, @"\t+")
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToArray();

            // We expect: Position, Team Name, Offensive, Defensive, Overall
            if (parts.Length >= 4)
            {
                var teamName = parts[1];
                var offensive = parts[2];
                var defensive = parts[3];

                var shortName = ConvertTeamNameToShortName(teamName);

                teams.Add(
                    new SolioData
                    {
                        ShortName = shortName,
                        Attack = FormatDecimal(offensive),
                        Defence = FormatDecimal(defensive),
                    }
                );
            }
        }

        return teams;
    }

    static string ConvertTeamNameToShortName(string teamName)
    {
        var teamMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Arsenal", "ARS" },
            { "Aston Villa", "AVL" },
            { "Bournemouth", "BOU" },
            { "Brentford", "BRE" },
            { "Brighton", "BHA" },
            { "Burnley", "BUR" },
            { "Chelsea", "CHE" },
            { "Crystal Palace", "CRY" },
            { "Everton", "EVE" },
            { "Fulham", "FUL" },
            { "Leeds", "LEE" },
            { "Liverpool", "LIV" },
            { "Man City", "MCI" },
            { "Man Utd", "MUN" },
            { "Newcastle", "NEW" },
            { "Nott'm Forest", "NFO" },
            { "Spurs", "TOT" },
            { "Sunderland", "SUN" },
            { "West Ham", "WHU" },
            { "Wolves", "WOL" },
        };

        if (teamMap.TryGetValue(teamName, out var shortName))
        {
            return shortName;
        }

        return teamName.Length >= 3 ? teamName.Substring(0, 3).ToUpper() : teamName.ToUpper();
    }

    static string FormatDecimal(string value)
    {
        if (
            decimal.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var decimalValue
            )
        )
        {
            return decimalValue.ToString("0.000", CultureInfo.InvariantCulture);
        }
        return value;
    }

    static void WriteSolioCsv(List<SolioData> teams, string outputFile)
    {
        using var writer = new StreamWriter(outputFile);

        writer.WriteLine("Team,Goals,Goals Conceded");

        foreach (var team in teams)
        {
            writer.WriteLine($"{team.ShortName},{team.Attack},{team.Defence}");
        }
    }

    public class SolioData
    {
        public required string ShortName { get; set; }
        public required string Attack { get; set; }
        public required string Defence { get; set; }
    }
}
