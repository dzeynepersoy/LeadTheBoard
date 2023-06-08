using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Department;
using LeadTheBoard.Shared.Models.Product;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class DepartmansController : BaseController<DepartmansController>
    {
        public async Task<IActionResult> Index()
        {
            var products = UnitOfWork.Products.Find().ToList();

            ViewBag.Products = products.Select(i => new ProductListModel()
            {
                Id = i.Id,
                Name = i.Name,
            }).ToList();

            var departmans = await UnitOfWork.Departments
                .Find()
                .Include(x => x.DepartmentsAndProducts)
                .ThenInclude(x => x.Product)
                .ToListAsync();

            var result = departmans.Select(i => new DepartmentListModel()
            {
                Description = i.Description,
                Id = i.Id,
                Name = i.Name,
                Products = string.Join(", ", i.DepartmentsAndProducts.Select(i => i.Product.Name)),
                ProductIds = string.Join(", ", i.DepartmentsAndProducts.Select(i => i.ProductId)),
            }).ToList();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(DepartmentModel model)
        {
            //bu bir edit işlemi
            if (model.Id > 0)
            {
                var department = await UnitOfWork.Departments.GetByIdAsync(model.Id);
                if (department == null)
                {
                    return NotFound();
                }

                department.Name = model.Name;
                department.Description = model.Description;

                var departmentAndProducts = await UnitOfWork.DepartmentsAndProducts.Find(i => i.DepartmentId == department.Id).ToListAsync();
                var onlyProductIds = departmentAndProducts.Select(i => i.ProductId).ToList();

                var newProductIds = model.Products.Except(onlyProductIds).ToList();
                if (newProductIds.Count > 0)
                {
                    var products = await UnitOfWork.Products.Find(i => newProductIds.Contains(i.Id)).ToListAsync();
                    foreach (var product in products)
                    {
                        await UnitOfWork.DepartmentsAndProducts.AddAsync(new DepartmentAndProduct()
                        {
                            DepartmentId = department.Id,
                            ProductId = product.Id
                        });
                    }
                }

                var deletedProductIds = onlyProductIds.Except(model.Products).ToList();
                if (deletedProductIds.Count > 0)
                {
                    var deletedDepartmentAndProducts = await UnitOfWork.DepartmentsAndProducts.Find(i => deletedProductIds.Contains(i.ProductId) && i.DepartmentId == department.Id).ToListAsync();
                    foreach (var deletedDepartmentAndProduct in deletedDepartmentAndProducts)
                    {
                        await UnitOfWork.DepartmentsAndProducts.DeleteAsync(deletedDepartmentAndProduct);
                    }
                }

                await UnitOfWork.CommitAsync();

            }
            //bu bir create işlemi
            else
            {
                var department = await UnitOfWork.Departments.AddAsyncReturnEntity(new Department()
                {
                    Description = model.Description,
                    Name = model.Name
                });

                await UnitOfWork.CommitAsync();

                if (model.Products.Any())
                {
                    var products = await UnitOfWork.Products.Find(i => model.Products.Contains(i.Id)).ToListAsync();

                    foreach (var product in products)
                    {
                        await UnitOfWork.DepartmentsAndProducts.AddAsync(new DepartmentAndProduct()
                        {
                            DepartmentId = department.Id,
                            ProductId = product.Id
                        });
                    }

                    await UnitOfWork.CommitAsync();

                }

            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await UnitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            await UnitOfWork.Departments.DeleteAsync(department);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }

    }
}
