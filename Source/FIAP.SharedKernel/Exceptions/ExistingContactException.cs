namespace FIAP.SharedKernel.Exceptions
{
    public class ExistingContactException : Exception
    {
        public ExistingContactException() : base("A contact with same E-mail or Phone Number already exists") { }
        public ExistingContactException(string message) : base(message) { }
    }
}
