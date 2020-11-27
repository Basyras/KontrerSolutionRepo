using Kontrer.OwnerServer.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.Business.Abstraction.UnitOfWork;
using Kontrer.OwnerServer.Business.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kontrer.OwnerServer.Business.Tests.Pricing
{
    public class PricingManagerTests
    {
        [Fact]
        public void asdd()
        {
            var mockRepo = new Mock<IPricingSettingsRepository>();
            var mockUoWFactory = new Mock<IUnitOfWorkFactory<IPricingSettingsUnitOfWork>>();
            var pricingManager = new PricingManager(mockRepo.Object, mockUoWFactory.Object,null,null);


            
       
        }
    }
}
