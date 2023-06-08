using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Department;
using LeadTheBoard.Shared.Models.Machine;
using LeadTheBoard.Shared.Models.Operation;
using LeadTheBoard.Shared.Models.Product;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class OperationsController : BaseController<OperationsController>
    {
        public IActionResult Index()
        {
            var machine = UnitOfWork.Machines.Find().ToList();

            ViewBag.Machine = machine.Select(i => new MachineListModel()
            {
                Description = i.Description,
                Id = i.Id,
                Name = i.Name,
            }).ToList();

            var products = UnitOfWork.Products.Find().ToList();
            ViewBag.Products = products.Select(i => new ProductListModel()
            {
                Id = i.Id,
                Name = i.Name,
            }).ToList();

            var operations = UnitOfWork.Operations.Find().ToList();

            var result = operations.Select(i => new OperationListModel()
            {
                Id = i.Id,
                MachineId = i.MachineId,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                MachineName = i.Machine.Name,
                DifficultyLevel = i.DifficultyLevel,
                Name = i.Name,
                Point = i.Point
            }).ToList();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(OperationModel model)
        {
            //bu bir edit işlemi
            if (model.Id > 0)
            {
                var operation = await UnitOfWork.Operations.GetByIdAsync(model.Id);
                if (operation == null)
                {
                    return NotFound();
                }

                operation.Name = model.Name;
                operation.DifficultyLevel = model.DifficultyLevel;
                operation.Point = model.Point;

                if (model.MachineId > 0)
                {
                    var machine = await UnitOfWork.Machines.GetByIdAsync(model.MachineId.Value);
                    if (machine == null)
                    {
                        return NotFound();
                    }
                    operation.MachineId = machine.Id;
                }

                if (model.ProductId > 0)
                {
                    var product = await UnitOfWork.Products.GetByIdAsync(model.ProductId.Value);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    operation.ProductId = product.Id;
                }

                await UnitOfWork.Operations.UpdateAsync(operation);

                await UnitOfWork.CommitAsync();
            }
            //bu bir create işlemi
            else
            {
                var operation = new Operation()
                {
                    Name = model.Name,
                    DifficultyLevel = model.DifficultyLevel,
                    Point = model.Point,
                };

                if (model.MachineId > 0)
                {
                    var machine = await UnitOfWork.Machines.GetByIdAsync(model.MachineId.Value);
                    if (machine == null)
                    {
                        return NotFound();
                    }
                    operation.MachineId = machine.Id;
                }

                if (model.ProductId > 0)
                {
                    var product = await UnitOfWork.Products.GetByIdAsync(model.ProductId.Value);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    operation.ProductId = product.Id;
                }

                await UnitOfWork.Operations.AddAsync(operation);

                await UnitOfWork.CommitAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var operation = await UnitOfWork.Operations.GetByIdAsync(id);
            if (operation == null)
            {
                return NotFound();
            }
            await UnitOfWork.Operations.DeleteAsync(operation);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }

    }
}
