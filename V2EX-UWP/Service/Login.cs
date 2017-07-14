using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net;
using System.Net.Http;

namespace V2EX.Service.Login {
  public class Service {
    public delegate void Resolve(bool isLogin);
    public delegate void Reject(string type);

    /// <summary>
    /// 是否处于登陆状态.
    /// </summary>
    public static bool isLogin { get; set; }

    /// <summary>
    /// Cookie 字符串.
    /// </summary>
    public static string cookie { get; set; }

    /// <summary>
    /// 登陆方法.
    /// V2EX 没有提供 API 登陆接口，需要解析网页获取 input 中的动态字段后提交登陆.
    /// </summary>
    public static async void login (string username, string password, Resolve resolve = null, Reject reject = null) {
      // 解析网页获取必要信息后发送登陆请求.
      HtmlWeb web = new HtmlWeb();
      var loginPage = await web.LoadFromWebAsync("https://www.v2ex.com/signin");

      string usernameKey = "";
      string passwordKey = "";
      string once = "";

      var inputNodes = loginPage.DocumentNode.Descendants("input").ToList();

      // 从登陆页面解析节点获取必要信息.
      try {
        // 获取 usernameKey.
        var usernameNode = inputNodes
            .Find(item => {
              return item.Attributes["placeholder"] != null
               ? item.Attributes["placeholder"].Value == "用户名或电子邮箱地址"
               : false;
            });

        usernameKey = usernameNode != null
          ? usernameNode.Attributes["name"].Value
          : "";

        // 获取 passwordKey.
        var passwordNode = inputNodes
            .Find(item => {
              return item.Attributes["type"] != null
                ? item.Attributes["type"].Value == "password"
                : false;
            });

        passwordKey = passwordNode != null
          ? passwordNode.Attributes["name"].Value
          : "";

        // 获取 once.
        var onceNode = inputNodes
          .Find(item => {
            return item.Attributes["name"] != null
              ? item.Attributes["name"].Value == "once"
              : false;
          });

        once = onceNode != null
          ? onceNode.Attributes["value"].Value
          : "";

        if (usernameKey == "" || passwordKey == "" || once == "") {
          throw new Exception("未在登陆页面获取到必要信息.");
        }

        // 创建发送数据.
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(usernameKey, username);
        data.Add(passwordKey, password);
        data.Add("once", once);
        data.Add("next", "/");
        FormUrlEncodedContent postData = new FormUrlEncodedContent(data);

        // 创建 Cookie Container.
        CookieContainer cookieCtnr = new CookieContainer();

        // 创建处理器.
        HttpClientHandler handler = new HttpClientHandler();
        handler.CookieContainer = cookieCtnr;

        // 创建客户端对象.
        Uri url = new Uri("https://www.v2ex.com/signin");
        HttpClient client = new HttpClient(handler);

        // 设置 Header.
        client.DefaultRequestHeaders.Host = "www.v2ex.com";
        client.DefaultRequestHeaders.Add("Origin", "https://www.v2ex.com");
        client.DefaultRequestHeaders.UserAgent.Clear();
        client.DefaultRequestHeaders.Remove("UserAgent");
        client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36 V2Explorer/0.1.0 (Windows UWP)");

        // 发送请求.
        HttpResponseMessage res = await client.PostAsync(url, postData);

        // 登陆成功.
        if (res != null && res.StatusCode == HttpStatusCode.OK) {
          // 将 Cookie 拼接为字符串并赋值给静态成员 cookie.
          var cookieList = cookieCtnr.GetCookies(url).Cast<Cookie>().ToList();

          // 拼接字符串.
          cookieList.ForEach(item => {
            cookie += item.ToString() + ";";
          });

          // 去除不必要引号.
          cookie = cookie.Replace("\"", "");

          // 登陆成功.
          isLogin = true;
          resolve?.Invoke(true);
        } else {
          // 其他情况均为失败.
          reject?.Invoke("logic");
        }
      } catch (Exception error) {
        reject?.Invoke("error");

      }
    }

    /// <summary>
    /// 退出方法.
    /// </summary>
    public static void logout () {
    }
  }
}
