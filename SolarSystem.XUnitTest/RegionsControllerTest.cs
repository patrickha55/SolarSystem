using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.IRepository;
using SolarSystem.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using Xunit;

namespace SolarSystem.XUnitTest
{
    public class RegionsControllerTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<ILogger<RegionsController>> loggerMock;
        private readonly Mock<IMapper> mapperMock;

        public RegionsControllerTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            loggerMock = new Mock<ILogger<RegionsController>>();
            mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async void Get_Should_Return_AllRegions()
        {
            var requestParam = new PaginationParam { PageNumber = 1, PageSize = 5 };
            var expectedRegions = GetRegions();

            unitOfWorkMock.Setup<Task<IPagedList<Region>>>(repo => repo.Regions.GetAllAsync(requestParam, null, null, null))
                .ReturnsAsync(value: expectedRegions);

            var controller = new RegionsController(unitOfWorkMock.Object, loggerMock.Object, mapperMock.Object);

            var result = await controller.Get(null);

            result.Value.Should().AllBeEquivalentTo(expectedRegions, o => o.ComparingByMembers<Region>());
        }

        public IPagedList<Region> GetRegions()
        {
            var regions = new List<Region>();
            var rand = new Random();

            regions.Add(new Region
            {
                Id = 1,
                Name = Guid.NewGuid().ToString(),
                DistanceToTheSun = rand.NextDouble() * 11,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            
            regions.Add(new Region
            {
                Id = 2,
                Name = Guid.NewGuid().ToString(),
                DistanceToTheSun = rand.NextDouble() * 11,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            return regions.ToPagedList(pageNumber: 1, pageSize: 5);
        }
    }
}
