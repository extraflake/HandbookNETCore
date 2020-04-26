using API.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Employee")]
    public class Employee : IEntity
    {
        [ForeignKey("User")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [ForeignKey("Department")]
        public int Department_Id { get; set; }
        public Department Department { get; set; }

        public User User { get; set; }
        int IEntity.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}