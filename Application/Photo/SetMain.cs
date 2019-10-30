using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Photo
{
    public class SetMain
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
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
                var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                var mainPhoto = user.Photos.SingleOrDefault(a => a.IsMain);
                if (mainPhoto != null)
                    mainPhoto.IsMain = false;
                photo.IsMain = true;

                var success = await _dataContext.SaveChangesAsync(cancellationToken) > 0;
                if (success) return Unit.Value;


                throw new Exception("Something went wrong creating a user");
            }

        }
    }
}
