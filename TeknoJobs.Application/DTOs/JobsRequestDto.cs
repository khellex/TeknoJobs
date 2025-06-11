using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Application.DTOs
{
    public class JobsRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public DateTime ClosingDate { get; set; }
        public int DepartmentId { get; set; }
    }
}
