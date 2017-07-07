using System;
using System.Collections.Generic;
using V2EX.Service.Http;

namespace V2EX.Service.Topic {
  public class Member {
    public int id { get; set; }
    public string username { get; set; }
    public string tagline { get; set; }
    public string avatar_mini { get; set; }
    public string avatar_normal { get; set; }
    public string avatar_large { get; set; }
  }

  public class Node {
    public int id { get; set; }
    public string name { get; set; }
    public string title { get; set; }
    public string title_alternative { get; set; }
    public string url { get; set; }
    public int topics { get; set; }
    public string avatar_mini { get; set; }
    public string avatar_normal { get; set; }
    public string avatar_large { get; set; }
  }

  /// <summary>
  /// 话题节点数据类型.
  /// 用于定义话题帖预览列表中的话题帖子.
  /// </summary>
  public class Topic {
    public int id { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public string content { get; set; }
    public string content_rendered { get; set; }
    public int replies { get; set; }
    public Member member { get; set; }
    public Node node { get; set; }
    public int created { get; set; }
    public int last_modified { get; set; }
    public int last_touched { get; set; }
  }

  /// <summary>
  /// 话题页面服务.
  /// </summary>
  /// 
  public class Service {
    public delegate void RequestResolve (List<Topic> topicList = null);
    public delegate void RequestReject (Exception error = null);

    /// <summary>
    /// HTTP 服务.
    /// </summary>
    private HttpRequest httpRequest { get; set; }

    /// <summary>
    /// 获取特定节点下话题列表.
    /// </summary>
    /// <param name="topicID"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public void getTopics (int topicID = 0, RequestResolve resolve = null, RequestReject reject = null) {
      try {
        // TODO; Request data.
        if (resolve != null) {
          resolve();
        }
      } catch (Exception error) {
        if (reject != null) {
          reject(error);
        }
      }
    }

    /// <summary>
    /// 获取最新话题列表.
    /// </summary>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public void getLatestTopics (RequestResolve resolve = null, RequestReject reject = null) {
      this.httpRequest.get<Topic>("https://www.v2ex.com/api/topics/latest.json", resolve, reject);
    }

    Service () {
      this.httpRequest = new HttpRequest();
    }
  }
}
