using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
namespace BD_1
{
    class Program
    {
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MFE;Integrated Security=True";
        static void Main(string[] args)
        {

            string Id;

            Console.Write("1->1_1-Return your title if it exists\n" +
                          "1->1_2-To check there is your term  in database\n"+
                          "2-To delete the title\n"+
                          "3-To add a new title\n"+
                          "4-To update the title\n");
            Console.Write("Your choice:");
            string caseSwitch = Console.ReadLine();

            string title;
            bool exist;
            exist = false;
            switch (caseSwitch)
            {
                case "1":
                    string caseSwitch_1 = Console.ReadLine();
                    switch (caseSwitch_1)
                    {
                        case "1_1":

                            Id = Input_Name_Title();
                            title = Get_Title(Id);
                            if (title.Length == 0)
                            {
                                Console.Write("fail");
                            }
                            else
                            {
                                title = title.Trim();
                                Console.WriteLine("Your title is:" + title);
                                Console.ReadKey();
                            }

                            break;
                        case "1_2":
                            Id = Input_Name_Title();
                            exist = Check_Title(Id);
                            if (exist)
                            {
                                Console.Write("Fine");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.Write("Fail");
                                Console.ReadKey();
                            }
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }

                    break;
                case "2":
                    Id = Input_Name_Title();
                    Delete_Title(Id);
                    break;

                case "3":
                    Id = Input_Name_Title();
                    title = Input_Title();
                    Add_In_Database(Id, title);
                    Console.WriteLine("The title was added");
                    Console.ReadKey();
                    break;

                case "4":
                    Id = Input_Name_Title();
                    title = Input_Title();
                    Update_Title(Id, title);
                    Console.WriteLine("The title was updated");
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }
        private static string Input_Name_Title()
        {
            Console.Write("Input the name of title :");
            string Id = Console.ReadLine();

            return Id;
        }

        
        private static string Input_Title()
        {
            Console.Write("Input  title :");
            string title= Console.ReadLine();

            return title;
        }
        private static void Update_Title(string Id, string title)
        {
            string sqlExpression = "Update_Def";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection with database ");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@header_id";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                //ID , который ищем
                param.Value = Id;
                command.Parameters.Add(param);
                param = new SqlParameter();
                param.ParameterName = "@definition";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                param.Value = title;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
        }

        private static bool Check_Title(string Id)
        {
            string temp;
            temp = Get_Title(Id);
            if (temp.Length == 0)
            {
                return false;
            }
            else
            {

                return true;

            }
        }
        private static void Add_In_Database(string Id,string title)
        {

            string sqlExpression = "Insert_Def";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection with database ");            
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@header_id";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                //ID , который ищем
                param.Value = Id;
                command.Parameters.Add(param);
                param = new SqlParameter();
                param.ParameterName = "@definition";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                param.Value = title;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
        }
        private static void Delete_Title(string Id)
            {
            string sqlExpression = "Delete_Def";
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection with database ");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@header_id";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                //ID , который ищем
                param.Value = Id;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
        }
        // По введеному ключу ищем статью и возвращаем ее
        private static string Get_Title(string Id)
        {
            string sqlExpression = "Return_Def";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection with database ");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // если имя определено, добавляем параметр для Id

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@header_id";
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Char;
                //ID , который ищем
                param.Value = Id;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@definition";
                param.Direction = ParameterDirection.Output;
                param.SqlDbType = SqlDbType.Char;
                param.Size = 7900;
                command.Parameters.Add(param);
                //выполняем хранимую процедуру
                command.ExecuteNonQuery();

                object Indic ;
                Indic=Convert.ToString(command.Parameters["@definition"].Value);
                string temp;
                temp = (string)Indic;
            
                
                   return temp;
      

            }
            
        }
    }
}

