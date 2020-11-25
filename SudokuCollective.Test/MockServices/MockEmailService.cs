using Moq;
using SudokuCollective.Core.Interfaces.Services;

namespace SudokuCollective.Test.MockServices
{
    public class MockEmailService
    {
        internal Mock<IEmailService> EmailServiceSuccessfulRequest { get; set; }
        internal Mock<IEmailService> EmailServiceFailedRequest { get; set; }

        public MockEmailService()
        {
            EmailServiceSuccessfulRequest = new Mock<IEmailService>();
            EmailServiceFailedRequest = new Mock<IEmailService>();

            EmailServiceSuccessfulRequest.Setup(emailService =>
                emailService.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            EmailServiceFailedRequest.Setup(emailService =>
                emailService.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
        }
    }
}
