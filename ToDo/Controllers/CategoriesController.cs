using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoListApp.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/")]
        public ActionResult Index()
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/categories/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpPost("/categories")]
        public ActionResult Create()
        {
            Category newCategory = new Category(Request.Form["new-category"]);
            newCategory.Save();
            List<Category> allCategories = Category.GetAll();
            return View("Index", allCategories);
        }

        [HttpGet("/categories/details/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems();
            model.Add("category", selectedCategory);
            model.Add("items", categoryItems);
            return View("Details", model);
        }

        [HttpPost("/categories/details")]
        public ActionResult PostDetails()
        {
            Item newItem = new Item(Request.Form["new-item"]);
            newItem.Save();
            Category thisCategory = Category.Find(Int32.Parse(Request.Form["category-id"]));
            thisCategory.AddItem(newItem);
            return RedirectToAction("Details", new {id = newItem.GetCategories()});
        }

        [HttpPost("/categories/delete")]
        public ActionResult DeleteAll()
        {
          Category.DeleteAll();
          return View();
        }

    }
}
