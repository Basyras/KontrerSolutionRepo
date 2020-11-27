using Kontrer.OwnerServer.Business.Abstraction.Accommodations;
using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Presentation.AspApi.Controllers;

using Kontrer.OwnerServer.Presentation.AspApi.Tests.Repositories.Accommodation;
using Kontrer.Shared.Models;
using Kontrer.Shared.Tests.FakeData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.Presentation.AspApi.Tests.Controllers
{
    public class AccommodationsControllerTests
    {      

        [Fact]
        public async Task TestGetOneNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            mockRepo.Setup(x => x.GetAsync(It.IsAny<int>())).Returns(() => Task.FromResult<AccommodationModel>(null));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockManager = new Mock<IAccommodationManager>();
            mockManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);

            var mockPricingManager = new Mock<IPricingManager>();
            

            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockManager.Object, mockPricingManager.Object, mockLogger.Object);
            ActionResult<AccommodationModel> result = await controller.Get(int.MaxValue);            
            Assert.IsAssignableFrom<ObjectResult>(result.Result);            
            var objectResult =  (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);            
            Assert.Null(result.Value);

            //var t = CustomerFakeData.GetCustomersWithAccommodation(5);
            //var tt = BlueprintFakeData.GetAccommodationBlueprints(5);
            //var ttt = CostFakeData.GetAccommodationCosts(1);
            var tttt = AccommodationFakeData.GetAccommodationsWithoutCustomer(5);
            //ItemCost obj = (ItemCost)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(ItemCost));            
            //ItemCost cst = CostFakeData.GetItemCosts(1)[0];
        }

        [Fact]
        public async Task TestGetOneNotNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            var record = new AccommodationModel() { AccomodationId = 68 };

            mockRepo.Setup(x => x.GetAsync(It.Is<int>(x=>x==record.AccomodationId))).Returns(Task.FromResult(record));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockAccoManager = new Mock<IAccommodationManager>();
            mockAccoManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);

            var mockPricingManager = new Mock<IPricingManager>();
            

            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockAccoManager.Object, mockPricingManager.Object, mockLogger.Object);
            ActionResult<AccommodationModel> result = await controller.Get(record.AccomodationId);
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(objectResult.Value,record);            
        }

        [Fact]
        public async Task TestGetAllNotNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            Dictionary<int, AccommodationModel> records = AccommodationFakeData.GetAccommodationsWithoutCustomers(15).ToDictionary(x => x.AccomodationId);

            mockRepo.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(records));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockManager = new Mock<IAccommodationManager>();
            mockManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);

            var mockPricingManager = new Mock<IPricingManager>();
            
            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockManager.Object, mockPricingManager.Object, mockLogger.Object);
            ActionResult<Dictionary<int, AccommodationModel>> result = await controller.Get();
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            ObjectResult objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(objectResult.Value, records);
        }

        [Fact]
        public async Task TestGetAllNull()
        {
            var mockRepo = new Mock<IAccommodationRepository>();
            Dictionary<int, AccommodationModel> records = null;

            mockRepo.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(records));

            var uow = new Mock<IAccommodationUnitOfWork>();
            uow.Setup(x => x.Accommodations).Returns(mockRepo.Object);

            var mockManager = new Mock<IAccommodationManager>();
            mockManager.Setup(x => x.CreateUnitOfWork()).Returns(uow.Object);
            var mockPricingManager = new Mock<IPricingManager>();
            var mockLogger = new Mock<ILogger<AccommodationsController>>();

            AccommodationsController controller = new AccommodationsController(mockManager.Object, mockPricingManager.Object, mockLogger.Object);
            ActionResult<Dictionary<int, AccommodationModel>> result = await controller.Get();
            Assert.IsAssignableFrom<ObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(objectResult.Value, records);
        }

    }
}
