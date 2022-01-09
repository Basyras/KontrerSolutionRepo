using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Serialization.ProtobufNet.Tests
{
    public class TestCar
    {
        public TestCar()
        {

        }
        public TestCar(string name)
        {
            Name = name;
        }
        public string Name { get; init; }        
    }
}
