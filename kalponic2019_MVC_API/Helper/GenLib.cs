using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
//using System.Web.Script.Serialization;

namespace kalponic2019_MVC_API.Helper
{
    public class GenLib
    {
        public string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }

        public string ObjToJsonWithJsonNet(object obj)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(obj);
            return jsonString;
        }

        public string GetUserIP()
        {
            string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        public object JsonToObj(string obj)
        {
            var returnValue = JsonConvert.DeserializeObject(obj);
            return returnValue;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public DataTable ToDataTableWithRowID<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            //Set AutoIncrement True for the First Column.
            dataTable.Columns["RowID"].AutoIncrement = true;

            //Set the Starting or Seed value.
            dataTable.Columns["RowID"].AutoIncrementSeed = 1;

            //Set the Increment value.
            dataTable.Columns["RowID"].AutoIncrementStep = 1;
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public HttpResponseMessage RecvAPIData(ResponseObj obj)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string jsonString = new GenLib().ObjToJsonWithJsonNet(obj);
            StringContent sc = new StringContent(jsonString);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }

        public HttpResponseMessage WriteErrorLog(System.Exception ex)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            obj.Status = 0;
            obj.Message = Convert.ToString("Error, try agian later!");
            string jsonString = objLib.ObjToJsonWithJsonNet(obj);
            StringContent sc = new StringContent(jsonString);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        public string CallToAPI(string ApiUrl)
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string json = client.UploadString(ApiUrl, "POST");
            return json;
        }

        public string CallToAPI(string ApiUrl, object input)
        {
            string inputJson = new JavaScriptSerializer().Serialize(input);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string json = client.UploadString(ApiUrl, "POST", inputJson);
            return json;
        }
    }
}