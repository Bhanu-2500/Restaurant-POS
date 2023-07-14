using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        
        public DateTime LastLogin { get; set; }
        public List<WorkHistory> PersonalWorkHistory { get; set; }
        public bool IsAdmin { get; set; }
        public TimeSpan TotalWorkofTheDay {
            get
            {
                return (DateTime.Now.Subtract(LastLogin));
            } 
        }
    }
}
