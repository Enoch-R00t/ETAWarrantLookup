using ETAWarrantLookup.Models;
using ETAWarrantLookup.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ETAWarrantLookup.Controllers
{
    public class WarrantController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WarrantController> _logger;


        public WarrantController(IMemoryCache memoryCache, IConfiguration configuration, ILogger<WarrantController> logger)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _logger = logger;

         }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search()
        {
            WarrantSearchViewModel vm = new WarrantSearchViewModel();

            if(!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Fname")))
            {
                vm.FirstName = HttpContext.Session.GetString("Fname");
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Lname")))
            {
                vm.LastName = HttpContext.Session.GetString("Lname");
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Sex")))
            {
                vm.Sex = HttpContext.Session.GetString("Sex");
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Race")))
            {
                vm.Race = HttpContext.Session.GetString("Race");
            }

            if (HttpContext.Session.GetString("Court") != null)
            {
                vm.Court = HttpContext.Session.GetString("Court");
            }


            vm.CourtList = await GetCourtDropdownValues();
            vm.SexList = await GetSexDropdownValues();
            vm.RaceList = await GetRaceDropdownValues();        

            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SearchResults(WarrantSearchViewModel vm)
        {
            //retain the search criteria
            HttpContext.Session.SetString("Fname", vm.FirstName ?? string.Empty);
            HttpContext.Session.SetString("Lname", vm.LastName ?? string.Empty);
            HttpContext.Session.SetString("Race", vm.Race ?? string.Empty);
            HttpContext.Session.SetString("Sex", vm.Sex ?? string.Empty);
            HttpContext.Session.SetString("Court", vm.Court ?? string.Empty);


            //filter the data based on query
            //IQueryable<WarrantModel> query = GetWarrants().GetAwaiter().GetResult().ToList() as IQueryable<WarrantModel>;

            var query = GetWarrants().GetAwaiter().GetResult();

            if (!string.IsNullOrWhiteSpace(vm.FirstName))
            {
                query = query.Where(m => m.GFNAME.Contains(vm.FirstName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(vm.LastName))
            {
                query = query.Where(m => m.GLNAME.Contains(vm.LastName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(vm.Court))
            {
                query = query.Where(m => m.GNAME == vm.Court);
            }
            if (!string.IsNullOrWhiteSpace(vm.Race))
            {
                query = query.Where(m => m.GRACE == vm.Race);
            }
            if (!string.IsNullOrWhiteSpace(vm.Sex))
            {
                query = query.Where(m => m.GSEX == vm.Sex);
            }

            var results = query.ToList();

            //IEnumerable<WarrantModel> model = new List<WarrantModel>();

            //iterate over the results and build the details url           
            foreach (var warrant in results)
            {

                warrant.GWARRANTDETAILSURL = Url.Content("~/Warrant/Details" + "?c1=" + warrant.GCASE1 + "&c2=" + warrant.GCASE2 + "&c3=" + warrant.GCASE3 + "&offend=" + warrant.GLNAME);
            }

            return PartialView("SearchResults", results);
        }


        //TODO - use the view model for the parameter
        [HttpGet]
        [Authorize]
        public IActionResult Details(string c1, string c2, string c3, string offend)
        {          
            var data = GetWarrantDetails(c1, c2, c3, offend).GetAwaiter().GetResult();

            return View(data);
        }


        // TODO - this needs to take a view model, why not.
        // This also allows the calling page to know what the query was for the back button on details page
        [Authorize]
        private async Task<IEnumerable<WarrantModel>> GetWarrantDetails(string c1, string c2, string c3, string offend)
        {
            var data = GetWarrants().GetAwaiter().GetResult().ToList();

            // this will retrieve the 1 record that matches the query parameters
            var warrants = 
                data.Where(m => m.GCASE1 == c1)
                .Where(m => m.GCASE2 == c2)
                .Where(m => m.GCASE3 == c3)
                .Where(m => m.GLNAME == offend)
                .ToList();

            if (warrants.Count > 0)
            {
                // now we need to see if there are others that match.
                var otherWarrants =
                    data.Where(m => m.GLNAME == warrants.First().GLNAME)
                    .Where(m => m.GFNAME == warrants.First().GFNAME)
                    .Where(m => m.GHGT == warrants.First().GHGT)
                    .Where(m => m.GWHT == warrants.First().GWHT)
                    .Where(m => m.GRACE == warrants.First().GRACE)
                    .Where(m => m.GSEX == warrants.First().GSEX);



                warrants.AddRange(otherWarrants.Where(m => m.GCASE != warrants.First().GCASE));
            }

            return warrants;
        }


        
        /// <summary>
        /// Responsible for retrieving unique court values from the data collection
        /// </summary>
        private async Task<IEnumerable<SelectListItem>> GetCourtDropdownValues()
        {
            var cacheKey = "courtddlValues";

            // check if cache entry exists
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<SelectListItem> courtddlValues))
            {

                var data = await GetWarrants();
                var dataList = data.Select(m => m.GNAME).Distinct().ToList();
                dataList.Sort();

                List<SelectListItem> selectListItems = new();

                foreach (var item in dataList)
                {
                    selectListItems.Add(new SelectListItem(item, item));
                }


                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(5000),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(2000)
                };
                //setting cache entries
                _memoryCache.Set(cacheKey, selectListItems, cacheExpiryOptions);

                return selectListItems;
            }
            else
            {
                return courtddlValues;
            }
        }


        /// <summary>
        /// Responsible for retrieving unique sex values from the data collection
        /// </summary>
        private async Task<IEnumerable<SelectListItem>> GetSexDropdownValues()
        {
            var cacheKey = "sexddlValues";

            // check if cache entry exists
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<SelectListItem> sexddlValues))
            {

                var data = await GetWarrants();
                var dataList = data.Select(m => m.GSEX).Distinct().ToList();
                dataList.Sort();

                List<SelectListItem> selectListItems = new();

                foreach (var item in dataList)
                {
                    selectListItems.Add(new SelectListItem(item, item));
                }

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(5000),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(2000)
                };
                //setting cache entries
                _memoryCache.Set(cacheKey, selectListItems, cacheExpiryOptions);

                return selectListItems;
            }
            else
            {
                return sexddlValues;
            }
        }

        /// <summary>
        /// Responsible for retrieving unique race values from the data collection
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetRaceDropdownValues()
        {
            var cacheKey = "raceddlValues";

            // check if cache entry exists
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<SelectListItem> raceDdlValues))
            {

                var data = await GetWarrants();
                var dataList = data.Select(m => m.GRACE).Distinct().ToList();
                dataList.Sort();

                List<SelectListItem> selectListItems = new();

                foreach (var item in dataList)
                {
                    selectListItems.Add(new SelectListItem(item, item));
                }

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(5000),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(2000)
                };
                //setting cache entries
                _memoryCache.Set(cacheKey, selectListItems, cacheExpiryOptions);

                return selectListItems;
            }
            else
            {
                return raceDdlValues;
            }
        }

        /// <summary>
        /// Responsible for retrieving all the data from the datafile and serializing into a collection of WarrantModel objects
        /// </summary>
        /// <returns></returns>
        // TODO - make this return an IQueryable once the data is in the db
        private async Task<IEnumerable<WarrantModel>> GetWarrants()
        {
            var cacheKey = "warrantList";

            // check if cache entry exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<WarrantModel> warrants))
            {
                DataTable csvData = new DataTable();
                string jsonString = string.Empty;

                // get working directory
                var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());               

                // Get file name and parent directory from config
                var dataFileName = _configuration.GetSection("WarrantDataFile").GetChildren().FirstOrDefault(config => config.Key == "Name").Value;
                var dataFileParentDir = _configuration.GetSection("WarrantDataFile").GetChildren().FirstOrDefault(config => config.Key == "ParentDirectory").Value;

                var dataFilePath = provider.Root + dataFileParentDir + "\\" + dataFileName;

                try
                {
                    using (TextFieldParser csvReader = new TextFieldParser(dataFilePath))
                    {
                        csvReader.SetDelimiters(new string[] { "," });
                        csvReader.HasFieldsEnclosedInQuotes = true;
                        string[] colFields;
                        bool tableCreated = false;
                        while (tableCreated == false)
                        {
                            colFields = csvReader.ReadFields();
                            foreach (string column in colFields)
                            {
                                DataColumn datecolumn = new DataColumn(column);
                                datecolumn.AllowDBNull = true;
                                csvData.Columns.Add(datecolumn);
                            }
                            tableCreated = true;
                        }
                        while (!csvReader.EndOfData)
                        {
                            csvData.Rows.Add(csvReader.ReadFields());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                }
                
                //if everything goes well, serialize csv to json 
                jsonString = JsonConvert.SerializeObject(csvData);


                List<WarrantModel> warrantList = (List<WarrantModel>)JsonConvert.DeserializeObject(jsonString,
                    (typeof(List<WarrantModel>)));


                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(5000),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(2000)
                };

                //setting cache entries
                _memoryCache.Set(cacheKey, warrantList, cacheExpiryOptions);

                return warrantList;
            }
            else
            {
                return warrants;
            }
          
        }
    }
}
