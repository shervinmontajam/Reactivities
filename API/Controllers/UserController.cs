﻿using Application.User;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{

    public class UserController : BaseController
    {

        [HttpPost("login")]
        public async Task<ActionResult<User>> Create(Login.Query query)
        {
            return await Mediator.Send(query);
        }


    }
}