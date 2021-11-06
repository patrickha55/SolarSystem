using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SolarSystem.Data;
using SolarSystem.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.NUnitTest
{
    public class GenericRepositoryTest
    {
        private static DbContextOptions<ApplicationContext> _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "SolarSystemDbTest")
            .Options;

        private ApplicationContext _context;
        private UnitOfWork _unitOfWork;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new ApplicationContext(_options);
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Test]
        public async Task GetRegions()
        {
            var result = await _unitOfWork.Regions.GetAllAsync();
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetRegionsWithExpression()
        {
            var result = await _unitOfWork.Regions.GetAllAsync(r => r.Name == "Inner Solar System");
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}
