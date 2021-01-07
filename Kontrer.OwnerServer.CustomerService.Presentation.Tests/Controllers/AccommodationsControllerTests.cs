using Kontrer.OwnerServer.CustomerService.Business.Abstraction.Accommodations;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.CustomerService.Presentation.AspApi.Controllers;
using Kontrer.Shared.Models;
using Kontrer.Shared.Tests.FakeData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.CustomerService.Presentation.Tests.Controllers
{
    public class AccommodationsControllerTests
    {
        [Fact]
        public async Task TestGetOneNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            mockRepo.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(() => Task.FromResult<FinishedAccommodationModel>(null));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockManager = new Mock<IAccommodationManager>();
            mockManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);            


            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockManager.Object, mockLogger.Object);
            ActionResult<FinishedAccommodationModel> result = await controller.Get(int.MaxValue);
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
            Assert.Null(result.Value);                       

        }

        [Fact]
        public async Task TestGetOneNotNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            var record = new FinishedAccommodationModel() { AccommodationId = 68 };

            mockRepo.Setup(x => x.GetAsync(It.Is<int>(x => x == record.AccommodationId))).Returns(Task.FromResult(record));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockAccoManager = new Mock<IAccommodationManager>();
            mockAccoManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);           


            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockAccoManager.Object,mockLogger.Object);
            ActionResult<FinishedAccommodationModel> result = await controller.Get(record.AccommodationId);
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(objectResult.Value, record);
        }

        [Fact]
        public async Task TestGetAllNotNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            Dictionary<int, FinishedAccommodationModel> records = AccommodationFakeData.GetAccommodationsWithoutCustomers(15).ToDictionary(x => x.AccommodationId);

            mockRepo.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(records));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockManager = new Mock<IAccommodationManager>();
            mockManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);

            

            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockManager.Object, mockLogger.Object);
            ActionResult<Dictionary<int, FinishedAccommodationModel>> result = await controller.Get();
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            ObjectResult objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(objectResult.Value, records);
        }

        [Fact]
        public async Task TestGetAllNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            Dictionary<int, FinishedAccommodationModel> records = null;

            mockRepo.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(records));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockManager = new Mock<IAccommodationManager>();
            mockManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);
            
            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockManager.Object, mockLogger.Object);
            ActionResult<Dictionary<int, FinishedAccommodationModel>> result = await controller.Get();
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(objectResult.Value, records);
        }
    }
}
