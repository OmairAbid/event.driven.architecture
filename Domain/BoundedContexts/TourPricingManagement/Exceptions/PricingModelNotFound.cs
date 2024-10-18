using Domain.Exceptions;

namespace Domain.BoundedContexts.TourPricingManagement.Exceptions
{
    class PricingModelNotFound : DomainException
    {
        public PricingModelNotFound() : base("Pricing Model not found.") { }
    }
}
