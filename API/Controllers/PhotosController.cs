﻿using Application.Photo;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;

namespace API.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm]Add.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await Mediator.Send(new Delete.Command{Id = id});
        }

        [HttpPost("{is}/setMain")]
        public async Task<ActionResult<Unit>> SetMain(string id)
        {
            return await Mediator.Send(new SetMain.Command {Id = id});
        }

    }
}
