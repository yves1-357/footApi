using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace footApi.Services
{
    public class Match
    {
        public Fixture Fixture { get; set; }
        public League League { get; set; }
        public Teams Teams { get; set; }
        public Goals Goals { get; set; }
    }

    public class Fixture
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public Status Status { get; set; }
    }

    public class Status
    {
        public string Long { get; set; } // Ex:premier mi-temps, "match fini"
        public string Short { get; set; } // ex: "1h"
        public int? Elapsed { get; set; } // minute en cours 
    }

    public class League
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
    }

    public class Teams
    {
        public TeamInfo Home { get; set; }
        public TeamInfo Away { get; set; }
    }

    public class TeamInfo
    {
        public string Name { get; set; }
        public string Logo { get; set; }
    }

    public class Goals
    {
        public int? Home { get; set; }
        public int? Away { get; set; }
    }

    public class ApiResponse
    {
        public List<Match> Response { get; set; }
    }

    public class FootService
    {
        private readonly HttpClient _httpClient;

        public FootService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string ConvertToBelgiumTime(string utcTime, Status status)
        {
            DateTime utcDateTime;
            if (!DateTime.TryParse(utcTime, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out utcDateTime))
            {
                return "unknown";
            }

            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            
            //convertir en heure belge
            TimeZoneInfo belgiumTimeZone;
            try
            {
                belgiumTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // Windows"
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    belgiumTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels"); // Linux/MacOS
                }
                catch (TimeZoneNotFoundException)
                {
                    Console.WriteLine("impossible de trouver l'horaire");
                    return utcDateTime.ToString("HH:mm"); // On retourne l'heure en UTC pour éviter un crash
                }
            }
            DateTime belgiumDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, belgiumTimeZone);

            switch (status.Short)
            {
                case "NS": return belgiumDateTime.ToString("HH:mm"); // affichage heure match non debuté
                case "1H":
                    case "2H": return $"{status.Elapsed}'";// Match en cours (affiche la minute)
                case "HT": return "Half-Time"; // Mi-temps
                case "FT": return "Full-Time"; // Match-terminé
                case "AET": return "Prolongations";
                default: return belgiumDateTime.ToString("HH:mm");
            }
        }
        
        private static int GetLeaguePriority(string country)
        {
            //priorité pour les ligues européenes
            string[] europe =
                { "England", "Spain", "Italy", "Germany", "France", "Portugal", "Netherlands", "Belgium" };
            // 2 ligue americaine
            string[] americas = { "USA", "Mexico", "Brazil", "Argentina", "Colombia", "Chile" };

            if (europe.Contains(country)) return 1;
            if (americas.Contains(country)) return 2;
            return 3; // le reste
        }

        public async Task<List<Match>> GetTodayMatchesAsync()
        {
            Console.WriteLine("Blazor : Début de l'appel API pour récupérer les matchs du jour ");

            // Utilisation de la date actuelle au format correct
            string todayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>($"fixtures?date={todayDate}");
                if (response == null || response.Response == null)
                {
                    Console.WriteLine("aucun match trouvé !");
                    return new List<Match>();
                }
                
                Console.WriteLine($"✅ {response.Response.Count} matchs trouvés !");
                //appliquer l'heure 
                foreach (var match in response.Response)
                {
                    Console.WriteLine($"Match : {match.Teams.Home.Name} vs {match.Teams.Away.Name} - Heure (UTC) : {match.Fixture.Date}");
                    match.Fixture.Date = ConvertToBelgiumTime(match.Fixture.Date, match.Fixture.Status);
                }
                
                //trier les matchs par priorité
                var sortedMatches = response.Response
                    .OrderBy(m => GetLeaguePriority(m.League.Country)) // tri priorité de ligue
                    .ThenBy(m => m.League.Name)// trie par nom ligue
                    .ToList();
                
                return  sortedMatches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erreur lors de l'appel API : {ex.Message}");
                return new List<Match>();
            }
        }

        public async Task<List<Match>> GetLiveMatchesAsync()
        {
            Console.WriteLine("Recuperation des matchs en direct..");

            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>("fixtures?live=all");
                if (response == null || response.Response == null)
                {
                    Console.WriteLine("Aucun match en direct trouvé");
                    return new List<Match>();
                }

                Console.WriteLine($"{response.Response.Count} matches en direct reçus ");
                return response.Response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'appel api : {ex.Message}");
                return new List<Match>();
            }
        }

        
    }
    
}
