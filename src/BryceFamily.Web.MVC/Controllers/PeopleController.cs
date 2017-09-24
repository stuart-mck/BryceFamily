using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Http;
using BryceFamily.Repo.Core.Repository;
using OfficeOpenXml;
using System.Linq;

namespace BryceFamily.Web.MVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IWriteModel<Repo.Core.Model.Person, Guid> _writeModel;
        private readonly IReadModel<Repo.Core.Model.Person, Guid> _readModel;

        public PeopleController(IReadModel<Repo.Core.Model.Person, Guid> readModel, IWriteModel<Repo.Core.Model.Person, Guid> writeModel)
        {
            _readModel = readModel;
            _writeModel = writeModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailUnsubscribe([FromQuery] Guid personId)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult EmailSubscribe([FromQuery] Guid personId)
        {
            return Ok();
        }



        [HttpGet]
        public IActionResult SMSSubscribe([FromQuery] Guid personId)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult SMSUnsubscribe([FromQuery] Guid personId)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Person(PersonWriteModel person)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Email()
        {
            return View(new EmailCreateModel());
        }

        [HttpPost]
        public IActionResult Email(EmailCreateModel email)
        {
            return View();
        }

        [HttpPut, Route("{personId}")]
        public IActionResult Person([FromRoute]Guid personId, [FromBody]PersonWriteModel person)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult People(List<IFormFile> files)
        {
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    using (var excel = new ExcelPackage(file.OpenReadStream()))
                    {
                        var sheet = excel.Workbook.Worksheets.FirstOrDefault();
                        if (sheet != null)
                        {
                            var rowId = 1;
                            var colId = 1;
                            while (sheet.Cells[rowId, 1].Value != string.Empty)
                            {
                                var person = new Person()
                                {

                                };
                            }
                        }
                    }
                }
            }
            return Ok();
        }


    }
}