using CustomerApp.Controllers;
using CustomerApp.DTOS;
using CustomerApp.Models;
using CustomerApp.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;

namespace TestCustomerApp
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task AddCustomer_Should_Call_Service_Once()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var kafkaOptions = Options.Create(new KafkaServer { Host = "localhost:9092" });

            var controller = new CustomerController(mockService.Object, kafkaOptions);
            var customer = new Customer { CustomerId = 1, Email = "test@example.com" };

            // Act
            await controller.AddCustomer(customer);

            // Assert
            mockService.Verify(s => s.AddCustomer(customer), Times.Once);
        }

        

        [Fact]
        public async Task GetAllCustomers_Should_Return_List_And_Print_Kafka_Info()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var kafkaOptions = Options.Create(new KafkaServer { Host = "localhost:9092" });

            var customers = new List<Customer>
        {
            new Customer { CustomerId = 1, Email = "a@example.com" },
            new Customer { CustomerId = 2, Email = "b@example.com" }
        };

            mockService.Setup(s => s.GetAllCustomers()).ReturnsAsync(customers);

            var controller = new CustomerController(mockService.Object, kafkaOptions);

            // Act
            var result = await controller.GetAllCustomers();

            // Assert
            mockService.Verify(s => s.GetAllCustomers(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Customer>)result).Count);
        }

        [Fact]
        public async Task GetCustomerById_Should_Return_Correct_Customer()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var kafkaOptions = Options.Create(new KafkaServer { Host = "localhost:9092" });
            var controller = new CustomerController(mockService.Object, kafkaOptions);

            var expected = new Customer { CustomerId = 5, Email = "found@example.com" };
            mockService.Setup(s => s.GetCustomerById(5)).ReturnsAsync(expected);

            // Act
            var result = await controller.GetCustomerById(5);

            // Assert
            mockService.Verify(s => s.GetCustomerById(5), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(5, result.CustomerId);
            Assert.Equal("found@example.com", result.Email);
        }
        [Fact]
        public async Task UpdateCustomer_Should_Return_True_When_Updated()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var kafkaOptions = Options.Create(new KafkaServer { Host = "localhost:9092" });
            var controller = new CustomerController(mockService.Object, kafkaOptions);

            var updatedCustomer = new Customer { CustomerId = 10, Email = "update@example.com" };
            mockService.Setup(s => s.UpdateCustomer(updatedCustomer)).ReturnsAsync(true);

            // Act
            var result = await controller.UpdateCustomer(updatedCustomer);

            // Assert
            mockService.Verify(s => s.UpdateCustomer(updatedCustomer), Times.Once);
            Assert.True(result);
        }
        [Fact]
        public async Task DeleteCustomer_Should_Return_True_When_Deleted()
        {
            // Arrange
            var mockService = new Mock<ICustomerService>();
            var kafkaOptions = Options.Create(new KafkaServer { Host = "localhost:9092" });
            var controller = new CustomerController(mockService.Object, kafkaOptions);

            long customerId = 123;
            mockService.Setup(s => s.DeleteCustomer(customerId)).ReturnsAsync(true);

            // Act
            var result = await controller.DeleteCustomer(customerId);

            // Assert
            mockService.Verify(s => s.DeleteCustomer(customerId), Times.Once);
            Assert.True(result);
        }
    }
}
