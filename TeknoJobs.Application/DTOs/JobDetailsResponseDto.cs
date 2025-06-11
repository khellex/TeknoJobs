using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Application.DTOs
{
    public class JobDetailsResponseDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public LocationDto Location { get; set; } = null!;
        public DepartmentDto Department { get; set; } = null!;
        public DateTime? PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }

    public class LocationDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int Zip { get; set; }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
