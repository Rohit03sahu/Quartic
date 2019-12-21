using AssignmentQuartic.AI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace AssignmentQuartic.AI
{
    public class DB
    {

        #region Declaration

        private string DBConnectionString = System.Configuration.ConfigurationManager.AppSettings["DBConnectionString"];
        SqlConnection ObjSqlConnection = null;
        SqlCommand ObjSqlCommand = null;
        string Query = "";

        #endregion

        #region DB Functions

        /// <summary>
        /// Perform Rule Operation Create Delete & Update
        /// </summary>
        /// <param name="_Rule"></param>
        /// <param name="Flag"></param>
        /// <param name="_ErrorMessage"></param>
        /// <param name="RuleID"></param>
        /// <returns></returns>
        public bool fn_ModifyRule(Models.Rules _Rule, int Flag, ref string _ErrorMessage, int RuleID = 0)
        {
            try
            {
                DataTable DTS = new DataTable();
                switch (Flag)
                {
                    case 0:
                        Query = "Insert into T_Rules(rule_signal,rule_minvalues,rule_maxvalues,rule_valuetype,rule_desc,EntryDateTime) values ('" + _Rule.rule_signal + "','" + _Rule.rule_minvalues + "','" + _Rule.rule_maxvalues + "','"+ _Rule.rule_valuetype + "','"+ _Rule.rule_desc + "',GETDATE())";
                        break;
                    case 1:
                        Query = "update T_Rules set rule_signal='" + _Rule.rule_signal + "',rule_minvalues='" + _Rule.rule_minvalues + "',rule_valuetype='" + _Rule.rule_valuetype + "',rule_maxvalues='" + _Rule.rule_maxvalues + "',rule_desc='" + _Rule.rule_desc + "' where Rule_TbleRefID =" + _Rule.Rule_ID;
                        break;
                    case 2:
                        Query = "delete from T_Rules where Rule_TbleRefID =" + RuleID;
                        break;
                    default:
                        break;
                }

                ObjSqlConnection = new SqlConnection(DBConnectionString);

                using (ObjSqlCommand = new SqlCommand(Query))
                {
                    ObjSqlCommand.CommandType = CommandType.Text;
                    ObjSqlCommand.Connection = ObjSqlConnection;

                    ObjSqlConnection.Open();
                    ObjSqlCommand.ExecuteNonQuery();
                }

                return true;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                ObjSqlConnection.Close();
                return false;
            }
            finally
            {
                ObjSqlConnection.Close();
                ObjSqlConnection = null;
                ObjSqlCommand = null;
                GC.Collect();
            }
        }
       
        /// <summary>
        /// Insert Data Stream 
        /// </summary>
        /// <param name="_LstRootObject"></param>
        /// <param name="_ErrorMessage"></param>
        /// <returns></returns>
        public bool fn_Insert(List<RootObject> _LstRootObject, ref string _ErrorMessage)
        {
            try
            {
                string xml = fn_Buildxml(_LstRootObject, ref _ErrorMessage);
                if (xml != null)
                {
                    if (!fn_dbInsert(xml, ref _ErrorMessage))
                        throw new Exception(_ErrorMessage);
                }
                else
                {
                    throw new Exception(_ErrorMessage);
                }
                return true;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

        private bool fn_dbInsert(string xml, ref string _ErrorMessage)
        {
            try
            {
                DataTable DTS = new DataTable();
                Query = "USP_Save_DataStreaming";
                ObjSqlConnection = new SqlConnection(DBConnectionString);

                using (ObjSqlCommand = new SqlCommand(Query))
                {
                    ObjSqlCommand.CommandType = CommandType.StoredProcedure;
                    ObjSqlCommand.Connection = ObjSqlConnection;

                    ObjSqlCommand.Parameters.AddWithValue("@xml", xml);
                    ObjSqlConnection.Open();
                    ObjSqlCommand.ExecuteNonQuery();
                }

                return true;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                ObjSqlConnection.Close();
                return false;
            }
            finally
            {
                ObjSqlConnection.Close();
                ObjSqlConnection = null;
                ObjSqlCommand = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// to Get all rules
        /// </summary>
        /// <param name="_ErrorMessage"></param>
        /// <returns></returns>
        public List<Rules> fn_GetRules(ref string _ErrorMessage)
        {
            try
            {

                DataTable _dt = fn_dbGetRules(ref _ErrorMessage);
                List<Rules> _rules = Generic.ConvertDataTable<Rules>(_dt);
                return _rules;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }

        private DataTable fn_dbGetRules(ref string _ErrorMessage)
        {
            try
            {
                DataTable DTS = new DataTable();
                Query = "select Rule_TbleRefID 'Rule_ID', rule_signal,rule_minvalues,rule_maxvalues,rule_valuetype,rule_desc,EntryDateTime from T_Rules ";
                ObjSqlConnection = new SqlConnection(DBConnectionString);

                ObjSqlConnection = new SqlConnection(DBConnectionString);
                ObjSqlCommand = new SqlCommand(Query, ObjSqlConnection);
                ObjSqlConnection.Open();
                SqlDataReader Sdr = ObjSqlCommand.ExecuteReader();
                DTS.Load(Sdr);
                ObjSqlConnection.Close();
                return DTS;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                ObjSqlConnection.Close();
                return null;
            }
            finally
            {
                ObjSqlConnection.Close();
                ObjSqlConnection = null;
                ObjSqlCommand = null;
                GC.Collect();
            }
        }


        /// <summary>
        /// Get Violation Rule Data Report
        /// </summary>
        /// <param name="_ErrorMessage"></param>
        /// <returns></returns>
        public List<Violation> fn_GetViolationdata(ref string _ErrorMessage)
        {
            try
            {

                DataTable _dt = fn_dbGetViolationdata(ref _ErrorMessage);
                List<Violation> _Violation = Generic.ConvertDataTable<Violation>(_dt);
                return _Violation;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }

        private DataTable fn_dbGetViolationdata(ref string _ErrorMessage)
        {
            try
            {
                DataTable DTS = new DataTable();
                Query = "select signal,value_type,value from T_StreamingData where ViolationFlag=1 ";
                ObjSqlConnection = new SqlConnection(DBConnectionString);

                ObjSqlConnection = new SqlConnection(DBConnectionString);
                ObjSqlCommand = new SqlCommand(Query, ObjSqlConnection);
                ObjSqlConnection.Open();
                SqlDataReader Sdr = ObjSqlCommand.ExecuteReader();
                DTS.Load(Sdr);
                ObjSqlConnection.Close();
                return DTS;

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                ObjSqlConnection.Close();
                return null;
            }
            finally
            {
                ObjSqlConnection.Close();
                ObjSqlConnection = null;
                ObjSqlCommand = null;
                GC.Collect();
            }
        }


        #endregion

        #region XML Functions

        /// <summary>
        /// Build XML for Data Stream with Validation (Rule Engine)
        /// </summary>
        /// <param name="_LstRootObject"></param>
        /// <param name="_ErrorMessage"></param>
        /// <returns></returns>
        public string fn_Buildxml(List<RootObject> _LstRootObject, ref string _ErrorMessage)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                List<Rules> _lstRules = fn_GetRules(ref _ErrorMessage);

                str.Append("<root>");
                foreach (var _RootObject in _LstRootObject)
                {
                    int ViolationFlag = 0;
                    str.Append("<datastream>");
                    str.Append("<signal>" + _RootObject.signal + "</signal>");
                    str.Append("<value>" + _RootObject.value + "</value>");
                    str.Append("<value_type>" + _RootObject.value_type + "</value_type>");

                    DateTime result=DateTime.Now;

                    if (_RootObject.value_type.ToLower().Trim() == "datetime")
                    {
                        if ((!DateTime.TryParse(_RootObject.value, out result)))
                        {
                            ViolationFlag = 1;
                        }
                        else if (DateTime.Compare(result, DateTime.Now) > 0) { ViolationFlag = 1; }
                    }


                    if (_lstRules != null)
                    {
                        foreach (var item in _lstRules)
                        {
                            if (item.rule_signal.Trim() == _RootObject.signal.Trim() && item.rule_valuetype.ToLower().Trim() == _RootObject.value_type.ToLower().Trim())
                            {
                                switch (_RootObject.value_type.ToLower().Trim())
                                {
                                    case "datetime":

                                        if (item.rule_minvalues.Trim().Length > 0)
                                        {
                                            if (Convert.ToDateTime(item.rule_minvalues) > result)
                                            {
                                                ViolationFlag = 1;
                                            }
                                        }

                                        if (item.rule_maxvalues.Trim().Length > 0)
                                        {
                                            if (Convert.ToDateTime(item.rule_maxvalues) < result)
                                            {
                                                ViolationFlag = 1;
                                            }
                                        }

                                        break;
                                    case "string":

                                        if (!item.rule_minvalues.Equals(null))
                                        {
                                            if (item.rule_minvalues == _RootObject.value)
                                            {
                                                ViolationFlag = 1;
                                            }
                                        }

                                        if (!item.rule_maxvalues.Equals(null))
                                        {
                                            if (item.rule_maxvalues == _RootObject.value)
                                            {
                                                ViolationFlag = 1;
                                            }
                                        }

                                        
                                        break;
                                    case "integer":

                                        if (!item.rule_minvalues.Equals(null))
                                        {
                                            if (!(Convert.ToDouble(item.rule_minvalues) <= Convert.ToDouble(_RootObject.value)))
                                            {
                                                ViolationFlag = 1;
                                            }
                                        }

                                        if (!item.rule_maxvalues.Equals(null))
                                        {
                                            if (!(Convert.ToDouble(item.rule_maxvalues) >= Convert.ToDouble(_RootObject.value)))
                                            {
                                                ViolationFlag = 1;
                                            }
                                        }

                                        break;
                                }


                                break;
                            }
                        }
                    }
                    
                    str.Append("<ViolationFlag>" + ViolationFlag + "</ViolationFlag>");
                    str.Append("</datastream>");
                }
                str.Append("</root>");


                return str.ToString();

            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }


        #endregion


    }
}