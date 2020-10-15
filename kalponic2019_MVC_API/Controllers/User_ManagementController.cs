using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using kalponic2019_MVC_API.Helper;
using kalponic2019_MVC_API.Models;
using System.Net.Http;

namespace kalponic2019_MVC_API.Controllers
{
    public class User_ManagementController : Controller
    {
        // GET: User_Management
        [HttpPost]
        [Route("api/User_Management/Insert")]
        public HttpResponseMessage Insert(List<UserManagement>RequestData)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            ResponseObj ro = new ResponseObj();
            try
            {

                if (RequestData.Count == 0)
                {
                    ro.Status = 0;
                    ro.Message = "Pass Proper Data!!";

                }
                else
                {
                    SqlParameter[] prm = new SqlParameter[]
                        {
                        new SqlParameter("@TYPE","INSERT"),
                        new SqlParameter("@FIRST_NAME",RequestData[0].FIRST_NAME),
                        new SqlParameter("@LAST_NAME",RequestData[0].LAST_NAME),
                        new SqlParameter("@EMAIL_ID",RequestData[0].EMAIL_ID),
                        new SqlParameter("@PHONE_NO",RequestData[0].PHONE_NO),
                        new SqlParameter("@ADDRESS_L1",RequestData[0].ADDRESS_L1),
                        new SqlParameter("@ADDRESS_L2",RequestData[0].ADDRESS_L2),
                        new SqlParameter("@CITY",RequestData[0].CITY),
                        new SqlParameter("@STATE",RequestData[0].STATE),
                        new SqlParameter("@COUNTRY",RequestData[0].COUNTRY)
                        };
                    string MSG = Convert.ToString(new SQLHelper().ExecuteScalar("SP_USER_LOGIN_INFORMATION", prm, CommandType.StoredProcedure));
                    if (MSG == "SUCCESSFULLY INSERT")
                    {
                        ro.Status = 1;
                        ro.Message = "Data Insert Successfully";
                    }
                    else
                    {
                        ro.Status = 0;
                        ro.Message = "Data not save!!";
                    }
                }
                hrm = new GenLib().RecvAPIData(ro);
                return hrm;

            }
            catch (Exception ex)
            {
                hrm = new GenLib().WriteErrorLog(ex);
                return hrm;
            }
        }
        


    }
}