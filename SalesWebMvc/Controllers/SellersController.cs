using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SalesWebMcv.Models;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;
        private readonly DepartmentServices _departmentServices;
        public SellersController(SellerService sellerService, DepartmentServices departmentServices) {
            _sellerService = sellerService;
            _departmentServices = departmentServices;
        }

        public async Task<IActionResult> Index() {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create() {
            var departments = await _departmentServices.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller) {
            if (!ModelState.IsValid) {
                var departments = await _departmentServices.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not foundd" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            try {
                _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            } catch {
                return RedirectToAction(nameof(Error), new { message = "Can't delete seller because he/she has sales" });
            }
        }
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Department> departments = await _departmentServices.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) {
            if (!ModelState.IsValid) {
                var departments = await _departmentServices.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            if (id != seller.Id) {
                return RedirectToAction(nameof(Error), new { message = "Id missmatch" });
            }
            try {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            } catch (NotFoundException e) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            } catch (DbConcurrencyException e) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string mensagem) {
            var viewModel = new ErrorViewModel {
                Message = mensagem,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
