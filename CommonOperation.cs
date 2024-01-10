using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GenerateKeyforPartners
{
    public class CommonOperation
    {
        public string ComputeHashFromJsonSMS(string jsonstr)
        {
            string finaloutput = string.Empty;
            string CheckSumKey = ConfigurationManager.AppSettings["SMSKey"];
            byte[] key = Encoding.UTF8.GetBytes(CheckSumKey);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonstr);
            HMACSHA256 hashstring = new HMACSHA256(key);
            byte[] hash = hashstring.ComputeHash(bytes);
            finaloutput = Convert.ToBase64String(hash);


            return finaloutput;
        }

        public async Task<string> ConsumeSMSServiceAPI(string Request, string HmacChecksum)
        {
            try
            {
                string Url = string.Empty;
                dynamic _responseJson = string.Empty;
                Url = ConfigurationManager.AppSettings["SendSMS"];
                using (HttpClientHandler Handler = new HttpClientHandler())
                using (HttpClient client = new HttpClient(Handler))
                {
                    client.DefaultRequestHeaders.Add("x-HMAC-CS", HmacChecksum);
                    var content = new StringContent(Request, Encoding.UTF8, "application/json");
                    var APIResponse = client.PostAsync(Url, content).Result;
                    _responseJson = APIResponse.Content.ReadAsStringAsync().Result;
                }
                return _responseJson;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}