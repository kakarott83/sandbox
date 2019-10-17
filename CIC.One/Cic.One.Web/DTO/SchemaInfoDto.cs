using Cic.One.DTO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.DAO;
using Devart.Data.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// 
    /// <tables>
    //    <table name="PERSON" pkey="SYSPERSON"/>
		
    //    <table name="PEOPTION" pkey="SYSID">
    //        <fkey column="SYSID" foreign="PERSON.SYSPERSON"/>
    //    </table>
	
    //    <table name="VT" pkey="SYSID">
    //        <fkey column="SYSPERSON" foreign="PERSON.SYSPERSON"/>
    //    </table>
    //</tables>

    //Algorithm:
    //* fetch all schema information (column alias, table, length, type)
    //* iterate all conf tables, fkeyless
    //  create or update table, remember pkey for table
    //  -> these tables are done!
    //* iterate all conf tables with fkeys
    //  create or update table, assigned with fkeys, remember pkey for table
    //    if one fkey not yet done, mark table as update again for fkey
    //* iterate all conf tables with fkeys and marked as update again
    //  create or update table, assigned ONLY with fkeys
    /// </summary>
    public class SchemaInfoDto
    {
        /// <summary>
        /// Primary Key Information, also holding value during create/Update
        /// </summary>
        public Pkey pkey { get; set; }
        /// <summary>
        /// DB schema name
        /// </summary>
        public String baseSchemaName { get; set; }
        /// <summary>
        /// Physical table name
        /// </summary>
        public String baseTableName { get; set; }
        /// <summary>
        /// Lista all columns of the table used by the query
        /// </summary>
        public List<SchemaColumnDto> columns { get; set; }
        /// <summary>
        /// When true than this Table has been configured with fkeys
        /// </summary>
        public bool hasFkeys { get; set; }
        /// <summary>
        /// when true, not all fkeys have been updated yet
        /// </summary>
        public bool updateAgain { get; set; }
        /// <summary>
        /// when true this is the querys main table definition holding the entityid
        /// </summary>
        private bool isMainTable { get; set; }
        /// <summary>
        /// Table Schema Definition from User
        /// </summary>
        private Table tab;

        /// <summary>
        /// Creates a Tables Schema definition, holding no column info yet
        /// </summary>
        /// <param name="row"></param>
        public SchemaInfoDto(DataRow row)
        {
            baseSchemaName = (String)row["BaseSchemaName"];
            baseTableName = (String)row["BaseTableName"];
            hasFkeys = false;
            columns = new List<SchemaColumnDto>();
        }

        /// <summary>
        /// Adds a DB Column Meta Information
        /// </summary>
        /// <param name="row"></param>
        public void addColumn(DataRow row)
        {
            SchemaColumnDto col = new SchemaColumnDto(row);
            columns.Add(col);
        }

        /// <summary>
        /// Sets the user defined table schema holding the fkey/pkey definitions
        /// </summary>
        /// <param name="tab"></param>
        public void setTab(Table tab)
        {
            this.tab = tab;
            if(tab==null) return;
            if (tab.fkeys != null && tab.fkeys.Count > 0)
                hasFkeys = true;
            this.pkey = new Pkey(0, tab.pkey);
        }

        /// <summary>
        /// uses the wfvconfig defined query to create a db data reader and analyse the used columns and fields
        /// returns a dictionary with all used tables and the table infos
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static Dictionary<String, SchemaInfoDto> getSchemaInfos(ObjectContext ctx, String viewId)
        {

            WfvEntry entry = DAOFactoryFactory.getInstance().getWorkflowDao().getWfvEntry(viewId);
            //create sql query from configuration to fetch db meta data
            Dictionary<String, SchemaInfoDto> tabInfo = new Dictionary<String, SchemaInfoDto>();
            if(entry.customentry.viewmeta.query!=null)
            {
                QueryInfoData qid = null;
                String query = null;
                if (String.IsNullOrEmpty(entry.customentry.viewmeta.query.query))
                {
                    //qid = new QueryInfoDataType4(entry.customentry.viewmeta.query, SchemaInfoDto.getPrimaryKeyQueryPrefix(tabInfo), entry.customentry.searchmode);
                    qid = new QueryInfoDataType1(entry.customentry.viewmeta.query, "", entry.customentry.searchmode);
                    qid.addAdditionalSearchConditions(" and " + qid.entityField + "=-1");
                    query = qid.getCompleteQuery();
                }
                else
                {
                    /*qid = new QueryInfoDataType5(entry.customentry.viewmeta, null, entry.customentry.searchmode);
                    if (entry.customentry.viewmeta.query.pkey != null)
                        qid.addAdditionalSearchConditions(" and " + entry.customentry.viewmeta.query.pkey + "=-1");*/
                    query = entry.customentry.viewmeta.query.query;
                }

                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                con.Open();
                DbCommand infocmd = con.CreateCommand();
                infocmd.CommandText = query;
                DbDataReader reader = infocmd.ExecuteReader();
                DataRowCollection rows = reader.GetSchemaTable().Rows;
                
                foreach (DataRow row in rows)
                {
                    if(row["BaseTableName"] is System.DBNull) continue;
                    String baseTableName = (String)row["BaseTableName"];
                    if (baseTableName == null || baseTableName.Length == 0)
                        continue;
                    String colname = (String)row["ColumnName"];
                    if (colname == null || colname.Length == 0 || colname.ToUpper().Equals("SYSROWNUM"))
                        continue;
                    if (!tabInfo.ContainsKey(baseTableName))
                    {
                        SchemaInfoDto i = new SchemaInfoDto(row);

                        if (entry.customentry.viewmeta.tables != null)
                        {
                            Table tab = (from t in entry.customentry.viewmeta.tables
                                         where t.name.Equals(baseTableName)
                                         select t).FirstOrDefault();
                            if (tab == null) continue;
                            i.setTab(tab);
                        }

                        if(entry.customentry.viewmeta.query.table==null || entry.customentry.viewmeta.query.table.Equals(baseTableName))//our main table
                        {
                            i.isMainTable = true;
                            
                            if (entry.customentry.viewmeta.query.table == null)//no table definition, try to get from query (for WFLIST)
                            {
                                Table t = new Table();
                                t.pkey = entry.customentry.viewmeta.query.pkey;
                                t.name = baseTableName;
                                i.setTab(t);
                            }
                                
                        }

                        tabInfo[baseTableName] = i;
                    }
                    SchemaInfoDto ti = tabInfo[baseTableName];
                    ti.addColumn(row);
                }
            }
            return tabInfo;
        }

        /// <summary>
        /// Returns a string mapping all primary key fields of the query to internal field aliases
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static String getPrimaryKeyQueryPrefix(Dictionary<String, SchemaInfoDto> schema)
        {
            StringBuilder rval = new StringBuilder();
            foreach(String tab in schema.Keys)
            {
                SchemaInfoDto si = schema[tab];
                if (rval.Length > 0)
                    rval.Append(", ");
                rval.Append(getPrimaryKeyQueryPrefix(si));

            }
            return rval.ToString();
        }

        /// <summary>
        /// returns the internally used primary key fetching field prefix for sql
        /// </summary>
        /// <returns></returns>
        public String getPrimaryKeyQueryPrefix()
        {
            return "I_PK_" + this.tab.pkey;
        }
        /// <summary>
        /// returns the internally used primary key fetching field prefix for sql
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        private static String getPrimaryKeyQueryPrefix(SchemaInfoDto si)
        {
            return si.baseTableName + "." + si.tab.pkey + " I_PK_" + si.tab.pkey;
        }

        /// <summary>
        /// creates or updates all the fields used from this tables schema definition
        /// also updates foreign keys if applicable
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="gview"></param>
        /// <param name="schema"></param>
        public void createOrUpdate(ObjectContext ctx, GviewDto gview, Dictionary<String, SchemaInfoDto> schema)
        {
            //primary key value for current table part
            Pkey pk = (from t in gview.pkeys
                       where t.table.Equals(baseTableName)
                       select t).FirstOrDefault();
            if(pk==null)
            {
                pk = new Pkey(gview.sysId, baseTableName);
            }
            if (pk.value > 0)
            {
                this.pkey.value = pk.value;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pkey", Value = gview.sysId });

               

                String query = "UPDATE " + baseTableName + " SET ";
                int p = 0;
                Dictionary<String, int> used = new Dictionary<String, int>();
                if(!updateAgain)
                { 
                    //alle gui-felder werden aktualisiert
                    foreach (Viewfield vf in gview.fields)
                    {
                        if (used.ContainsKey(vf.attr.field))//momentan: wenn mehrfach gleiches feld, nur 1. nehmen
                            continue;

                        SchemaColumnDto coldef = (from c in this.columns
                                                  where c.columnName.Equals(vf.attr.field.ToUpper())
                                                  select c).FirstOrDefault();
                        if (coldef == null) continue; //target table doesnt have this view column!

                        if (coldef.baseColumnName.Equals(tab.pkey))//primärschlüssel weglassen!
                            continue;

                        if (p > 0)
                            query += ", ";
                        String pname = "p" + p++;
                        //map the queried field back to physical col name
                        query += coldef.baseColumnName + "=:" + pname;
                        used[vf.attr.field] = 1;
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = pname, Value = vf.getValue() });
                    }
                }
                if(hasFkeys&&schema!=null)//if we have foreign keys defined in this table fetch them from updated schema 
                {
                    foreach(Fkey fk in tab.fkeys)
                    {
                        String[] fkinfo = fk.foreign.Split('.');
                        Pkey fkval = schema[fkinfo[0]].pkey;//primary key value from fkey referenced field
                        if (fk.column.Equals(tab.pkey))//dont update pkey through fkey
                            continue;
                        if(fkval.value>0)
                        {
                            if (p > 0)
                                query += ", ";
                            String pname = "p" + p++;
                            //map the queried field back to physical col name
                            query += fk.column + "=:" + pname;
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = pname, Value = fkval.value });
                        }
                        else
                        {
                            this.updateAgain = true;//value not yet there, must be updated again!
                        }
                    }
                }


                query += " where " + tab.pkey + "=:pkey";
                if(p>0)
                    ctx.ExecuteStoreCommand(query, parameters.ToArray());
            }
            else
            {

                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                con.Open();

                DbCommand cmd = con.CreateCommand();

                Dictionary<String, int> used = new Dictionary<String, int>();

                String query = "INSERT INTO " + baseTableName + " ( ";
                int p = 0;
                String qvalue = " VALUES(";
                foreach (Viewfield vf in gview.fields)
                {
                    if (used.ContainsKey(vf.attr.field))
                        continue;
                    SchemaColumnDto coldef = (from c in this.columns
                                              where c.columnName.Equals(vf.attr.field.ToUpper())
                                              select c).FirstOrDefault();
                    if (coldef == null) continue; //target table doesnt have this view column!

                    if (coldef.baseColumnName.Equals(tab.pkey))//primärschlüssel weglassen!
                        continue;

                    if (p > 0)
                    {
                        query += ", ";
                        qvalue += ", ";
                    }
                    String pname = "p" + p++;
                    //map queried field back to physical field
                    query += coldef.baseColumnName;
                    qvalue += ":" + pname;
                    used[vf.attr.field] = 1;
                    cmd.Parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = pname, Value = vf.getValue() });
                }
                if (hasFkeys && schema != null)//if we have foreign keys defined in this table fetch them from updated schema 
                {
                    foreach (Fkey fk in tab.fkeys)
                    {
                        String[] fkinfo = fk.foreign.Split('.');
                        Pkey fkval = schema[fkinfo[0]].pkey;//primary key value from fkey referenced field
                        if (fk.column.Equals(tab.pkey))//dont update pkey through fkey
                            continue;
                        if (fkval.value > 0)
                        {
                            if (p > 0)
                            {
                                query += ", ";
                                qvalue += ", ";
                            }
                            String pname = "p" + p++;
                            //map queried field back to physical field
                            query += fk.column;
                            qvalue += ":" + pname;
                            cmd.Parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = pname, Value = fkval.value });
                        }
                        else
                        {
                            this.updateAgain = true;//value not yet there, must be updated again!
                        }
                    }
                }



                cmd.Parameters.Add(new OracleParameter("myOutputParameter", OracleDbType.Long, System.Data.ParameterDirection.ReturnValue));

                query += ") " + qvalue + ")  returning sysid  into :myOutputParameter";

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = query;
                if(p>0)
                    cmd.ExecuteNonQuery();
                this.pkey.value = Convert.ToInt64(cmd.Parameters["myOutputParameter"].Value);
                
            }
            //remember the main id
            if (isMainTable)
                gview.sysId = this.pkey.value;
        }
    }
}
