using Microsoft.AspNetCore.Mvc;
using Allergy.Extensions;
using Allergy.Services;
using System.Linq;
using MotleyFlash;
using MotleyFlash.Extensions;
using Allergy.ViewModels;
using Allergy.Models;
using System.Threading.Tasks;

namespace Allergy.Controllers
{    
    public class AllergicsController : Controller
    {
        private readonly IAllergiesService _allergiesService;
        private readonly IMessenger _messenger;

        public AllergicsController(IAllergiesService allergiesService, IMessenger messenger)
        {
            _allergiesService = allergiesService;
            _messenger = messenger;
        }

        public IActionResult Index()
        {
            var dbAllergics = _allergiesService.GetAll().ToList();

            var viewModel = dbAllergics.ToAllergicsListViewModel();

            return View(viewModel);
        }

        public IActionResult AddNewAllergic()
        {
            var viewModel = new EditAllergicViewModel();

            viewModel.AvailableAllergens = AllergiesExtensions.GetAllAllergensAsListItems();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAllergic(EditAllergicViewModel allergicVm)
        {
            var allergiesScore = allergicVm.SelectedAllergens.Select(a => int.Parse(a)).Sum();

            var newAllergic = new Allergies(allergicVm.Name, allergiesScore);

            await _allergiesService.CreateAsync(newAllergic);

            _messenger.Success($"Person {newAllergic.Name} has been successfully added");

            return RedirectToAction("Index");
        }

        public IActionResult EditAllergic(int allergicId)
        {
            var allergic = _allergiesService.Get(allergicId);

            var viewModel = allergic.ToEditAllergicViewModel();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllergic(EditAllergicViewModel allergicVm)
        {
            var allergic = _allergiesService.Get(allergicVm.AllergicId);

            if (allergic == null)
            {
                _messenger.Error($"Person {allergicVm.Name} with ID {allergicVm.AllergicId} doesn't exist in the database");

                return RedirectToAction("Index");
            }

            var allergiesScore = allergicVm.SelectedAllergens.Select(a => int.Parse(a)).Sum();

            allergic.Name = allergicVm.Name;
            allergic.Score = allergiesScore;

            await _allergiesService.UpdateAsync(allergic);

            _messenger.Success($"Person {allergic.Name} has been successfully updated");

            return RedirectToAction("Index");
        }

        public IActionResult RemoveAllergic(int allergicId)
        {
            var allergic = _allergiesService.Get(allergicId);

            var viewModel = allergic.ToAllergicViewModel();

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RemoveAllergic")]
        public async Task<IActionResult> RemoveAllergicRecord(int id)
        {
            var allergic = _allergiesService.Get(id);

            if (allergic != null)
                await _allergiesService.DeleteAsync(allergic);

            _messenger.Success("Person has been successfully removed");

            return RedirectToAction("Index");
        }
    }
}
