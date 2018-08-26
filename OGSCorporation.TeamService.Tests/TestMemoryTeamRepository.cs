using OGSCorporation.TeamService.Models;
using OGSCorporation.TeamService.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace OGSCorporation.TeamService.Tests
{
    public class TestMemoryTeamRepository : MemoryTeamRepository {
		public TestMemoryTeamRepository() : base(CreateInitialFake()) {
			 
		}

		private static ICollection<Team> CreateInitialFake()
		{
			var teams = new List<Team>();
			teams.Add(new Team("one"));
			teams.Add(new Team("two"));

			return teams;
		}
	}	
}
