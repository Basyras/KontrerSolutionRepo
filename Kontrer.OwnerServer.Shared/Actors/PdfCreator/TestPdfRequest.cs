using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Actors.PdfCreator
{
    public class TestPdfRequest
    {
        public TestPdfRequest(string testValue)
        {
            TestValue = testValue;
        }

        public string TestValue { get; }
    }
}
