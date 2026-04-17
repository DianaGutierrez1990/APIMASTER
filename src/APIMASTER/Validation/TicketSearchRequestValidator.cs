using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class TicketSearchRequestValidator : AbstractValidator<TicketSearchRequest>
{
    public TicketSearchRequestValidator()
    {
        RuleFor(x => x.ScaleTicket).NotEmpty().MaximumLength(50);
    }
}
