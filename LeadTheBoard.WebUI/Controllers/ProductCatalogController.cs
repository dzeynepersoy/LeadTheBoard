using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Department;
using LeadTheBoard.Shared.Models.Product;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ProductCatalogController : BaseController<ProductCatalogController>
    {
        public IActionResult Index()
        {
            var products = UnitOfWork.Products.Find().ToList();

            var result = products.Select(i => new ProductListModel() 
            {
                Id = i.Id,
                Name = i.Name   

            }).ToList();

            return View(result);
        }

        public async Task<IActionResult> CreateOrEdit(ProductModel model)
        {
            //bu bir edit işlemi
            if (model.Id > 0)
            {
                var product = await UnitOfWork.Products.GetByIdAsync(model.Id);
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = model.Name;

                await UnitOfWork.Products.UpdateAsync(product);

                await UnitOfWork.CommitAsync();

            }
            //bu bir create işlemi
            else
            {
                await UnitOfWork.Products.AddAsync(new Product()
                {
                    Name = model.Name
                });

                await UnitOfWork.CommitAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await UnitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await UnitOfWork.Products.DeleteAsync(product);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
