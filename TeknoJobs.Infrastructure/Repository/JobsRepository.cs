using Microsoft.EntityFrameworkCore;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;
using TeknoJobs.Infrastructure.Data;

namespace TeknoJobs.Infrastructure.Repository
{
    /// <summary>
    /// Provides methods for managing and retrieving job-related data from the database.
    /// </summary>
    /// <remarks>This repository is responsible for handling operations specific to the <see cref="Jobs"/>
    /// entity,  including generating unique job codes and updating job records. It extends the base repository 
    /// functionality and implements the <see cref="IJobsRepository"/> interface.</remarks>
    public class JobsRepository : Repository<Jobs>, IJobsRepository
    {
        private readonly ApplicationDbContext _db;
        public JobsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //function to generate a new job code in the format JOB-01, JOB-02, etc.
        public async Task<string> GenerateJobCodeAsync()
        {
            // Get the last job in the database
            var lastJobInDb = await _db.Jobs
                .OrderByDescending(j => j.Code)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastJobInDb != null &&
                !string.IsNullOrEmpty(lastJobInDb.Code) &&
                lastJobInDb.Code.StartsWith("JOB-") &&
                int.TryParse(lastJobInDb.Code.Split('-')[1], out int numberPart))
            {
                nextNumber = numberPart + 1;
            }

            // Pad with 0 only if nextNumber is between 1 and 9
            string numberPartFormatted = nextNumber < 10
                ? $"0{nextNumber}"
                : nextNumber.ToString();

            return $"JOB-{numberPartFormatted}";
        }

        public void Update(Jobs entity)
        {
            _db.Jobs.Update(entity);
        }
    }
}
