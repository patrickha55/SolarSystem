using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolarSystem.Data;
using SolarSystem.Repository.Repository;
using Xunit;
using XUnitPriorityOrderer;
using AutoMapper;
using SolarSystem.Data.Configuration;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;

namespace SolarSystem.XUnitTest
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class GenericRepositoryTest : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture _fixture;

        public GenericRepositoryTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        #region GetAllAsync

        [Fact, Order(1)]
        public async Task GetRegions()
        {
            var result = await _fixture._unitOfWork.Regions.GetAllAsync(null);

            Assert.Equal(3, result.Count);

            var regionDtOs = _fixture._mapper.Map<IList<RegionDTO>>(result);

            Assert.Equal(3, regionDtOs.Count);
            Assert.IsType<RegionDTO>(regionDtOs.FirstOrDefault());
        }

        [Fact, Order(2)]
        public async Task GetRegionsWithExpression()
        {
            var result = await _fixture._unitOfWork.Regions.GetAllAsync(null, r => r.Name == "Inner Solar System");

            Assert.Equal(1, result.Count);

            Assert.Contains("Inner Solar System", result.FirstOrDefault()!.Name);
        }

        [Fact, Order(3)]
        public async Task GetRegionsWithOrderedNameByDescAndIncludesItsBodies()
        {
            var result =
                await _fixture._unitOfWork.Regions.GetAllAsync(
                    null,
                    null,
                    q => q.OrderByDescending(r => r.Name),
                    new List<string>() {"Bodies"});

            Assert.Equal(3, result.Count);
            Assert.Contains("Trans-Neptunian", result.FirstOrDefault()!.Name);

            Assert.Equal(2, result.FirstOrDefault(r => r.Id == 1)!.Bodies.Count);
        }

        #endregion

        #region GetAsync

        [Fact, Order(4)]
        public async Task GetRegionById()
        {
            var region = await _fixture._unitOfWork.Regions.GetAsync(r => r.Id == 2, new List<string>() {"Bodies"});

            Assert.NotNull(region);
            Assert.Equal(1, region.Bodies.Count);

            var regionDTO = _fixture._mapper.Map<RegionDTO>(region);

            Assert.NotNull(regionDTO);
            Assert.IsType<RegionDTO>(regionDTO);
        }

        #endregion

        #region CreateAsync & DeleteAsync

        [Fact, Order(98)]
        public async Task CreateNewRegion()
        {
            
            var request = new CreateRegionDTO
            {
                Name = "test",
                DistanceToTheSun = 100.2D
            };

            var region = _fixture._mapper.Map<Region>(request);

            Assert.NotNull(region);

            await _fixture._unitOfWork.Regions.CreateAsync(region);
            await _fixture._unitOfWork.SaveAsync();

            var regions = await _fixture._unitOfWork.Regions.GetAllAsync(null);

            Assert.Equal(4, regions.Count);

            var newRegion = regions.FirstOrDefault(r => r.Name == "test");

            Assert.NotNull(newRegion);
        }

        [Fact, Order(99)]
        public async Task DeleteARegion()
        {
            await _fixture._unitOfWork.Regions.DeleteAsync(2);
            await _fixture._unitOfWork.SaveAsync();

            var region = await _fixture._unitOfWork.Regions.GetAsync(r => r.Id == 2);

            Assert.Null(region);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        public async Task DeleteRegions(int id)
        {
            await _fixture._unitOfWork.SaveAsync();
            Func<Task> action = () => _fixture._unitOfWork.Regions.DeleteAsync(id);

            await Assert.ThrowsAsync<Exception>(action);
        }

        #endregion

        #region Update

        [Fact, Order(101)]
        public async Task UpdateRegion()
        {
            await _fixture._unitOfWork.Bodies.CreateAsync(new Body
            {
                Name = "test",
                EarthMass = 2,
                DistanceToTheSun = 2,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ComponentId = 1,
                RegionId = 1,
            });
            await _fixture._unitOfWork.SaveAsync();

            var request = new UpdateRegionDTO
            {
                Name = "test test",
                DistanceToTheSun = 1000.23D,
                BodiesId = new List<int>() {4}
            };

            var region = await _fixture._unitOfWork.Regions.GetAsync(r => r.Id == 1);

            var regionUpdated = _fixture._mapper.Map(request, region);

            _fixture._unitOfWork.Regions.Update(regionUpdated);
            await _fixture._unitOfWork.SaveAsync();

            Assert.Equal("test test", region.Name);
        }

        #endregion
    }
}