using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using Hatfield.Web.Portal;
using Hatfield.Web.Portal.Data;

namespace HatCMS
{
    public class CmsLocalFileOnDiskMetaItem
    {
        public int autoincid;

        private int resourceid;
        public int ResourceId
        {
            get { return resourceid; }
            set { resourceid = value; }
        }

        private int resourcerevisionnumber;
        public int ResourceRevisionNumber
        {
            get { return resourcerevisionnumber; }
            set { resourcerevisionnumber = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string itemvalue;
        public string ItemValue
        {
            get { return itemvalue; }
            set { itemvalue = value; }
        }



        public CmsLocalFileOnDiskMetaItem()
        {
            autoincid = -1;
            resourceid = -1;
            resourcerevisionnumber = -1;
            name = "";
            itemvalue = "";            
        } // constructor

        public CmsLocalFileOnDiskMetaItem(CmsLocalFileOnDisk res, MetaDataItem metaItem)
        {
            autoincid = -1;
            resourceid = res.ResourceId;
            resourcerevisionnumber = res.RevisionNumber;
            name = metaItem.Name;
            itemvalue = metaItem.ItemValue;            

        } // constructor

        public CmsLocalFileOnDiskMetaItem(CmsLocalFileOnDisk res, string itemName, string itemVal)
        {
            autoincid = -1;
            resourceid = res.ResourceId;
            resourcerevisionnumber = res.RevisionNumber;
            name = itemName;
            itemvalue = itemVal;

        } // constructor

        public bool SaveToDatabase()
        {
            if (autoincid < 0)
            {
                return (new CmsResourceMetaItemDB()).Insert(this);
            }
            else
            {
                return (new CmsResourceMetaItemDB()).Update(this);
            }
        } // SaveToDatabase
        

        public static CmsLocalFileOnDiskMetaItem[] FetchAll(CmsLocalFileOnDisk parentResource)
        {
            return (new CmsResourceMetaItemDB()).FetchAllForFile(parentResource);
        } // getAll


        public static bool BulkInsert(CmsLocalFileOnDisk item, CmsLocalFileOnDiskMetaItem[] subItems)
        {
            return (new CmsResourceMetaItemDB()).BulkInsert(item, subItems);
        } // getAll

        public static bool Delete(int AutoIncId)
        {
            return (new CmsResourceMetaItemDB()).Delete(AutoIncId);
        } // Delete

        public static CmsLocalFileOnDiskMetaItem[] FromMetaDataItems(CmsLocalFileOnDisk parentFile, MetaDataItem[] metaDataItems)
        {
            List<CmsLocalFileOnDiskMetaItem> ret= new List<CmsLocalFileOnDiskMetaItem>();                
            foreach (MetaDataItem mi in metaDataItems)
            {
                ret.Add(new CmsLocalFileOnDiskMetaItem(parentFile, mi));
            }
            return ret.ToArray();
        }


        #region CmsResourceMetaItemDB
        private class CmsResourceMetaItemDB : Hatfield.Web.Portal.Data.MySqlDbObject
        {
            public CmsResourceMetaItemDB()
                : base(ConfigUtils.getConfigValue("ConnectionString", ""))
            { }
            
            public bool Insert(CmsLocalFileOnDiskMetaItem item)
            {
                string sql = "INSERT INTO resourceitemmetadata ";
                sql += "(ResourceId, ResourceRevisionNumber, `Name`, `Value`, Deleted)";
                sql += " VALUES ( ";
                sql += item.resourceid.ToString() + ", ";
                sql += item.resourcerevisionnumber.ToString() + ", ";
                sql += "'" + dbEncode(item.name) + "'" + ", ";
                sql += "'" + dbEncode(item.itemvalue) + "'" + " ";                
                sql += " ); ";

                int newId = this.RunInsertQuery(sql);
                if (newId > -1)
                {
                    item.autoincid = newId;
                    return true;
                }
                return false;

            } // Insert
            

            public bool BulkInsert(CmsLocalFileOnDisk item, CmsLocalFileOnDiskMetaItem[] subItems)
            {
                if (subItems.Length < 1)
                    return true;

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO resourceitemmetadata ");
                sql.Append("(ResourceId, ResourceRevisionNumber, `Name`, `Value`)");
                sql.Append(" VALUES ");
                foreach (CmsLocalFileOnDiskMetaItem sub in subItems)
                {
                    sql.Append(" ( ");
                    sql.Append(item.ResourceId.ToString() + ", ");
                    sql.Append(item.RevisionNumber.ToString() + ", ");
                    sql.Append("'" + dbEncode(sub.name) + "'" + ", ");
                    sql.Append("'" + dbEncode(sub.ItemValue) + "'" + " ");                    
                    sql.Append(" ),");
                } // foreach

                // remove trailing comma
                string s = sql.ToString().Substring(0, sql.ToString().Length - 1);
                int numInserted = this.RunUpdateQuery(s); // do not use RunInsertQuery
                if (numInserted == subItems.Length)
                    return true;

                return false;

            } // Insert

            public bool Update(CmsLocalFileOnDiskMetaItem item)
            {
                string sql = "UPDATE resourceitemmetadata SET ";
                sql += "ResourceId = " + item.resourceid.ToString() + ", ";
                sql += "ResourceRevisionNumber = " + item.resourcerevisionnumber.ToString() + ", ";
                sql += "`Name` = " + "'" + dbEncode(item.name) + "'" + ", ";
                sql += "`Value` = " + "'" + dbEncode(item.itemvalue) + "'" + " ";
                
                sql += " WHERE AutoIncId = " + item.autoincid.ToString();
                sql += " ; ";

                int numAffected = this.RunUpdateQuery(sql);
                if (numAffected < 0)
                {
                    return false;
                }
                return true;

            } // Update


            public bool Delete(int AutoIncId)
            {
                string sql = "UPDATE resourceitemmetadata ";
                sql += " set deleted = " + dbEncode(DateTime.Now) + " ";
                sql += " WHERE AutoIncId = " + AutoIncId.ToString();
                int numAffected = this.RunUpdateQuery(sql);
                if (numAffected < 0)
                {
                    return false;
                }
                return true;
            }
            
            public CmsLocalFileOnDiskMetaItem[] FetchAllForFile(CmsLocalFileOnDisk resource)
            {
                string sql = "SELECT AutoIncId, ResourceId, ResourceRevisionNumber, `Name`, `Value` from resourceitemmetadata ";
                sql += " WHERE " + DBDialect.isNull("Deleted") + " AND ResourceId = " + resource.ResourceId + " AND ResourceRevisionNumber = "+resource.RevisionNumber+"   ; ";

                List<CmsLocalFileOnDiskMetaItem> arrayList = new List<CmsLocalFileOnDiskMetaItem>();
                DataSet ds = this.RunSelectQuery(sql);
                if (this.hasRows(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CmsLocalFileOnDiskMetaItem item = new CmsLocalFileOnDiskMetaItem();
                        item.autoincid = Convert.ToInt32(dr["AutoIncId"]);

                        item.resourceid = Convert.ToInt32(dr["ResourceId"]);

                        item.resourcerevisionnumber = Convert.ToInt32(dr["ResourceRevisionNumber"]);

                        item.name = (dr["Name"]).ToString();

                        item.itemvalue = (dr["Value"]).ToString();                        

                        arrayList.Add(item);
                    } // foreach row
                } // if there is data

                return arrayList.ToArray();
            } // getAll

        } // class CmsResourceMetaItemDB
        #endregion
    } // class CmsResourceMetaItem
} // namespace
