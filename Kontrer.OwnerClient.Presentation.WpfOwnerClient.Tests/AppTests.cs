using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace Kontrer.OwnerClient.Presentation.WpfOwnerClient.Tests
{
    public class AppTests
    {
        [Fact]
        public void TestDIOptions()
        {
            var app = new App(x => 
            { 
                x.AddOptions<FakeServiceOptions>().BindConfiguration(nameof(FakeServiceOptions));     
               
            });

            var options = app.AppHost.Services.GetService<IOptions<FakeServiceOptions>>();
            Assert.Equal(2, options.Value.TestInt);
        }
        
    }

    class FakeServiceOptions
    {
        public int TestInt { get; set; }
    }
}
