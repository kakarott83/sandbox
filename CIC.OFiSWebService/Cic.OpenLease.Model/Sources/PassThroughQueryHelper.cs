// OWNER MK, 04-06-2009
namespace Cic.OpenLease.Model
{
    [System.CLSCompliant(true)]
    public class PassThroughQueryHelper
    {
        public static System.Collections.Generic.List<System.Collections.Generic.List<string>> GetBySql(System.Data.IDbConnection connection, string sql, bool disposeConnection)
        {
            System.Collections.Generic.List<string> SubList;
            System.Collections.Generic.List<System.Collections.Generic.List<string>> ReturnList;
            System.Data.IDbCommand Command;
            System.Data.IDataReader Reader;

            if (connection == null)
            {
                throw new System.ArgumentException("connection");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new System.ArgumentException("sql");
            }


            ReturnList = new System.Collections.Generic.List<System.Collections.Generic.List<string>>();

            using (Command = connection.CreateCommand())
            {
                Command.CommandText = sql;

                if (connection.State == System.Data.ConnectionState.Broken || connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    SubList = new System.Collections.Generic.List<string>();
                    for (int i = 0; i < Reader.FieldCount && i < 5; i++)
                    {
                        SubList.Add(Reader.GetString(i));
                    }
                    ReturnList.Add(SubList);
                }
            }

            if (disposeConnection && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Dispose();
            }

            return ReturnList;
        }
    }
}
