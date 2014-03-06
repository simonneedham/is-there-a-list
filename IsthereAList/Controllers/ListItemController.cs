using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using IsThereAList.Models;

namespace IsThereAList.Controllers
{
    public class ListItemController : AuthenticatedController
    {
        //GET: /List/3/ListItem
        [Route("~/list/{listId:int}/listitem", Name="ListListItem")]
        [Route("~/list/{listId:int}/listitem/index")]
        public async Task<ActionResult> ListItem(int listId)
        {
            var list = await Repository.GetListAndListItemsAsync(listId, this.CurrentUser.Id);

            if (list == null)
                return HttpNotFound();

            string viewName;
            if (list.OwnerId == this.CurrentUser.Id)
                viewName = "ListItemsForOwner";
            else
                viewName = "ListItemsForNonOwner";

            return View(viewName, list);
        }


        // GET: /ListItem/Details/5
        [Route("list/{listId:int}/listitem/{listItemId:int}")]
        [Route("list/{listId:int}/listitem/{listItemId:int}/claim")]
        [Route("list/{listId:int}/listitem/{listItemId:int}/details")]
        public async Task<ActionResult> Claim(int listId, int listItemId)
        {
            var listItem = await Repository.GetListItemAsync(listItemId);

            if (listItem == null)
                return HttpNotFound();

            var vm = new ClaimListItemViewModel
            {
                ListId = listItem.ListId,
                ListItemId = listItem.ListItemId,
                ListName = listItem.List.Name,
                ListOwnerFirstName = listItem.UserPurchased != null ? listItem.UserPurchased.FirstName : String.Empty,
                Name = listItem.Name,
                Description = listItem.Description,
                Url = listItem.Url,
                PictureUrl = listItem.PictureUrl,
                HasBeenPurchased = listItem.HasBeenPurchased,
                UserPurchasedFullName = listItem.UserPurchased != null ? listItem.UserPurchased.FullName : String.Empty,
                Deleted = listItem.Deleted,
                IsPurchasee = (listItem.UserPurchased != null && listItem.UserPurchased.Id == this.CurrentUser.Id) ? true : false
            };

            string viewName;
            if (listItem.List.OwnerId == this.CurrentUser.Id)
                viewName = "DetailsForOwner";
            else
                viewName = "DetailsForNonOwner";

            return View(viewName, vm);
        }

        // POST: /ListItem/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("list/{listId:int}/listitem/{listItemId:int}")]
        [Route("list/{listId:int}/listitem/{listItemId:int}/claim")]
        [Route("list/{listId:int}/listitem/{listItemId:int}/details")]
        public async Task<ActionResult> Claim(int listId, int listItemId, ListItem listItem)
        {
            string errMsg = String.Empty;
            try
            {
                await Repository.PurchaseListItem(listItemId, this.CurrentUser.Id);
                return RedirectToAction(String.Format("list/{0}/listitem", listId));
            }
            catch(AppException ae)
            {
                errMsg = ae.Message;
            }

            listItem = await Repository.GetListItemAsync(listItemId);

            if (listItem == null)
                return HttpNotFound();

            var vm = new ClaimListItemViewModel
            {
                ListId = listItem.ListId,
                ListItemId = listItem.ListItemId,
                ListName = listItem.List.Name,
                ListOwnerFirstName = listItem.UserPurchased.FirstName,
                Name = listItem.Name,
                Description = listItem.Description,
                Url = listItem.Url,
                PictureUrl = listItem.PictureUrl,
                HasBeenPurchased = listItem.HasBeenPurchased,
                UserPurchasedFullName = listItem.UserPurchased != null ? listItem.UserPurchased.FullName : String.Empty,
                Deleted = listItem.Deleted,
                IsPurchasee = (listItem.UserPurchased != null && listItem.UserPurchased.Id == this.CurrentUser.Id) ? true : false,
                ErrorMessage = errMsg
            };

            string viewName;
            if (listItem.List.OwnerId == this.CurrentUser.Id)
                viewName = "DetailsForOwner";
            else
                viewName = "DetailsForNonOwner";

            return View(viewName, vm);
        }

        // GET: /List/3/ListItem/Create
        [Route("list/{listId:int}/listitem/create")]
        public async Task<ActionResult> Create(int listId)
        {
            var list = await Repository.GetListAsync(listId);

            if (list.OwnerId != this.CurrentUser.Id)
                throw new AppException("You can only add items to your own lists!");

            var listItem = new ListItem { ListId = listId };
            return View(listItem);
        }

        // POST: /ListItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("list/{listId:int}/listitem/create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int listId, [Bind(Include="Name,Description,Url,PictureUrl")] ListItem listItem)
        {
            listItem.ListId = listId;
            listItem.ApplicationUserIdUpdated = this.CurrentUser.Id;

            if (ModelState.IsValid)
            {
                await Repository.CreateNewListItemAsync(listItem, this.CurrentUser.Id);
                return RedirectToAction("ListItem", "ListItem", new { listId = listId });
            }

            return View(listItem);
        }

        // GET: /ListItem/Edit/5
        [Route("list/{listId:int}/listitem/{listItemId:int}/edit")]
        public async Task<ActionResult> Edit(int listItemId)
        {
            var listItem = await Repository.GetListItemAsync(listItemId);

            if (listItem == null)
                return HttpNotFound();

            return View(listItem);
        }

        // POST: /ListItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("list/{listId:int}/listitem/{listItemId:int}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int listId, int listItemId, [Bind(Include = "Name,Description,Url,PictureUrl")] ListItem listItem)
        {
            listItem.ListId = listId;
            listItem.ListItemId = listItemId;

            if (ModelState.IsValid)
            {
                await Repository.UpdateListItemAsync(listItem, this.CurrentUser.Id);
                return RedirectToAction("ListItem", "ListItem", new { listId = listId });
            }

            return View(listItem);
        }

        // GET: /ListItem/Delete/5
        [Route("list/{listId:int}/listitem/{listItemId:int}/delete")]
        public async Task<ActionResult> Delete(int listId, int listItemId)
        {
            var listItem = await Repository.GetListItemAsync(listItemId);

            if (listItem == null)
                return HttpNotFound();

            return View(listItem);
        }

        // POST: /ListItem/Delete/5
        [HttpPost]
        [Route("list/{listId:int}/listitem/{listItemId:int}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int listId, int listItemId)
        {
            await Repository.DeleteListItemAsync(listItemId, this.CurrentUser.Id);
            return RedirectToAction("ListItem", "ListItem", new { listId = listId });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
