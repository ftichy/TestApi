using System.Linq;
using Xunit;
using TestApi.HelloWord.Server.Controllers;
using TestApi.HelloWord.Server;

namespace TestApi.HelloWord.Server.Tests
{
    public class WeatherForecastControllerTests_Comprehensive
    {
        private readonly WeatherForecastController _controller = new();
        private readonly string[] _validSummaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        [Fact]
        public void Get_Returns_Five_Entries()
        {
            // Act
            var result = _controller.Get().ToList();

            // Assert
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void Get_All_Items_Have_Valid_Temperature_Range()
        {
            // Act
            var result = _controller.Get().ToList();

            // Assert
            foreach (var item in result)
            {
                Assert.InRange(item.TemperatureC, -20, 55);
            }
        }

        [Fact]
        public void Get_All_Items_Have_Valid_Summary()
        {
            // Act
            var result = _controller.Get().ToList();

            // Assert
            foreach (var item in result)
            {
                Assert.NotNull(item.Summary);
                Assert.Contains(item.Summary, _validSummaries);
            }
        }

        [Fact]
        public void Get_All_Items_Have_Future_Dates()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = _controller.Get().ToList();

            // Assert
            foreach (var item in result)
            {
                Assert.True(item.Date > today, $"Expected future date but got {item.Date}");
            }
        }

        [Fact]
        public void Get_Dates_Are_Sequential()
        {
            // Act
            var result = _controller.Get().ToList();

            // Assert
            for (int i = 0; i < result.Count - 1; i++)
            {
                Assert.True(
                    result[i].Date < result[i + 1].Date,
                    $"Date at index {i} ({result[i].Date}) should be before date at index {i + 1} ({result[i + 1].Date})"
                );
            }
        }

        [Fact]
        public void Get_Each_Item_Is_One_Day_After_Previous()
        {
            // Act
            var result = _controller.Get().ToList();

            // Assert
            for (int i = 0; i < result.Count - 1; i++)
            {
                var expectedNextDate = result[i].Date.AddDays(1);
                Assert.Equal(expectedNextDate, result[i + 1].Date);
            }
        }

        [Fact]
        public void Get_Returns_IEnumerable()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<IEnumerable<WeatherForecast>>(result, exactMatch: false);
        }

        [Fact]
        public void Get_Does_Not_Return_Null()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void TemperatureF_Calculated_Correctly()
        {
            // Act
            var result = _controller.Get().First();
            var expectedF = 32 + (int)(result.TemperatureC / 0.5556);

            // Assert
            Assert.Equal(expectedF, result.TemperatureF);
        }
              
    }
}
