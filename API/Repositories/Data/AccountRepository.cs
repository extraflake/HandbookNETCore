using API.Context;
using API.Models;
using API.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class AccountRepository
    {
        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        IConfiguration _configuration { get; }
        
        public Account Get(LoginVM loginVM)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var result = connection.Query<Account>("SP_Retrieve_AspNetUsers_AspNetRoles @email = @Email, @password = @Password", new { email = loginVM.Email, password = loginVM.Password }).SingleOrDefault();
                return result;
            }
        }

        public Account Post(RegisterVM registerVM)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var result = connection.Query<Account>("SP_Insert_AspNetUsers @email = @Email, @password = @Password", new { email = registerVM.Email, password = registerVM.Password }).SingleOrDefault();
                return result;
            }
        }
    }
}
