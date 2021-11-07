using SolarSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Region> Regions { get; }
        IGenericRepository<Component> Components { get; }
        IGenericRepository<Body> Bodies { get; }
        Task SaveAsync();
    }
}
