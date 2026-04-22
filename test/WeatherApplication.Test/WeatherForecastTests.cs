using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.API;
using WeatherApplication.API.Controllers;

namespace WeatherApplication.Test
{
    public class WeatherForecastTests
    {
        [Fact]
        public void TemperatureF_ReturnsCorrectConversion()
        {
            var forecast = new WeatherForecast { TemperatureC = 0 };
            Assert.Equal(32, forecast.TemperatureF);
        }


        [Fact]
        public void Properties_SetAndGetCorrectly()
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            var forecast = new WeatherForecast
            {
                Date = date,
                TemperatureC = 25,
                Summary = "Warm"
            };

            Assert.Equal(date, forecast.Date);
            Assert.Equal(25, forecast.TemperatureC);
            Assert.Equal("Warm", forecast.Summary);
        }

        [Fact]
        public void Summary_DefaultIsNull()
        {
            var forecast = new WeatherForecast();
            Assert.Null(forecast.Summary);
        }
    }

    public class WeatherForecastControllerTests
    {
        private readonly WeatherForecastController _controller;

        public WeatherForecastControllerTests()
        {
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            _controller = new WeatherForecastController(mockLogger.Object);
        }

        [Fact]
        public void Get_ReturnsFiveForecasts()
        {
            var result = _controller.Get();
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Get_ReturnsForecasts_WithFutureDates()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var result = _controller.Get().ToList();

            foreach (var forecast in result)
            {
                Assert.True(forecast.Date > today);
            }
        }

        [Fact]
        public void Get_ReturnsForecasts_WithValidTemperatureRange()
        {
            var result = _controller.Get().ToList();

            foreach (var forecast in result)
            {
                Assert.InRange(forecast.TemperatureC, -20, 54);
            }
        }

        [Fact]
        public void Get_ReturnsForecasts_WithNonNullSummary()
        {
            var result = _controller.Get().ToList();

            foreach (var forecast in result)
            {
                Assert.NotNull(forecast.Summary);
            }
        }
    }
}
