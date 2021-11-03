using SolarSystem.Data;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private IGenericRepository<Region> regions;
        private IGenericRepository<Component> components;
        private IGenericRepository<Body> bodies;

        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
        }

        public IGenericRepository<Region> Regions => regions ??= new GenericRepository<Region>(context);

        public IGenericRepository<Component> Components => components ??= new GenericRepository<Component>(context);

        public IGenericRepository<Body> Bodies => bodies ??= new GenericRepository<Body>(context);

        public void Dispose()
        {
            this.context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
