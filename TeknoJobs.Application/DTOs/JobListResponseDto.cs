using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Application.DTOs
{
    public class JobListItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }

    public class JobListResponseDto
    {
        public int Total { get; set; }
        public List<JobListItemDto> Data { get; set; } = new();
    }
}
