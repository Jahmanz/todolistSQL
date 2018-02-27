using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {

        [HttpGet("/items")]
        public ActionResult Index()
        {
            List<Item> allItems = Item.GetAll();
            return View(allItems);
        }

        [HttpGet("/items/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpPost("/items")]
        public ActionResult Create()
        {
          Item newItem = new Item(Request.Form["item-description"]);
          newItem.Save();
          return RedirectToAction("Success", "Home");
        }
        //ONE TASK
        [HttpGet("/items/{id}")]
        public ActionResult Details(int id)
        {
          Dictionary<string, object> model = new Dictionary<string, object>();
          Item selectedItem = Item.Find(id);
          List<Category> itemCategories = selectedItem.GetCategories();
          List<Category> allCategories = Category.GetAll();
          model.Add("item", selectedItem);
          model.Add("itemCategories", itemCategories);
          model.Add("allCategories", allCategories);
          return View(model);
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
        //We find the correct Category object and the correct Item object from the form inputs. Then we run our AddCategory() method on our Item object, and then we render our Home/Success.cshtml file to confirm that our form submitted correctly.
        [HttpPost("/items/{itemId}/categories/new")]
        public ActionResult AddCategory(int itemId)
        {
            Item item = Item.Find(itemId);
            Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
            item.AddCategory(category);
            return RedirectToAction("Success", "Home");
        }
        [HttpGet("/items{id}/delete")]
        public ActionResult DeleteOne(int id)
        {
            Item thisItem = Item.Find(id);
            thisItem.Delete();
            return RedirectToAction("index");
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
          Item.DeleteAll();
          return View();
        }
    }
}
