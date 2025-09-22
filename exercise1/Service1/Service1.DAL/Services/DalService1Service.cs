using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Service1.DAL.Repositories;
using System.Collections.Generic;

namespace Service1.DAL.Services
{
    public class DalService1Service : IDalService1Repository
    {
        public IDalLogRepository _log;
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly string service2 = "http://service2:8443";
        private readonly string storage = "http://storage:8444";

        private readonly string vstorage_dir = "/app/vStorage";
        public DalService1Service(IDalLogRepository log)
        {
            this._log = log;
        }

        public string getStatus()
        {
            try
            {
                double uptimeHours = GetContainerUptimeHours();
                long freeDiskMB = GetFreeDiskSpaceOnRootInMB();

                string response =  $"Timestamp1: uptime {uptimeHours:F2} hours, free disk in root: {freeDiskMB} Mbytes";
                var payload = new
                {
                    response = response
                };
                dynamic resStorageServer = CallApi(storage, "/log", "application/json", "POST", payload);
                if (resStorageServer != null && resStorageServer["success"] != null && resStorageServer["success"].ToObject<bool>())
                {
                    _log.SaveLog(response);
                    dynamic resService2Server = CallApi(service2, "/status", "application/json", "GET", null);
                    if (resService2Server != null && resService2Server["response"] != null)
                        response += "\n" + resService2Server["response"].ToObject<string>();
                }

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Stacktrace: {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public string getLogs()
        {
            try
            {
                var resStorageServer = CallApi(storage, "/log", "application/json", "GET", null);
                string logString = resStorageServer["log"].ToString(); 
                return logString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Stacktrace: {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        public bool deleteLogs()
        {
            try
            {
                var resStorageServer = CallApi(storage, "/log", "application/json", "DELETE", null);
                if (resStorageServer["success"].ToObject<bool>())
                {
                    using (var stream = new FileStream(vstorage_dir, FileMode.Truncate))
                    {
                        // File cleared
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Stacktrace: {ex.StackTrace}");
                throw new Exception(ex.Message);
            }
        }

        private static double GetContainerUptimeHours()
        {
            var uptimeText = File.ReadAllText("/proc/uptime");
            var uptimeSeconds = double.Parse(uptimeText.Split(' ')[0], System.Globalization.CultureInfo.InvariantCulture);
            return uptimeSeconds / 3600.0;
        }
        private static long GetFreeDiskSpaceOnRootInMB()
        {
            var drive = new DriveInfo("/");
            return drive.AvailableFreeSpace / (1024 * 1024);
        }
        private dynamic CallApi(string serverAddr, string endpoint, string contentType, string method, object obj)
        {
            string uri = serverAddr + endpoint;
            var json = JsonConvert.SerializeObject(obj);
            var httpMethod = new HttpMethod(method.ToUpper());

            using var request = new HttpRequestMessage(httpMethod, uri);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (httpMethod != HttpMethod.Get)
            {
                request.Content = new StringContent(json, Encoding.UTF8, contentType);
            }

            try
            {
                using var response = _httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status {response.StatusCode}: {responseBody}");
                }

                var jObj = JObject.Parse(responseBody);
                return jObj.ToObject<dynamic>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling API {uri}: {ex.Message}", ex);
            }
        }
    }
}
