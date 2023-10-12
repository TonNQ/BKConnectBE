using System.ComponentModel.DataAnnotations;

namespace BKConnectBE.Common.Attributes
{
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateTime MinDate { get; set; }

        public DateRangeAttribute(string minDate)
        {
            DateTime min;

            if (!DateTime.TryParse(minDate, out min))
            {
                throw new ArgumentException("Định dạng ngày không hợp lệ");
            }

            MinDate = min;
        }
        public override bool IsValid(object value)
        {
            if (value is DateTime dateValue)
            {
                return dateValue >= MinDate && dateValue <= DateTime.Today;
            }

            return false;
        }
    }
}