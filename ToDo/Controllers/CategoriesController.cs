using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/categories")]
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
          Category newCategory = new Category(Request.Form["category-name"]);
          newCategory.Save();
          return RedirectToAction("Success", "Home");
        }

        [HttpGet("/categories/{id}")]
        public ActionResult CategoryDetail(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems();
            List<Item> allItems = Item.GetAll();
            model.Add("category", selectedCategory);
            model.Add("categoryItems", categoryItems);
            model.Add("allItems", allItems);

            return View(model);
        }
        //We find the correct Item object and the correct Category object from the form inputs. Then we run our AddItem() method on our Item object, and then we render our Home/Success.cshtml file to confirm that our form submitted correctly.
        [HttpPost("/categories/{categoryId}/items/new")]
        public ActionResult AddItem(int categoryId)
        {
            Category category = Category.Find(categoryId);
            Item item = Item.Find(Int32.Parse(Request.Form["item-id"]));
            category.AddItem(item);
            return RedirectToAction("Success", "Home");
        }
        //This route will take you to the UpdateForm page for the category
        [HttpGet("/categories/{categoryId}/update")]
        public ActionResult UpdateForm(int categoryId)
        {
          Category thisCategory = Category.Find(categoryId);
          return View("update", thisCategory);
        }

        [HttpPost("/categories{categoryId}/update")]
        public ActionResult Update(int categoryId)
        {
          Category thisCategory = Category.Find(categoryId);
          thisCategory.Edit(Request.Form["newname"]);
          return RedirectToAction("Index");
        }

        [HttpGet("/categories/{categoryId}/delete")]
        public ActionResult DeleteOne(int categoryId)
        {
          Category thisCategory = Category.Find(categoryId);
          thisCategory.Delete();
          return RedirectToAction("index");
        }

        [HttpPost("/categories/delete")]
        public ActionResult DeleteAll()
        {
          Category.DeleteAll();
          return View();
        }
    }
}
