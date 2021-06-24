using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class LibraryController : Controller
    {
        public ILogger<LibraryController> _logger;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        

        public LibraryController(ILogger<LibraryController> logger , UserManager<User> userManager
            ,SignInManager<User> signInManager ,RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser ru/*, ReaderDetails rd*/)
        {
            if (ModelState.IsValid)
            {
                var Iuser = new User()
                {
                    Email = ru.Email,
                    EmailConfirmed = true,
                    UserName = ru.UserName,
                    IsApproved = false
                };

                var resp = await userManager.CreateAsync(Iuser, ru.Password);
                if (resp.Succeeded)
                {
                 await userManager.AddToRoleAsync(Iuser, "reader");
                }
                return RedirectToAction("Login");
                }
            return View(ru);
            }
        
          
            /*var readerDetails = new ReaderDetails()
            {
            };*/
            // ;
            // roleManager.role

            //return View();

        public IActionResult Login()
        {
            ViewBag.ErrorMessage = "";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Login lm)
        {

            var user = await userManager.FindByEmailAsync(lm.Email);
            ViewBag.ErrorMessage = "username passwod invalid";
            if (user != null)
            {
                var isvalid = await userManager.CheckPasswordAsync(user, lm.Password);
                if (user.IsApproved)
                {
                    if (isvalid)
                    {
                        await signInManager.SignInAsync(user, true);
                        return RedirectToAction("index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("NotApproved");
                }
               
            }
            return View();
        }

        public IActionResult NotApproved()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole(CreateRole cr)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = cr.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ViewRolesList", "Library");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(cr);
        }

        public IActionResult ViewRolesList()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);

            return RedirectToAction("ViewRolesList");
        }

        public async Task<IActionResult> EditRoles(string id)
        {
            // Find the role by Role ID
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("DataNotFound");
            }

            var model = new ManageRole
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Retrieve all the Users
            foreach (var user in userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRoles(ManageRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null) 
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("PageNotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ViewRolesList");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        public async Task<IActionResult> AssignRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<AssignRoleModel>();

            foreach (var user in userManager.Users)
            {
                var assignRoleModel = new AssignRoleModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    assignRoleModel.IsSelected = true;
                }
                else
                {
                    assignRoleModel.IsSelected = false;
                }

                model.Add(assignRoleModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<AssignRoleModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if ((role.Name).Equals("admin") || (role.Name).Equals("librarian"))
                {
                    user.IsApproved = true;
                    await userManager.UpdateAsync(user);
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRoles", new { Id = roleId });
                }

                
            }

            return RedirectToAction("EditRoles", new { Id = roleId });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListUsers()
        {
            var users = userManager.Users;

            await userManager.GetRolesAsync(users.FirstOrDefault());

            return View(users);
        }

        [Authorize(Roles = "librarian")]
        public IActionResult Approval()
        {
            var users = userManager.Users;
            var unapproved_users = users.Where(a => a.IsApproved.Equals(false));
            return View(unapproved_users);
        }

        [Authorize(Roles = "librarian")]
        [HttpPost]
        public async Task<IActionResult> Approval(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("PageNotFound");
            }

            user.IsApproved = true;

            var result =await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Approval");
            }

            return RedirectToAction("PageNotFound");
        }

        public IActionResult DataNotFound()
        {
            return View();
        }


        [Authorize(Roles = "librarian")]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(Bookdetails book)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterUser ru/*, ReaderDetails rd*/)
        {
            var selectedrole = Request.Form["Selectedrole"];
            if (ModelState.IsValid)
            {
                bool approval = true;
                if (selectedrole.Equals("reader"))
                {
                     approval = false;
                }
                    var Iuser = new User()
                {
                    Email = ru.Email,
                    EmailConfirmed = true,
                    UserName = ru.UserName,
                    IsApproved = approval
                    };
                

                var resp = await userManager.CreateAsync(Iuser, ru.Password);
                if (resp.Succeeded)
                {
                    await userManager.AddToRoleAsync(Iuser, selectedrole);
                }
                return RedirectToAction("Login");
            }
            return View(ru);
        }


    }
}
