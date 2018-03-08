using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using V2EX.Service.Http;

namespace V2EX.Service.Topic {
  public class Member {
    public string username { get; set; }
    public string avatarURL { get; set; }
    public string url { get; set; }
  }

  /// <summary>
  /// 话题节点数据类型.
  /// 用于定义话题帖预览列表中的话题帖子.
  /// </summary>
  public class Topic {
    public string id { get; set; }
    public string title { get; set; }
    public string url { get; set; }
    public string content { get; set; }
    public string content_rendered { get; set; }
    public int replies { get; set; }
    public Member member { get; set; }
    public Node.NodeSimple node { get; set; }
    public string lastRepliedTime { get; set; }
    public string lastRepliedUser { get; set; }
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
    private HttpRequest httpRequest {
      get {
        return new HttpRequest();
      }
    }

    /// <summary>
    /// 获取特定节点下话题列表.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public async void getTopics (string nodeName, RequestResolve resolve = null, RequestReject reject = null) {
      // 读取 V2EX 首页后解析节点并返回数据.
      HtmlWeb web = new HtmlWeb();

      try {
        HtmlDocument topicPage = await web.LoadFromWebAsync("https://www.v2ex.com/?tab=" + nodeName);

        // 帖子节点.
        var topicElements = topicPage.DocumentNode.Descendants("div")
          .ToList()
          .FindAll(item => item.Attributes["class"] != null && item.Attributes["class"].Value == "cell item");

        // 转换为数据列表.
        var topicList = topicElements.Select(topicNode => {
          // 帖子标题节点.
          var titleNode = topicNode.Descendants("span").ToList().Find(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "item_title").Descendants("a").First();

          // [节点名称[0], 发帖用户[3], 发帖时间[6], 最后回复[9]]
          var supportingInfos = new Regex("[\\s\\d\\w]+").Matches(
            topicNode.Descendants("span").ToList().Find(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "small fade").InnerText
          );

          // 节点链接节点.
          var nodeElement = topicNode.Descendants("a").ToList().Find(item => item.Attributes["class"] != null && item.Attributes["class"].Value == "node");

          // 回复节点.
          var repliesElement = topicNode.Descendants("a").ToList().Find(x => x.Attributes["class"] != null && x.Attributes["class"].Value == "count_livid");

          return new Topic() {
            id = new Regex("\\d+").Match(titleNode.Attributes["href"].Value).ToString() ?? "",
            title = titleNode.InnerText ?? "",
            url = "https://www.v2ex.com" + titleNode.Attributes["href"].Value ?? "",
            content = "",
            content_rendered = "",
            replies = Int32.Parse(repliesElement != null ? repliesElement.InnerText : "0"),
            member = new Member() {
              username = new Regex("[\\s\\d\\w]+$").Match(topicNode.Descendants("img").First().ParentNode.Attributes["href"].Value).Value ?? "--",
              avatarURL = ("https:" + topicNode.Descendants("img").First().Attributes["src"].Value) ?? "",
              url = ("https://www.v2ex.com" + topicNode.Descendants("img").First().ParentNode.Attributes["href"].Value) ?? ""
            },
            node = new Node.NodeSimple() {
              label = nodeElement.InnerText ?? "--",
              name = (new Regex("\\w+$").Match(nodeElement.Attributes["href"].Value).Value) ?? ""
            },
            //lastRepliedTime = supportingInfos[6].Value.Trim() ?? "",
            //lastRepliedUser = supportingInfos[9].Value.Trim() ?? ""
            lastRepliedTime = "",
            lastRepliedUser = ""
          };
        });

        resolve?.Invoke(topicList.ToList());
      } catch (Exception error) {
        reject?.Invoke(error);
      }
    }

    public Service () {}
  }
}
