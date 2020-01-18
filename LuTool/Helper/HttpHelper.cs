using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;

namespace LuTool
{
    /// <summary>
    /// 网络请求帮助类
    /// </summary>
    public class HttpHelper
    {
        #region property

        /// <summary>
        /// 请求超时设置（以毫秒为单位），默认为10秒。
        /// 说明：此处常量专为提供给方法的参数的默认值，不是方法内所有请求的默认超时时间。
        /// </summary>
        public const int Request_Time_Out = 10000;

        #endregion property

        #region Get

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Get(url, null, out HttpStatusCode statusCode);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="statusCode">Http响应状态码</param>
        /// <returns></returns>
        public static string Get(string url, out HttpStatusCode statusCode)
        {
            return Get(url, null, out statusCode);
        }

        /// <summary>
        /// Get请求-具有Basic验证
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="header">选填，请求头</param>
        /// <returns></returns>
        public static string GetWithBasic(string url, string name, string pwd, Dictionary<string, string> header = null)
        {
            string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", name, pwd)));
            if (header == null || header.Count == 0)
            {
                header = new Dictionary<string, string> { { "Authorization", "Basic " + code } };
            }
            else
            {
                if (header.ContainsKey("Authorization"))
                    header["Authorization"] = "Basic " + code;
                else
                    header.Add("Authorization", "Basic " + code);
            }
            return Get(url, header, out HttpStatusCode statusCode);
        }

        /// <summary>
        /// Get请求-返回实体
        /// </summary>
        /// <typeparam name="T">返回数据实体</typeparam>
        /// <param name="url">请求Url地址</param>
        /// <returns></returns>
        public static T Get<T>(string url) where T : class, new()
        {
            T result = default(T);

            var resp = Get(url, out HttpStatusCode statusCode);
            if (string.IsNullOrEmpty(resp))
                return result;
            return JsonConvert.DeserializeObject<T>(resp);
        }

        public static string Get(string url, Dictionary<string, string> header)
        {
            return Get(url, header, out HttpStatusCode statusCode);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="header">额外请求头数据</param>
        /// <param name="statusCode">Http响应状态码</param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, string> header, out HttpStatusCode statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (header != null && header.Count > 0)
            {
                foreach (var key in header.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, header[key]);
                }
            }
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                httpClient.Dispose();
                statusCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
            catch (Exception ex)
            {
                var msg = $@"发送Get请求异常。请求URL：{url}。请求Header头：{JsonConvert.SerializeObject(header)}";
                throw new Exception(msg, ex);
            }
        }

        #endregion Get

        #region Post

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="postData">post数据</param>
        /// <param name="contentType">请求内容的格式</param>
        /// <returns></returns>
        public static string Post(string url, string postData, string contentType = "application/json")
        {
            return Post(url, postData, null, out HttpStatusCode statusCode, contentType);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="postData">post数据</param>
        /// <param name="statusCode">Http响应状态码</param>
        /// <param name="contentType">请求内容的格式</param>
        /// <returns></returns>
        public static string Post(string url, string postData, out HttpStatusCode statusCode, string contentType = "application/json")
        {
            return Post(url, postData, null, out statusCode, contentType);
        }

        /// <summary>
        /// Post请求-返回实体
        /// </summary>
        /// <typeparam name="T">返回数据实体</typeparam>
        /// <param name="url">请求Url地址</param>
        /// <param name="postData">post数据</param>
        /// <param name="header">额外请求头数据</param>
        /// <param name="contentType">请求内容的格式</param>
        /// <returns></returns>
        public static T Post<T>(string url, string postData, Dictionary<string, string> header = null, string contentType = "application/json") where T : class, new()
        {
            T result = default(T);

            var resp = Post(url, postData, header, out HttpStatusCode statusCode, contentType);
            if (string.IsNullOrEmpty(resp))
                return result;
            return JsonConvert.DeserializeObject<T>(resp);
        }

        /// <summary>
        /// Post请求-具有Basic验证
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="header"></param>
        /// <param name="contentType">请求内容的格式</param>
        /// <returns></returns>
        public static string PostWithBasic(string url, string postData, string name, string pwd, Dictionary<string, string> header = null, string contentType = "application/json")
        {
            string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", name, pwd)));
            if (header == null || header.Count == 0)
            {
                header = new Dictionary<string, string> { { "Authorization", "Basic " + code } };
            }
            else
            {
                if (header.ContainsKey("Authorization"))
                    header["Authorization"] = "Basic " + code;
                else
                    header.Add("Authorization", "Basic " + code);
            }
            return Post(url, postData, header, out HttpStatusCode statusCode, contentType);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="postData">post数据</param>
        /// <param name="header">额外请求头数据</param>
        /// <param name="statusCode">Http响应状态码</param>
        /// <param name="contentType">请求内容的格式</param>
        /// <returns></returns>
        public static string Post(string url, string postData, Dictionary<string, string> header, out HttpStatusCode statusCode, string contentType = "application/json")
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            try
            {
                if (postData.IsNullOrEmpty()) postData = "";
                HttpContent httpContent = new StringContent(postData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                httpContent.Headers.ContentType.CharSet = "utf-8";

                //if (header != null && header.Count > 0)
                //{
                //    foreach (var key in header.Keys)
                //    {
                //        httpContent.Headers.Add(key, header[key]);
                //    }
                //}

                HttpClient httpClient = new HttpClient();

                if (header != null && header.Count > 0)
                {
                    foreach (var key in header.Keys)
                    {
                        httpClient.DefaultRequestHeaders.Add(key, header[key]);
                    }
                }
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

                httpClient.Dispose();
                httpContent.Dispose();

                statusCode = response.StatusCode;
                //if (response.IsSuccessStatusCode)
                //{
                //    string result = response.Content.ReadAsStringAsync().Result;
                //    return result;
                //}
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                var msg = $@"发送Post请求异常。请求URL：{url}。请求Header头：{JsonConvert.SerializeObject(header)}。Body内容：{postData}";
                throw new Exception(msg, ex);
            }
        }

        #endregion Post

        #region Patch

        /// <summary>
        /// Patch请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="data">请求数据</param>
        /// <returns></returns>
        public static string PATCH(string url, string data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "PATCH";

            byte[] btBodys = Encoding.UTF8.GetBytes(data);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            try
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();

                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();

                return responseContent;
            }
            catch (Exception ex)
            {
                var msg = $@"发送Post请求异常。请求URL：{url}。请求Header头： 。数据：{data}";
                throw new Exception(msg, ex);
            }
        }

        #endregion Patch

        #region Put

        /// <summary>
        /// Put请求
        /// </summary>
        /// <typeparam name="T">返回数据实体</typeparam>
        /// <param name="url">请求Url地址</param>
        /// <param name="data">请求数据</param>
        /// <returns></returns>
        public static T Put<T>(string url, string data)
        {
            T result = default(T);

            var resp = Put(url, data);
            if (string.IsNullOrEmpty(resp))
                return result;
            return resp.ToObject<T>();
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="data">请求数据</param>
        /// <returns></returns>
        public static string Put(string url, string data)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            try
            {
                HttpContent httpContent = new StringContent(data);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };

                var httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.PutAsync(url, httpContent).Result;
                httpClient.Dispose();
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
            catch (Exception ex)
            {
                var msg = $@"发送Put请求异常。请求URL：{url}。请求Header头：。Body内容：{data}";
                throw new Exception(msg, ex);
            }
        }

        #endregion Put

        #region Delete

        /// <summary>
        /// Delete-请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <returns></returns>
        public static string Delete(string url)
        {
            return Delete(url, out HttpStatusCode statusCode);
        }

        /// <summary>
        /// Delete-请求
        /// </summary>
        /// <typeparam name="T">返回数据实体</typeparam>
        /// <param name="url">请求Url地址</param>
        /// <returns></returns>
        public static T Delete<T>(string url)
        {
            T result = default(T);

            var resp = Delete(url, out HttpStatusCode statusCode);
            if (string.IsNullOrEmpty(resp))
                return result;
            return resp.ToObject<T>();
        }

        /// <summary>
        /// Delete-请求
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="statusCode">Http响应状态码</param>
        /// <returns></returns>
        public static string Delete(string url, out HttpStatusCode statusCode)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            try
            {
                var httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.DeleteAsync(url).Result;

                httpClient.Dispose();
                statusCode = response.StatusCode;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                var msg = $@"发送Delete请求异常。请求URL：{url}。请求Header头：。";
                throw new Exception(msg, ex);
            }
        }

        #endregion Delete

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileUrl"></param>
        /// <param name="filename"></param>
        /// <param name="name"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string PostFile(string url, string fileUrl, string filename, string name = "media", string encode = "UTF-8")
        {
            WebClient my = new WebClient();
            byte[] imgbyte = my.DownloadData(fileUrl);
            Stream ms = new MemoryStream(imgbyte);
            return PostFile(url, ms, filename, name, encode);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ms"></param>
        /// <param name="filename"></param>
        /// <param name="name"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string PostFile(string url, Stream ms, string filename, string name = "media", string encode = "UTF-8")
        {
            HttpContent hc = null;
            var key = StringHelper.GetGuid(true);

            if (true)
            {
                //通过表单上传文件
                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                var multipartFormDataContent = new MultipartFormDataContent(boundary);
                hc = multipartFormDataContent;

                //写入文件,这里有些问题，需要再次测试
                if (ms != null)
                {
                    //存在文件
                    var memoryStream = new MemoryStream();
                    ms.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //multipartFormDataContent.Add(new StreamContent(memoryStream), file.Key, Path.GetFileName(fileName)); //报流已关闭的异常

                    multipartFormDataContent.Add(CreateFileContent(memoryStream, key, filename), key, filename);
                    ms.Dispose();
                }
                else
                {
                    //不存在文件或只是注释
                    multipartFormDataContent.Add(new StringContent(""), "\"" + key + "\"");
                }

                hc.Headers.ContentType = MediaTypeHeaderValue.Parse(string.Format("multipart/form-data; boundary={0}", boundary));
            }
            else
            {
                hc = new StreamContent(ms);

                hc.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

                //使用Url格式Form表单Post提交的时候才使用application/x-www-form-urlencoded
                //去掉注释以测试Request.Body为空的情况
                //hc.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            }

            HttpClient client = new HttpClient();
            var response = client.PostAsync(url, hc).GetAwaiter().GetResult();

            try
            {
                ms.Close();

                client.Dispose();
                hc.Dispose();//关闭HttpContent（StreamContent）
            }
            catch (Exception ex)
            {
                var msg = $@"发送PostFile请求异常。请求URL：{url}。请求Header头：。filename：{filename}";
                throw new Exception(msg, ex);
            }
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        #region 微信sdk方法，未发布

        /// <summary>
        /// 使用Post方法获取HttpWebResponse或HttpResponseMessage对象，本方法独立使用时通常用于测试）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream"></param>
        /// <param name="fileDictionary">需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="encoding"></param>
        /// <param name="cer">证书，如果不需要则保留null</param>
        /// <param name="useAjax"></param>
        /// <param name="timeOut"></param>
        /// <param name="refererUrl"></param>
        /// <returns></returns>
        public static string HttpResponsePost(string url, CookieContainer cookieContainer = null, Stream postStream = null,
            Dictionary<string, string> fileDictionary = null, string refererUrl = null, Encoding encoding = null,
            X509Certificate2 cer = null, bool useAjax = false, int timeOut = Request_Time_Out)
        {
            if (cookieContainer == null)
            {
                cookieContainer = new CookieContainer();
            }

            var postStreamIsDefaultNull = postStream == null;
            if (postStreamIsDefaultNull)
            {
                postStream = new MemoryStream();
            }

            HttpContent hc;
            var client = HttpPostClient(url, out hc, cookieContainer, postStream, fileDictionary, refererUrl, encoding, cer, useAjax, timeOut);

            var response = client.PostAsync(url, hc).GetAwaiter().GetResult();

            try
            {
                if (postStreamIsDefaultNull && postStream.Length > 0)
                {
                    postStream.Close();
                }

                hc.Dispose();//关闭HttpContent（StreamContent）
                var retString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return retString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 给.NET Core使用的HttpPost请求公共设置方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="hc"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream">优先文件字典，如果不是文件则是xml流</param>
        /// <param name="fileDictionary"></param>
        /// <param name="refererUrl"></param>
        /// <param name="encoding"></param>
        /// <param name="cer"></param>
        /// <param name="useAjax"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static HttpClient HttpPostClient(string url, out HttpContent hc, CookieContainer cookieContainer = null,
        Stream postStream = null, Dictionary<string, string> fileDictionary = null, string refererUrl = null,
        Encoding encoding = null, X509Certificate2 cer = null, bool useAjax = false, int timeOut = Request_Time_Out)
        {
            var handler = new HttpClientHandler()
            {
                UseProxy = false,
                Proxy = null,
                UseCookies = true,
                CookieContainer = cookieContainer,
            };

            if (cer != null)
            {
                handler.ClientCertificates.Add(cer);
            }

            HttpClient client = new HttpClient(handler);
            HttpClientHeader(client, refererUrl, useAjax, timeOut);

            #region 处理Form表单文件上传

            var formUploadFile = fileDictionary != null && fileDictionary.Count > 0;//是否用Form上传文件
            if (formUploadFile)
            {
                //通过表单上传文件
                string boundary = "----" + DateTime.Now.Ticks.ToString("x");

                var multipartFormDataContent = new MultipartFormDataContent(boundary);
                hc = multipartFormDataContent;

                foreach (var file in fileDictionary)
                {
                    try
                    {
                        var fileName = file.Value;
                        //准备文件流
                        using (var fileStream = FileHelper.GetFileStream(fileName))
                        {
                            if (fileStream != null)
                            {
                                //存在文件
                                var memoryStream = new MemoryStream();
                                fileStream.CopyTo(memoryStream);
                                memoryStream.Seek(0, SeekOrigin.Begin);

                                //multipartFormDataContent.Add(new StreamContent(memoryStream), file.Key, Path.GetFileName(fileName)); //报流已关闭的异常

                                multipartFormDataContent.Add(CreateFileContent(memoryStream, file.Key, Path.GetFileName(fileName)), file.Key, Path.GetFileName(fileName));
                                fileStream.Dispose();
                            }
                            else
                            {
                                //不存在文件或只是注释
                                multipartFormDataContent.Add(new StringContent(file.Value), "\"" + file.Key + "\"");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                hc.Headers.ContentType = MediaTypeHeaderValue.Parse(string.Format("multipart/form-data; boundary={0}", boundary));
            }
            else
            {
                hc = new StreamContent(postStream);

                hc.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

                //使用Url格式Form表单Post提交的时候才使用application/x-www-form-urlencoded
                //去掉注释以测试Request.Body为空的情况
                //hc.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            }

            //HttpContentHeader(hc, timeOut);

            #endregion 处理Form表单文件上传

            if (!string.IsNullOrEmpty(refererUrl))
            {
                client.DefaultRequestHeaders.Referrer = new Uri(refererUrl);
            }

            return client;
        }

        /// <summary>
        /// 设置HTTP头
        /// </summary>
        /// <param name="client"></param>
        /// <param name="refererUrl"></param>
        /// <param name="useAjax">是否使用Ajax</param>
        /// <param name="timeOut"></param>
        private static void HttpClientHeader(HttpClient client, string refererUrl, bool useAjax, int timeOut)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

            //HttpContent hc = new StringContent(null);
            //HttpContentHeader(hc, timeOut);

            //httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla","5.0 (Windows NT 10.0; WOW64)"));
            //httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "537.36 (KHTML, like Gecko)"));
            //httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "61.0.3163.100 Safari/537.36"));

            //httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36"));

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            client.DefaultRequestHeaders.Add("Timeout", timeOut.ToString());
            client.DefaultRequestHeaders.Add("KeepAlive", "true");

            if (!string.IsNullOrEmpty(refererUrl))
            {
                client.DefaultRequestHeaders.Referrer = new Uri(refererUrl);
            }

            if (useAjax)
            {
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            }
        }

        private static StreamContent CreateFileContent(Stream stream, string formName, string fileName, string contentType = "application/octet-stream")
        {
            fileName = fileName.UrlEncode();
            var fileContent = new StreamContent(stream);
            //上传格式参考：
            //https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1444738729
            //https://work.weixin.qq.com/api/doc#10112
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{formName}\"",
                FileName = "\"" + fileName + "\"",
                Size = stream.Length
            }; // the extra quotes are key here
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return fileContent;
        }

        #endregion 微信sdk方法，未发布
    }
}