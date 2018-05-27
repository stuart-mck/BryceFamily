using BryceFamily.Repo.PG.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Controllers
{
    public class PostgresController : Controller
    {
        private readonly PersonRepository personRepository;

        public PostgresController(PersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public IActionResult Index()
        {
            return Ok(this.personRepository.Load(1));
        }
    }
}
