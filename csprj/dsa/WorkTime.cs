using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace csprj.dsa {
    public class WorkTime {
        private static Dictionary<string, JToken> GetDetails(string userId, string userPwd, string[] months) {
            var handler = new HttpClientHandler() { UseCookies = true };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.134 Safari/537.36 Edg/103.0.1264.71");
            var url = "http://ics.chinasoftinc.com/r1portal/login";
            var data = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("userName",userId),
                new KeyValuePair<string, string>("j_username",userId),
                new KeyValuePair<string, string>("password",userPwd),
                new KeyValuePair<string, string>("j_password",userPwd),
            });
            var response = client.PostAsync(url, data).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            url = Regex.Match(result, @"\'(.+)\'").Groups[1].Value;
            response = client.GetAsync(url).Result;

            var roltpaToken = handler.CookieContainer.GetCookies(new Uri("http://ics.chinasoftinc.com"))["ROLTPAToken"].Value;
            client.DefaultRequestHeaders.Add("ROLTPAToken", roltpaToken);
            url = "http://ics.chinasoftinc.com/elp/getUserProtocol";
            var json = new JObject();
            json["tacticsStatus"] = 2;
            response = client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, "application/json")).Result;
            result = response.Content.ReadAsStringAsync().Result;

            url = "http://ics.chinasoftinc.com:8010/sso/toLoginYellow";
            response = client.GetAsync(url).Result;
            result = response.Content.ReadAsStringAsync().Result;
        
            var empCode = Regex.Match(response.RequestMessage.RequestUri.OriginalString, @"empCode=(.+)$").Groups[1].Value;
            url = "http://ics.chinasoftinc.com:8010/ehr_saas/web/user/loginByEmpCode.jhtml";
            json = new JObject();
            json["empCode"] = empCode;
            response = client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, "application/json")).Result;
            result = response.Content.ReadAsStringAsync().Result;
            var result_jo = JObject.Parse(result);
            var token = result_jo["result"]["data"]["token"].ToString();

            url = "http://ics.chinasoftinc.com:8010/ehr_saas/web/icssAttEmpDetail/getLocSetDataByPage.empweb";
            client.DefaultRequestHeaders.Add("token", token);

            var res = new Dictionary<string, JToken>();
            foreach (var m in months) {
                response = client.PostAsync(url, new StringContent($"pageIndex=1&pageSize=100&search=%7B%22dt%22%3A%22{m}%22%7D", Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
                result = response.Content.ReadAsStringAsync().Result;
                res[m] = JObject.Parse(result)["result"]["data"]["page"]["items"];
            }

            return res;

        }

        private static double ComputeWorkTime(string from_dt, string to_dt) {
            if (from_dt == to_dt) return 0;
            var from_t = from_dt[^8..];
            var to_t = to_dt[^8..];

            var t0800 = "08:00:00";
            var t1200 = "12:00:00";
            var t1330 = "13:30:00";
            var t1730 = "17:30:00";
            var t1800 = "18:00:00";

            if (from_t.CompareTo(t0800) <= 0)
                from_t = t0800;
            else if (t1200.CompareTo(from_t) <= 0 && from_t.CompareTo(t1330) <= 0)
                from_t = t1330;
            else if (t1730.CompareTo(from_t) <= 0 && from_t.CompareTo(t1800) <= 0)
                from_t = t1800;

            if (to_t.CompareTo(t0800) <= 0)
                to_t = t0800;
            else if (t1200.CompareTo(to_t) <= 0 && to_t.CompareTo(t1330) <= 0)
                to_t = t1200;
            else if (t1730.CompareTo(to_t) <= 0 && to_t.CompareTo(t1800) <= 0)
                to_t = t1730;

            if (from_t.CompareTo(to_t) >= 0) return 0;

            var rest_hour = 0.0;
            if (from_t.CompareTo(t1200) <= 0 && t1330.CompareTo(to_t) <= 0 && to_t.CompareTo(t1730) <= 0)
                rest_hour = 1.5;
            else if (from_t.CompareTo(t1200) <= 0 && to_t.CompareTo(t1800) >= 0)
                rest_hour = 2;
            else if (t1330.CompareTo(from_t) <= 0 && from_t.CompareTo(t1730) <= 0 && to_t.CompareTo(t1800) >= 0)
                rest_hour = 0.5;

            from_dt = from_dt[..^8] + from_t;
            to_dt = to_dt[..^8] + to_t;
            var from_datetime = DateTime.Parse(from_dt);
            var to_datetime = DateTime.Parse(to_dt);
            return (to_datetime - from_datetime).TotalHours - rest_hour;
        }

        private static void compute(Dictionary<string, JToken> months_data) {
            foreach (var (m, records) in months_data) {
                var data = new Dictionary<string, List<string>>();
                var today = DateTime.Now.ToString("yyyy-MM-dd");
                foreach (var r in records) {
                    var dt = r["dt"].ToString();
                    if (!data.ContainsKey(dt))
                        data[dt] = new List<string>();
                    data[dt].Add(r["checktime"].ToString());
                }

                var dailyWorkTimes = new Dictionary<string, double>();
                foreach (var (dt, checktimelist) in data) {
                    if (dt == today && checktimelist.Count == 1) continue;
                    dailyWorkTimes[dt] = ComputeWorkTime(checktimelist.Min(), checktimelist.Max());
                }

                var totalWorkTime = dailyWorkTimes.Sum((x) => x.Value);
                var daysCount = dailyWorkTimes.Count;
                if (daysCount == 0) return;
                var averageWorkTime = totalWorkTime / daysCount;
                var lackTime = daysCount * 8 - totalWorkTime;
                var sep = "****************************************************************************";
                Console.WriteLine(sep);
                Console.WriteLine($"{m}的数据：");
                Console.WriteLine($"出勤天数：{daysCount}");
                Console.WriteLine($"总工时：{totalWorkTime}");
                Console.WriteLine($"平均工时：{averageWorkTime}");
                if (lackTime < 0)
                    Console.WriteLine($"超出标准工时：{-lackTime}小时，即{-lackTime * 60}分钟");
                else
                    Console.WriteLine($"缺工时：{lackTime}小时，即{lackTime * 60}分钟");

                lackTime += daysCount * 0.5;
                if (lackTime < 0)
                    Console.WriteLine($"已满足8.5，且超过{-lackTime}小时，即{-lackTime * 60}分钟");
                else
                    Console.WriteLine($"平均工时要达到8.5，缺工时：{lackTime}小时，即{lackTime * 60}分钟");

                var exceptionData = new Dictionary<string, List<string>>();
                foreach (var (dt, checktimelist) in data) {
                    if (checktimelist.Min()[^8..].CompareTo("09:00:00") > 0 || checktimelist.Max()[^8..].CompareTo("17:30:00") < 0 && today != dt)
                        exceptionData[dt] = checktimelist;
                }

                if (exceptionData.Count > 0) {
                    foreach (var (dt, checktimelist) in exceptionData) {
                        Console.WriteLine($"{dt}: {string.Join(",", checktimelist)}");
                    }
                }

                Console.WriteLine(sep);
            }
        }

        public static void Compute() {
            var user_id = "335524";
            var user_pwd = "";
            Console.Write("请输入月份(如2022-07, 不输入的话，默认当月):");
            var monthsStr = Console.ReadLine();
            if (monthsStr.Length == 0)
                monthsStr = DateTime.Now.ToString("yyyy-MM");

            var months = Regex.Split(monthsStr, @"[,\s]+");

            var monthsData = GetDetails(user_id, user_pwd, months);

            compute(monthsData);

            Console.Write("按任意键退出...");
            Console.ReadKey();
        }
    }
}