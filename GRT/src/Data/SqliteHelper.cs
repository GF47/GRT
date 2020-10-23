#if SQLITE

using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace GRT.Data
{
    public static class SqliteHelper
    {
        public static readonly string DefaultPath = string.Format("{0}/Data/data.db", Application.dataPath);

        /// <summary>
        /// 创建一个[SqliteConnection]
        /// </summary>
        /// <param name="cString">连接字符串</param>
        /// <returns>一个[sqlite]连接</returns>
        public static SqliteConnection CreateConnection(string cString)
        {
            if (string.IsNullOrEmpty(cString))
            {
                throw new ArgumentNullException("cString");
            }
            return new SqliteConnection(cString);
        }

        /// <summary>
        /// 创建一个[SqliteCommand]
        /// </summary>
        /// <param name="connection">指定的[sqlite]连接</param>
        /// <param name="commandText">数据库操作命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>一个[sqlite]操作命令</returns>
        public static SqliteCommand CreateCommand(SqliteConnection connection, string commandText, params SqliteParameter[] parameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            SqliteCommand cmd = new SqliteCommand(commandText, connection);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.Add(parameters[i]);
                }
            }
            return cmd;
        }

        /// <summary>
        /// 创建一个[SqliteParameter]
        /// </summary>
        /// <param name="name">参数的名字，格式为[@name]</param>
        /// <param name="type">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns>一个[SqliteCommand]的参数</returns>
        public static SqliteParameter CreateParameter(string name, DbType type, object value)
        {
            SqliteParameter parameter = new SqliteParameter();
            parameter.DbType = type;
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        public static DataSet ExecuteDataSet(SqliteCommand cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            DataSet ds = new DataSet();
            SqliteDataAdapter da = new SqliteDataAdapter(cmd);
            da.Fill(ds);
            cmd.Connection.Close();
            cmd.Dispose();
            da.Dispose();
            return ds;
        }

        /// <summary>
        /// 以事务的方式执行命令，不是很懂 (O_o)
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(SqliteTransaction transaction, SqliteCommand cmd)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction.Connection == null)
            {
                throw new NullReferenceException("transaction.Connection");
            }
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            SqliteConnection connection = transaction.Connection;
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }

        public static IDataReader ExecuteReader(SqliteCommand cmd)
        {
            if (cmd == null || cmd.Connection == null)
            {
                throw new NullReferenceException("command or Connection");
            }
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            SqliteDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Dispose();
            return reader;
        }

        public static int ExecuteNonQuery(SqliteCommand cmd)
        {
            if (cmd == null || cmd.Connection == null)
            {
                throw new NullReferenceException("command or Connection");
            }
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            int result = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Dispose();
            return result;
        }
    }
}

#endif
