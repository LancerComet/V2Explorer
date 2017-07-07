using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Web.Http;
using Newtonsoft.Json;

namespace V2EX.Service.Node {
  /// <summary>
  /// 话题节点数据类型. 用于定义单个节点.
  /// </summary>
  public class Node {
    public int id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public string title_alternative { get; set; }
    public int topics { get; set; }
    public string header { get; set; }
    public string footer { get; set; }
    public int created { get; set; }
  }

  /// <summary>
  /// 节点服务.
  /// </summary>
  public class Service : INotifyPropertyChanged {
    /// <summary>
    /// 所有节点数据.
    /// </summary>
    private List<Node> _allNodes;
    public List<Node> allNodes {
      get {
        return this._allNodes;
      }
      set {
        this._allNodes = value;
        this.notify("allNodes");
      }
    }

    /// <summary>
    /// 请求回调委托定义.
    /// </summary>
    public delegate void RequestCallback();

    /// <summary>
    /// 获取所有节点.
    /// </summary>
    /// 
    public async void getAllNodes(RequestCallback resolve = null, RequestCallback reject = null) {
      HttpClient client = new HttpClient();
      try {
        HttpResponseMessage res = await client.GetAsync(new Uri("https://www.v2ex.com/api/nodes/all.json"));
        if (res != null && res.StatusCode == HttpStatusCode.Ok) {
          var result = JsonConvert.DeserializeObject<List<Node>>(res.Content.ToString());  // 使用 JSON.NET 将 JSON 字符串转为 List<Node>.
          this.allNodes = result;
          resolve?.Invoke();
        }
      } catch {
        // TODO: 错误提示.
        reject?.Invoke();
      }
    }

    /// <summary>
    /// 实现 Notify 接口通知视图更新.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify (string propertyName) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
