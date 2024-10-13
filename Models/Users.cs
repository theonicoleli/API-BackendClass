﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static TrabalhoBackEnd.Models.Roles;

namespace TrabalhoBackEnd.Models
{
    public class Users
    {
        [Table("tbUser")]
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            // Many-to-Many relationship with Role entity
            public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
        }
    }
}
