﻿using Microsoft.AspNetCore.Mvc;
using OGSCorporation.TeamService.Controllers;
using OGSCorporation.TeamService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OGSCorporation.TeamService.Tests
{
    public class TeamsControllerTest
    {	    
        /// <summary>
        /// This test return the number of teams
        /// </summary>
        [Fact]
        public void QueryTeamListReturnsCorrectTeams()
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var rawTeams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> teams = new List<Team>(rawTeams);
            Assert.Equal(teams.Count, 2);
            Assert.Equal(teams[0].Name, "one");
            Assert.Equal(teams[1].Name, "two");            
        }

        /// <summary>
        /// Retrieve a Team using the Guid Id
        /// </summary>
        [Fact]
        public void GetTeamRetrievesTeam() 
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());

            string sampleName = "sample";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            controller.CreateTeam(sampleTeam);

            Team retrievedTeam = (Team)(controller.GetTeam(id) as ObjectResult).Value;
            Assert.Equal(retrievedTeam.Name, sampleName);
            Assert.Equal(retrievedTeam.ID, id);            
        }

        [Fact]
        public void GetNonExistentTeamReturnsNotFound() 
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());

            Guid id = Guid.NewGuid();
            var result = controller.GetTeam(id);
            Assert.True(result is NotFoundResult);                                
        }
        /// <summary>
        /// Create Team and add to the teams list
        /// </summary>
        [Fact]
        public void CreateTeamAddsTeamToList() 
        {
            //Arrange:
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);            
            Team t = new Team("sample");

            //Act:
            var result = controller.CreateTeam(t);            
            Assert.Equal(201, (result as ObjectResult).StatusCode);
            var actionResult = controller.GetAllTeams() as ObjectResult;            
            var newTeamsRaw = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);

            //Assert:
            Assert.Equal(newTeams.Count, original.Count+1);
            var sampleTeam = newTeams.FirstOrDefault( target => target.Name == "sample");
            Assert.NotNull(sampleTeam);            
        }

        [Fact]
        public void UpdateTeamModifiesTeamToList() 
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);
            
            Guid id = Guid.NewGuid();
            Team t = new Team("sample", id);
            var result = controller.CreateTeam(t);

            Team newTeam = new Team("sample2", id);
            controller.UpdateTeam(newTeam, id);

            var newTeamsRaw = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            var sampleTeam = newTeams.FirstOrDefault( target => target.Name == "sample");
            Assert.Null(sampleTeam);

            Team retrievedTeam = (Team)(controller.GetTeam(id) as ObjectResult).Value;
            Assert.Equal(retrievedTeam.Name, "sample2");
        }        

        [Fact]
        public void UpdateNonExistentTeamReturnsNotFound() 
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);
            
            Team someTeam = new Team("Some Team", Guid.NewGuid());
            controller.CreateTeam(someTeam);

            Guid newTeamId = Guid.NewGuid();
            Team newTeam = new Team("New Team", newTeamId);
            var result = controller.UpdateTeam(newTeam, newTeamId);

            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public void DeleteTeamRemovesFromList() 
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            var teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            int ct = teams.Count();

            string sampleName = "sample";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            controller.CreateTeam(sampleTeam);

            teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.NotNull(sampleTeam);            

            controller.DeleteTeam(id);

            teams = (IEnumerable<Team>)(controller.GetAllTeams() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.Null(sampleTeam);            
        }

        [Fact]
        public void DeleteNonExistentTeamReturnsNotFound() 
        {
            TeamsController controller = new TeamsController(new TestMemoryTeamRepository());
            Guid id = Guid.NewGuid();

            var result = controller.DeleteTeam(id);
            Assert.True(result is NotFoundResult);
        }
    }
}
