﻿using System.Net.Http.Json;
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
    }

    public class League
    {
        public string Name { get; set; }
        public string Country { get; set; }
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
                    Console.WriteLine("⚠ Réponse API vide ou invalide !");
                    return new List<Match>();
                }

                Console.WriteLine($"✅ {response.Response.Count} matchs reçus !");
                return response.Response;
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
