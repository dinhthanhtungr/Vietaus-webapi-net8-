using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColorChipRecordUpsertRepositories : IColorChipRecordUpsertRepositories
    {
        private readonly ApplicationDbContext _context;

        public ColorChipRecordUpsertRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
