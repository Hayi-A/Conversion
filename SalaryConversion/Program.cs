using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Net;
using System.IO;
using System.Globalization;

namespace SalaryConversion
{
    class Program
    {

        public class ArrayData
        {
            public List<SalaryData> Array { get; set; }
        }
            public class SalaryData
        {
            public string salaryInIDR { get; set; }
            public int id { get; set; }
        }
        public class geo
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }
        public class address
        {
            public string street { get; set; }
            public string suite { get; set; }
            public string city { get; set; }
            public string zipcode { get; set; }
            public geo geo { get; set; }
        }

        public class company
        {            
            public string name { get; set; }
            public string catchPhrase { get; set; }
            public string bs { get; set; }
        }
        public class CON_USD_IDR
        {
            public string USD_IDR { get; set; }
        }
            public class EmployeeData
        {            
            public int id { get; set; }
            public string name { get; set; }
            public string username { get; set; }
            public string email { get; set; }            
            public address address { get; set; }
            public string phone { get; set; }
            public string website { get; set; }
            public company company { get; set; }
        }
        public class JsonResultsEmployeeData
        {
            public int id { get; set; }
            public string name { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public address address { get; set; }
            public string phone { get; set; }
            public string website { get; set; }
            public company company { get; set; }
            public string salary_IDR { get; set; }
            public string salary_USD { get; set; }

        }
        static void Main(string[] args)
        {
            string url = "http://jsonplaceholder.typicode.com/users";
            string urlcurrconv = "https://free.currconv.com/api/v7/convert?q=USD_IDR&compact=ultra&apiKey=0bfcd41e62614ab0ebe6";

            var json = new WebClient().DownloadString(url);
            var jsoncurrconv = new WebClient().DownloadString(urlcurrconv);

            var usd_idr  = JsonConvert.DeserializeObject<CON_USD_IDR>(jsoncurrconv);
            double conidr = Convert.ToDouble(usd_idr.USD_IDR);
            var EmployeeData = JsonConvert.DeserializeObject<List<EmployeeData>>(json);

            List<JsonResultsEmployeeData> Results = new List<JsonResultsEmployeeData>();

            string workingDirectory = Environment.CurrentDirectory + "\\json_file\\salary_data.json";
            using (StreamReader r = new StreamReader(workingDirectory))
            {
                var FileJson = r.ReadToEnd();
                var SalaryArray = JsonConvert.DeserializeObject<ArrayData>(FileJson);
                
                foreach (var EmpData in EmployeeData)
                {
                    foreach (var SalArray in SalaryArray.Array.Where(n => n.id == EmpData.id))
                    {
                        JsonResultsEmployeeData JoinData = new JsonResultsEmployeeData();
                        JoinData.id = EmpData.id;
                        JoinData.name = EmpData.name;
                        JoinData.username = EmpData.username;
                        JoinData.email = EmpData.email;
                        JoinData.address = EmpData.address;
                        JoinData.phone = EmpData.phone;
                        JoinData.website = EmpData.website;
                        JoinData.company = EmpData.company;                                                
                        JoinData.salary_IDR = SalArray.salaryInIDR;
                        double SalIDR = Convert.ToDouble(SalArray.salaryInIDR);
                        JoinData.salary_USD = (SalIDR * conidr).ToString();                        
                        Results.Add(JoinData);
                    }
                }
            }
            var jsonObj = JsonConvert.SerializeObject(Results); 
            Console.WriteLine(jsonObj.ToString());
            Console.ReadLine();
            

        }


    }
}
