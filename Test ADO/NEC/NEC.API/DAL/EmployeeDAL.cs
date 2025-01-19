using System.Data;
using Microsoft.Data.SqlClient;
using NEC.API.Models;

namespace NEC.API.DAL
{
    public class EmployeeDAL
    {
        SqlConnection _connection = null;
        SqlCommand _command = null;

        public static IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory
                ()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("NZWalksConnectionString");
        }

        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Get_Employees]";
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read()) 
                {
                    Employee employee = new Employee();
                    employee.Id =  Convert.ToInt32( dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).Date;
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]);
                    employeeList.Add(employee);
                }
                _connection.Close();
            }
            return employeeList;
        }

        public bool Insert(Employee employee)
        {
            int id = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Insert_Employee]";
                _command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                _command.Parameters.AddWithValue("@LastName", employee.LastName);
                _command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                _command.Parameters.AddWithValue("@Email", employee.Email);
                _command.Parameters.AddWithValue("@Salary", employee.Salary);
                _connection.Open();
                id = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return id > 0? true : false ;
        }

        public Employee GetById(int id)
        {
            Employee employee = new Employee();
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[usp_Get_EmployeeById]";
                _command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                SqlDataReader dr = _command.ExecuteReader();

                while (dr.Read())
                {
                    employee.Id = Convert.ToInt32(dr["Id"]);
                    employee.FirstName = dr["FirstName"].ToString();
                    employee.LastName = dr["LastName"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]).Date;
                    employee.Email = dr["Email"].ToString();
                    employee.Salary = Convert.ToDouble(dr["Salary"]); 
                }
                _connection.Close();
            }
            return employee;
        }

        public bool Update(int id, Employee employee)
        {
            Employee EMPLOYEE = GetById(id); 
            int d = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType= CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[usp_Update_Employee]";
                _command.Parameters.AddWithValue("@Id", employee.Id);
                _command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                _command.Parameters.AddWithValue("@LastName", employee.LastName);
                _command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                _command.Parameters.AddWithValue("@Email", employee.Email);
                _command.Parameters.AddWithValue("@Salary", employee.Salary);
                _connection.Open();
                d = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return d > 0? true : false;
        }


        public bool Delete(int id) 
        {
            int deletedRowCount = 0;
            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[usp_Delete_Employee]";
                _command.Parameters.AddWithValue("@Id", id);
                _connection.Open();
                deletedRowCount = _command.ExecuteNonQuery();
                _connection.Close();
            }
            return deletedRowCount > 0 ? true : false;
        }
    }
}
