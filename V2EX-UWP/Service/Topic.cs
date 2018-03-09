using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using V2EX.Service.Http;
using V2EX.Service.Member;

namespace V2EX.Service.Topic {
  /// <summary>
  /// 话题节点数据类型.
  /// 用于定义话题帖预览列表中的话题帖子.
  /// </summary>
  public class ModelTopic: INotifyPropertyChanged {
    public string id { get; set; }
    public string title { get; set; }
    public string url { get; set; }

    private string _content;
    public string content {
      get {
        return this._content;
      }
      set {
        this._content = value;
        this.notify("content");
      }
    }

    private List<ModelRepliedTopic> _repliedTopics;
    public List<ModelRepliedTopic> repliedTopics {
      get {
        return this._repliedTopics;
      }
      set {
        this._repliedTopics = value;
        this.notify("repliedTopics");
      }
    }

    public string contentRendered { get; set; }
    public int replies { get; set; }
    public ModelMember member { get; set; }
    public Node.ModelNodeSimple node { get; set; }
    public string lastRepliedTime { get; set; }
    public string lastRepliedUser { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    private void notify(string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public class ModelRepliedTopic {
    public string id { get; set; }
    public int thanks { get; set; }
    public string content { get; set; }
    public string contentRendered { get; set; }
    public ModelMember member { get; set; }
    public int created { get; set; }
    public int lastModified { get; set; }
  }

  /// <summary>
  /// Topic 相关服务.
  /// </summary>
  public class Service {
    public delegate void RequestResolve (List<ModelTopic> topicList = null);
    public delegate void RequestReject (Exception error = null);

    private Regex urlRegExp = new Regex("http(s)?:");

    /// <summary>
    /// HTTP 服务.
    /// </summary>
    private HttpRequest httpRequest = new HttpRequest();

    /// <summary>
    /// 获取特定节点下话题列表.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="resolve"></param>
    /// <param name="reject"></param>
    public async Task<List<ModelTopic>> getTopicsUnderNode (string nodeName) {
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

          return new ModelTopic() {
            id = new Regex("\\d+").Match(titleNode.Attributes["href"].Value).ToString() ?? "",
            title = titleNode.InnerText ?? "",
            url = "https://www.v2ex.com" + titleNode.Attributes["href"].Value ?? "",
            content = "",
            contentRendered = "",
            replies = Int32.Parse(repliesElement != null ? repliesElement.InnerText : "0"),
            member = new ModelMember() {
              username = new Regex("[\\s\\d\\w]+$").Match(topicNode.Descendants("img").First().ParentNode.Attributes["href"].Value).Value ?? "--",
              avatarURL = ("https:" + topicNode.Descendants("img").First().Attributes["src"].Value) ?? "",
              url = ("https://www.v2ex.com" + topicNode.Descendants("img").First().ParentNode.Attributes["href"].Value) ?? ""
            },
            node = new Node.ModelNodeSimple() {
              label = nodeElement.InnerText ?? "--",
              name = (new Regex("\\w+$").Match(nodeElement.Attributes["href"].Value).Value) ?? ""
            },
            //lastRepliedTime = supportingInfos[6].Value.Trim() ?? "",
            //lastRepliedUser = supportingInfos[9].Value.Trim() ?? ""
            lastRepliedTime = "",
            lastRepliedUser = ""
          };
        });
        var result = topicList.ToList();

        return result;
      } catch (Exception error) {
        throw error;
      }
    }

    /// <summary>
    /// 获取某话题的主体数据.
    /// </summary>
    /// <param name="topicId"></param>
    public async Task<ModelTopic> getTopicMainData (string topicId) {
      try {
        var result = await this.httpRequest.getSingle<TopicMainDataFromServer>("https://www.v2ex.com/api/topics/show.json?id=" + topicId);
        var data = result;
        var modelTopic = new ModelTopic() {
          id = data.id.ToString(),
          title = data.title,
          url = data.url,
          content = data.content,
          replies = data.replies,
          member = new ModelMember() {
            username = data.member.username,
            avatarURL = this.urlRegExp.IsMatch(data.member.avatar_normal) ? data.member.avatar_normal : "https:" + data.member.avatar_normal,
            url = ""
          },
          node = new Node.ModelNodeSimple() {
            name = data.node.name,
            label = data.node.title
          }
        };
        return modelTopic;
      } catch (Exception error) {
        throw error;
      }
    }

    /// <summary>
    /// 获取某话题的回帖数据.
    /// </summary>
    /// <param name="topicId"></param>
    /// <returns></returns>
    public async Task<List<ModelRepliedTopic>> getReplies (string topicId) {
      try {
        var data = await this.httpRequest.get<RepliedTopicFromServer>("https://www.v2ex.com/api/replies/show.json?topic_id=" + topicId);
        List<ModelRepliedTopic> result = data.Select(item => {
          return new ModelRepliedTopic() {
            id = item.id.ToString(),
            thanks = item.thanks,
            content = item.content,
            contentRendered = item.content_rendered,
            member = new ModelMember() {
              username = item.member.username,
              avatarURL = this.urlRegExp.IsMatch(item.member.avatar_normal) ? item.member.avatar_normal : "https:" + item.member.avatar_normal,
              url = ""
            },
            created = item.created,
            lastModified = item.last_modified
          };
        }).ToList();
        return result;
      } catch (Exception error) {
        throw error;
      }
    }

    public class TopicMainDataFromServer {
      public int id { get; set; }
      public string title { get; set; }
      public string url { get; set; }
      public string content { get; set; }
      public string content_rendered { get; set; }
      public int replies { get; set; }
      public TopicUserInfoFromServer member { get; set; }
      public TopicNodeInfoFromServer node { get; set; }
      public int created { get; set; }
      public int last_modified { get; set; }
      public int last_touched { get; set; }
    }

    public class TopicUserInfoFromServer {
      public int id { get; set; }
      public string username { get; set; }
      public string tagline { get; set; }
      public string avatar_mini { get; set; }
      public string avatar_normal { get; set; }
      public string avatar_large { get; set; }
    }

    public class TopicNodeInfoFromServer {
      public int id { get; set; }
      public string name { get; set; }
      public string title { get; set; }
      public string url { get; set; }
      public int topics { get; set; }
      public string avatar_mini { get; set; }
      public string avatar_normal { get; set; }
      public string avatar_large { get; set; }
    }

    public class RepliedTopicFromServer {
      public int id { get; set; }
      public int thanks { get; set; }
      public string content { get; set; }
      public string content_rendered { get; set; }
      public TopicUserInfoFromServer member { get; set; }
      public int created { get; set; }
      public int last_modified { get; set; }
    }

    public Service () {}
  }
}
