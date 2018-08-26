using System;
using System.Collections.Generic;
using System.Text;

namespace OGSCorporation.TeamService.Models
{
    public class Member
    {
        public string FirstName {get;set; }
        public string LastName {get;set;}
        public Guid ID {get;set;}

        public Member(){}

        public Member(Guid id): this()
        {
            this.ID = id;
        }

        public Member(string firstName, string lastName, Guid id) : this(id)
        {
            this.FirstName = firstName;
            this.LastName= lastName;
        }

        public override string ToString()
        {
            return this.LastName;
        }
    }
}
