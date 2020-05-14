using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SelfMonitoring.Model;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SelfMonitoring.Helper
{
    public class DbHelper
    {
        //for data insertion write condition to check if data for that user exist or not
        public static async Task<bool> PostDataAsync<T>(T model, string ops)
        {
            string sqlStr = null;
            switch (ops)
            {
                case Constants.postUserInfo:
                    sqlStr = "Insert into[dbo].[UserInfo] " +
                        "([UserId], " +
                        "[UserType], " +
                        "[FirstName], " +
                        "[LastName], " +
                        "[Role], " +
                        "[Age], " +
                        "[MobileNumber], " +
                        "[EmailAddress], " +
                        "[StreetAddress1], " +
                        "[StreetAddress2], " +
                        "[City], " +
                        "[State], " +
                        "[Country], " +
                        "[ZipCode], " +
                        "[TeamsAddress], " +
                        "[TwilioAddress]) " +
                        "values" +
                        "('" + typeof(T).GetProperty("UserId").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("UserType").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("FirstName").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("LastName").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Role").GetValue(model) + "'," +
                        "" + typeof(T).GetProperty("Age").GetValue(model) + ", " +
                        "'" + typeof(T).GetProperty("MobileNumber").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("EmailAddress").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("StreetAddress1").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("StreetAddress2").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("City").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("State").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Country").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ZipCode").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("TeamsAddress").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("TwilioAddress").GetValue(model) + "')";
                    break;

                case Constants.postScreeningInfo:
                    sqlStr = "Insert into [dbo].[ScreeningInfo]" +
                        " ([UserId], " +
                        "[DateOfEntry], " +
                        "[UserExposed], " +
                        "[ExposureDirect], " +
                        "[ExposureIndirect], " +
                        "[ExposureMultiple], " +
                        "[ExposureNotsure], " +
                        "[ExposureDate], " +
                        "[SymptomsYesNo], " +
                        "[SymptomFever], " +
                        "[SymptomShortnessOfBreath], " +
                        "[SymptomCough], " +
                        "[SymptomRunningNose], " +
                        "[SymptomSoreThroat], " +
                        "[SymptomChills], " +
                        "[SymptomDizziness], " +
                        "[SymptomAbdomenPain], " +
                        "[SymptomOther], " +
                        "[GUID], " +
                        "[QuarantineRequired])" +
                        "values" +
                        "('" + typeof(T).GetProperty("UserId").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("DateOfEntry").GetValue(model) + "'," +
                        "'" + typeof(T).GetProperty("UserExposed").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ExposureDirect").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ExposureIndirect").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ExposureMultiple").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ExposureNotsure").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ExposureDate").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomsYesNo").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomFever").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomShortnessOfBreath").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomCough").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomRunningNose").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomSoreThroat").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomChills").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomDizziness").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomAbdomenPain").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomOther").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("GUID").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("QuarantineRequired").GetValue(model) + "')";
                    break;

                case Constants.postQuarantineInfo:
                    sqlStr = "Insert into [dbo].[QuarantineInfo] " +
                        "([UserId], " +
                        "[Cycle], " +
                         "[QuarStartDate], " +
                        "[QuarMidpointDate], " +
                        "[QuarEndDate], " +
                        "[DateOfEntry], " +
                        "[SymptomFever], " +
                        "[SymptomShortnessOfBreath], " +
                        "[SymptomCough], " +
                         "[SymptomRunningNose], " +
                        "[SymptomSoreThroat], " +
                        "[SymptomChills], " +
                        "[SymptomDizziness], " +
                        "[SymptomAbdomenPain], " +
                        "[SymptomDiarrhea], " +
                        "[SymptomFatigue], " +
                        "[SymptomOther], " +
                        "[Temperature], " +
                        "[O2Saturation]," +
                        "[AntibodyTestDate], " +
                        "[AntibodyTestResult], " +
                        "[RequestRTW], " +
                        "[ApprovalRTW], " +
                        "[TeamsCallInitiated], " +
                        "[TeamsCallCompleted]) " +
                        "values" +
                        "('" + typeof(T).GetProperty("UserId").GetValue(model) + "'," +
                        " " + typeof(T).GetProperty("Cycle").GetValue(model) + "," +
                        "'" + typeof(T).GetProperty("QuarStartDate").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("QuarMidpointDate").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("QuarEndDate").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("DateOfEntry").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomFever").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomShortnessOfBreath").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomCough").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomRunningNose").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomSoreThroat").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomChills").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomDizziness").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("SymptomAbdomenPain").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomDiarrhea").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomFatigue").GetValue(model) + "', " +
                         "'" + typeof(T).GetProperty("SymptomOther").GetValue(model) + "', " +
                        "" + typeof(T).GetProperty("Temperature").GetValue(model) + ", " +
                        "" + typeof(T).GetProperty("O2Saturation").GetValue(model) + ", " +
                        "'" + typeof(T).GetProperty("AntibodyTestDate").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("AntibodyTestResult").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("RequestRTW").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("ApprovalRTW").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("TeamsCallInitiated").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("TeamsCallCompleted").GetValue(model) + "')";
                    break;
                case Constants.postUserUnderlyingInfo:
                    sqlStr = "Insert into[dbo].[UserUnderlyingInfo] " +
                        "([UserId], " +
                        "[HeartDisease], " +
                        "[Asthma], " +
                        "[LungProblems], " +
                        "[Cancer], " +
                        "[Diabetes], " +
                        "[Chemotherapy], " +
                        "[Arthritis], " +
                        "[isThermometerHandy], " +
                        "[isO2SatMonitorHandy]) " +
                        "values" +
                        "('" + typeof(T).GetProperty("UserId").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("HeartDisease").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Asthma").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("LungProblems").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Cancer").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Diabetes").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Chemotherapy").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Arthritis").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("isThermometerHandy").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("isO2SatMonitorHandy").GetValue(model) + "')";
                    break;
            }
            bool datainserted = await InsertData(sqlStr);
            return datainserted;
        }

        private static async Task<bool> InsertData(string sqlStr)
        {
            var conStr = System.Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);
            try
            {
                using (var conn = new SqlConnection(conStr))
                {
                    string data = sqlStr;

                    using (SqlCommand cmd = new SqlCommand(data, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<T> GetDataAsync<T>(string ops, string paramString)
        {
            var conStr = System.Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);

            switch (ops)
            {
                case Constants.getUserInfo:
                    UserInfo userInfo = new UserInfo();
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "SELECT * FROM UserInfo where UserId = " + "'" + paramString + "'";
                        cmd.Connection = conn;

                        reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInfo.UserId = reader["UserId"].ToString();
                                userInfo.UserType = reader["UserType"].ToString();
                                userInfo.FirstName = reader["FirstName"].ToString();
                                userInfo.LastName = reader["LastName"].ToString();
                                userInfo.Role = reader["Role"].ToString();
                                userInfo.Age = (int)reader["Age"];
                                userInfo.MobileNumber = reader["MobileNumber"].ToString();
                                userInfo.EmailAddress = reader["EmailAddress"].ToString();
                                userInfo.StreetAddress1 = reader["StreetAddress1"].ToString();
                                userInfo.StreetAddress1 = reader["StreetAddress2"].ToString();
                                userInfo.StreetAddress1 = reader["City"].ToString();
                                userInfo.StreetAddress1 = reader["State"].ToString();
                                userInfo.StreetAddress1 = reader["Country"].ToString();
                                userInfo.StreetAddress1 = reader["ZipCode"].ToString();
                                userInfo.TeamsAddress = reader["TeamsAddress"].ToString();
                                userInfo.TwilioAddress = reader["TwilioAddress"].ToString();
                            }
                        }
                        return (T)Convert.ChangeType(userInfo, typeof(T));
                    }

                case Constants.getScreeningInfo:
                    
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        List<ScreeningInfo> lstScreenInfo = new List<ScreeningInfo>();
                        cmd.CommandText = "SELECT * FROM ScreeningInfo where UserId = " + "'" + paramString + "'";
                        cmd.Connection = conn;

                        reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ScreeningInfo screeningInfo = new ScreeningInfo();
                                screeningInfo.UserId = reader["UserId"].ToString();
                                screeningInfo.DateOfEntry = reader["DateOfEntry"].ToString();
                                screeningInfo.UserExposed = DBNull.Value.Equals(reader["UserExposed"]) ? false : (bool)reader["UserExposed"];
                                screeningInfo.ExposureDirect = DBNull.Value.Equals(reader["ExposureDirect"]) ? false : (bool)reader["ExposureDirect"];
                                screeningInfo.ExposureIndirect = DBNull.Value.Equals(reader["ExposureIndirect"]) ? false : (bool)reader["ExposureIndirect"];
                                screeningInfo.ExposureMultiple = DBNull.Value.Equals(reader["ExposureMultiple"]) ? false : (bool)reader["ExposureMultiple"];
                                screeningInfo.ExposureNotsure = DBNull.Value.Equals(reader["ExposureNotsure"]) ? false : (bool)reader["ExposureNotsure"];
                                screeningInfo.ExposureDate = reader["ExposureDate"].ToString();
                                screeningInfo.SymptomsYesNo = DBNull.Value.Equals(reader["SymptomsYesNo"]) ? false : (bool)reader["SymptomsYesNo"];
                                screeningInfo.SymptomFever = DBNull.Value.Equals(reader["SymptomFever"]) ? false : (bool)reader["SymptomFever"];
                                screeningInfo.SymptomShortnessOfBreath = DBNull.Value.Equals(reader["ExposureDirect"]) ? false : (bool)reader["SymptomShortnessOfBreath"];
                                screeningInfo.SymptomCough = DBNull.Value.Equals(reader["SymptomShortnessOfBreath"]) ? false : (bool)reader["SymptomCough"];
                                screeningInfo.SymptomRunningNose = DBNull.Value.Equals(reader["SymptomRunningNose"]) ? false : (bool)reader["SymptomRunningNose"];
                                screeningInfo.SymptomSoreThroat = DBNull.Value.Equals(reader["SymptomSoreThroat"]) ? false : (bool)reader["SymptomSoreThroat"];
                                screeningInfo.SymptomChills = DBNull.Value.Equals(reader["SymptomChills"]) ? false : (bool)reader["SymptomChills"];
                                screeningInfo.SymptomDizziness = DBNull.Value.Equals(reader["SymptomDizziness"]) ? false : (bool)reader["SymptomDizziness"];
                                screeningInfo.SymptomAbdomenPain = DBNull.Value.Equals(reader["SymptomAbdomenPain"]) ? false : (bool)reader["SymptomAbdomenPain"];
                                screeningInfo.SymptomOther = reader["SymptomOther"].ToString();
                                screeningInfo.GUID = reader["GUID"].ToString();
                                screeningInfo.QuarantineRequired = DBNull.Value.Equals(reader["QuarantineRequired"]) ? false : (bool)reader["QuarantineRequired"];
                                lstScreenInfo.Add(screeningInfo);
                            }
                        }
                        return (T)Convert.ChangeType(lstScreenInfo, typeof(T));
                    }

                case Constants.getUserUnderlyingInfo:
                    UserUnderlyingInfo userUnderlyingInfo = new UserUnderlyingInfo();
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "SELECT * FROM UserUnderlyingInfo where UserId = " + "'" + paramString + "'";
                        cmd.Connection = conn;

                        reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userUnderlyingInfo.UserId = reader["UserId"].ToString();
                                userUnderlyingInfo.HeartDisease = DBNull.Value.Equals(reader["HeartDisease"]) ? false : (bool)reader["HeartDisease"];
                                userUnderlyingInfo.Asthma = DBNull.Value.Equals(reader["Asthma"]) ? false : (bool)reader["Asthma"];
                                userUnderlyingInfo.LungProblems = DBNull.Value.Equals(reader["LungProblems"]) ? false : (bool)reader["LungProblems"];
                                userUnderlyingInfo.Cancer = DBNull.Value.Equals(reader["Cancer"]) ? false : (bool)reader["Cancer"];
                                userUnderlyingInfo.Diabetes = DBNull.Value.Equals(reader["Diabetes"]) ? false : (bool)reader["Diabetes"];
                                userUnderlyingInfo.Chemotherapy = DBNull.Value.Equals(reader["Chemotherapy"]) ? false : (bool)reader["Chemotherapy"];
                                userUnderlyingInfo.Arthritis = DBNull.Value.Equals(reader["Arthritis"]) ? false : (bool)reader["Arthritis"];
                                userUnderlyingInfo.isThermometerHandy = DBNull.Value.Equals(reader["isThermometerHandy"]) ? false : (bool)reader["isThermometerHandy"];
                                userUnderlyingInfo.isO2SatMonitorHandy = DBNull.Value.Equals(reader["isO2SatMonitorHandy"]) ? false : (bool)reader["isO2SatMonitorHandy"];
                            }
                        }
                        return (T)Convert.ChangeType(userUnderlyingInfo, typeof(T));
                    }

                default:
                    QuarantineInfo quarantineInfo = new QuarantineInfo();
                    List<QuarantineInfo> ListqData = new List<QuarantineInfo>();

                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "SELECT * FROM QuarantineInfo where UserId = " + "'" + paramString + "'";
                        cmd.Connection = conn;

                        reader = cmd.ExecuteReader();
                        if (reader != null)
                        {

                            while (reader.Read())
                            {
                                QuarantineInfo qData = new QuarantineInfo();
                                qData.UserId = reader["UserId"].ToString();
                                qData.Cycle = DBNull.Value.Equals(reader["Cycle"]) ? 0 : (int)reader["Cycle"];
                                qData.QuarStartDate = reader["QuarStartDate"].ToString();
                                qData.QuarMidpointDate = reader["QuarMidpointDate"].ToString();
                                qData.QuarEndDate = reader["QuarEndDate"].ToString();
                                qData.DateOfEntry = reader["DateOfEntry"].ToString();
                                qData.SymptomFever = DBNull.Value.Equals(reader["SymptomFever"]) ? false : (bool)reader["SymptomFever"];
                                qData.SymptomShortnessOfBreath = DBNull.Value.Equals(reader["SymptomShortnessOfBreath"]) ? false : (bool)reader["SymptomShortnessOfBreath"];
                                qData.SymptomCough = DBNull.Value.Equals(reader["SymptomCough"]) ? false : (bool)reader["SymptomCough"];
                                qData.SymptomRunningNose = DBNull.Value.Equals(reader["SymptomRunningNose"]) ? false : (bool)reader["SymptomRunningNose"];
                                qData.SymptomSoreThroat = DBNull.Value.Equals(reader["SymptomSoreThroat"]) ? false : (bool)reader["SymptomSoreThroat"];
                                qData.SymptomChills = DBNull.Value.Equals(reader["SymptomChills"]) ? false : (bool)reader["SymptomChills"];
                                qData.SymptomDizziness = DBNull.Value.Equals(reader["SymptomDizziness"]) ? false : (bool)reader["SymptomDizziness"];
                                qData.SymptomAbdomenPain = DBNull.Value.Equals(reader["SymptomAbdomenPain"]) ? false : (bool)reader["SymptomAbdomenPain"];
                                qData.SymptomDiarrhea = DBNull.Value.Equals(reader["SymptomDiarrhea"]) ? false : (bool)reader["SymptomDiarrhea"];
                                qData.SymptomFatigue = DBNull.Value.Equals(reader["SymptomFatigue"]) ? false : (bool)reader["SymptomFatigue"];
                                qData.SymptomOther = reader["SymptomOther"].ToString();
                                qData.Temperature = DBNull.Value.Equals(reader["Temperature"]) ? 0 : (decimal)reader["Temperature"];
                                qData.O2Saturation = DBNull.Value.Equals(reader["O2Saturation"]) ? 0 : (decimal)reader["O2Saturation"];
                                qData.AntibodyTestDate = reader["AntibodyTestDate"].ToString();
                                qData.AntibodyTestResult = DBNull.Value.Equals(reader["AntibodyTestResult"]) ? false : (bool)reader["AntibodyTestResult"];
                                qData.RequestRTW = reader["RequestRTW"].ToString();
                                qData.ApprovalRTW = DBNull.Value.Equals(reader["ApprovalRTW"]) ? false : (bool)reader["ApprovalRTW"];
                                qData.TeamsCallInitiated = DBNull.Value.Equals(reader["TeamsCallInitiated"]) ? false : (bool)reader["TeamsCallInitiated"];
                                qData.TeamsCallCompleted = DBNull.Value.Equals(reader["TeamsCallCompleted"]) ? false : (bool)reader["TeamsCallCompleted"];

                                ListqData.Add(qData);
                            }
                        }
                        return (T)Convert.ChangeType(ListqData, typeof(T));
                    }
            }
        }

        public static async Task<bool> GetTeamsAddress(List<string> memberList, List<TeamsAddressQuarantineInfo> teamsAddressQuarantineInfoCollector)
        {
            var conStr = System.Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                var sql_query = "select ui.UserId, ui.TeamsAddress, si.QuarantineRequired from UserInfo as ui left join ScreeningInfo as si on(si.UserId = ui.UserId) where ui.UserId IN(";
                for (int i = 0; i < memberList.Count - 1; i++)
                {
                    sql_query = sql_query + "'" + memberList[i] + "',";
                }
                sql_query = sql_query + "'" + memberList[memberList.Count - 1] + "')";
                //var sql_conditions = ") AND si.QuarantineRequired = 0";
                //cmd.CommandText = "SELECT TeamsAddress FROM UserInfo where UserId = " + "'" + UserId + "'";
                cmd.CommandText = sql_query;
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        TeamsAddressQuarantineInfo teamsAddressQuarantineInfo = new TeamsAddressQuarantineInfo();
                        teamsAddressQuarantineInfo.UserId = reader["UserId"].ToString();
                        teamsAddressQuarantineInfo.TeamsAddress = reader["TeamsAddress"].ToString();

                        if (reader.IsDBNull("QuarantineRequired"))
                        {
                            teamsAddressQuarantineInfo.QuarantineRequired = false;
                        }
                        else
                        {
                            teamsAddressQuarantineInfo.QuarantineRequired = (bool)reader["QuarantineRequired"];
                        }
                        teamsAddressQuarantineInfoCollector.Add(teamsAddressQuarantineInfo);
                    }
                    return true;
                }
                return false;
            }
        }
    }
}
