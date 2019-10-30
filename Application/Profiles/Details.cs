using Application.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Profile>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Profile>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.SingleOrDefaultAsync(x=>x.UserName == request.Username, cancellationToken: cancellationToken);
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { user = "Not Found" });

                return new Profile
                {
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Image = user.Photos.FirstOrDefault(a=>a.IsMain)?.Url,
                    Bio = user.Bio,
                    Photos = user.Photos
                };

            }
        }
    }
}
