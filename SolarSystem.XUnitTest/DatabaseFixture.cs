using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SolarSystem.Data;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.Repository;

namespace SolarSystem.XUnitTest
{
    public class DatabaseFixture : IDisposable
    {
        private static DbContextOptions<ApplicationContext> _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "SolarSystemDbTest")
            .Options;

        private ApplicationContext _context;
        public UnitOfWork _unitOfWork;
        public Mapper _mapper;
        
        public DatabaseFixture()
        {
            _context = new ApplicationContext(_options);
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Region, RegionDTO>().ReverseMap();
                cfg.CreateMap<Region, RegionDetailDTO>().ReverseMap();
                cfg.CreateMap<Region, CreateRegionDTO>().ReverseMap();
            }));
        }
        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }
    }
}