using System;
using System.Collections.Generic;

namespace CRUD.Model.Models
{
    public partial class Country
    {
        public Country()
        {
            States = new HashSet<State>();
            Users = new HashSet<User>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;

        public virtual ICollection<State> States { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
