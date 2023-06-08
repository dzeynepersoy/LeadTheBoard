using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Department;
using LeadTheBoard.Shared.Models.Machine;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class MachinesController : BaseController<MachinesController>
    {
        public IActionResult Index()
        {
            var machines = UnitOfWork.Machines.Find().ToList();

            var result = machines.Select(i => new MachineListModel()
            {
                Description = i.Description,
                Id = i.Id,
                Name = i.Name

            }).ToList();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(DepartmentModel model)
        {
            //bu bir edit işlemi
            if (model.Id > 0)
            {
                var machine = await UnitOfWork.Machines.GetByIdAsync(model.Id);
                if (machine == null)
                {
                    return NotFound();
                }

                machine.Name = model.Name;
                machine.Description = model.Description;

                await UnitOfWork.Machines.UpdateAsync(machine);

                await UnitOfWork.CommitAsync();

            }
            //bu bir create işlemi
            else
            {
                await UnitOfWork.Machines.AddAsync(new Machine()
                {
                    Name = model.Name,
                    Description = model.Description
                });

                await UnitOfWork.CommitAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var machine = await UnitOfWork.Machines.GetByIdAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            await UnitOfWork.Machines.DeleteAsync(machine);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
