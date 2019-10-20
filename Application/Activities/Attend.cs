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
    public class Attend
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

                if (attend != null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Attend = "You are already attended to this activity " });

                attend = new UserActivity
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = true,
                    DateJoined = DateTime.Now
                };

                _dataContext.UserActivities.Add(attend);

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
