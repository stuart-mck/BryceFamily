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
using Microsoft.AspNetCore.Authorization;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using System.Text;

namespace BryceFamily.Web.MVC.Controllers
{

    public class PeopleController : BaseController
    {
        private readonly IWriteRepository<Repo.Core.Model.Person, int> _writeModel;
        private readonly IPersonReadRepository _readModel;
        private readonly ClanAndPeopleService _clanAndPeopleService;
        private readonly IUnionReadRepository _unionReadRepository;
        private readonly IWriteRepository<Repo.Core.Model.Union, Guid> _unionWriteRepository;

        public PeopleController(IPersonReadRepository readModel, IWriteRepository<Repo.Core.Model.Person, int> writeModel, ClanAndPeopleService clanAndPeopleService, IUnionReadRepository unionReadRepository, IWriteRepository<Repo.Core.Model.Union, Guid> unionWriteRepository):base("People", "people")
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

        [HttpGet, Route("EmailUnsubscribe/{personId}")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailUnsubscribe([FromQuery] int personId)
        {
            var cancellationToken = GetCancellationToken();
            var person = _clanAndPeopleService.People.FirstOrDefault(t => t.Id == personId);
            var msg = string.Empty;
            if (person != null)
            {
                person.SubscribeToEmail = false;
                await _writeModel.Save(person.MapToEntity(_clanAndPeopleService), cancellationToken);
                msg = "Thank you, you have successfully unsubscribed and will no longer receive emails from the site.";
            }
            else
            {
                msg = "Hmm - we couldn't find a person with that email.";
            }
            ViewData["response"] = msg;
            return View("EmailSubscribe");
        }

        [HttpGet, Route("EmailSubscribe/{personId}")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailSubscribe([FromQuery] int personId)
        {
            var cancellationToken = GetCancellationToken();
            var person = _clanAndPeopleService.People.FirstOrDefault(t => t.Id == personId);
            var msg = string.Empty;
            if (person != null) {
                person.SubscribeToEmail = true;
                await _writeModel.Save(person.MapToEntity(_clanAndPeopleService), cancellationToken);
                msg = "Thank you, you have successfully subscribed and will now receive emails from the site.";
            }
            else
            {
                msg = "Hmm - we couldn't find a person with that email.";
            }
            ViewData["response"] = msg;
            return View();
        }


        
        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult Search()
        {
            return View(new SearchPersonModel());
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult Search(SearchPersonModel searchPersonModel)
        {
            var clan = searchPersonModel.Clan == 0 ? null : _clanAndPeopleService.Clans.First(c => c.Id == searchPersonModel.Clan);
            var clanSearch = clan == null ? string.Empty : $"{clan.Family}, {clan.FamilyName}";
            
            var results = _clanAndPeopleService.People.Where(p => (searchPersonModel.Clan == 0 || p.ClanId == searchPersonModel.Clan)
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.FirstName) || (p.FirstName != null &&  p.FirstName.Equals(searchPersonModel.FirstName, StringComparison.CurrentCultureIgnoreCase)))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.LastName) || (p.LastName != null && p.LastName.Equals(searchPersonModel.LastName, StringComparison.CurrentCultureIgnoreCase)))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.EmailAddress) || (p.EmailAddress != null && p.EmailAddress.Equals(searchPersonModel.EmailAddress, StringComparison.CurrentCultureIgnoreCase)))
                                                                            && (string.IsNullOrWhiteSpace(searchPersonModel.Occupation) || p.Occupation != null && (p.Occupation.Equals(searchPersonModel.Occupation, StringComparison.CurrentCultureIgnoreCase))));


            return View("List", results);
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public async Task<IActionResult> List(IEnumerable<Models.Person> people)
        {
            return await Task.FromResult(View(people));
        }



        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> Person(Models.Person person)
        {
            await _writeModel.Save(person.MapToEntity(_clanAndPeopleService), new CancellationToken());
            _clanAndPeopleService.ClearPeople();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Email()
        {
            return View(new EmailCreateModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Email(EmailCreateModel email)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult Bluebook()
        {
            var output = new StringBuilder();

            output.AppendLine("Id,Family,First Name,Last Name,Middle Name,Gender,Birth,Mother,Father,Death,Phone,Address1,Address2,City,State,PostCode,Email");
            foreach (var person in _clanAndPeopleService.People)
            {
                output.AppendLine($"{person.Id}," +
                    $"{_clanAndPeopleService.Clans.First(t => t.Id == person.ClanId).Family} - {_clanAndPeopleService.Clans.First(t => t.Id == person.ClanId).FamilyName} ," +
                    $"{person.FirstName}," +
                    $"{person.LastName}," +
                    $"{person.MiddleName}," +
                    $"{person.Gender}," +
                    $"{person.DateOfBirth:dd-MMM-yyyy}," +
                    $"{person.Mother?.FullName}," +
                    $"{person.Father?.FullName}," +
                    $"{person?.DateOfDeath:dd-MMM-yyyy}," +
                    $"{person.Phone}," +
                    $"{person.Address}," +
                    $"{person.Address1}," +
                    $"{person.Suburb}," +
                    $"{person.State}," +
                    $"{person.PostCode}," +
                    $"{person.EmailAddress}");
            }

            byte[] buffer = Encoding.Default.GetBytes(output.ToString());
            return File(buffer, "text/csv", "bluebook.csv");
        }

        [HttpPut, Route("{personId}")]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult Person([FromRoute]Guid personId, [FromBody]PersonWriteModel person)
        {
            return Ok();
        }

        [HttpGet, Route("AddChild/{parent1Id}/{parent2Id}")]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult AddChild([FromRoute]int parent1Id, [FromRoute]int parent2Id)
        {
            var parents = _clanAndPeopleService.People.Where(p => p.Id == parent1Id || p.Id == parent2Id);

            var model = new NewChildModel(
                parents.First(t => t.Gender.Equals("f", StringComparison.CurrentCultureIgnoreCase)),
                parents.First(t => t.Gender.Equals("m", StringComparison.CurrentCultureIgnoreCase))); 
            return View("NewChild", model);
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> NewChild(NewChildModel model)
        {
            var cancellationToken = GetCancellationToken();
            var parent1 = _clanAndPeopleService.People.FirstOrDefault(t => t.Id == model.Parent1);
            var parent2 = _clanAndPeopleService.People.FirstOrDefault(t => t.Id == model.Parent2);
            var newId = _clanAndPeopleService.People.Max(t => t.Id) + 1;

            var union = (await _unionReadRepository.GetAllUnions(cancellationToken)).FirstOrDefault(t => (t.PartnerID == model.Parent1 && t.Partner2ID == model.Parent2) || (t.PartnerID == model.Parent2 && t.Partner2ID == model.Parent1));

            await _writeModel.Save(new Repo.Core.Model.Person()
            {
                ID = newId,
                ClandId = parent1.ClanId,
                DateOfBirth = model.DateOfBirth,
                FatherID = model.Parent1,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName.ToUpper(),
                Gender = model.Gender,
                IsClanManager = false,
                IsSpouse = false,
                MotherID = model.Parent2,
                ParentRelationship = union.ID,
                SubscribeToEmail = false
            }, cancellationToken);

            _clanAndPeopleService.ClearPeople();
            return RedirectToAction("Person", new { id = parent1.Id });
        }


        [HttpGet, Route("Relationship/{partner1}/{partner2}")]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult Relationship([FromRoute]int partner1, [FromRoute]int partner2)
        {
            var person = _clanAndPeopleService.People.First(t => t.Id == partner1);
            var relationship = person.Unions.FirstOrDefault(t => t.Partner1.Id == partner2 || t.Partner2.Id == partner2);
            return View(relationship);
        }

        [HttpGet, Route("NewRelationship/{id}")]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult NewRelationship(int id)
        {
            var person = _clanAndPeopleService.People.FirstOrDefault(t => t.Id == id);
            return View(new NewUnionPersonModel(person));
        }

        [HttpPost, Route("NewRelationship/{id}")]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> NewRelationship(NewUnionPersonModel model)
        {
            var cancellationToken = GetCancellationToken();
            var person = _clanAndPeopleService.People.FirstOrDefault(t => t.Id == model.PartnerId);
            var newId = _clanAndPeopleService.People.Max(t => t.Id) + 1;

            await _writeModel.Save(new Repo.Core.Model.Person()
            {
                ID = newId,
                ClandId = person.ClanId,
                DateOfBirth = model.DateOfBirth,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                IsClanManager = false,
                IsSpouse = true,
                MaidenName = model.MaidenName,
                Occupation = model.Occupation,
                Phone = model.PhoneNumber
            }, cancellationToken);


            await _unionWriteRepository.Save(new Repo.Core.Model.Union()
            {
                MarriageDate = model.DateOfUnion,
                PartnerID = model.PartnerId,
                Partner2ID = newId
            }, cancellationToken);

            _clanAndPeopleService.ClearPeople();

            return RedirectToAction("Person", model.PartnerId);

        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult Person(int id)
        {
            return View(_clanAndPeopleService.People.FirstOrDefault(p => p.Id == id));
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.SuperAdminRole)]
        public IActionResult Import()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.SuperAdminRole)]
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
                                        ID = ReadIntCell(sheet, rowId, PersonImport.ID)
                                    };
                                }

                                person.FirstName = ReadStringCell(sheet, rowId, PersonImport.FirstName).ToTitleCase();
                                person.LastName = ReadStringCell(sheet, rowId, PersonImport.LastName).ToUpper();
                                person.EmailAddress = ReadStringCell(sheet, rowId, PersonImport.EmailAddress);
                                person.MiddleName = ReadStringCell(sheet, rowId, PersonImport.MiddleName).ToTitleCase();
                                person.MaidenName = ReadStringCell(sheet, rowId, PersonImport.MaindenName).ToUpper();
                                person.Phone = ReadStringCell(sheet, rowId, PersonImport.PhoneNumber);
                                person.ClandId = ReadIntCell(sheet, rowId, PersonImport.Clan);
                                person.IsSpouse = ReadStringCell(sheet, rowId, PersonImport.IsSpouse).Equals("y", StringComparison.CurrentCultureIgnoreCase);
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
                                person.IsClanManager = false;
                                person.LastUpdated = DateTime.Now;
                                person.SubscribeToEmail = !string.IsNullOrEmpty(person.EmailAddress);

                                try
                                {
                                    await _writeModel.Save(person, cancellationToken);
                                }
                                catch (Exception ex)
                                {

                                }

                                rowId++;

                            }

                            //do unions from pointers to parents
                            var parentChildRowId = 2;
                            while (sheet.Cells[parentChildRowId, 1].Text != string.Empty)
                            {
                                var father = string.IsNullOrWhiteSpace(sheet.Cells[parentChildRowId, 12].Text) ? null : await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(sheet, parentChildRowId, PersonImport.FatherId) },
                                                                                cancellationToken);
                                var mother = string.IsNullOrWhiteSpace(sheet.Cells[parentChildRowId, 11].Text) ? null : await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(sheet, parentChildRowId, PersonImport.MotherId) },
                                                                                cancellationToken);

                                if (father != null || mother != null) // we have a parent / parents
                                {
                                    //is there a union?
                                    var thisUnion = await _unionWriteRepository.FindByQuery(new UnionQuery
                                    {
                                        Partner1Id = father?.ID,
                                        Partner2Id = mother?.ID

                                    }, cancellationToken);
                                    if (thisUnion == null)
                                    {
                                        thisUnion = await _unionWriteRepository.FindByQuery(new UnionQuery
                                        {
                                            Partner1Id = mother?.ID,
                                            Partner2Id = father?.ID

                                        }, cancellationToken);
                                    }
                                    if (thisUnion == null)
                                    {
                                        thisUnion = new Repo.Core.Model.Union()
                                        {
                                            PartnerID = father?.ID,
                                            Partner2ID = mother?.ID,
                                            ID = Guid.NewGuid()
                                        };
                                        await _unionWriteRepository.Save(thisUnion, cancellationToken);
                                    }
                                    var child = await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(sheet, parentChildRowId, PersonImport.ID) },
                                                                                cancellationToken);
                                    child.ParentRelationship = thisUnion.ID;
                                    await _writeModel.Save(child, cancellationToken);
                                }
                                parentChildRowId++;
                            }
                        }

                        var spouseSheet = excel.Workbook.Worksheets["Spouses"];
                        if (spouseSheet == null) continue;
                        {
                            var rowId = 2; // sheet has header row!
                            while (spouseSheet.Cells[rowId, 1].Text != string.Empty)
                            {

                                var clanMember = await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(spouseSheet, rowId, 1) },
                                                                                cancellationToken);

                                var spouse = await _writeModel.FindByQuery(new PersonIdentifier { PersonId = ReadIntCell(spouseSheet, rowId, 2) },
                                                                                cancellationToken);

                                var union = await _unionWriteRepository.FindByQuery(new UnionQuery
                                {
                                    Partner1Id = ReadIntCell(spouseSheet, rowId, 1),
                                    Partner2Id = ReadIntCell(spouseSheet, rowId, 2)

                                }, cancellationToken);

                                if (union == null)
                                {
                                    //is there one going the other way?
                                    union = await _unionWriteRepository.FindByQuery(new UnionQuery
                                    {
                                        Partner1Id = ReadIntCell(spouseSheet, rowId, 2),
                                        Partner2Id = ReadIntCell(spouseSheet, rowId, 1)

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
                                }


                                union.MarriageDate = ReadNullableDate(spouseSheet, rowId, 3);
                                union.DivorceDate = ReadNullableDate(spouseSheet, rowId, 4);
                                union.IsEnded = string.IsNullOrEmpty(ReadStringCell(spouseSheet, rowId, 5)) ? false : true;

                                await _unionWriteRepository.Save(union, cancellationToken);
                                
                                rowId++;
                            }
                        }
                    }
                }
                _clanAndPeopleService.ClearPeople();
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

            var children2 = await _readModel.GetChildrenByParents(union.Partner2ID, union.PartnerID, cancellationToken);
            children2.ForEach(async c =>
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
            if (string.IsNullOrEmpty(cellValue)) throw new ArgumentOutOfRangeException($"Cannot convert columnId [{columnId}] to integer as it is null from rowId of {rowNumber} and sheet {sheet.Name}");
            if (int.TryParse(cellValue, out var intValue))
                return intValue;
            throw new ArgumentOutOfRangeException($"Cannot convert columnId [{columnId}] to integer with supplied value of {cellValue} from rowId of {rowNumber} and sheet {sheet.Name}");
        }

        private static int ReadIntCell(ExcelWorksheet sheet, int rowNumber, PersonImport columnId)
        {
            return ReadIntCell(sheet, rowNumber, columnId.ToInt());
        }

        private static string ReadStringCell(ExcelWorksheet sheet, int rowNumber, PersonImport columnId)
        {
            return sheet.Cells[rowNumber, columnId.ToInt()].Text.Trim();
        }

        private static string ReadStringCell(ExcelWorksheet sheet, int rowNumber, int columnId)
        {
            return sheet.Cells[rowNumber, columnId].Text.Trim();
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
