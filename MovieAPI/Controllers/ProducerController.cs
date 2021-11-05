using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProducerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select ProducerID,ProducerName,ProducerNationality from dbo.ProducerL";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Producer producer)
        {
            string query = @"insert into dbo.ProducerL (ProducerID,ProducerName,ProducerNationality) 
                    values (@ProducerID,@ProducerName,@ProducerNationality)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ProducerID", producer.ProducerID);
                    myCommand.Parameters.AddWithValue("@ProducerName", producer.ProducerName);
                    myCommand.Parameters.AddWithValue("@ProducerNationality", producer.ProducerNationality);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Producer successfully");

        }

        [HttpPut]
        public JsonResult Put(Producer producer)
        {
            string query = @"update dbo.ProducerL
                            set ProducerID=@ProducerID,
                            ProducerName=@ProducerName,
                            ProducerNationality=@ProducerNationality 
                            where ProducerID=@ProducerID";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ProducerID", producer.ProducerID);
                    myCommand.Parameters.AddWithValue("@ProducerName", producer.ProducerName);
                    myCommand.Parameters.AddWithValue("@ProducerNationality", producer.ProducerNationality);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Producer successfully");

        }
    }
}

