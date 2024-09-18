using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using OfficeOpenXml;
using RepositoryContracts;
using Service.Helpers;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Globalization;

namespace Service
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly IPersonsRepository _personsRepository;
        // private readonly ICountryService _countryService;
        // private readonly IDiagnosticContext _diagnosticContext;

        public PersonService()
        {
            
        }

        //public PersonService(IDiagnosticContext diagnosticContext)
        //{
        //    _diagnosticContext = diagnosticContext;
        //}

        public PersonService(IPersonsRepository personsRepository, bool initialize = false)
        {
            _persons = new List<Person>(); 
            _personsRepository = personsRepository;
            //_countryService = new CountryService();
            // _countryService = countryService;

            if (initialize)
            {
                _persons.AddRange(new List<Person>()
                {
                    new Person
                    {
                        PersonID = Guid.NewGuid(),
                        PersonName = "Adams",
                        Email = "adams@email.com",
                        DateOfBirth = DateTime.Parse("01/02/1998"),
                        CountryID = Guid.Parse("23B7D838-B045-47A3-ACCB-1192A0F88FE7"),
                        Gender = GenderOptions.Male.ToString(),
                        ReceiveNewsLetters = true,
                    },
                    new Person
                    {
                        PersonID = Guid.NewGuid(),
                        PersonName = "John",
                        Email = "john@email.com",
                        DateOfBirth = DateTime.Parse("01/02/1997"),
                        CountryID = Guid.Parse("630BBDF6-B703-439E-9CBD-6020045BBA8C"),
                        Gender = GenderOptions.Male.ToString(),
                        ReceiveNewsLetters = true,
                    },
                    new Person
                    {
                        PersonID = Guid.NewGuid(),
                        PersonName = "Leva",
                        Email = "Leva@email.com",
                        DateOfBirth = DateTime.Parse("01/02/1996"),
                        CountryID = Guid.Parse("23B7D838-B045-47A3-ACCB-1192A0F88FE7"),
                        Gender = GenderOptions.Female.ToString(),
                        ReceiveNewsLetters = true,
                    },
                    new Person
                    {
                        PersonID = Guid.NewGuid(),
                        PersonName = "Matt",
                        Email = "matt@email.com",
                        DateOfBirth = DateTime.Parse("01/02/1999"),
                        CountryID = Guid.Parse("F62A9B10-9DF9-44CC-9E11-CBCF5DBA6813"),
                        Gender = GenderOptions.Female.ToString(),
                        ReceiveNewsLetters = false,
                    },
                    new Person
                    {
                        PersonID = Guid.NewGuid(),
                        PersonName = "Billy",
                        Email = "billy@email.com",
                        DateOfBirth = DateTime.Parse("01/02/1994"),
                        CountryID = Guid.Parse("8C3C3838-7BE1-454B-8155-775526351802"),
                        Gender = GenderOptions.Male.ToString(),
                        ReceiveNewsLetters = false,
                    },
                    new Person
                    {
                        PersonID = Guid.NewGuid(),
                        PersonName = "Bob",
                        Email = "bob@email.com",
                        DateOfBirth = DateTime.Parse("01/01/1994"),
                        CountryID = Guid.Parse("2300968F-EEA0-4E45-877D-F1C2845A5BDF"),
                        Gender = GenderOptions.Male.ToString(),
                        ReceiveNewsLetters = true,
                    }
                });
            }
        }


        // Private method to use in same class
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResposne = person.ToPersonResponse();
            //personResposne.Country = _countryService.GetCountryByCountryID(personResposne.CountryID)?.CountryName;
            personResposne.Country = "";
            //? if value is not null then only access CountryName prpert else personResponse.Country will remian null
            return personResposne;
        }


        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            // check if PersonAddRequest is not null
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));


            // Validate PersonName
            if (string.IsNullOrEmpty(personAddRequest.PersonName))
                throw new ArgumentException("Person name cannot be blank");

            //Model Validations, from custom created helper class function
            ValidationHelper.ModelValidation(personAddRequest);

            // Convert PersonAddRequest into Person type to insert value
            Person person = personAddRequest.ToPerson();

            // generate new PersonID & insert
            person.PersonID = Guid.NewGuid();

            // add person obbject to person list
            await _personsRepository.AddPerson(person);

            // using Sp
            // _db.sp_InsertPerson(person);


            // return response with CountryName
            // return ConvertPersonToPersonResponse(person);
            return person.ToPersonResponse();
        }


        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //List<PersonResponse> person = _persons.Select(x => x.ToPersonResponse()).ToList();
            // To Load Country Name - Method ConvertPersonToPersonResponse(person) , chk def. imp.

            // below cause projection error bcoz we are using own method in linq which is not allowed to prevent memeory leaks
            //List<PersonResponse> person = _db.Persons.Select(x => ConvertPersonToPersonResponse(x)).ToList();

            // Better way
            // Select * From Persons, data has been loaded from database into our memory, after that we are free to call any user defined methods

            // Iclude relation table data 
            // Here Country is not Modal or Class name, its name of Navigation Property from Person.cs
            var personns = await _personsRepository.GetAllPersons();

            List<PersonResponse> person = personns.ToList()
                .Select(x => ConvertPersonToPersonResponse(x)).ToList();

            // sp_GetAllPersons() is SP
            // List<PersonResponse> persons = _personsRepository.sp_GetAllPersons()
               // .Select(x => x.ToPersonResponse()).ToList();

            return person;
        }


        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if(personID == Guid.Empty)
                return null;

            //Person? person = await _personsRepository.Persons.Include("Country")
            //    .FirstOrDefaultAsync(temp => temp.PersonID == personID);

            Person? person = await _personsRepository.GetPersonByPersonId(personID.Value);

            // To Get Country Name
            // string countryName = person.Country.CountryName;

            if (person == null)
                return null;

            return person.ToPersonResponse();
        }


        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            // can be improved for larger data sets
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
                return matchingPersons;

            switch(searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp => 
                    // if PersonName is not null
                    (!string.IsNullOrEmpty(temp.PersonName)?
                    // if PersonName contains searchString with case insensitive then return else true
                    temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp =>
                    // if PersonName is not null
                    (!string.IsNullOrEmpty(temp.Email)?
                    // if Email contains searchString with case insensitive then return else true
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:
                    matchingPersons = allPersons;
                    break;
            }

            // _diagnosticContext.Set("Persons", matchingPersons);
            return matchingPersons;
        }


        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortorderOptions)
        {
            if(string.IsNullOrEmpty(sortBy))
                return allPersons;

            // using swith expression
            List<PersonResponse> sortedPersons =
                (sortBy, sortorderOptions) switch
                {
                    // ASC
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    // DESC
                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    // Email ASC
                    (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    // Email DESC
                    (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    // more cases we can write i.e. DOB etc. 

                    // Default case 
                    _ => allPersons
                };

            return sortedPersons;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            // validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            // getMatchingPersonObject to update
            Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonID);
            if(matchingPerson == null)
            {
                throw new InvalidPersonIDException("Given person Id does not exist");
            }

            // Update All Details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _personsRepository.UpdatePerson(matchingPerson);

            //return matchingPerson.ToPersonResponse();
            return ConvertPersonToPersonResponse(matchingPerson);
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if(personID == null)
            {
                throw new ArgumentNullException();
            }

            //Person? person = await _personsRepository.Persons.FirstOrDefaultAsync(temp => temp.PersonID == personID);
            Person? person = await _personsRepository.GetPersonByPersonId(personID.Value);
            if (person == null)
            {
                return false;
            }

            await _personsRepository.DeletePersonByPersonID(personID.Value);
            return true;
        }


        public async Task<MemoryStream> GetPersonCSV()
        {
            // Step 1 - Imp watch vid
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            // Step 2
            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>(); // it writes header automatically i.e PersonID, PersonName etc
            csvWriter.NextRecord(); // move cursor to nextline

            //List<PersonResponse> persons = _personsRepository.Persons
            //    .Include("Country")
            //    .Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> persons = await GetAllPersons();

            await csvWriter.WriteRecordsAsync(persons);
            // 1, abc ....

            // after writing all info .. the cursor of MS is at end, so we need to move its position to 0 to gat all gathered info. 
            memoryStream.Position = 0;
            return memoryStream;

        }

        public async Task<MemoryStream> GetPersonCSVCustomFields()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);


            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);


            // PersonName, Email
            //csvWriter.WriteField(nameof(PersonResponse.PersonName));
            //csvWriter.WriteField(nameof(PersonResponse.Email));
            //csvWriter.WriteField(nameof(PersonResponse.Country));
            //csvWriter.NextRecord();


            //List<PersonResponse> persons = _personsRepository.Persons
            //    .Include("Country")
            //    .Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> persons = await GetAllPersons();

            foreach (PersonResponse person in persons) 
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                csvWriter.WriteField(person.Country);

                csvWriter.NextRecord();
                csvWriter.Flush();
            }
            
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> GetPersonExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            
            using(ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("SheetName");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date of Birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Receive Newsletter";

                // Go to EPPLUS documentation more advanced usage
                // Optional - Formatting Cells Heading
                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    // Style Type
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    // Style Color
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                    // Font Style
                    headerCells.Style.Font.Bold = true;
                }
                // End

                int row = 2;
                //List<PersonResponse> persons = _personsRepository.Persons.Include("Country").Select(person => person.ToPersonResponse()).ToList();

                List<PersonResponse> persons = await GetAllPersons();

                foreach (PersonResponse person in persons)
                {
                    //worksheet.Cells["A2"] = person.PersonName;
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    worksheet.Cells[row, 3].Value = person.DateOfBirth;
                    worksheet.Cells[row, 4].Value = person.Age;
                    worksheet.Cells[row, 5].Value = person.Gender;
                    worksheet.Cells[row, 6].Value = person.Country;
                    worksheet.Cells[row, 7].Value = person.ReceivesNewsLetter;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();

            }// auto dispose method get call to release memory resources.

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}