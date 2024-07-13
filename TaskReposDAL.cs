using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using TaskManager.DbContextApp;
using TaskManager.Models;

namespace TaskManager
{
    public class TaskReposDAL
    {
        private readonly MyDbContext _context; //Dependency Injection
        private readonly string _connectionString ;
        public TaskReposDAL(MyDbContext Dbontext, string connectionString)
        {
            _context = Dbontext;
            _connectionString = connectionString;
        }
        public List<TaskItem> GetAllTasks()
        {
            return _context.TaskMaster.ToList();
        }

        public string AddTaskRepo(AddTaskModel obj)
        {
            string result = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddTaskSP", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@Title", obj.TaskName);
                        cmd.Parameters.AddWithValue("@Description", obj.Description);
                        cmd.Parameters.AddWithValue("@Status", obj.Status);
                        cmd.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        cmd.Parameters.AddWithValue("@DueDate", obj.DueDate);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                result = "Task added successfully";
            }
            catch (Exception ex)
            {
                result = $"Error adding task: {ex.Message}";
            }

            return result;
        }
        public string AddUser(AddUserModels obj)
        {
            string result = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddUserSp", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", obj.UserName);
                        cmd.Parameters.AddWithValue("@Email", obj.Email);
                        cmd.Parameters.AddWithValue("@Password", obj.Password);
                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.ParameterName = "@ReturnVal";
                        returnParameter.SqlDbType = SqlDbType.Int;
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        int returnValue = (int)returnParameter.Value;

                        if (returnValue == 1)
                        {
                            result = "User added successfully";
                        }
                        else if (returnValue == 0)
                        {
                            result = "User already exists";
                        }
                        else
                        {
                            result = "Unknown error occurred";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = $"Error adding User: {ex.Message}";
            }

            return result;
        }

        public string Login(AddUserModels obj)
        {
            string result = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("LoginUserSp", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserName", obj.UserName);
                        cmd.Parameters.AddWithValue("@Password", obj.Password);

                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                                string username = reader.GetString(reader.GetOrdinal("UserName"));
                            }
                            result = "1";
                        }
                        else
                        {
                            result = "0";
                        }

                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"Error during login: {ex.Message}";
            }
            return result;
        }



    }


}

