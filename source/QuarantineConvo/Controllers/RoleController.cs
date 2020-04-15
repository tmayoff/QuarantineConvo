using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuarantineConvo.Data;
using QuarantineConvo.Identity.Models;
using QuarantineConvo.Models;

namespace Identity.Controllers {
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller {

        QuarantineConvoIdentityContext identityContext;

        RoleManager<IdentityRole> roleManager;
        UserManager<User> userManager;

        public RoleController(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager, QuarantineConvoIdentityContext _identityContext) {
            roleManager = _roleManager;
            identityContext = _identityContext;
            userManager = _userManager;
        }

        public ViewResult Index() => View(roleManager.Roles);

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required]string name) {
            if (ModelState.IsValid) {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    Errors(result);
                }
            }
            return View(name);
        }

        public async Task<IActionResult> Edit(string id) {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<User> users = userManager.Users.ToList();
            List<User> members = new List<User>();
            List<User> nonMembers = new List<User>();
            foreach (User user in users) {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModification model) {
            IdentityResult result;
            if (ModelState.IsValid) {
                foreach (string userId in model.AddIds ?? new string[] { }) {
                    User user = await userManager.FindByIdAsync(userId);
                    if (user != null) {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { }) {
                    User user = await userManager.FindByIdAsync(userId);
                    if (user != null) {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Edit(model.RoleId);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null) {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    Errors(result);
                }
            }
            else {
                ModelState.AddModelError("", "No role found");
            }
            return View("Index", roleManager.Roles);
        }

        private void Errors(IdentityResult result) {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}