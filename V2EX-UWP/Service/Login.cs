using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace V2EX.Service.Login {
  public abstract class Account {
    /// <summary>
    /// 是否处于登陆状态.
    /// </summary>
    public static bool isLogin {
      get {
        return Account.accountInfo.username != "" && Account.accountInfo.username != null;
      }
    }

    /// <summary>
    /// Cookie 字符串.
    /// </summary>
    public static string cookie { get; set; }

    /// <summary>
    /// 用户账号数据.
    /// </summary>
    public static Member.ModelLoginMember accountInfo = new Member.ModelLoginMember() {
      username = "",
      avatarURL = "",
      url = ""
    };
  }

  /// <summary>
  /// V2ex 登陆预置数据.
  /// usernameKey, passwordKey, captchaKey 作为 Post 请求发送时的表单 Name.
  /// </summary>
  public class V2EXLoginPeparingData {
    public string usernameKey { get; set; }
    public string passwordKey { get; set; }
    public string captchaKey { get; set; }
    public string once { get; set; }
    public string captchaUrl {
      get {
        return "https://www.v2ex.com/_captcha?once=" + this.once;
      }
    }
  }

  public abstract class Service {
    /// <summary>
    /// 访问 V2EX 登陆页面获取必要信息.
    /// </summary>
    /// <returns></returns>
    public static async Task<V2EXLoginPeparingData> getLoginPresetData () {
      HtmlWeb web = new HtmlWeb();
      var loginPage = await web.LoadFromWebAsync("https://www.v2ex.com/signin");

      string usernameKey = "";
      string passwordKey = "";
      string captchaKey = "";
      string once = "";

      var inputNodes = loginPage.DocumentNode.Descendants("input").ToList();

      // 从登陆页面解析节点获取必要信息.
      try {
        // 获取 usernameKey.
        var usernameNode = inputNodes.Find(item => {
          return item.Attributes["placeholder"] != null
            ? item.Attributes["placeholder"].Value == "用户名或电子邮箱地址"
            : false;
        });

        usernameKey = usernameNode != null
          ? usernameNode.Attributes["name"].Value
          : "";

        // 获取 passwordKey.
        var passwordNode = inputNodes.Find(item => {
          return item.Attributes["type"] != null
            ? item.Attributes["type"].Value == "password"
            : false;
        });

        passwordKey = passwordNode != null
          ? passwordNode.Attributes["name"].Value
          : "";

        // 获取 CaptchaKey.
        var captchaInputNode = inputNodes.Find(item => {
          return item.Attributes["placeholder"] != null
            ? item.Attributes["placeholder"].Value == "请输入上图中的验证码"
            : false;
        });

        captchaKey = captchaInputNode != null
          ? captchaInputNode.Attributes["name"].Value
          : "";

        // 获取 once.
        var onceNode = inputNodes.Find(item => {
          return item.Attributes["name"] != null
            ? item.Attributes["name"].Value == "once"
            : false;
        });

        once = onceNode != null
          ? onceNode.Attributes["value"].Value
          : "";

        if (usernameKey == "" || passwordKey == "" || captchaKey == "" || once == "") {
          throw new Exception("DATA_NOT_ENOUGH");
        }

        return new V2EXLoginPeparingData() {
          usernameKey = usernameKey,
          passwordKey = passwordKey,
          captchaKey = captchaKey,
          once = once
        };
      } catch (Exception error) {
        throw error;
      }
    }

    /// <summary>
    /// 登陆请求函数.
    /// </summary>
    public static async Task<string> login (
      string username, string password,
      string once, string captcha,
      string usernameKey, string passwordKey, string captchaKey
    ) {
      try {
        // 创建发送数据.
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(usernameKey, username);
        data.Add(passwordKey, password);
        data.Add(captchaKey, captcha);
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
          // 将 Cookie 拼接为字符串.
          var cookieList = cookieCtnr.GetCookies(url).Cast<Cookie>().ToList();
          var cookie = "";

          // 拼接字符串.
          cookieList.ForEach(item => {
            cookie += item.ToString() + ";";
          });

          // 去除不必要引号.
          cookie = cookie.Replace("\"", "");

          // 登陆成功.
          return cookie;
        } else {
          // 其他情况均为失败.
          throw new Exception("SERVER_RESPONSE_" + res.StatusCode);
        }
      } catch (Exception error) {
        throw error;
      }
    }

    /// <summary>
    /// 退出方法.
    /// </summary>
    public static void logout () {
    }
  }
}
