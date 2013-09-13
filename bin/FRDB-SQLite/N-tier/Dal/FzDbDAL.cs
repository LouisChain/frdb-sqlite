using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N_tier.Library;
using System.Data.SQLite;
using System.Data;
using System.Threading;

namespace N_tier
{
    public class FzDbDAL
    {
        #region Methods

        public static String GetRootPath(String path)
        {
            String root = String.Empty;

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '\\')
                {
                    root = path.ToString().Substring(0, i + 1);
                    break;
                }
            }

            return root;
        }

        public  Boolean CheckConnection(FzDbBLL fdb)
        {
            SqliteConnection connection = new SqliteConnection(fdb.ConnString);

            if (connection.CheckConnection(fdb.ConnString))
            {
                return true;
            }
            else
            {
                throw new Exception("ERROR:\n" + connection.ErrorMessage);
                //return false;
            }
        }

        ///A blank Fuzzy Database isn't contents anything!!!
        public bool CreateBlankFdb(FzDbBLL fdb)
        {
            SQLiteConnection.CreateFile(fdb.FzDbPath);
            return true;
        }

        public bool CreateFuzzyDatabase(FzDbBLL fdb)
        {
            String query = String.Empty;

            try
            {
                SQLiteConnection.CreateFile(fdb.FzDbPath);
                SqliteConnection connection = new SqliteConnection(fdb.ConnString);

                // Record set of schemes to the database system
                query = "";
                query += "CREATE TABLE SystemScheme ( ";
                query += "ID INT, ";
                query += "SchemeName NVARCHAR(200) ";
                query += " );";

                if (!connection.CreateTable(query))
                {
                    throw new Exception(connection.ErrorMessage);
                }

                // Record set of relations to the database system
                query = "";
                query += "CREATE TABLE SystemRelation ( ";
                query += "ID INT, ";
                query += "RelationName NVARCHAR(200), ";
                query += "SchemeID INT ";
                query += " );";

                if (!connection.CreateTable(query))
                {
                    throw new Exception(connection.ErrorMessage);
                }

                // Record set of attributes to the database system  
                query = "";
                query += "CREATE TABLE SystemAttribute ( ";
                query += "ID INT, ";
                query += "PrimaryKey NVARCHAR(10), ";
                query += "AttributeName NVARCHAR(200), ";
                query += "DataType NVARCHAR(200), ";
                query += "Domain TEXT, ";
                query += "Description TEXT, ";
                query += "SchemeID INT ";
                query += " ); ";

                if (!connection.CreateTable(query))
                {
                    throw new Exception(connection.ErrorMessage);
                }
                // Record set of queries to the database system
                query = "";
                query += "CREATE TABLE SystemQuery ( ";
                query += "ID INT, ";
                query += "QueryName NVARCHAR(200), ";
                query += "QueryString TEXT ";
                query += " );";

                if (!connection.CreateTable(query))
                {
                    throw new Exception(connection.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ERROR:\n" + ex.Message);
                throw new Exception("ERROR:\n" + ex.Message);
                return false;
            }

            return true;
        }

        public bool OpenFuzzyDatabase(FzDbBLL fdb)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(fdb.ConnString);
                DataSet ds = new DataSet();

                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemScheme", "system_scheme"));//Table [0]
                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemRelation", "system_relation"));//Table [1]
                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemAttribute", "system_attribute"));//Table [2]
                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemQuery", "system_query"));//Table [3]

                ///Load Schemes////////////////////////////////////////////////////////////////////////////////////////
                foreach (DataRow row in ds.Tables["system_scheme"].Rows)
                {
                    String schemeName = row[1].ToString();
                    FzSchemeBLL tmpScheme = new FzSchemeBLL(schemeName);
                    DataTable tmpDt = new DataTable();
                    tmpDt = connection.GetDataTable("SELECT * FROM SystemAttribute Where SchemeID=" + Convert.ToInt16(row[0]));

                    if (tmpDt != null)
                    {
                        foreach (DataRow item in tmpDt.Rows)
                        {
                            Boolean primaryKey = Convert.ToBoolean(item[1]);
                            String attributeName = Convert.ToString(item[2]);
                            String typeName = Convert.ToString(item[3]);
                            String domain = Convert.ToString(item[4]);
                            String description = Convert.ToString(item[5]);

                            FzDataTypeBLL tmpDataType = new FzDataTypeBLL(typeName, domain);
                            FzAttributeBLL tmpAttribute = new FzAttributeBLL(primaryKey, attributeName, tmpDataType, description);

                            tmpScheme.Attributes.Add(tmpAttribute);
                        }

                        fdb.Schemes.Add(tmpScheme);
                    }
                }

                ///Load Relations//////////////////////////////////////////////////////////////////////////////////////////
                foreach (DataRow row in ds.Tables["system_relation"].Rows)
                {
                    String relationName = row[1].ToString();
                    int schemeID = Convert.ToInt16(row[2]);//To get scheme is referenced
                    String schemeName = connection.GetValueField("SELECT SchemeName FROM SystemScheme WHERE ID=" + schemeID).ToString();
                    DataTable tmpDt = new DataTable();
                    tmpDt = connection.GetDataTable("SELECT * FROM " + relationName);

                    FzRelationBLL tmpRelation = new FzRelationBLL(relationName);//Relation only content relation name, but scheme referenced and list tuples is null
                    tmpRelation.Scheme = FzSchemeDAL.GetSchemeByName(schemeName, fdb);//Assign scheme referenced to relation, but tuples is null

                    int nColumns = tmpRelation.Scheme.Attributes.Count;//Get number columns of per row

                    if (tmpDt != null)//
                    {
                        foreach (DataRow tupleRow in tmpDt.Rows)
                        {
                            List<Object> objs = new List<object>();

                            for (int i = 0; i < nColumns; i++)//Add values on per row from tupleRow[i]
                            {
                                //values += tupleRow[i].ToString();
                                objs.Add(tupleRow[i]);
                            }

                            FzTupleBLL tmpTuple = new FzTupleBLL() { ValuesOnPerRow = objs };
                            tmpRelation.Tuples.Add(tmpTuple);
                        }
                    }

                    fdb.Relations.Add(tmpRelation);
                }

                ///Load Queries////////////////////////////////////////////////////////////////////////////////////////////
                foreach (DataRow row in ds.Tables["system_query"].Rows)
                {
                    FzQueryBLL tmpQuery = new FzQueryBLL(row[1].ToString(), row[2].ToString());
                    fdb.Queries.Add(tmpQuery);
                }

                return true;
            }
            catch (SQLiteException ex)
            {
                throw new Exception("ERROR:\n" + ex.Message);
                //return false;
            }
        }

        public void DropFuzzyDatabase(FzDbBLL fdb)
        {
            try
            {
                SqliteConnection connection = new SqliteConnection(fdb.ConnString);
                DataSet ds = new DataSet();


                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemScheme", "system_scheme"));
                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemRelation", "system_relation"));
                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemAttribute", "system_attribute"));
                ds.Tables.Add(connection.GetDataTable("SELECT * FROM SystemQuery", "system_query"));


                foreach (DataRow row in ds.Tables["system_relation"].Rows)
                {
                    String relationname = row[1].ToString();

                    if (!connection.DropTable(relationname))
                    {
                        throw new Exception(connection.ErrorMessage);
                    }
                }

                if (!connection.Update("DELETE FROM SystemScheme"))
                {
                    throw new Exception(connection.ErrorMessage);
                }

                if (!connection.Update("DELETE FROM SystemRelation"))
                {
                    throw new Exception(connection.ErrorMessage);
                }

                if (!connection.Update("DELETE FROM SystemAttribute"))
                {
                    throw new Exception(connection.ErrorMessage);
                }

                if (!connection.Update("DELETE FROM SystemQuery"))
                {
                    throw new Exception(connection.ErrorMessage);
                }

                connection.CloseConnect();
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR:\n" + ex.Message);
            }
        }

        public bool SaveFuzzyDatabaseAs(FzDbBLL fdb)
        {
            try
            {
                if (!CreateFuzzyDatabase(fdb) && !SaveFuzzyDatabase(fdb))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR:\n" + ex.Message);
                //return false;
            }

            return true;
        }

        public bool SaveFuzzyDatabase(FzDbBLL fdb)
        {
            //To do here...
            try
            {
                String query = String.Empty;
                SqliteConnection connection = new SqliteConnection(fdb.ConnString);

                ///Save Name//////////////////////////////////////////////////////////////////////////////////


                ///Save Schemes///////////////////////////////////////////////////////////////////////////////
                int schemeID = 0;

                foreach (FzSchemeBLL s in fdb.Schemes)
                {
                    /// Update <Scheme> to System Scheme Table //////////////////////////////////////////////
                    String schemeName = s.SchemeName;
                    schemeID++;

                    query = "";
                    query += "INSERT INTO SystemScheme VALUES (";
                    query += schemeID + ",";
                    query += "'" + schemeName + "'";
                    query += " );";

                    if (!connection.Update(query))
                    {
                        throw new Exception(connection.ErrorMessage);
                    }

                    ///Save attributes of the scheme to the System Attribute Table /////////////////////////
                    if (s.Attributes.Count > 0)
                    {
                        int attributeID = 0;

                        foreach (FzAttributeBLL attr in s.Attributes)
                        {
                            attributeID++;

                            query = "";
                            query += "INSERT INTO SystemAttribute VALUES ( ";
                            query += attributeID + ",";
                            query += "'" + attr.PrimaryKey + "'" + ",";
                            query += "'" + attr.AttributeName + "'" + ",";
                            query += "'" + attr.DataType.TypeName + "'" + ",";
                            query += "'" + attr.DataType.DomainString + "'" + ",";
                            query += "'" + attr.Description + "'" + ",";
                            query += schemeID;
                            query += " );";

                            if (!connection.Update(query))
                            {
                                throw new Exception(connection.ErrorMessage);
                            }
                        }
                    }
                }
                //System.Windows.Forms.MessageBox.Show("Save scheme OK");
                ///Save Relations //////////////////////////////////////////////////////////////////////////
                int relationID = 0;

                foreach (FzRelationBLL relation in fdb.Relations)
                {
                    //int schemeID = 0;
                    relationID++;
                    String relationName = relation.RelationName;
                    schemeID = connection.GetSchemeID(relation.Scheme.SchemeName);

                    query = "";
                    query += "INSERT INTO SystemRelation VALUES ( ";
                    query += relationID + ",";
                    query += "'" + relationName + "'" + ",";
                    query += schemeID;
                    query += " );";

                    if (!connection.Update(query))
                    {
                        throw new Exception(connection.ErrorMessage);
                    }
                    //System.Windows.Forms.MessageBox.Show("insert to system relaton ok");
                    ///Create Table <Relation> ////////////////////////////////////////////////
                    if (relation.Scheme.Attributes.Count > 0)
                    {
                        query = "";
                        query += "CREATE TABLE " + relationName + " ( ";

                        foreach (FzAttributeBLL attribute in relation.Scheme.Attributes)
                        {
                            query += attribute.AttributeName + " " + "TEXT" + ", ";
                        }

                        query = query.Remove(query.LastIndexOf(','), 1);
                        query += " ); ";

                        if (!connection.CreateTable(query))
                        {
                            throw new Exception(connection.ErrorMessage);
                        }
                    }
                    //System.Windows.Forms.MessageBox.Show("create table relation ok!");
                    ///Insert tuples into Talbe <Relation> ////////////////////////////////////
                    if (relation.Tuples.Count > 0)
                    {
                        foreach (FzTupleBLL tuple in relation.Tuples)
                        {
                            query = "";
                            query += "INSERT INTO " + relationName + " VALUES (";

                            foreach (var value in tuple.ValuesOnPerRow)
                            {
                                query += "'" + value + "'" + ",";
                            }
                            query = query.Remove(query.LastIndexOf(','), 1);
                            query += " );  ";

                            if (!connection.Update(query))
                            {
                                throw new Exception(connection.ErrorMessage);
                            }
                        }
                    }
                }
                //System.Windows.Forms.MessageBox.Show("Insert to tuple ok");
                ///Save queries////////////////////////////////////////////////////////////////
                int queryID = 0;

                foreach (FzQueryBLL q in fdb.Queries)
                {
                    queryID++;
                    query = "";
                    query += "INSERT INTO SystemQuery VALUES (";
                    query += queryID + ",";
                    query += "'" + q.QueryName + "'" + ",";
                    query += "'" + q.QueryString + "'";
                    query += " );";

                    if (!connection.Update(query))
                    {
                        throw new Exception(connection.ErrorMessage);
                    }
                }

                connection.CloseConnect();
            }
            catch (SQLiteException ex)
            {
                throw new Exception("ERROR:\n" + ex.Message);
            }

            return true;
        }

        #endregion

        #region Privates

        #endregion
    }
}
