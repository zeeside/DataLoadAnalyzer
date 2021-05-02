using DataLoadAnalyzer.Configuration;
using DataLoadAnalyzer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace DataLoadAnalyzer.Tests
{
    public class DataAnalyzerServiceTests
    {
        [Fact]
        public void DataAnalyzerServiceTests_TestContructor()
        {
            // Arrange
            var config = new Config();
            Assert.Throws<ArgumentNullException>(() => new DataAnalyzerService(Options.Create(config), null));
        }
    }
}
