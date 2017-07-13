using System;
using System.Collections.Generic;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web.Http.Headers;

namespace V2EX.Service.Http {
   public class HttpRequest {
    /// <summary>
    /// 发送 Get 请求.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public void get<T> (string url, Delegate resolve, Delegate reject) {
      this.doGetRequest<T>(url, resolve, reject);
    }

    /// <summary>
    /// GET 请求发送函数.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    private async void doGetRequest<T> (string url, Delegate resolve = null, Delegate reject = null) {
      HttpClient ajax = new HttpClient();
      try {
        HttpResponseMessage res = await ajax.GetAsync(new Uri(url));
        if (res != null && res.StatusCode == HttpStatusCode.Ok) {
          List<T> result = JsonConvert.DeserializeObject<List<T>>(res.Content.ToString());
          resolve?.DynamicInvoke(result);
        } else {
          throw new Exception("Failed to request " + url);
        }
      } catch (Exception error) {
        reject?.DynamicInvoke(error);
      }
    }
  }
}
