using Api.Shared.Exceptions;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Repositories;
using HouseBroker.Domain.Enums;
using Moq;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Tests.Commands
{
    [TestFixture]
    public class CreatePropertyListingHandlerTests
    {
        private Mock<IPropertyListingWriteRepository> _writeRepositoryMock;
        private Mock<ILogger<CreatePropertyListing.Handler>> _loggerMock;
        private CreatePropertyListing.Handler _handler;

        [SetUp]
        public void Setup()
        {
            _writeRepositoryMock = new Mock<IPropertyListingWriteRepository>();
            _loggerMock = new Mock<ILogger<CreatePropertyListing.Handler>>();
            _handler = new CreatePropertyListing.Handler(_writeRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Handle_WithInvalidData_ShouldThrowAppValidationException()
        {
            // Arrange
            var command = new CreatePropertyListing.Command(
                Name: "asdfasdf",
                CurrencyCode: "USD",
                Price: -10,
                PropertyType: PropertyType.House,
                ImageUrls: new List<string>(),
                UserId: "12313",
                Country: "Country",
                Street: "Street",
                City: "City"
            );

            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var ex = Assert.ThrowsAsync<AppValidationException>(async () =>
                await _handler.Handle(command, cancellationToken));

            Assert.That(ex.Identifier, Is.Not.Null.And.Not.Empty);
            Assert.That(ex.Message, Is.Not.Empty);

            _writeRepositoryMock.Verify(r => r.Add(It.IsAny<HouseBroker.Domain.PropertyListing>()), Times.Never);
            _writeRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
