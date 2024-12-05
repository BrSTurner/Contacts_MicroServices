using System.ComponentModel.DataAnnotations;

namespace FIAP.SharedKernel.DomainObjects
{
    public class Email
    {
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Address { get; init; }

        public Email(string address)
        {
            if (!IsValidEmail(address))
            {
                throw new ArgumentException("Invalid email address format.", nameof(address));
            }

            Address = address;
        }

        private static bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public override string ToString() => Address;

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(Email other)
        {
            if (other is null)
            {
                return false;
            }

            return Address == other.Address;
        }

        public override int GetHashCode()
        {
            return Address != null ? Address.GetHashCode() : 0;
        }

        public static bool operator ==(Email left, Email right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !Equals(left, right);
        }
    }
}
