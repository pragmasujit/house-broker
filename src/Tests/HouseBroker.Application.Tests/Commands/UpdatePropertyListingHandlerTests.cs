using Api.Shared.Exceptions;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseBroker.Application.Tests.Commands
{
    [TestFixture]
    public class UpdatePropertyListingHandlerTests
    {
        private Mock<IPropertyListingReadRepository> _readRepoMock;
        private Mock<IPropertyListingWriteRepository> _writeRepoMock;
        private Mock<ILogger<UpdatePropertyListing.Handler>> _loggerMock;
        private UpdatePropertyListing.Handler _handler;

        [SetUp]
        public void Setup()
        {
            _readRepoMock = new Mock<IPropertyListingReadRepository>();
            _writeRepoMock = new Mock<IPropertyListingWriteRepository>();
            _loggerMock = new Mock<ILogger<UpdatePropertyListing.Handler>>();

            _handler = new UpdatePropertyListing.Handler(
                readRepository: _readRepoMock.Object,
                writeRepository: _writeRepoMock.Object,
                logger: _loggerMock.Object);
        }

        private UpdatePropertyListing.Command CreateValidCommand(Guid guid)
            => new UpdatePropertyListing.Command(
                Guid: guid,
                Name: "Updated Property",
                CurrencyCode: "USD",
                Price: 1000m,
                PropertyType: PropertyType.Apartment,
                ImageUrls: new List<string> { "https://image1.jpg", "https://image2.jpg" },
                Country: "USA",
                Street: "123 Main St",
                City: "New York",
                UserId: "user-123");

        private PropertyListing CreateRealListing(Guid guid, string userId)
        {
            var address = PropertyListingAddress.Create("InitialCountry", "InitialStreet", "InitialCity");

            var listing = PropertyListing.Create(
                name: "Initial Property",
                currencyCode: "USD",
                price: 500m,
                propertyType: PropertyType.Apartment,
                imageUrls: new List<string> { "https://oldimage.jpg" },
                propertyListingAddress: address,
                createdBy: userId);

            var guidProp = typeof(PropertyListing).GetProperty("Guid");
            guidProp?.SetValue(listing, guid);

            return listing;
        }

        [Test]
        public async Task Handle_Should_Run_Successfully_When_Listing_Found()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var command = CreateValidCommand(guid);
            var listing = CreateRealListing(guid, command.UserId);

            _readRepoMock.Setup(r => r.GetAllAsync(
                    It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PropertyListing> { listing });


            _writeRepoMock.Setup(w => w.Update(It.IsAny<PropertyListing>()));
            _writeRepoMock.Setup(w => w.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(guid, result.Guid);

            _writeRepoMock.Verify(w => w.Update(It.IsAny<PropertyListing>()), Times.Once);
            _writeRepoMock.Verify(w => w.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Handle_Should_Throw_When_Listing_Not_Found()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());

            _readRepoMock.Setup(r => r.GetAllAsync(
                    It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PropertyListing> {  });


            // Act & Assert
            Assert.ThrowsAsync<AppValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
