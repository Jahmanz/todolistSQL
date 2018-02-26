using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {

        // [HttpGet("/")]
        // public ActionResult Index()
        // {
        //     List<Item> allItems = Item.GetAll();
        //     return View(allItems);
        // }

        [HttpGet("/items/new/{categoryid}")]
        public ActionResult CreateForm(int categoryid)
        {
            return View(categoryid);
        }

        [HttpGet("/items/{id}")]
        public ActionResult Details(int id)
        {
            Item item = Item.Find(id);
            return View(item);
        }

        [HttpPost("/items")]
        public ActionResult Create()
        {
          Item newItem = new Item (Request.Form["new-item"]);
          newItem.Save();
          List<Item> allItems = Item.GetAll();
          return View("Index", allItems);
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
          Item.DeleteAll();
          return View();
        }
        [HttpGet("/items/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Item thisItem = Item.Find(id);
            return View(thisItem);
        }
        [HttpPost("/items/{id}/update")]
        public ActionResult Update(int id)
        {
            Item thisItem = Item.Find(id);
            thisItem.Edit(Request.Form["newname"]);
            return RedirectToAction("Index");
        }

        // [HttpGet("/items/{id}/deleteitem")]
        // public ActionResult DeleteItemForm(int id)
        // {
        //     Item thisItem = Item.Find(id);
        //     thisItem.DeleteItem();
        //     return RedirectToAction("Details", "Categories", new {id=thisItem.GetCategoryId()});
        // }
        // [HttpPost("/items/{id}/deleteitem")]
        // public ActionResult DeleteItem(int id)
        // {
        //     Item thisItem = Item.Find(id);
        //     thisItem.DeleteItem();
        //     return RedirectToAction("Index");
        // }


    }
}
