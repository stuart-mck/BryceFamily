using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Http;
using BryceFamily.Repo.Core.Repository;
using OfficeOpenXml;
using System.Linq;
using BryceFamily.Web.MVC.Extensions;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading.Tasks;
using System.Threading;
using BryceFamily.Repo.Core.Read.People;

namespace BryceFamily.Web.MVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IWriteRepository<Repo.Core.Model.Person, Guid> _writeModel;
        private readonly IPersonReadRepository _readModel;

        public PeopleController(IPersonReadRepository readModel, IWriteRepository<Repo.Core.Model.Person, Guid> writeModel)
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
        public IActionResult Search()
        {
            return View(new SearchPersonModel());
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchPersonModel searchPersonModel)
        {
            var results = await _readModel.SearchByName(searchPersonModel.FirstName, searchPersonModel.LastName, searchPersonModel.EmailAddress, searchPersonModel.Occupation, new CancellationToken());

            return View("List", results.Select(Models.Person.Map));
        }

        [HttpGet]
        public async Task<IActionResult> List(IEnumerable<Person> people)
        {
            return await Task.FromResult(View(people));
        }



        [HttpPost]
        public async Task<IActionResult> Person(Person  person)
        {

            await _writeModel.Save(person.MapToEntity(), new CancellationToken());
            return RedirectToAction("Index");
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


        [HttpGet]
        public IActionResult Person()
        {
            return View(new Person());
        }




        [HttpPost]
        public async Task<IActionResult> People(List<IFormFile> files)
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
                            while (sheet.Cells[rowId, 1].Text != string.Empty)
                            {
                                var identifier = new PersonIdentifier()
                                {
                                    FirstName = sheet.Cells[PersonImport.FirstName.ToInt(), 1].Text,
                                    LastName = sheet.Cells[PersonImport.LastName.ToInt(), 1].Text,
                                    EmailAddress = sheet.Cells[PersonImport.EmailAddress.ToInt(), 1].Text,
                                    MiddleName = sheet.Cells[PersonImport.EmailAddress.ToInt(), 1].Text
                                };

                                var person = await  _writeModel.FindByQuery(identifier);

                                if (person == null) {
                                    person = new Repo.Core.Model.Person()
                                    {
                                        FirstName = sheet.Cells[PersonImport.FirstName.ToInt(), 1].Text,
                                        LastName = sheet.Cells[PersonImport.LastName.ToInt(), 1].Text,
                                        Email = sheet.Cells[PersonImport.EmailAddress.ToInt(), 1].Text,
                                        ID = Guid.NewGuid()
                                    };
                                }

                                person.Address = sheet.Cells[PersonImport.Address1.ToInt(), 1].Text;
                                person.Address1 = sheet.Cells[PersonImport.Address2.ToInt(), 1].Text;
                                person.Suburb = sheet.Cells[PersonImport.Suburb.ToInt(), 1].Text;
                                person.State = sheet.Cells[PersonImport.State.ToInt(), 1].Text;
                                person.PostCode = sheet.Cells[PersonImport.PostCode.ToInt(), 1].Text;
                                person.Country = sheet.Cells[PersonImport.Country.ToInt(), 1].Text;
                                person.Occupation = sheet.Cells[PersonImport.Occupation.ToInt(), 1].Text;
                                
                                await _writeModel.Save(person, new CancellationToken());

                            }
                        }
                    }
                }
            }
            return Ok();
        }


    }
}
