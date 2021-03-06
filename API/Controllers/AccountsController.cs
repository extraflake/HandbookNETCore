﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Bases;
using API.Models;
using API.Repositories.Data;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        AccountRepository _accountRepository;
        AuthController _authController = new AuthController();

        public AccountsController(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("Login")]
        public IActionResult Get(LoginVM loginVM)
        {
            var get = _accountRepository.Get(loginVM);
            if (get != null)
            {
                var token = _authController.GetToken(get);
                return Ok(token);
            }
            return BadRequest();
        }
    }
}