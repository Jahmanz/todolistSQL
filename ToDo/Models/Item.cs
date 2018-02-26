using System;
using System.Collections.Generic;
using ToDoList;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    // private int _categoryId;
    private int _id;

    public Item (string description, int id = 0)
    {
      _description = description;
      // _categoryId = categoryId;
      _id = id;
    }

    public override bool Equals(System.Object otherItem)
    {
        if (!(otherItem is Item))
        {
            return false;
        }
        else
        {
            Item newItem = (Item) otherItem;
            bool idEquality = this.GetId() == newItem.GetId();
            bool descriptionEquality = this.GetDescription() == newItem.GetDescription();
            // bool categoryEquality = this.GetCategoryId() == newItem.GetCategoryId();
            return (idEquality && descriptionEquality);
        }
    }
    public override int GetHashCode()
        {
             return this.GetDescription().GetHashCode();
        }

    public string GetDescription()
    {
      return _description;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public int GetId()
    {
      return _id;
    }
    // public int GetCategoryId()
    // {
    //     return _categoryId;
    // }
    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        // int itemCategoryId = rdr.GetInt32(2);

        Item newItem = new Item(itemDescription, itemId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description) VALUES (@Description);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@Description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      // MySqlParameter categoryId = new MySqlParameter();
      // categoryId.ParameterName = "@category_id";
      // categoryId.Value = this._categoryId;
      // cmd.Parameters.Add(categoryId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int itemId = 0;
      string itemDescription = "";
      // int itemCategoryId = 0;

      while (rdr.Read())
      {
        itemId = rdr.GetInt32(0);
        itemDescription = rdr.GetString(1);
        // itemCategoryId = rdr.GetInt32(2);
      }

      Item foundItem = new Item(itemDescription, itemId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundItem;
    }
    public void Edit(string newDescription)
    {
        MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _description = newDescription;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
    }

    public void DeleteItem()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM items WHERE id = @thisId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@thisId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
