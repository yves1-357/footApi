# Le Onze – Projet Blazor avec Football API

Ce projet est une application Blazor qui affiche des informations sur les matchs de football, récupérées depuis une API externe.  
Il permet de :

- **Consulter les matchs du jour** ou d’une date spécifique
- **Voir les matchs en direct** (“live”)
- **Basculer entre différents filtres** (Hommes, Femmes, Favoris, etc.)
- **Gérer un cache** pour éviter de recharger inutilement les mêmes données
- **Changer le thème** (mode sombre/clair)

## Lancement local

Installer le SDK .NET 9, puis créer `api/wwwroot/appsettings.json` (ignoré par Git) :

```json
{
  "ApiSportsKey": "VOTRE_CLE_API_SPORTS"
}
```

Lancer `dotnet run --project api/footApi.csproj --launch-profile http`, puis ouvrir http://localhost:5298.

La configuration est chargée dans le navigateur par Blazor WebAssembly ; la clé reste accessible aux utilisateurs de l'application.

## Structure du projet

- **FootService.cs**  
  Gère les appels à l’API de football, la conversion des dates (en heure belge) et le tri des matchs.

- **Index.razor** (page principale)  
  Affiche les matchs sous forme de tableau, propose la recherche, les filtres, la sélection de dates, etc.

- **FootServiceTests.cs**  
  Contient un exemple de test unitaire (MSTest) pour valider la conversion UTC → heure belge etc...
