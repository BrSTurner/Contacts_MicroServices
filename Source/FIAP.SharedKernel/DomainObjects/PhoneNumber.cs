using FIAP.SharedKernel.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FIAP.SharedKernel.DomainObjects
{
    public class PhoneNumber
    {
        [RegularExpression(@"^[9]\d{8}$", ErrorMessage = "Invalid cellphone number format. It must be a 9-digit number starting with 9.")]
        public string Number { get; init; }

        public int Code { get; init; }

        public PhoneNumber(int code, string number)
        {
            if (!PhoneCodes.IsCodeValid(code))
                throw new ArgumentException("Invalid phone code.", nameof(code));

            if (!IsValidCellphone(number))
                throw new ArgumentException("Invalid cellphone number format. It must be a 9-digit number starting with 9.", nameof(number));

            Code = code;
            Number = number;
        }

        private static bool IsValidCellphone(string number)
        {
            var regex = new Regex(@"^[9]\d{8}$");
            return regex.IsMatch(number);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PhoneNumber)obj;
            return Code == other.Code && Number == other.Number;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Number);
        }

        public static bool operator ==(PhoneNumber left, PhoneNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhoneNumber left, PhoneNumber right)
        {
            return !Equals(left, right);
        }

        public override string ToString() => $"({Code}) {Number}";
    }
}
