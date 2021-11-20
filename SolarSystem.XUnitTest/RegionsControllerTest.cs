using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarSystem.Data.Configuration;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.IRepository;
using SolarSystem.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IMapper mapper;

        public RegionsControllerTest()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            loggerMock = new Mock<ILogger<RegionsController>>();
            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MapperInitializer())));
        }

        [Fact]
        public async void Get_Should_Return_AllRegions()
        {
            var requestParam = new PaginationParam { PageNumber = 1, PageSize = 5 };
            var expectedRegions = GetRegions();

            unitOfWorkMock.Setup(
                repo => repo.Regions.GetAllAsync(
                    It.IsAny<PaginationParam>(),
                    It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IOrderedQueryable<Region>>>(),
                    It.IsAny<List<string>>()))
                .ReturnsAsync(value: expectedRegions.Item1);

            var controller = new RegionsController(unitOfWorkMock.Object, loggerMock.Object, mapper);

            var actionResult = await controller.Get(requestParam);

            var result = actionResult.Result as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedRegions.Item2, o => o.ComparingByMembers<Region>().ExcludingMissingMembers());
        }

        [Fact]
        public async void Get_Should_Return_NotFoundResult()
        {
            unitOfWorkMock.Setup(
                repo => repo.Regions.GetAllAsync(
                    It.IsAny<PaginationParam>(),
                    It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<Func<IQueryable<Region>, IOrderedQueryable<Region>>>(),
                    It.IsAny<List<string>>()))
                .ReturnsAsync(value: null);

            var controller = new RegionsController(unitOfWorkMock.Object, loggerMock.Object, mapper);

            var actionResult = await controller.Get(new PaginationParam { PageNumber = 1 , PageSize = 5});

            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void Get_Should_Return_AnExpectedResut()
        {
            var region = GetRegion();

            unitOfWorkMock.Setup(
                repo => repo.Regions.GetAsync(
                    It.IsAny<Expression<Func<Region, bool>>>(),
                    It.IsAny<List<string>>()))
                .ReturnsAsync(value: region);

            var controller = new RegionsController(unitOfWorkMock.Object, loggerMock.Object, mapper);

            var actionResult = await controller.Get(new Random().Next(1, 1000));

            var result = actionResult.Result as OkObjectResult;

            result.Value.Should().BeOfType<RegionDetailDTO>();
            result.Value.Should().BeEquivalentTo(region, o => o.ComparingByMembers<Region>());
        }

        public Region GetRegion() => new Region
        {
            Id = 1,
            Name = Guid.NewGuid().ToString(),
            DistanceToTheSun = new Random().Next(1, 1000),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Bodies = new List<Body>()
        };

        public (IPagedList<Region>, List<Region>) GetRegions()
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

            return (regions.ToPagedList(pageNumber: 1, pageSize: 5), regions);
        }
    }
}
