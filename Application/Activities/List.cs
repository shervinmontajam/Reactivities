using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities.DataTransferObjects;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<ActivityDto>>
        {

        }

        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {

            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _dataContext.Activities.Include(i => i.UserActivities).ThenInclude(a => a.AppUser).ToListAsync(cancellationToken: cancellationToken);
                return _mapper.Map<List<Activity>, List<ActivityDto>>(activities);
            }
        }
    }
}
