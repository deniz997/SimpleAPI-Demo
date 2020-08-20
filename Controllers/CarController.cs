using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using web_api_crud.Models;

namespace web_api_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private IConfiguration Configuration;
        string connString;
   
        public CarController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            connString = this.Configuration.GetConnectionString("Default");

        }

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
                MySqlCommand comm = new MySqlCommand("SELECT id, num_doors, brand, model FROM car_table", conn);
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

        [HttpPost]
        public int Post(CarModel car)
        {
            int numRowsAffected = 0;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {

                MySqlCommand comm = new MySqlCommand("INSERT INTO car_table (num_doors,brand, model) VALUES (@num_doors,@brand,@model)", conn);
                conn.Open();
                comm.Parameters.AddWithValue("num_doors", car.num_doors);
                comm.Parameters.AddWithValue("brand", car.brand);
                comm.Parameters.AddWithValue("model", car.model);
                numRowsAffected = comm.ExecuteNonQuery();
                conn.Close();
            }

            return numRowsAffected;
        }

        [HttpPut]
        public CarModel Update(CarModel car)
        {

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("UPDATE car_table SET num_doors=@num_doors, brand=@brand, model=@model WHERE id=@id", conn);
                conn.Open();

                comm.Parameters.AddWithValue("id", car.id);
                comm.Parameters.AddWithValue("brand", car.brand);
                comm.Parameters.AddWithValue("model", car.model);
                comm.Parameters.AddWithValue("num_doors", car.num_doors);

                comm.ExecuteNonQuery();
                conn.Close();
            }
            return car;
        }

        [HttpGet("{id}")]
        public CarModel Get(int id)
        {
            CarModel carWithId = new CarModel();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("SELECT id,num_doors, brand, model FROM car_table WHERE id=@id", conn);
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

        [HttpDelete("{id}")]
        public CarModel Delete(int id)
        {
            CarModel carToDelete = new CarModel();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand comm = new MySqlCommand("DELETE FROM car_table WHERE id=@id", conn);
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
