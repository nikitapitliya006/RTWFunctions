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
                        "[isO2SatMonitorHandy], " +
                        "values" +
                        "('" + typeof(T).GetProperty("UserId").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("HeartDisease").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Asthma").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("LastName").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("LungProblems").GetValue(model) + "'," +
                        "" + typeof(T).GetProperty("Cancer").GetValue(model) + ", " +
                        "'" + typeof(T).GetProperty("Diabetes").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Chemotherapy").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("Arthritis").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("isThermometerHandy").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("isO2SatMonitorHandy").GetValue(model) + "')";
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
                        "[SymotomRunningNose], " +
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
                        "'" + typeof(T).GetProperty("SymotomRunningNose").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("SymptomSoreThroat").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomChills").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomDizziness").GetValue(model) + "', " +
                        "'" + typeof(T).GetProperty("SymptomAbdomenPain").GetValue(model) + "', " +
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
                        "[SymotomRunningNose], " +
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
                        " '" + typeof(T).GetProperty("Cycle").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("QuarStartDate").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("QuarMidpointDate").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("QuarEndDate").GetValue(model) + "'," +
                        " " + typeof(T).GetProperty("DateOfEntry").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomFever").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomShortnessOfBreath").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomCough").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymotomRunningNose").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomSoreThroat").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomChills").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomDizziness").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomAbdomenPain").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomDiarrhea").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomFatigue").GetValue(model) + "," +
                        " " + typeof(T).GetProperty("SymptomOther").GetValue(model) + "," +
                        " '" + typeof(T).GetProperty("Temperature").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("O2Saturation").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("AntibodyTestDate").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("AntibodyTestResult").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("RequestRTW").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("ApprovalRTW").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("TeamsCallInitiated").GetValue(model) + "'," +
                        " '" + typeof(T).GetProperty("TeamsCallCompleted").GetValue(model) + "')";
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
                    ScreeningInfo screeningInfo = new ScreeningInfo();
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "SELECT * FROM ScreeningInfo where UserId = " + "'" + paramString + "'";
                        cmd.Connection = conn;

                        reader = cmd.ExecuteReader();
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                screeningInfo.UserId = reader["UserId"].ToString();
                                screeningInfo.DateOfEntry = reader["DateOfEntry"].ToString();
                                screeningInfo.UserExposed = (bool)reader["UserExposed"];
                                screeningInfo.ExposureDirect = (bool)reader["ExposureDirect"];
                                screeningInfo.ExposureIndirect = (bool)reader["ExposureIndirect"];
                                screeningInfo.ExposureMultiple = (bool)reader["ExposureMultiple"];
                                screeningInfo.ExposureNotsure = (bool)reader["ExposureNotsure"];
                                screeningInfo.ExposureDate = reader["ExposureDate"].ToString();
                                screeningInfo.SymptomsYesNo = (bool)reader["SymptomsYesNo"];
                                screeningInfo.SymptomFever = (bool)reader["SymptomFever"];
                                screeningInfo.SymptomShortnessOfBreath = (bool)reader["SymptomShortnessOfBreath"];
                                screeningInfo.SymptomCough = (bool)reader["SymptomCough"];
                                screeningInfo.SymotomRunningNose = (bool)reader["SymotomRunningNose"];
                                screeningInfo.SymptomSoreThroat = (bool)reader["SymptomSoreThroat"];
                                screeningInfo.SymptomChills = (bool)reader["SymptomChills"];
                                screeningInfo.SymptomDizziness = (bool)reader["SymptomDizziness"];
                                screeningInfo.SymptomAbdomenPain = (bool)reader["SymptomAbdomenPain"];
                                screeningInfo.SymptomOther = reader["SymptomOther"].ToString();
                                screeningInfo.GUID = reader["GUID"].ToString();
                                screeningInfo.QuarantineRequired = (bool)reader["QuarantineRequired"];
                            }
                        }
                        return (T)Convert.ChangeType(screeningInfo, typeof(T));
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
                                userUnderlyingInfo.HeartDisease = (bool)reader["HeartDisease"];
                                userUnderlyingInfo.Asthma = (bool)reader["Asthma"];
                                userUnderlyingInfo.LungProblems = (bool)reader["LungProblems"];
                                userUnderlyingInfo.Cancer = (bool)reader["Cancer"];
                                userUnderlyingInfo.Diabetes = (bool)reader["Diabetes"];
                                userUnderlyingInfo.Chemotherapy = (bool)reader["Chemotherapy"];
                                userUnderlyingInfo.Arthritis = (bool)reader["Arthritis"];
                                userUnderlyingInfo.isThermometerHandy = (bool)reader["isThermometerHandy"];
                                userUnderlyingInfo.isO2SatMonitorHandy = (bool)reader["isO2SatMonitorHandy"];
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
                                qData.Cycle = (int)reader["Cycle"];
                                qData.QuarStartDate = reader["QuarStartDate"].ToString();
                                qData.QuarMidpointDate = reader["QuarMidpointDate"].ToString();
                                qData.QuarEndDate = reader["QuarEndDate"].ToString();
                                qData.DateOfEntry = reader["DateOfEntry"].ToString();
                                qData.SymptomFever = (bool)reader["SymptomFever"];
                                qData.SymptomShortnessOfBreath = (bool)reader["SymptomShortnessOfBreath"];
                                qData.SymptomCough = (bool)reader["SymptomCough"];
                                qData.SymptomRunningNose = (bool)reader["SymptomRunningNose"];
                                qData.SymptomSoreThroat = (bool)reader["SymptomSoreThroat"];
                                qData.SymptomChills = (bool)reader["SymptomChills"];
                                qData.SymptomDizziness = (bool)reader["SymptomDizziness"];
                                qData.SymptomAbdomenPain = (bool)reader["SymptomAbdomenPain"];
                                qData.SymptomDiarrhea = (bool)reader["SymptomDiarrhea"];
                                qData.SymptomFatigue = (bool)reader["SymptomFatigue"];
                                qData.SymptomOther = reader["SymptomOther"].ToString();
                                qData.Temperature = (decimal)reader["Temperature"];
                                qData.O2Saturation = (decimal)reader["O2Saturation"];
                                qData.AntibodyTestDate = reader["AntibodyTestDate"].ToString();
                                qData.AntibodyTestResult = (bool)reader["AntibodyTestResult"];
                                qData.RequestRTW = reader["RequestRTW"].ToString();
                                qData.ApprovalRTW = (bool)reader["ApprovalRTW"];
                                qData.TeamsCallInitiated = (bool)reader["TeamsCallInitiated"];
                                qData.TeamsCallCompleted = (bool)reader["TeamsCallCompleted"];

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
