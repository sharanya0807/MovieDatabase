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
    public class MovieController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MovieController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Gets all the movies with their movieTitle,actor name,producer name and release date
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select m.MovieTitle,c.ActorName,p.ProducerName,m.ReleaseDate
                            from dbo.CastL c,dbo.MoviesL m,dbo.ProducerL p
                            where c.ActorID=m.ActorID
                            and m.ProducerID=p.ProducerID
                            ";
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

        //Gets the Actors names of a particulat movie
        [HttpGet("{Name}")]
        public JsonResult Get(string Name)
        {
            string query = @"select c.ActorID,c.ActorName 
                            from dbo.CastL c,dbo.MoviesL m
                            where c.ActorID=m.ActorID
                            and m.MovieTitle=@MovieTitle"
                             ;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@MovieTitle", Name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Movie movie)
        {
            string query = @"insert into dbo.MoviesL (MovieID,MovieTitle,ReleaseDate,ActorID,ProducerID) 
                    values (@MovieID,@MovieTitle,@ReleaseDate,@ActorID,@ProducerID)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@MovieID", movie.MovieId);
                    myCommand.Parameters.AddWithValue("@MovieTitle", movie.MovieTitle);
                    myCommand.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
                    myCommand.Parameters.AddWithValue("@ActorID", movie.ActorID);
                    myCommand.Parameters.AddWithValue("@ProducerID", movie.ProducerID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Movie successfully");

        }

        [HttpPut]
        public JsonResult Put(Movie movie)
        {
            string query = @"update dbo.MoviesL
                            set MovieID=@MovieID,
                            MovieTitle=@MovieTitle,
                            ReleaseDate=@ReleaseDate,
                            ActorID=@ActorID,
                            ProducerID=@ProducerID";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@MovieID", movie.MovieId);
                    myCommand.Parameters.AddWithValue("@MovieTitle", movie.MovieTitle);
                    myCommand.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
                    myCommand.Parameters.AddWithValue("@ActorID", movie.ActorID);
                    myCommand.Parameters.AddWithValue("@ProducerID", movie.ProducerID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Movie successfully");

        }
    }
}
