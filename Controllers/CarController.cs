using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using web_api_crud.Models;

namespace web_api_crud.Controllers
{
    public class CarController : Controller
    {
        string connString = "server=127.0.0.1;uid=root;pwd=Zoka.7991;database=sys";

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public List<CarModel> GetCarList()
        {
            List<CarModel> carList = new List<CarModel>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlCommand comm = new MySqlCommand("SELECT id, num_doors, brand, model FROM cartable", conn);
                conn.Open();

                MySqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    var car = new CarModel();
                    car.id = Convert.ToInt32(reader["id"]);
                    car.brand = reader["brand"].ToString();
                    car.model = reader["model"].ToString();
                    car.num_doors = Convert.ToInt32(reader["num_doors"]);
                    carList.Add(car);

                }
                conn.Close();
            }
            return carList;
        }

        [HttpPost("/car/post/num_doors={num_doors}&brand={brand}&model={model}")]
        public int PostCar(int num_doors, string brand, string model)
        {
            int numRowsAffected = 0;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("INSERT INTO cartable (num_doors,brand, model) VALUES (@num_doors,@brand,@model)", conn);
                conn.Open();
                comm.Parameters.AddWithValue("num_doors", num_doors);
                comm.Parameters.AddWithValue("brand", brand);
                comm.Parameters.AddWithValue("model", model);
                numRowsAffected = comm.ExecuteNonQuery();
                conn.Close();
            }

            return numRowsAffected;
        }

        [HttpPut("car/update/id={id}&num_doors={num_doors}&brand={brand}&model={model}")]
        public CarModel Update(int id, int num_doors, string brand, string model)
        {
            CarModel updatedCarValues = new CarModel();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("UPDATE cartable SET num_doors=@num_doors, brand=@brand, model=@model WHERE id=@id", conn);
                conn.Open();

                updatedCarValues.id = id;
                comm.Parameters.AddWithValue("id", id);

                updatedCarValues.brand = brand;
                comm.Parameters.AddWithValue("brand", brand);

                updatedCarValues.model = model;
                comm.Parameters.AddWithValue("model", model);

                updatedCarValues.num_doors = num_doors;
                comm.Parameters.AddWithValue("num_doors", num_doors);

                comm.ExecuteNonQuery();
                conn.Close();
            }
            return updatedCarValues;
        }

        [HttpGet("/car/get/{id}")]
        public CarModel Get(int id)
        {
            CarModel carWithId = new CarModel();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("SELECT id,num_doors, brand, model FROM cartable WHERE id=@id", conn);
                conn.Open();
                comm.Parameters.AddWithValue("id", id);

                MySqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    carWithId.id = Convert.ToInt32(reader["id"]);
                    carWithId.brand = reader["brand"].ToString();
                    carWithId.model = reader["model"].ToString();
                    carWithId.num_doors = Convert.ToInt32(reader["num_doors"]);

                }
                conn.Close();
            }
            return carWithId;
        }

        [HttpDelete("/car/delete/{id}")]
        public CarModel Delete(int id)
        {
            CarModel carToDelete = new CarModel();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("DELETE FROM cartable WHERE id=@id", conn);
                conn.Open();
                comm.Parameters.AddWithValue("id", id);
                carToDelete = Get(id);
                if (carToDelete.id != 0)
                {
                    comm.ExecuteNonQuery();
                }

                conn.Close();
            }
            return carToDelete;
        }
    }
}
