using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class UnAttend
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(request.Id);
                var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == _userAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);
                Validate(activity, user);

                var attend = await _dataContext.UserActivities.SingleOrDefaultAsync(a => a.AppUserId == user.Id && a.ActivityId == activity.Id, cancellationToken);

                if (attend == null)
                    return Unit.Value;

                if(attend.IsHost)
                    throw new RestException(HttpStatusCode.BadRequest, new {UnAttend = "you can not remove yourself as a host"});

                _dataContext.UserActivities.Remove(attend);

                if (await _dataContext.SaveChangesAsync(cancellationToken) > 0)
                    return Unit.Value;

                throw new Exception("Problem in saving changes!");
            }

            private void Validate(Activity activity, AppUser user)
            {
                if (activity == null) throw new ArgumentNullException(nameof(activity));
                if (user == null) throw new ArgumentNullException(nameof(user));
            }
        }
    }
}
