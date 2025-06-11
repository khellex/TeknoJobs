using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Domain.Entities
{
    public class Departments
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
    }
}
