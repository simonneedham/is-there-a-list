using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using IsThereAList.Models;

namespace IsThereAList.Controllers
{
    [RoutePrefix("list")]
    public class ListController : AuthenticatedController
    {

        // GET: /List/Active
        [Route()]
        [Route("active")]
        [Route("index")]
        public async Task<ActionResult> Active()
        {
            return View(await Repository.GetActiveListsAsync(this.CurrentUser.Id));
        }

        //// GET: /List/
        //private async Task<ActionResult> Index()
        //{
        //    return RedirectToAction("Active");
        //}

        // GET: /List/My
        [Route("my")]
        public async Task<ActionResult> My()
        {
            return View(await Repository.GetOwnerListsAsync(this.CurrentUser.Id));
        }

        // GET: /List/Details/5
        [Route("{listId:int}", Name = "ListWithId")]
        [Route("{listId:int}/details")]
        public async Task<ActionResult> Details(int listId)
        {
            var list = await Repository.GetListAsync(listId);

            if (list == null)
                return HttpNotFound();

            return View(list);
        }

        // GET: /List/Create
        [Route("create")]
        public async Task<ActionResult> Create()
        {
            var vm = new CreateListViewModel
                            {
                                ListTypes = await Repository.GetListTypesNotCreatedAsync(this.CurrentUser.Id),
                                Owner = this.CurrentUser
                            };
            return View(vm);
        }

        // POST: /List/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "NewListTypeId")] CreateListViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await Repository.CreateNewListAsync(viewModel.NewListTypeId, this.CurrentUser);
                return RedirectToAction("My");
            }

            var vm = new CreateListViewModel
                            {
                                ListTypes = await Repository.GetListTypesNotCreatedAsync(this.CurrentUser.Id),
                                Owner = this.CurrentUser
                            };

            return View(vm);
        }

        // GET: /List/Edit/5
        [Route("{id:int?}/edit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var list = await Repository.GetListAsync(id.Value);

            if (list == null)
                return HttpNotFound();

            if (list.OwnerId != this.CurrentUser.Id)
                throw new AppException("You cannot edit somone else's list!");

            var vm = new EditListViewModel
            {
                EditableList = list,
                ListTypes = await Repository.GetListTypesNotCreatedAsync(this.CurrentUser.Id),
                Owner = this.CurrentUser
            };

            return View(vm);
        }

        // POST: /List/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("{id:int}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EditableList.ListId,NewListTypeId")] EditListViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await Repository.UpdateListAsync(viewModel.EditableList.ListId, viewModel.NewListTypeId, this.CurrentUser);
                return RedirectToAction("My");
            }

            var vm = new EditListViewModel
            {
                EditableList = await Repository.GetListAsync(viewModel.EditableList.ListId),
                ListTypes = await Repository.GetListTypesNotCreatedAsync(this.CurrentUser.Id),
                Owner = this.CurrentUser
            };

            return View(vm);
        }

        // GET: /List/Delete/5
        [Route("{id:int?}/delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var list = await Repository.GetListAsync(id.Value);

            if (list == null)
                return HttpNotFound();

            if (list.OwnerId != this.CurrentUser.Id)
                throw new AppException("You can only delete a list of your own!");

            return View(list);
        }

        // POST: /List/Delete/5
        [HttpPost]
        [Route("{id:int}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await Repository.DeleteListAsync(id, this.CurrentUser.Id);
            return RedirectToAction("My");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
