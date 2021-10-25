using StudyManager.Data.Models.Visit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IVisitService
    {
        Task<List<VisitModel>> GetVisits(Guid courseId, int take, int skip);
        Task<VisitModel> CreateVisit(CreateVisitModel model);
        Task<List<UsersVisitModel>> GetVisitsInPeriod(GetVisitsInPeriod model, string login);
    }
}
