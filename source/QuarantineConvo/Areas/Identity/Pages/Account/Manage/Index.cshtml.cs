using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuarantineConvo.Models;

namespace QuarantineConvo.Areas.Identity.Pages.Account.Manage {
    public partial class IndexModel : PageModel {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {

            [Required]
            [StringLength(50)]
            [RegularExpression("^[a-zA-Z]*$")]
            [DisplayName("First name")]
            [PersonalData]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50)]
            [RegularExpression("^[a-zA-Z]*$")]
            [DisplayName("Last name")]
            [PersonalData]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(User user) {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid) {
                await LoadAsync(user);
                return Page();
            }

            if (Input.FirstName != user.FirstName) {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName) {
                user.LastName = Input.LastName;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
