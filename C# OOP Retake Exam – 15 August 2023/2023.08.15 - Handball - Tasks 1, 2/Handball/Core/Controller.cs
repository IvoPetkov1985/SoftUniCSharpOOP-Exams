using Handball.Core.Contracts;
using Handball.Models.Contracts;
using Handball.Models.Players;
using Handball.Models.Teams;
using Handball.Repositories;
using Handball.Repositories.Contracts;
using System;
using System.Linq;
using System.Text;

namespace Handball.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IPlayer> players;
        private readonly IRepository<ITeam> teams;

        public Controller()
        {
            players = new PlayerRepository();
            teams = new TeamRepository();
        }

        public string LeagueStandings()
        {
            StringBuilder builder = new();
            builder.AppendLine("***League Standings***");

            foreach (ITeam team in teams.Models
                .OrderByDescending(t => t.PointsEarned)
                .ThenByDescending(t => t.OverallRating)
                .ThenBy(t => t.Name))
            {
                builder.AppendLine(team.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string NewContract(string playerName, string teamName)
        {
            if (players.ExistsModel(playerName) == false)
            {
                return $"Player with the name {playerName} does not exist in the {nameof(PlayerRepository)}.";
            }

            if (teams.ExistsModel(teamName) == false)
            {
                return $"Team with the name {teamName} does not exist in the {nameof(TeamRepository)}.";
            }

            IPlayer player = players.GetModel(playerName);

            if (player.Team != null)
            {
                return $"Player {playerName} has already signed with {player.Team}.";
            }

            player.JoinTeam(teamName);
            ITeam team = teams.GetModel(teamName);
            team.SignContract(player);
            return $"Player {playerName} signed a contract with {teamName}.";
        }

        public string NewGame(string firstTeamName, string secondTeamName)
        {
            ITeam firstTeam = teams.GetModel(firstTeamName);
            ITeam secondTeam = teams.GetModel(secondTeamName);

            if (firstTeam.OverallRating > secondTeam.OverallRating)
            {
                firstTeam.Win();
                secondTeam.Lose();
                return $"Team {firstTeam.Name} wins the game over {secondTeam.Name}!";
            }
            else if (firstTeam.OverallRating < secondTeam.OverallRating)
            {
                firstTeam.Lose();
                secondTeam.Win();
                return $"Team {secondTeam.Name} wins the game over {firstTeam.Name}!";
            }
            else
            {
                firstTeam.Draw();
                secondTeam.Draw();
                return $"The game between {firstTeamName} and {secondTeamName} ends in a draw!";
            }
        }

        public string NewPlayer(string typeName, string name)
        {
            if (typeName != nameof(Goalkeeper) && typeName != nameof(CenterBack) && typeName != nameof(ForwardWing))
            {
                return $"{typeName} is invalid position for the application.";
            }

            IPlayer player = players.GetModel(name);

            if (player != null)
            {
                return $"{name} is already added to the {players.GetType().Name} as {player.GetType().Name}.";
            }

            if (typeName == nameof(Goalkeeper))
            {
                player = new Goalkeeper(name);
            }
            else if (typeName == nameof(CenterBack))
            {
                player = new CenterBack(name);
            }
            else if (typeName == nameof(ForwardWing))
            {
                player = new ForwardWing(name);
            }

            players.AddModel(player);
            return $"{name} is filed for the handball league.";
        }

        public string NewTeam(string name)
        {
            if (teams.ExistsModel(name))
            {
                return $"{name} is already added to the {nameof(TeamRepository)}.";
            }

            ITeam team = new Team(name);
            teams.AddModel(team);
            return $"{name} is successfully added to the {teams.GetType().Name}.";
        }

        public string PlayerStatistics(string teamName)
        {
            ITeam team = teams.GetModel(teamName);

            StringBuilder builder = new();
            builder.AppendLine($"***{teamName}***");

            foreach (IPlayer player in team.Players
                .OrderByDescending(p => p.Rating)
                .ThenBy(p => p.Name))
            {
                builder.AppendLine(player.ToString());
            }

            return builder.ToString().TrimEnd();
        }
    }
}
