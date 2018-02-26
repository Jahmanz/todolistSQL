using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;

namespace ToDoList.Tests
{
    [TestClass]
    public class ItemTests : IDisposable
    {
        public ItemTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }
        public void Dispose()
        {
            Item.DeleteAll();
            Category.DeleteAll();
        }

        [TestMethod]
        public void Equals_OverrideTrueForSameDescription_Item()
        {
          //Arrange, Act
          Item firstItem = new Item("Mow the lawn");
          Item secondItem = new Item("Mow the lawn");

          //Assert
          Assert.AreEqual(firstItem, secondItem);
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
          int result = Item.GetAll().Count;
          Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetDescription_ReturnsDescription_String()
        {
            string description = "Walk the dog.";
            Item newItem = new Item(description);

            string result = newItem.GetDescription();

            Assert.AreEqual(description, result);
        }

        [TestMethod]
        public void Save_SavesItemToDatabase_ItemList()
        {
          Item testItem = new Item("Mow the lawn");
          testItem.Save();
          //Act
          List<Item> result = Item.GetAll();
          List<Item> testList = new List<Item>{testItem};

          CollectionAssert.AreEqual(testList, result);
        }
        [TestMethod]
        public void Save_DatabaseAssignsIdToObject_Id()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn");
          testItem.Save();

          //Act
          Item savedItem = Item.GetAll()[0];

          int result = savedItem.GetId();
          int testId = testItem.GetId();

          //Assert
          Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsItemInDatabase_Item()
        {
          //Arrange
          Item testItem = new Item("Mow the lawn");
          testItem.Save();

          //Act
          Item foundItem = Item.Find(testItem.GetId());

          //Assert
          Assert.AreEqual(testItem, foundItem);
        }

        // [TestMethod]
        // public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
        // {
        //   Item firstItem = new Item("Mow the lawn");
        //   Item secondItem = new Item("Mow the lawn");
        //
        //   Assert.AreEqual(firstItem, secondItem);
        // }

        [TestMethod]
        public void GetAll_ReturnsItems_ItemList()
        {
            string description01 = "Walk the dog";
            string description02 = "Wash the dishes";
            Item newItem1 = new Item(description01);
            Item newItem2 = new Item(description02);
            newItem1.Save();
            newItem2.Save();
            List<Item> newList = new List<Item> { newItem1, newItem2 };

            List<Item> result = Item.GetAll();

            CollectionAssert.AreEqual(newList, result);
        }

        // [TestMethod]
        // public void Find_FindITemInDatabase_Item()
        // {
        //   Item testItem = new Item("Mow the lawn");
        //   testItem.Save();
        //
        //   Item foundItem = Item.Find(testItem.GetId());
        //
        //   Assert.AreEqual(testItem, foundItem);
        // }
        [TestMethod]
        public void Edit_UpdatesItemInDatabase_String()
        {
            //Arrange
            string firstDescription = "Walk the dog";
            Item testItem = new Item(firstDescription, 1);
            testItem.Save();
            string secondDescription = "Mow the lawn";

            //Act
            testItem.Edit(secondDescription);

            string result = Item.Find(testItem.GetId()).GetDescription();

            //Assert
            Assert.AreEqual(secondDescription, result);
        }

        [TestMethod]
        public void Edit_DeleteItemInDatabase_Int()
        {
            //Arrange
            string firstDescription = "Walk the dog";
            Item testItem = new Item(firstDescription, 1);
            testItem.Save();


            //Act
            string result = Item.Find(testItem.GetId()).GetDescription();
            Assert.AreEqual(firstDescription, result);
            testItem.DeleteItem();


            //Assert
            Assert.AreEqual(0, Item.GetAll().Count);

        }
    }
}
