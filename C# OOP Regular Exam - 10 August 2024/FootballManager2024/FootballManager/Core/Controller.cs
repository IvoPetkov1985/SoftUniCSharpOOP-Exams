using FootballManager.Core.Contracts;
using FootballManager.Models;
using FootballManager.Models.Contracts;
using FootballManager.Repositories;
using FootballManager.Repositories.Contracts;
using System.Text;

namespace FootballManager.Core
{
    public class Controller : IController
    {
        private const int MaxCapacity = 10;
        private readonly IRepository<ITeam> championship;

        public Controller()
        {
            championship = new TeamRepository();
        }

        public string ChampionshipRankings()
        {
            StringBuilder builder = new();
            builder.AppendLine("***Ranking Table***");

            ITeam[] teams = championship.Models
                .Where(t => t.TeamManager != null)
                .OrderByDescending(t => t.ChampionshipPoints)
                .ThenByDescending(t => t.PresentCondition)
                .ToArray();

            for (int i = 0; i < teams.Length; i++)
            {
                builder.AppendLine($"{i + 1}. {teams[i].ToString()}/{teams[i].TeamManager.ToString()}");
            }

            return builder.ToString().TrimEnd();
        }

        public string JoinChampionship(string teamName)
        {
            if (championship.Models.Count == MaxCapacity)
            {
                return "Championship is full!";
            }

            ITeam team = championship.Get(teamName);

            if (team != null)
            {
                return $"{teamName} has already joined the Championship.";
            }

            team = new Team(teamName);
            championship.Add(team);
            return $"{teamName} has successfully joined the Championship.";
        }

        public string MatchBetween(string teamOneName, string teamTwoName)
        {
            ITeam teamOne = championship.Get(teamOneName);
            ITeam teamTwo = championship.Get(teamTwoName);

            if (teamOne == null || teamTwo == null)
            {
                return "This match does not meet the regulation rules of the Championship.";
            }

            if (teamOne.PresentCondition > teamTwo.PresentCondition)
            {
                teamOne.GainPoints(3);

                if (teamOne.TeamManager != null)
                {
                    teamOne.TeamManager.RankingUpdate(5);
                }

                if (teamTwo.TeamManager != null)
                {
                    teamTwo.TeamManager.RankingUpdate(-5);
                }

                return $"Team {teamOne.Name} wins the match against {teamTwo.Name}.";
            }
            else if (teamOne.PresentCondition < teamTwo.PresentCondition)
            {
                teamTwo.GainPoints(3);

                if (teamTwo.TeamManager != null)
                {
                    teamTwo.TeamManager.RankingUpdate(5);
                }

                if (teamOne.TeamManager != null)
                {
                    teamOne.TeamManager.RankingUpdate(-5);
                }

                return $"Team {teamTwo.Name} wins the match against {teamOne.Name}.";
            }
            else
            {
                teamOne.GainPoints(1);
                teamTwo.GainPoints(1);
                return $"The match between {teamOneName} and {teamTwoName} ends in a draw.";
            }
        }

        public string PromoteTeam(string droppingTeamName, string promotingTeamName, string managerTypeName, string managerName)
        {
            ITeam droppingTeam = championship.Get(droppingTeamName);

            if (droppingTeam == null)
            {
                return $"Team {droppingTeamName} does not exist in the Championship.";
            }

            ITeam promotingTeam = championship.Get(promotingTeamName);

            if (promotingTeam != null)
            {
                return $"{promotingTeamName} has already joined the Championship.";
            }

            promotingTeam = new Team(promotingTeamName);

            if (!(championship.Models.Where(t => t.TeamManager != null).Any(t => t.TeamManager.Name == managerName)) && (managerTypeName == nameof(AmateurManager) || managerTypeName == nameof(ProfessionalManager) || managerTypeName == nameof(SeniorManager)))
            {
                IManager manager = null;

                if (managerTypeName == nameof(AmateurManager))
                {
                    manager = new AmateurManager(managerName);
                }
                else if (managerTypeName == nameof(ProfessionalManager))
                {
                    manager = new ProfessionalManager(managerName);
                }
                else
                {
                    manager = new SeniorManager(managerName);
                }

                promotingTeam.SignWith(manager);
            }

            foreach (ITeam team in championship.Models)
            {
                team.ResetPoints();
            }

            championship.Remove(droppingTeamName);
            championship.Add(promotingTeam);
            return $"Team {promotingTeamName} wins a promotion for the new season.";
        }

        public string SignManager(string teamName, string managerTypeName, string managerName)
        {
            ITeam team = championship.Get(teamName);

            if (team == null)
            {
                return $"Team {teamName} does not take part in the Championship.";
            }

            if (managerTypeName != nameof(AmateurManager) &&
                managerTypeName != nameof(ProfessionalManager) &&
                managerTypeName != nameof(SeniorManager))
            {
                return $"{managerTypeName} is an invalid manager type for the application.";
            }

            if (team.TeamManager != null)
            {
                return $"Team {teamName} has already signed a contract with {team.TeamManager.Name}.";
            }

            if (championship.Models.Where(t => t.TeamManager != null).Any(t => t.TeamManager.Name == managerName))
            {
                return $"Manager {managerName} is already assigned to another team.";
            }

            IManager manager = null;

            if (managerTypeName == nameof(AmateurManager))
            {
                manager = new AmateurManager(managerName);
            }
            else if (managerTypeName == nameof(ProfessionalManager))
            {
                manager = new ProfessionalManager(managerName);
            }
            else
            {
                manager = new SeniorManager(managerName);
            }

            team.SignWith(manager);
            return $"Manager {managerName} is assigned to team {teamName}.";
        }
    }
}
