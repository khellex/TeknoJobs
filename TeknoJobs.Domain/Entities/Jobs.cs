using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Domain.Entities
{
    public class Jobs
    {
        [Key]
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Locations Locations { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Departments Department { get; set; }
    }
}
