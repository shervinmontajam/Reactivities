using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photo
{
    public class Add
    {
        public class Command : IRequest<Domain.Photo>
        {
            public IFormFile File { get; set; }
        }


        public class Handler : IRequestHandler<Command, Domain.Photo>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
            }


            public async Task<Domain.Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                var uploadResult = _photoAccessor.AddPhoto(request.File);

                var user = await _dataContext.Users.SingleOrDefaultAsync(a=>a.UserName == _userAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

                var photo = new Domain.Photo
                {
                    Id = uploadResult.PublicId,
                    Url = uploadResult.Url
                };
                if (!user.Photos.Any(x => x.IsMain))
                    photo.IsMain = true;
                user.Photos.Add(photo);

                var success = await _dataContext.SaveChangesAsync(cancellationToken) > 0;
                if (success) return photo;


                throw new Exception("Something went wrong creating a user");
            }

        }
    }
}
