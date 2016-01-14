using System;
using Fanex.MailDelivery.Admin.Utils;
using NUnit.Framework;

namespace Fanex.MailDelivery.Admin.Tests.Unit.Utils
{
    [TestFixture]
    public class DateRangeValidatorTests
    {
        [Test]
        public void ValidateMinute_ValidMinuteRange_NotThrowsException()
        {
            // Assert
            DateRangeValidator.ValidateMinute(0);
            DateRangeValidator.ValidateMinute(30);
            DateRangeValidator.ValidateMinute(59);
        }

        [Test]
        public void ValidateMinute_InvalidMinuteRange_ThrowsException()
        {
            // Assert
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateMinute(60));
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateMinute(-1));
        }

        [Test]
        public void ValidateHour_ValidMinuteRange_NotThrowsException()
        {
            // Assert
            DateRangeValidator.ValidateHour(0);
            DateRangeValidator.ValidateHour(23);
        }

        [Test]
        public void ValidateHour_InvalidMinuteRange_ThrowsException()
        {
            // Assert
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateHour(-1));
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateHour(24));
        }

        [Test]
        public void ValidateDayOfMonth_ValidMinuteRange_NotThrowsException()
        {
            // Assert
            DateRangeValidator.ValidateDayOfMonth(1);
            DateRangeValidator.ValidateDayOfMonth(31);
        }

        [Test]
        public void ValidateDayOfMonth_InvalidMinuteRange_ThrowsException()
        {
            // Assert
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateDayOfMonth(0));
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateDayOfMonth(32));
        }

        [Test]
        public void ValidateMonth_ValidMinuteRange_NotThrowsException()
        {
            // Assert
            DateRangeValidator.ValidateMonth(1);
            DateRangeValidator.ValidateMonth(12);
        }

        [Test]
        public void ValidateMonth_InvalidMinuteRange_ThrowsException()
        {
            // Assert
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateMonth(0));
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateMonth(13));
        }

        [Test]
        public void ValidateMinDayOfMonth_ValidMinuteRange_NotThrowsException()
        {
            // Assert
            DateRangeValidator.ValidateMinDayOfMonth(1);
            DateRangeValidator.ValidateMinDayOfMonth(28);
        }

        [Test]
        public void ValidateMinDayOfMonth_InvalidMinuteRange_ThrowsException()
        {
            // Assert
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateMinDayOfMonth(0));
            Assert.Catch<ArgumentException>(() => DateRangeValidator.ValidateMinDayOfMonth(29));
        }
    }
}
