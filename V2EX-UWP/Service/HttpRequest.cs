using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace V2EX.Service.Http {
  public class HttpRequest {
    /// <summary>
    /// 通过 Get 请求方式获取单条数据.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public async Task<T> getSingle<T> (string url) {
      try {
        var result = await this.get<T>(url);
        return result[0];
      } catch (Exception error) {
        throw error;
      }
    }

    /// <summary>
    /// GET 请求发送函数.
    /// 请注意 JSON 库将试图解析多个数据, 就算接口返回一个对象也会存入一个 List 中.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public async Task<List<T>> get<T>(string url) {
      HttpClientHandler handler = new HttpClientHandler();
      HttpClient ajax = new HttpClient(handler);
      try {
        HttpResponseMessage res = await ajax.GetAsync(new Uri(url));
        if (res != null && res.StatusCode == HttpStatusCode.OK) {
          List<T> result = JsonConvert.DeserializeObject<List<T>>(await res.Content.ReadAsStringAsync());
          return result;
        } else {
          throw new Exception("Failed to request " + url);
        }
      } catch (Exception error) {
        throw error;
      }
    }
  }
}
