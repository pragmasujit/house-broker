using HouseBroker.Application.Commands;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Specifications;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseBroker.Application.Tests.Commands
{
    [TestFixture]
    public class RemovePropertyListingHandlerTests
    {
        private Mock<IPropertyListingReadRepository> _readRepoMock = null!;
        private Mock<IPropertyListingWriteRepository> _writeRepoMock = null!;
        private Mock<ILogger<RemovePropertyListing.Handler>> _loggerMock = null!;
        private RemovePropertyListing.Handler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _readRepoMock = new Mock<IPropertyListingReadRepository>();
            _writeRepoMock = new Mock<IPropertyListingWriteRepository>();
            _loggerMock = new Mock<ILogger<RemovePropertyListing.Handler>>();

            _handler = new RemovePropertyListing.Handler(
                _readRepoMock.Object,
                _writeRepoMock.Object,
                _loggerMock.Object);
        }

        private RemovePropertyListing.Command CreateCommand(Guid guid) =>
            new RemovePropertyListing.Command(guid, "user");

        [Test]
        public async Task Handle_Should_Run_And_Call_Remove_And_Save()
        {
            var guid = Guid.NewGuid();
            var command = CreateCommand(guid);

            var listing = PropertyListing.Create(
                name: "name",
                currencyCode: "USD",
                price: 123m,
                propertyType: PropertyType.Apartment,
                imageUrls: new List<string> { "https://asfas/image.asdfasdf", "https://asfas/imasdfsdfge.asdfasdf" },
                propertyListingAddress: PropertyListingAddress.Create(
                    country: "country",
                    street: "street",
                    city: "city"
                ),
                createdBy: "user"
            );

            _readRepoMock.Setup(r => r.GetAllAsync(
                  It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(),
                  It.IsAny<CancellationToken>()))
              .ReturnsAsync(new List<PropertyListing> { listing });


            _writeRepoMock.Setup(w => w.Remove(listing));
            _writeRepoMock.Setup(w => w.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);

            _writeRepoMock.Verify(w => w.Remove(listing), Times.Once);
            _writeRepoMock.Verify(w => w.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
