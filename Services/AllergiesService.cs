using Allergy.Models;
using Microsoft.Extensions.Configuration;

namespace Allergy.Services
{
    public class AllergiesService : BaseService<Allergies>, IAllergiesService
    {
        public AllergiesService(AllergiesDbContext context, IConfiguration configuration) 
            : base(context, configuration)
        {
        }
    }
}
