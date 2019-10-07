using MediatR;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                if(activity == null) throw new RestException( HttpStatusCode.NotFound,new {activity = "Not Found"});

                _dataContext.Activities.Remove(activity);

                if (await _dataContext.SaveChangesAsync(cancellationToken) > 0)
                    return Unit.Value;

                throw new Exception("Problem in saving changes!");
            }
        }
    }
}
