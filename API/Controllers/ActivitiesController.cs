using Application.Activities;
using Application.Activities.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{

    public class ActivitiesController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }


        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<ActivityDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }


        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }


        [HttpPut("{id}")]
        [Authorize(policy: "IsActivityHost")]
        public async Task<ActionResult<Unit>> Update(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize(policy: "IsActivityHost")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command{Id = id});
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend(Guid id)
        {
            return await Mediator.Send(new Attend.Command {Id = id});
        }

        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> UnAttend(Guid id)
        {
            return await Mediator.Send(new UnAttend.Command {Id = id});
        }
    }
}
