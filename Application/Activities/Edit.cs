using MediatR;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using FluentValidation;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime? Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class CommandValidator : AbstractValidator<Create.Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Title).NotEmpty();
                RuleFor(p => p.Description).NotEmpty();
                RuleFor(p => p.Category).NotEmpty();
                RuleFor(p => p.Date).NotEmpty();
                RuleFor(p => p.City).NotEmpty();
                RuleFor(p => p.Venue).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(request.Id);

                if (activity == null) throw new RestException(HttpStatusCode.NotFound, new { activity = "Not Found" });

                activity.Title = request.Title ?? activity.Title;
                activity.Description = request.Description ?? activity.Description;
                activity.Category = request.Category ?? activity.Category;
                activity.Date = request.Date ?? activity.Date;
                activity.City = request.City ?? activity.City;
                activity.Venue = request.Venue ?? activity.Venue;

                if (await _dataContext.SaveChangesAsync(cancellationToken) > 0)
                    return Unit.Value;

                throw new Exception("Problem in saving changes!");
            }
        }
    }
}
