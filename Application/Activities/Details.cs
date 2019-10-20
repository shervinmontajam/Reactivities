using Application.Activities.DataTransferObjects;
using Application.Errors;
using AutoMapper;
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
    public class Details
    {
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.Include(a => a.UserActivities).ThenInclude(u => u.AppUser).SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
                if (activity == null) throw new RestException(HttpStatusCode.NotFound, new { activity = "Not Found" });
                return _mapper.Map<Activity,ActivityDto>(activity);
            }
        }
    }
}
