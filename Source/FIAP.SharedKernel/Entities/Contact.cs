using FIAP.SharedKernel.DomainObjects;

namespace FIAP.SharedKernel.Entities
{
    public class Contact : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }

        protected Contact() { }

        public Contact(string name, Email email, PhoneNumber phoneNumber)
        {
            ValidateName(name);

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public static Contact Create(string name, string email, int phoneCode, string phoneNumber)
        {
            var address = new Email(email);
            var phone = new PhoneNumber(phoneCode, phoneNumber);

            return new Contact(name, address, phone);
        }

        public void Update(Contact contact)
        {
            UpdateName(contact.Name);
            UpdateEmail(contact.Email.Address);
            UpdatePhoneNumber(contact.PhoneNumber.Code, contact.PhoneNumber.Number);
            SetUpdatedDate();
        }

        public void Update(string name, string email, int phoneCode, string phoneNumber)
        {
            UpdateName(name);
            UpdateEmail(email);
            UpdatePhoneNumber(phoneCode, phoneNumber);
            SetUpdatedDate();
        }

        public void UpdateName(string name)
        {
            if (Name.Equals(name))
                return;

            ValidateName(name);
            Name = name;
        }

        public void UpdateEmail(string email)
        {
            if (Email.Address.Equals(email, StringComparison.OrdinalIgnoreCase))
                return;

            Email = new Email(email);
        }

        public void UpdatePhoneNumber(int phoneCode, string phoneNumber)
        {
            if (PhoneNumber.Code.Equals(phoneCode) && PhoneNumber.Number.Equals(phoneNumber))
                return;

            PhoneNumber = new PhoneNumber(phoneCode, phoneNumber);
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(Name), "Name must be filled");
        }

        private void SetUpdatedDate()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
