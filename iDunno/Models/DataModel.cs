using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;

namespace iDunno.Models
{
    public class UserDoesNotExist:ValidationAttribute
    {
        public UserDoesNotExist()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            System.Threading.SynchronizationContext.SetSynchronizationContext(null);
            iDunnoDB db = new iDunnoDB();
            var tsktsktsktsktsktsktsktsktsktsk = db.LookupUser(value as string);
            tsktsktsktsktsktsktsktsktsktsk.Wait();
            return tsktsktsktsktsktsktsktsktsktsk.Result == null ? ValidationResult.Success : new ValidationResult("The specified username already exists.");
        }
    }

    public class ProductStatistics
    {
        public ProductStatistics()
        {

        }
        public BsonObjectId Id { get; set; }
        public string ProductID { get; set; }
        public long ViewCount { get; set; }
    }


    public class HomeScreen
    {
        public IEnumerable<TargetItem> UserItems {
            get
            {
                if(CurrentUser == null)
                {
                    return new TargetItem[0];
                }
                TargetAPI api = new TargetAPI();
                List<Task<dynamic>> trios = new List<Task<dynamic>>();
                if (CurrentUser.RecentClicks == null)
                {
                    CurrentUser.RecentClicks = new List<UserClickRecent>();
                }
                foreach(var et in CurrentUser.RecentClicks)
                {
                    trios.Add(api.FetchAsync(et.ProductID));
                }
                Task.WaitAll(trios.ToArray());
                return trios.Select(m => new TargetItem(m.Result));
            }
        } 
        public IEnumerable<TargetItem> Items { get; set; }
        public string SearchQuery { get; set; }
        public string QueryID { get; set; }
        public UserInformation CurrentUser { get; set; }
        public IEnumerable<ProductStatistics> PopularItems { get; set; }
        public IEnumerable<TargetItem> PopularItemsView
        {
            get
            {
                TargetAPI api = new TargetAPI();
                List<Task<dynamic>> quartets = new List<Task<dynamic>>();
                foreach(string et in PopularItems.Select(m=>m.ProductID))
                {
                    quartets.Add(api.FetchAsync(et));
                }
                Task.WaitAll(quartets.ToArray());
                return quartets.Select(m => new TargetItem(m.Result));
                
            }
        }
        public HomeScreen()
        {
            Items = new List<TargetItem>();
        }
    }



    //MongoDB command line -- mongod.exe --replSet idnSet
    [UserNameAndPasswordIsValid]
    public class LoginScreen
    {
        [Required]
        [Display(Name ="User name")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
    public class UserNameAndPasswordIsValid:ValidationAttribute
    {
        public UserNameAndPasswordIsValid()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            dynamic quartet = validationContext.ObjectInstance;
            string username = quartet.UserName;
            string password = quartet.Password;
            System.Threading.SynchronizationContext.SetSynchronizationContext(null);
            var tsktsktsktsk = new iDunnoDB().IsLoginValid(username, password);
            tsktsktsktsk.Wait();
            return tsktsktsktsk.Result ? ValidationResult.Success : new ValidationResult("The user name or password is \"incorrect\"");

        }
    }
    public class PasswordAndConfirmPasswordMatch:ValidationAttribute
    {
        public PasswordAndConfirmPasswordMatch()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //value is password field
            dynamic duo = validationContext.ObjectInstance;
            string confirmPassword = duo.ConfirmPassword.ToString();
            return confirmPassword == value as string ? ValidationResult.Success : new ValidationResult("Password and confirm password don't match.");
        }
    }
    public class RegistrationScreen
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [UserDoesNotExist]
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [PasswordAndConfirmPasswordMatch]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }



    }

    public class RegistrationScreenPt2
    {
        [Display (Name = "Age")]
        public int Age { get; set; }
        [Display (Name = "Gender")]
        public string Gender { get; set; }
        [Display (Name = "Christmas")]
        public bool ? interest_christmas { get; set; }
        [Display (Name = "Halloween")]
        public bool ? interest_halloween { get; set; }
        [Display (Name = "Accessories")]
        public bool ? interest_accessories { get; set; }
        [Display (Name = "Baby")]
        public bool ? interest_baby { get; set; }
        [Display (Name = "Beauty")]
        public bool ? interest_beauty { get; set; }
        [Display (Name = "Clothing")]
        public bool ? inerest_clothing { get; set; }
        [Display (Name = "Electronics")]
        public bool ? interest_electronics { get; set; }
        [Display (Name = "Entertainment")]
        public bool ? interest_entertainment { get; set; }
        [Display (Name = "Furniture")]
        public bool ? interest_furniture { get; set; }
        [Display (Name = "Grocery & Essentials")]
        public bool ? interest_grocery_and_essentials { get; set; }
        [Display (Name = "Health")]
        public bool ? interest_health { get; set; }
        [Display (Name = "Home")]
        public bool ? interest_home { get; set; }
        [Display (Name = "Household Essentials")]
        public bool ? interest_household_essentials { get; set; }
        [Display (Name = "Party Supplies")]
        public bool ? interest_party_supplies { get; set; }
        [Display (Name = "Patio & Garden")]
        public bool ? interest_patio_and_garden { get; set; }
        [Display (Name = "Pets")]
        public bool ? interest_pets { get; set; }
        [Display (Name = "Shoes")]
        public bool ? interest_shoes { get; set; }
        [Display (Name = "Toys")]
        public bool ? interest_toys { get; set; }
        [Display (Name = "Video Games")]
        public bool ? interest_video_games { get; set; }
    }


    public class UserInformation
    {
        public BsonObjectId Id { get; set; }
        public string Username { get; set; }        
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IpAddress { get; set; }
        public DateTime LastAccessTime { get; set; }
        public List<UserClickRecent> RecentClicks { get; set; }

    }
    public class UserClickRecent
    {
        public string ProductID { get; set; }
        public long TimesClicked { get; set; }
        public DateTime MostRecentClick { get; set; }
    }
    public class ProductSearch
    {
        public BsonObjectId Id { get; set; }
        public string SearchText { get; set; }
        public DateTime Time { get; set; }
        public BsonObjectId User { get; set; }

    }
    public class ProductClick
    {
        public BsonObjectId Id { get; set; }
        public BsonObjectId ProductSearch { get; set; }
        public DateTime Time { get; set; }
        public string ProductID { get; set; }
    }
    public class SessionInformation
    {
        public BsonObjectId Id { get; set; }
        public string SessionID { get; set; }
        public BsonObjectId User { get; set; }

    }

    public class iDunnoDB
    {
        public async Task<UserInformation> GetUserById(BsonObjectId id)
        {
            return (await db.GetCollection<UserInformation>("users").Find(Builders<UserInformation>.Filter.Eq(m => m.Id, id)).ToListAsync()).First();
        }
        public async Task<IEnumerable<ProductStatistics>> GetTopProducts()
        {
            var query = await db.GetCollection<ProductStatistics>("statistics").Find(Builders<ProductStatistics>.Filter.Exists(m => m.ProductID)).SortByDescending(m=>m.ViewCount).Limit(5).ToListAsync();
            return query;
        }

        public async Task<ProductStatistics> GetProductStatistics(string id)
        {
            var results = await db.GetCollection<ProductStatistics>("statistics").Find(Builders<ProductStatistics>.Filter.Eq(m => m.ProductID, id)).ToListAsync();
            if(results.Any())
            {
                return results.First();
            }else
            {
                var ps = new ProductStatistics();
                ps.ProductID = id;
                await db.GetCollection<ProductStatistics>("statistics").InsertOneAsync(ps);
                return ps;
            }
        }
        public async Task<UserInformation> LookupUser(string name)
        {
            return (await db.GetCollection<UserInformation>("users").Find(Builders<UserInformation>.Filter.Eq(m => m.Username, name)).ToListAsync()).FirstOrDefault();
        }
        public async Task LogClick(string queryID, string productID)
        {
            var session = await getCurrentSession();
            ProductClick click = new ProductClick();
            if (queryID != null)
            {
                click.ProductSearch = new BsonObjectId(new ObjectId(queryID));
            }
            click.Time = DateTime.UtcNow;
            click.ProductID = productID;
            //Update product statistics
            var stats = await GetProductStatistics(productID);
            await db.GetCollection<ProductStatistics>("statistics").UpdateOneAsync(Builders<ProductStatistics>.Filter.Eq(m=>m.ProductID,productID),Builders<ProductStatistics>.Update.Inc(m=>m.ViewCount,1));
            await db.GetCollection<ProductClick>("clicks").InsertOneAsync(click);

            var profile = (await db.GetCollection<UserInformation>("users").Find(Builders<UserInformation>.Filter.Eq(m=>m.Id,session.User)).ToListAsync()).First();
            if(profile.RecentClicks == null)
            {
                profile.RecentClicks = new List<UserClickRecent>();
            }
            if (profile.RecentClicks.Where(m => m.ProductID == productID).Any())
            {
                profile.RecentClicks.Where(m => m.ProductID == productID).First().TimesClicked++;
            }
            else
            {
                if (profile.RecentClicks.Count == 5)
                {
                    profile.RecentClicks.Remove(profile.RecentClicks.OrderBy(m => m.TimesClicked).First());

                }
                profile.RecentClicks.Add(new UserClickRecent() { MostRecentClick = DateTime.UtcNow, ProductID = productID, TimesClicked = 1 });

            }
            

            await db.GetCollection<UserInformation>("users").UpdateOneAsync(Builders<UserInformation>.Filter.Eq(m => m.Id, profile.Id), Builders<UserInformation>.Update.Set(m => m.RecentClicks, profile.RecentClicks));


        }
        public async Task<ProductSearch> LogSearch(string query)
        {
            SessionInformation session = await getCurrentSession();
            ProductSearch search = new ProductSearch();
            search.SearchText = query;
            search.Time = DateTime.UtcNow;
            search.User = session.User;
            await db.GetCollection<ProductSearch>("searches").InsertOneAsync(search);
            return search;
        }

        IMongoDatabase db;
        public async Task<bool> IsLoginValid(string username, string password)
        {
           var users =  await (await db.GetCollection<UserInformation>("users").FindAsync(Builders<UserInformation>.Filter.Eq(m => m.Username, username))).ToListAsync();
            if(!users.Any())
            {
                return false;
            }
            var user = users.First();
            using (Rfc2898DeriveBytes mDerive = new Rfc2898DeriveBytes(password, user.Salt))
            {
                byte[] hash = mDerive.GetBytes(32);
                for(int i = 0;i<hash.Length;i++)
                {
                    if(hash[i] != user.Password[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public async Task CreateUser(RegistrationScreen screen)
        {
            
            using (RandomNumberGenerator mrand = RandomNumberGenerator.Create()) {
                byte[] salt = new byte[32];
                mrand.GetBytes(salt);
                using (Rfc2898DeriveBytes mderive = new Rfc2898DeriveBytes(screen.Password,salt))
                {
                    byte[] hash = mderive.GetBytes(32);
                    UserInformation info = new UserInformation() { FirstName = screen.FirstName, LastName = screen.LastName, IpAddress = HttpContext.Current.Request.UserHostName, LastAccessTime = DateTime.Now };
                    info.Password = hash;
                    info.Salt = salt;
                    await db.GetCollection<UserInformation>("users").InsertOneAsync(info);
                }
            }

           
        }


        public async Task<SessionInformation> getCurrentSession()
        {
            var request = System.Web.HttpContext.Current.Request;
            var response = System.Web.HttpContext.Current.Response;
            if(request.Cookies["session"] == null)
            {
                //No session cookie
                var thisSessionInformation = new SessionInformation();
                thisSessionInformation.SessionID = Guid.NewGuid().ToString();
                response.AppendCookie(new HttpCookie("session", thisSessionInformation.SessionID));
                //TODO: Create new user
                UserInformation info = new UserInformation();
                await db.GetCollection<UserInformation>("users").InsertOneAsync(info);
                
                thisSessionInformation.User = info.Id;
                await db.GetCollection<SessionInformation>("sessions").InsertOneAsync(thisSessionInformation);
                
                return thisSessionInformation;
            }

            return (await (await db.GetCollection<SessionInformation>("sessions").FindAsync(Builders<SessionInformation>.Filter.Eq(m => m.SessionID, request.Cookies["session"].Value))).ToListAsync()).First();


        }

        public iDunnoDB()
        {
            string host = Dns.GetHostAddresses("db-0.cloudapp.net").First().ToString();
            TcpClient mclient = new TcpClient();

            //MongoDB.Driver.MongoClient client = new MongoDB.Driver.MongoClient(new MongoDB.Driver.MongoClientSettings() {  Servers = new MongoServerAddress[] { new MongoServerAddress(host,27019) } }); //Cloud
            MongoDB.Driver.MongoClient client = new MongoDB.Driver.MongoClient(new MongoDB.Driver.MongoClientSettings() {  Servers = new MongoServerAddress[] { new MongoServerAddress("127.0.0.1") } }); //Local testing

            db = client.GetDatabase("iDunno");

        }

    }
}