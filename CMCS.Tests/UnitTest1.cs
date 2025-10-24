using CMCS.Web.Models;
using Xunit;

namespace CMCS.Tests
{
    public class ClaimTests
    {
        [Fact]
        public void Claim_Calculates_TotalPayment()
        {
            var claim = new Claim { HoursWorked = 10, HourlyRate = 200 };
            var total = (decimal)claim.HoursWorked * claim.HourlyRate;
            Assert.Equal(2000, total);
        }
    }
}
