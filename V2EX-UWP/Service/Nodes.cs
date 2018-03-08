using System;
using System.Collections.Generic;
using System.ComponentModel;
using V2EX.Service.Http;

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
  /// 简化节点类型.
  /// </summary>
  public class NodeSimple {
    public string name { get; set; }
    public string label { get; set; }
    public NodeSimple () { }
  }

  /// <summary>
  /// 节点服务.
  /// </summary>
  public class Service {
    /// <summary>
    /// 请求回调委托定义.
    /// </summary>
    public delegate void Resolve(List<Node> list);
    public delegate void Reject(Exception error);

    /// <summary>
    /// Http 服务.
    /// </summary>
    private HttpRequest httpRequest { get; set; }

    /// <summary>
    /// 获取所有节点.
    /// </summary>
    public void getAllNodes (Resolve resolve = null, Reject reject = null) {
      httpRequest.get<Node>("https://www.v2ex.com/api/nodes/all.json", resolve, reject);
    }

    /// <summary>
    /// 实现 Notify 接口通知视图更新.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify (string propertyName) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Service () {
      this.httpRequest = new HttpRequest();
    }
  }
}
