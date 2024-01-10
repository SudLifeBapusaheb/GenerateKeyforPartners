using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using GenerateKeyforPartners.Models;
using System.Web.UI.WebControls;

namespace GenerateKeyforPartners
{
    public class Utility
    {
        string connString = ConfigurationManager.ConnectionStrings["SUD_UBI"].ToString();
        public int InsertPartnerDetails(string UserID, string PartnerID, string SecretKey, string MobileNo, string CreatedBy)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_PartnerDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_ID", UserID);
                    cmd.Parameters.AddWithValue("@Partner_ID", PartnerID);
                    cmd.Parameters.AddWithValue("@SecretKey", SecretKey);
                    cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                    cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPartnerDetailsByEmpID(string UserID, string PartnerID)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_GetAllPartnerDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@PartnerId", PartnerID);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPartnerDetails(string EmpID)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_GetPartnerDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Empid", EmpID);
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ValidateUserAndGetOTP(string Empid)
        {
            OTP_Details response = new OTP_Details();
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_ValidateUserAndGenerateOTP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpId", Empid.ToString());

                    //cmd.Parameters.AddWithValue("@CommandType", "GetSource");
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable ValidateOTPS(string Empid, string Password)
        {
            ClsUserDetails response = new ClsUserDetails();
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_ValidateOTP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpId", Empid.ToString());
                    cmd.Parameters.AddWithValue("@Password", Password);
                    con.Open();
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ExpiredOTP(string Empid,string Password)
        {
            ClsUserDetails response = new ClsUserDetails();
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_ValidateOTP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpId", Empid.ToString());
                    cmd.Parameters.AddWithValue("@Password", Password);
                    con.Open();
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSecretKeyExist(string SecretKeyID)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connString);
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("Sp_GetSecretKeyForPartner", con);
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@PartnerId", SecretKeyID);
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}