using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

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
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient ajax = new HttpClient(handler);
      try {
        HttpResponseMessage res = await ajax.GetAsync(new Uri(url));
        if (res != null && res.StatusCode == HttpStatusCode.OK) {
          List<T> result = JsonConvert.DeserializeObject<List<T>>(await res.Content.ReadAsStringAsync());
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
