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
using BryceFamily.Repo.Core.Model;
using BryceFamily.Web.MVC.Infrastructure;

namespace BryceFamily.Web.MVC.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IWriteRepository<Repo.Core.Model.Person, Guid> _writeModel;
        private readonly IPersonReadRepository _readModel;
        private readonly ClanAndPeopleService _clanAndPeopleService;
        private readonly IUnionReadRepository _unionReadRepository;
        private readonly IWriteRepository<Repo.Core.Model.Union, Guid> _unionWriteRepository;

        public PeopleController(IPersonReadRepository readModel, IWriteRepository<Repo.Core.Model.Person, Guid> writeModel, ClanAndPeopleService clanAndPeopleService, IUnionReadRepository unionReadRepository, IWriteRepository<Repo.Core.Model.Union, Guid> unionWriteRepository)
        {
            _readModel = readModel;
            _writeModel = writeModel;
            _clanAndPeopleService = clanAndPeopleService;
            _unionReadRepository = unionReadRepository;
            _unionWriteRepository = unionWriteRepository;
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
        public IActionResult Search(SearchPersonModel searchPersonModel)
        {
            var results = _clanAndPeopleService.People.Where(p => (string.IsNullOrWhiteSpace(searchPersonModel.Clan) || p.Clan.Equals(searchPersonModel.Clan, StringComparison.CurrentCultureIgnoreCase))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.FirstName) || p.FirstName.Equals(searchPersonModel.FirstName, StringComparison.CurrentCultureIgnoreCase))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.LastName) || p.LastName.Equals(searchPersonModel.LastName, StringComparison.CurrentCultureIgnoreCase))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.EmailAddress) || p.EmailAddress.Equals(searchPersonModel.EmailAddress, StringComparison.CurrentCultureIgnoreCase))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.Occupation) || p.Occupation.Equals(searchPersonModel.Occupation, StringComparison.CurrentCultureIgnoreCase)));


            return View("List", results);
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
            var p = id != null ? Models.Person.Map(await _readModel.Load(id.Value, new CancellationToken())) : new Models.Person();

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
            var cancellationToken = CancellationToken.None;

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

                                var person = await _writeModel.FindByQuery(identifier, cancellationToken);

                                if (person == null)
                                {
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
                                person.Clan = _clanAndPeopleService.Clans[ReadIntCell(sheet, rowId, PersonImport.Clan) - 1];
                                person.MotherID = GetNullableInt(sheet, PersonImport.MotherId, rowId);
                                person.FatherID = GetNullableInt(sheet, PersonImport.FatherId, rowId);
                                person.Address1 = ReadStringCell(sheet, rowId, PersonImport.Address1);
                                person.Address2 = ReadStringCell(sheet, rowId, PersonImport.Address2);
                                person.Suburb = ReadStringCell(sheet, rowId, PersonImport.Suburb).ToUpper();
                                person.State = ReadStringCell(sheet, rowId, PersonImport.State).ToUpper();
                                person.PostCode = ReadStringCell(sheet, rowId, PersonImport.PostCode);
                                person.Occupation = ReadStringCell(sheet, rowId, PersonImport.Occupation);
                                person.DateOfBirth = ReadNullableDate(sheet, rowId, PersonImport.DOB);
                                person.DateOfDeath = ReadNullableDate(sheet, rowId, PersonImport.DOD);
                                person.Gender = ReadStringCell(sheet, rowId, PersonImport.Gender);

                                await _writeModel.Save(person, cancellationToken);

                                rowId++;

                            }
                        }

                        var spouseSheet = excel.Workbook.Worksheets["Spouses"];
                        if (spouseSheet == null) continue;
                        {
                            var rowId = 2; // sheet has header row!
                            while (spouseSheet.Cells[rowId, 1].Text != string.Empty)
                            {

                                var husband = await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(spouseSheet, rowId, 1) }, 
                                                                                CancellationToken.None);

                                var wife = await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(spouseSheet, rowId, 2) },
                                                                                CancellationToken.None);

                                var union = await _unionWriteRepository.FindByQuery(new UnionQuery
                                {
                                    Partner1Id = ReadIntCell(spouseSheet, rowId, 1),
                                    Partner2Id = ReadIntCell(spouseSheet, rowId, 2)

                                }, cancellationToken);

                                if (union == null)
                                {
                                    union = new Repo.Core.Model.Union()
                                    {
                                        PartnerID = ReadIntCell(spouseSheet, rowId, 1),
                                        Partner2ID = ReadIntCell(spouseSheet, rowId, 2),
                                        ID = Guid.NewGuid()
                                    };
                                }


                                union.MarriageDate = ReadNullableDate(spouseSheet, rowId, 3);
                                union.DivorceDate = ReadNullableDate(spouseSheet, rowId, 4);

                                await _unionWriteRepository.Save(union, cancellationToken);

                                await ProcessDirectChildrenOfUnion(union, husband, wife, cancellationToken);
                                
                                rowId++;
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        private async Task ProcessDirectChildrenOfUnion(Repo.Core.Model.Union union, Repo.Core.Model.Person husband, Repo.Core.Model.Person wife, CancellationToken cancellationToken)
        {
            var children = await _readModel.GetChildrenByParents(union.PartnerID, union.Partner2ID, cancellationToken);
            children.ForEach(async c =>
            {
                c.ParentRelationship = union.ID;
                await _writeModel.Save(c, cancellationToken);
            });
        }

        private List<Descendant> ProcessChildren(int personId, IReadOnlyList<PersonLookup> people, int level)
        {
            var descendants = new List<Descendant>();
            // mothers
            
            if (people.Any(t => t.MotherId.HasValue && t.MotherId == personId))
            {
                people.Where(t => t.MotherId == personId).ToList().ForEach(des => descendants.Add(new Descendant
                {
                    Level = level,
                    PersonID = des.PersonId,
                    Descendants = ProcessChildren(des.PersonId, people, level + 1)
                }));
            }

            // fathers
            if (people.Any(t => t.FatherId.HasValue && t.FatherId == personId))
            {
                people.Where(t => t.FatherId == personId).ToList().ForEach(des => descendants.Add(new Descendant
                {
                    Level = level,
                    PersonID = des.PersonId,
                    Descendants = ProcessChildren(des.PersonId, people, level + 1)
                }));
            }
            

            return descendants;
        }

        private static DateTime? ReadNullableDate(ExcelWorksheet sheet, int rowId, int columnId)
        {
            var cellValue = sheet.Cells[rowId, columnId].Text;

            if (string.IsNullOrEmpty(cellValue)) return null;
            if (DateTime.TryParse(cellValue, out var date))
                return date;
            return null;
        }


        private static DateTime? ReadNullableDate(ExcelWorksheet sheet, int rowId, PersonImport columnId)
        {
            return ReadNullableDate(sheet, rowId, (int)columnId);
        }

            private static int ReadIntCell(ExcelWorksheet sheet, int rowNumber, int columnId)
        {
            var cellValue = sheet.Cells[rowNumber, columnId].Text;
            if (string.IsNullOrEmpty(cellValue)) throw new ArgumentOutOfRangeException($"Cannot convert {columnId} to integer");
            if (int.TryParse(cellValue, out var intValue))
                return intValue;
            throw new ArgumentOutOfRangeException($"Cannot convert {columnId} to integer");
        }

        private static int ReadIntCell(ExcelWorksheet sheet, int rowNumber, PersonImport columnId)
        {
            return ReadIntCell(sheet, rowNumber, columnId.ToInt());
        }

        private static string ReadStringCell(ExcelWorksheet sheet, int rowNumber, PersonImport columnId)
        {
            return sheet.Cells[rowNumber, columnId.ToInt()].Text.Trim();
        }

        private static int? GetNullableInt(ExcelWorksheet sheet, PersonImport columnId, int rowNumber)
        {
            var cellValue = sheet.Cells[rowNumber, columnId.ToInt()].Text;
            if (string.IsNullOrEmpty(cellValue)) return null;
            if (int.TryParse(cellValue, out var intValue))
                return intValue;
            return null;
        }
    }

}
