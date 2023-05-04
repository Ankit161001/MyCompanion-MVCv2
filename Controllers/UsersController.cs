using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCompanion.Data;
using MyCompanion.Models;
using MyCompanion.Models.Domain;

namespace MyCompanion.Controllers
{
    public class UsersController : Controller
    {
        private readonly MVCDbContext mvcDbContext;

        public UsersController(MVCDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(AddUserViewModel addUserRequest)
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Name = addUserRequest.Name,
                Email = addUserRequest.Email,
                Phone = addUserRequest.Phone,
                Aadhar = addUserRequest.Aadhar,
                City = addUserRequest.City,
                Pin = addUserRequest.Pin,
                Username = addUserRequest.Username,
                Password = addUserRequest.Password
            };
               
            await mvcDbContext.Users.AddAsync(user);
            await mvcDbContext.SaveChangesAsync();
            var userid = new UserCredId()
            {
                UserId = user.UserId
            };
            /*return RedirectToAction("Choice", new {user.UserId});*/
            return RedirectToAction("Choice", userid);
        }

        [HttpGet]
        public async Task<IActionResult> Choice(UserCredId userCredId)
        {
            var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userCredId.UserId);

            if (user != null)
            {
                var viewModel = new ViewUserViewModel()
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Phone = user.Phone,
                    Email = user.Email,
                    Password = user.Password,
                    Username = user.Username,
                    City = user.City,
                    Pin = user.Pin,
                    Aadhar = user.Aadhar
                };

                return await Task.Run(() => View("Choice", viewModel));
            }

            return RedirectToAction("Signin");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentials userCredentials)
        {
            var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.Username == userCredentials.Username && x.Password == userCredentials.Password);

            if(user != null)
            {
                var userid = new UserCredId()
                {
                    UserId = user.UserId
                };
                
                return RedirectToAction("Choice", userid);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Postjob(Guid id)
        {
            var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (user != null)
            {
                Random r = new Random();
                int rInt = r.Next(1, 9999);
                var viewModel = new ViewUserJobViewModel()
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Phone = user.Phone,
                    Email = user.Email,
                    Password = user.Password,
                    Username = user.Username,
                    City = user.City,
                    Pin = user.Pin,
                    Aadhar = user.Aadhar,
                    JobId = Guid.NewGuid(),
                    Jobrole = "Default",
                    Description = "Default",
                    Duration = 0,
                    Price = 0,
                    Date = DateTime.Now,
                    Status = "Posted",
                    Posterid = user.UserId,
                    Pname = user.Name,
                    Pemail = user.Email,
                    Pphone = user.Phone,
                    Pcity = user.City,
                    Ppin = user.Pin,
                    Accepterid = Guid.NewGuid(),
                    Potp = rInt,
                    Aotp = 0000
                };

                return await Task.Run(() => View("Postjob", viewModel));
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Postjob(ViewUserJobViewModel addJobRequest)
        {
            var job = new Job()
            {
                JobId = Guid.NewGuid(),
                Jobrole = addJobRequest.Jobrole,
                Description = addJobRequest.Description,
                Duration = addJobRequest.Duration,
                Price = addJobRequest.Price,
                Date = addJobRequest.Date,
                Status = addJobRequest.Status,
                Posterid = addJobRequest.Posterid,
                Pname = addJobRequest.Pname,
                Pemail = addJobRequest.Pemail,
                Pphone=addJobRequest.Pphone,
                Pcity=addJobRequest.Pcity,
                Ppin=addJobRequest.Ppin,
                Accepterid = Guid.NewGuid(),
                Aname = "Not Assigned",
                Aemail = "Not Assigned",
                Acity = "Not Assigned",
                Aphone = "Not Assigned",
                Apin = "Not Assigned",
                Potp = addJobRequest.Potp,
                Aotp = addJobRequest.Aotp
            };
            await mvcDbContext.Jobs.AddAsync(job);
            await mvcDbContext.SaveChangesAsync();

            return RedirectToAction("Postjob", job.Posterid);
        }

        [HttpGet]
        public async Task<IActionResult> Searchjob(Guid id)
        {
            var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (user != null)
            {
                var viewModel = new ViewSearchTermUserModel()
                {
                    User = user,
                    Searchterm = "Default"
                };

                return await Task.Run(() => View("Searchjob", viewModel));
            }

            return RedirectToAction("Login");
        }

        /*[HttpPost]
        public IActionResult Searchjob(ViewSearchTermUserModel searchTermUserModel)
        {   
            var passModel = new ViewSearchTermUserModel()
            {
                Searchterm = searchTermUserModel.Searchterm,
                User = searchTermUserModel.User
            };
            return RedirectToAction("Searchresults", passModel);
        }*/

        [HttpPost]
        public async Task<IActionResult> Searchjob(ViewSearchTermUserModel searchTermUserModel)
        {
            var availablejobs = await mvcDbContext.Jobs.Where(x => x.Jobrole.Contains(searchTermUserModel.Searchterm)).ToListAsync();

            var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == searchTermUserModel.User.UserId);

            if (availablejobs != null && user != null)
            {
                var viewModel = new ViewPostedJobsModel()
                {
                    User = searchTermUserModel.User,
                    PostedJobs = availablejobs
                };
                return await Task.Run(() => View("Searchresults", viewModel));
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Acceptjob(Guid jid, Guid aid)
        {
            var acceptor = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == aid);

            var job = await mvcDbContext.Jobs.FirstOrDefaultAsync(x => x.JobId == jid);

            if (acceptor != null && job != null)
            {
                var accjobModel = new ViewUserJobDetailsView()
                {
                    User = acceptor,
                    Job = job
                };

                return await Task.Run(() => View("Acceptjob", accjobModel));
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Acceptjob(ViewUserJobDetailsView acceptJobRequest)
        {
            var job = await mvcDbContext.Jobs.FindAsync(acceptJobRequest.Job.JobId);
            if (job != null)
            {
                job.JobId = acceptJobRequest.Job.JobId;
                job.Jobrole = acceptJobRequest.Job.Jobrole;
                job.Description = acceptJobRequest.Job.Description;
                job.Duration = acceptJobRequest.Job.Duration;
                job.Price = acceptJobRequest.Job.Price;
                job.Date = acceptJobRequest.Job.Date;
                job.Status = "Accepted";
                job.Posterid = acceptJobRequest.Job.Posterid;
                job.Pname = acceptJobRequest.Job.Pname;
                job.Pemail = acceptJobRequest.Job.Pemail;
                job.Pphone = acceptJobRequest.Job.Pphone;
                job.Pcity = acceptJobRequest.Job.Pcity;
                job.Ppin = acceptJobRequest.Job.Ppin;
                job.Accepterid = acceptJobRequest.User.UserId;
                job.Aname = acceptJobRequest.User.Name;
                job.Aemail = acceptJobRequest.User.Email;
                job.Acity = acceptJobRequest.User.City;
                job.Aphone = acceptJobRequest.User.Phone;
                job.Apin = acceptJobRequest.User.Pin;
                job.Potp = acceptJobRequest.Job.Potp;
                job.Aotp = acceptJobRequest.Job.Aotp;

                await mvcDbContext.SaveChangesAsync();

                var jobs = await mvcDbContext.Jobs.Where(x => x.Accepterid.Equals(job.Accepterid)).ToListAsync();

                var jobList = new JobListModel()
                {
                    Jobs = jobs
                };

                return await Task.Run(() => View("Acceptedjob", jobList));
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Acceptedjob(Guid id)
        {
            var jobs = await mvcDbContext.Jobs.Where(x => x.Accepterid.Equals(id)).ToListAsync();

            var jobList = new JobListModel()
            {
                Jobs = jobs
            };

            return await Task.Run(() => View("Acceptedjob", jobList));
        }

        [HttpGet]
        public async Task<IActionResult> Postedjob(Guid id)
        {
            var postedjobs = await mvcDbContext.Jobs.Where(x => x.Posterid.Equals(id)).ToListAsync();

            var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (user != null)
            {
                var viewModel = new ViewPostedJobsModel()
                {
                    User = user,
                    PostedJobs = postedjobs
                };
                return await Task.Run(() => View("Postedjob", viewModel));
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Editjob(Guid id)
        {
            var job = await mvcDbContext.Jobs.FirstOrDefaultAsync(x => x.JobId == id);

            if (job != null)
            {
                return await Task.Run(() => View("Editjob", job));
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Editjob(Job jobEditRequest)
        {
            var job = await mvcDbContext.Jobs.FindAsync(jobEditRequest.JobId);
            if (job != null)
            {
                job.JobId = jobEditRequest.JobId;
                job.Jobrole = jobEditRequest.Jobrole;
                job.Description = jobEditRequest.Description;
                job.Duration = jobEditRequest.Duration;
                job.Price = jobEditRequest.Price;
                job.Date = jobEditRequest.Date;
                job.Status = jobEditRequest.Status;
                job.Posterid = jobEditRequest.Posterid;
                job.Pname = jobEditRequest.Pname;
                job.Pemail = jobEditRequest.Pemail;
                job.Pphone = jobEditRequest.Pphone;
                job.Pcity = jobEditRequest.Pcity;
                job.Ppin = jobEditRequest.Ppin;
                job.Accepterid = Guid.NewGuid();
                job.Aname = "Not Assigned";
                job.Aemail = "Not Assigned";
                job.Acity = "Not Assigned";
                job.Aphone = "Not Assigned";
                job.Apin = "Not Assigned";
                job.Potp = jobEditRequest.Potp;
                job.Aotp = jobEditRequest.Aotp;

                await mvcDbContext.SaveChangesAsync();

                var postedjobs = await mvcDbContext.Jobs.Where(x => x.Posterid.Equals(job.Posterid)).ToListAsync();

                var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == job.Posterid);
                
                if(user != null)
                {
                    var viewModel = new ViewPostedJobsModel()
                    {
                        User = user,
                        PostedJobs = postedjobs
                    };
                    return await Task.Run(() => View("Postedjob", viewModel));
                } 
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Deletejob(Job jobDel)
        {
            var job = await mvcDbContext.Jobs.FindAsync(jobDel.JobId);

            if (job != null)
            {
                mvcDbContext.Jobs.Remove(job);
                await mvcDbContext.SaveChangesAsync();

                var postedjobs = await mvcDbContext.Jobs.Where(x => x.Posterid.Equals(job.Posterid)).ToListAsync();

                var user = await mvcDbContext.Users.FirstOrDefaultAsync(x => x.UserId == job.Posterid);

                if (user != null)
                {
                    var viewModel = new ViewPostedJobsModel()
                    {
                        User = user,
                        PostedJobs = postedjobs
                    };
                    return await Task.Run(() => View("Postedjob", viewModel));
                }
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Completejob(Guid id)
        {
            var job = await mvcDbContext.Jobs.FirstOrDefaultAsync(x => x.JobId == id);

            if (job != null)
            {
                return await Task.Run(() => View("Completejob", job));
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Completejobac(Guid id)
        {
            var job = await mvcDbContext.Jobs.FirstOrDefaultAsync(x => x.JobId == id);

            if (job != null)
            {
                return await Task.Run(() => View("Completejobac", job));
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Completejobac(Job CompletedJob)
        {
            var job = await mvcDbContext.Jobs.FindAsync(CompletedJob.JobId);

            if (job != null)
            {
                var ACotp = CompletedJob.Aotp;
                var POotp = job.Potp;

                if (ACotp == POotp)
                {
                    job.Aotp = ACotp;
                    job.Status = "Completed";
                    await mvcDbContext.SaveChangesAsync();
                    return await Task.Run(() => View("Completedjob"));
                }

                return await Task.Run(() => View("Wrongotp"));
            }

            return RedirectToAction("Login");
        }
    }
}
