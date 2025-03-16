using Data.Repositories;
using Entites;
using System.Linq;

namespace Services.DataInitializer
{
    public class SkillsDataInitializer : IDataInitializer
    {
        private readonly IRepository<Skills> repository;

        public SkillsDataInitializer(IRepository<Skills> repository)
        {
            this.repository = repository;
        }

        public void InitializeData()
        {
            if (!repository.TableNoTracking.Any(p => p.Title == "دسته بندی اولیه 1"))
            {
                repository.Add(new Skills
                {
                    Title = "دسته بندی اولیه 1",
                    InsertUser = "System",
                    InsertDate = System.DateTime.Now,
                    IsActive = true
                });
            }
           
        }
    }
}
