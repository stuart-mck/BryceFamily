using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Linq;
using BryceFamily.Web.MVC.Extensions;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading.Tasks;
using System.Threading;
using BryceFamily.Repo.Core.Read.People;
using System.Globalization;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Web.MVC.Infrastructure;

namespace BryceFamily.Web.MVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IWriteRepository<Repo.Core.Model.Person, Guid> _writeModel;
        private readonly IPersonReadRepository _readModel;
        private readonly ClanAndPeopleService _clanAndPeopleService;

        public PeopleController(IPersonReadRepository readModel, IWriteRepository<Repo.Core.Model.Person, Guid> writeModel, ClanAndPeopleService clanAndPeopleService)
        {
            _readModel = readModel;
            _writeModel = writeModel;
            _clanAndPeopleService = clanAndPeopleService;
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
            var results = await _readModel.SearchByName(searchPersonModel.Clan, searchPersonModel.FirstName, searchPersonModel.LastName, searchPersonModel.EmailAddress, searchPersonModel.Occupation, new CancellationToken());

            return View("List", results.Select(Models.Person.Map));
        }

        [HttpGet]
        public async Task<IActionResult> List(IEnumerable<Models.Person> people)
        {
            return await Task.FromResult(View(people));
        }



        [HttpPost]
        public async Task<IActionResult> Person(Models.Person person)
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
        public async Task<IActionResult> Person(Guid? id = null)
        {
            Models.Person p = null;
            if (id != null)
                p = Models.Person.Map(await _readModel.Load(id.Value, new CancellationToken()));
            else
                p = new Models.Person();

            return View(p);
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> MasterList(List<IFormFile> files)
        {
            if (files.Count > 0)
            {


                foreach (var file in files)
                {
                    using (var excel = new ExcelPackage(file.OpenReadStream()))
                    {
                        var sheet = excel.Workbook.Worksheets["Family Master"]; 
                        if (sheet != null)
                        {
                            var rowId = 2; // sheet has header row!
                            while (sheet.Cells[rowId, 1].Text != string.Empty)
                            {
                                var identifier = new PersonIdentifier()
                                {
                                    PersonId = ReadIntCell(sheet, rowId, PersonImport.ID),
                                    FirstName = ReadStringCell(sheet, rowId, PersonImport.FirstName).ToTitleCase(),
                                    LastName = ReadStringCell(sheet, rowId, PersonImport.LastName).ToUpper(),
                                    EmailAddress = ReadStringCell(sheet, rowId, PersonImport.EmailAddress),
                                    MiddleName = ReadStringCell(sheet, rowId, PersonImport.MiddleName).ToTitleCase()
                                };

                                var person = await _writeModel.FindByQuery(identifier, CancellationToken.None);

                                if (person == null) {
                                    person = new Repo.Core.Model.Person()
                                    {
                                        PersonID = ReadIntCell(sheet, rowId, PersonImport.ID),
                                        ID = Guid.NewGuid()
                                    };
                                }

                                person.FirstName = ReadStringCell(sheet, rowId, PersonImport.FirstName).ToTitleCase();
                                person.LastName = ReadStringCell(sheet, rowId, PersonImport.LastName).ToUpper();
                                person.EmailAddress = ReadStringCell(sheet, rowId, PersonImport.EmailAddress);
                                person.MiddleName = ReadStringCell(sheet, rowId, PersonImport.MiddleName).ToTitleCase();
                                person.MaidenName = ReadStringCell(sheet, rowId, PersonImport.MaindenName).ToUpper();
                                person.Phone = ReadStringCell(sheet, rowId, PersonImport.PhoneNumber);
                                person.Clan =   _clanAndPeopleService.Clans[ReadIntCell(sheet, rowId, PersonImport.Clan) - 1];
                                person.MotherID = GetNullableInt(sheet, PersonImport.MotherId, rowId);
                                person.FatherID = GetNullableInt(sheet, PersonImport.FatherId, rowId);
                                person.Address1 = ReadStringCell(sheet, rowId, PersonImport.Address1);
                                person.Address2 = ReadStringCell(sheet, rowId, PersonImport.Address2);
                                person.Suburb = ReadStringCell(sheet, rowId, PersonImport.Suburb).ToUpper();
                                person.State = ReadStringCell(sheet, rowId, PersonImport.State).ToUpper();
                                person.PostCode = ReadStringCell(sheet, rowId, PersonImport.PostCode);
                                person.Occupation = ReadStringCell(sheet, rowId, PersonImport.Occupation);

                                await _writeModel.Save(person, CancellationToken.None);

                                rowId++;

                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Relationships(List<IFormFile> files)
        {
            if (files.Count > 0)
            {


                foreach (var file in files)
                {
                    using (var excel = new ExcelPackage(file.OpenReadStream()))
                    {
                        var sheet = excel.Workbook.Worksheets["Spouses"];
                        if (sheet != null)
                        {
                            var rowId = 2; // sheet has header row!
                            while (sheet.Cells[rowId, 1].Text != string.Empty)
                            {
                                var husbandId = new PersonIdentifier()
                                {
                                    PersonId = ReadIntCell(sheet, rowId, 1),
                                };

                                var wifeID = new PersonIdentifier()
                                {
                                    PersonId = ReadIntCell(sheet, rowId, 2),
                                };


                                var husband = await _writeModel.FindByQuery(husbandId, CancellationToken.None);
                                var wife = await _writeModel.FindByQuery(wifeID, CancellationToken.None);

                                if (husband != null && wife != null)
                                {
                                    
                                    var relationShip = new SpousalRelationship()
                                        {
                                            HusbandID = husband.PersonID,
                                            WifeID = wife.PersonID
                                        };
                                    
                                    relationShip.MarriageDate = ReadNullableDate(sheet, rowId, 3);
                                    relationShip.DivorceDate = ReadNullableDate(sheet, rowId, 4);

                                    
                                    if (husband.Relationships.Any(r => r.WifeID == relationShip.WifeID))
                                        husband.Relationships.Remove(husband.Relationships.First(t => t.WifeID == relationShip.WifeID));
                                    husband.Relationships.Add(relationShip);

                                    if (wife.Relationships.Any(r => r.HusbandID == relationShip.HusbandID))
                                        wife.Relationships.Remove(wife.Relationships.First(t => t.HusbandID == relationShip.HusbandID));
                                    wife.Relationships.Add(relationShip);

                                    await _writeModel.Save(husband, CancellationToken.None);
                                    await _writeModel.Save(wife, CancellationToken.None);
                                }

                                rowId++;
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private DateTime? ReadNullableDate(ExcelWorksheet sheet, int rowId, int columnId)
        {
            var cellValue = sheet.Cells[rowId, columnId].Text;

            if (!string.IsNullOrEmpty(cellValue))
            {
                if (DateTime.TryParse(cellValue, out var date))
                    return date;
                return null;
            }
            return null;
        }

        private int ReadIntCell(ExcelWorksheet sheet, int rowNumber, int columnId)
        {
            var cellValue = sheet.Cells[rowNumber, columnId].Text;
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (int.TryParse(cellValue, out var intValue))
                    return intValue;
            }
            throw new ArgumentOutOfRangeException($"Cannot convert {columnId} to integer");
        }

        private int ReadIntCell(ExcelWorksheet sheet, int rowNumber, PersonImport columnId)
        {
            return ReadIntCell(sheet, rowNumber, columnId.ToInt());
        }

        private string ReadStringCell(ExcelWorksheet sheet, int rowNumber, PersonImport columnId)
        {
            return sheet.Cells[rowNumber, columnId.ToInt()].Text.Trim();
        }

        private int? GetNullableInt(ExcelWorksheet sheet, PersonImport columnId, int rowNumber)
        {
            var cellValue = sheet.Cells[rowNumber, columnId.ToInt()].Text;
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (int.TryParse(cellValue, out var intValue))
                    return intValue;
                return null;
            }
            return null;
        }
    }

}
