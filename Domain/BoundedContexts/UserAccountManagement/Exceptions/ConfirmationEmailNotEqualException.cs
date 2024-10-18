using Domain.Exceptions;

namespace Domain.BoundedContexts.UserAccountManagement.Exceptions
{
    public class ConfirmationEmailNotEqualException : DomainException
    {
        public ConfirmationEmailNotEqualException() : base("The Accounts existing email address does not match the provided confirmation email address.") { }
    }
}
