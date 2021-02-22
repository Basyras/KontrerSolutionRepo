using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction
{
    public class AccommodationIdRequestedMessage
    {
        public string TestValue { get; set; }

        public AccommodationIdRequestedMessage(string testValue)
        {
            this.TestValue = testValue;
        }
    }
}
