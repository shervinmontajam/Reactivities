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

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public string Body { get; set; }

            public Guid ActivityId { get; set; }

            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentDto>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {

                var activity = await _dataContext.Activities.FindAsync(request.ActivityId);
                if(activity == null)
                    throw  new RestException(HttpStatusCode.NotFound, new {Activity = "Activity not found"} );

                var user = await _dataContext.Users.SingleOrDefaultAsync(a => a.UserName == request.Username, cancellationToken: cancellationToken);

                var comment = new Comment
                {
                    Activity = activity,
                    Author = user,
                    Body = request.Body,
                    CreatedAt = DateTime.Now
                };

                activity.Comments.Add(comment);

                if (await _dataContext.SaveChangesAsync(cancellationToken) > 0)
                    return _mapper.Map<Comment, CommentDto>(comment);

                throw new Exception("Problem in saving changes!");
            }
        }
    }
}
