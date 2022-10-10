using System;
using System.Collections.Generic;

namespace CRUD.Model.Models
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
            Users = new HashSet<User>();
        }

        public int StateId { get; set; }
        public string StateName { get; set; } = null!;
        public int CountryId { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
