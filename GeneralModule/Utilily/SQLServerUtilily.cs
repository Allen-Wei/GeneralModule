using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using GeneralModule;
using GeneralModule.Utilily;
using GeneralModule.Utilily.Extension;

namespace GeneralModule.Utilily
{

    public static class SQLServerUtilily
    {
        public static DataTable ReadDataFromDB(string strConnection, SqlCommand command)
        {
          
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = strConnection;
                command.Connection = connection;
                SqlDataAdapter daAdapter = new SqlDataAdapter(command);
                DataTable dtTable = new DataTable();
                daAdapter.Fill(dtTable);
                return dtTable;
           

        }
        public static DataTable ReadDataFromDB(string strConnection, string strQueryString)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = strQueryString;
            return ReadDataFromDB(strConnection, command);
        }
        public static DataTable ReadDataFromDB(string strConnection, string strQuery, CommandPara para)
        {
            var paras = new List<CommandPara> { para };
            return ReadDataFromDB(strConnection, strQuery, paras);
        }
        public static DataTable ReadDataFromDB(string strConnection, string strQueryString, List<CommandPara> paras)
        {
            var command = new SqlCommand { CommandText = strQueryString };
            Array.ForEach(paras.ToArray(), para => command.Parameters.Add(para.PlaceHolder, para.DBType).Value = para.Value);
            return ReadDataFromDB(strConnection, command);
        }
        public static DataTable ReadDataFromDB(string strConnection, string strQueryString, params CommandPara[] paras)
        {
            var command = new SqlCommand { CommandText = strQueryString };
            Array.ForEach(paras, para => command.Parameters.Add(para.PlaceHolder, para.DBType).Value = para.Value);
            return ReadDataFromDB(strConnection, command);
        }

        public static object ReadValueFromDB(string strConnection, string strQuery)
        {
            var table = ReadDataFromDB(strConnection, strQuery);
            if (table.Rows.Count == 0) return null;
            var value = table.Rows[0][0];
            return value.eIsNull() ? null : value;
        }

        public static object ReadValueFromDB(string strConnection, string strQuery, params CommandPara[] paras)
        {

            var table = ReadDataFromDB(strConnection, strQuery, paras.ToList());
            if (table.Rows.Count == 0) return null;
            var value = table.Rows[0][0];
            return value.eIsNull() ? null : value;
        }

        public static bool ExecuteSQL(string strConnection, string strSql)
        {
            var connection = new SqlConnection { ConnectionString = strConnection };
            var command = connection.CreateCommand();
            command.CommandText = strSql;
            try
            {
                connection.Open();
                var intResult = command.ExecuteNonQuery();
                return intResult >= 1;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool ExecuteSQL(string strConnection, SqlCommand command)
        {
            var connection = new SqlConnection { ConnectionString = strConnection };
            command.Connection = connection;
            try
            {
                connection.Open();
                var intResult = command.ExecuteNonQuery();
                return intResult >= 1;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool ExecuteSQL(string strConnection, string strSql, params CommandPara[] paras)
        {
            var command = new SqlCommand { CommandText = strSql };
            try
            {
                if (paras != null)
                {
                    foreach (var para in paras)
                    {
                        command.Parameters.Add(para.PlaceHolder, para.DBType).Value = para.Value;
                    }
                }
            }
            catch
            {
                return false;
            }

            return ExecuteSQL(strConnection, command);
        }

        public static int ExecSql(string strConnection, string strSql)
        {
            var connection = new SqlConnection { ConnectionString = strConnection };
            var command = connection.CreateCommand();
            command.CommandText = strSql;
            try
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

        public static int ExecSql(string strConnection, string strSql, params CommandPara[] paras)
        {
            var connection = new SqlConnection { ConnectionString = strConnection };
            var command = connection.CreateCommand();
            command.CommandText = strSql;
            Array.ForEach(paras, para => command.Parameters.Add(para.PlaceHolder, para.DBType).Value = para.Value);
            try
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }



        public static DataTable ReadDataFromDBWithProcedure(string strConnection, string strProcedureName, params CommandPara[] paras)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = strConnection;
            SqlCommand command = connection.CreateCommand();
            command.CommandText = strProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            foreach (CommandPara para in paras)
            {
                command.Parameters.Add(para.PlaceHolder, para.DBType).Value = para.Value;
            }
            SqlDataAdapter daAdapter = new SqlDataAdapter(command);
            DataTable dtTable = new DataTable();
            daAdapter.Fill(dtTable);
            return dtTable;
        }
        public static DataTable ReadDataFromDBWithProcedure(string strConnection, string strProcedureName, ref List<SqlParameter> paras)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = strConnection;
            SqlCommand command = connection.CreateCommand();
            command.CommandText = strProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter para in paras)
            {
                command.Parameters.Add(para);
            }
            SqlDataAdapter daAdapter = new SqlDataAdapter(command);
            DataTable dtTable = new DataTable();
            daAdapter.Fill(dtTable);
            return dtTable;
        }

        public class CommandPara
        {
            public string PlaceHolder { get; set; }
            public SqlDbType DBType { get; set; }
            public object Value { get; set; }
            public CommandPara() { }
            public CommandPara(string placeHolder, SqlDbType dbType, object value)
            {
                this.PlaceHolder = placeHolder;
                this.DBType = dbType;
                this.Value = value;
            }
        }
    }
}