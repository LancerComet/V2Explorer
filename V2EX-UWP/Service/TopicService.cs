using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2EX.Service {
  /// <summary>
  /// 话题节点数据类型.
  /// 用于定义话题帖预览列表中的话题帖子.
  /// </summary>
  class TopicPost {
    string name = "";
    string label = "";

    TopicPost (string name = "", string label = "") {
      this.name = name;
      this.label = label;
    }
  }

  /// <summary>
  /// 主题数据类型.
  /// 用于定义话题帖预览列表.
  /// </summary>
  class TopicList {
    
  }
  
  /// <summary>
  /// 话题页面服务.
  /// </summary>
  /// 
  public class TopicService {
    // 话题节点.
    List<TopicPost> topicNodes { get; set; }

    // 主题列表.
    List<TopicList> topicList { get; set; }

    // 获取话题节点.
    public void GetTopics () {

    }

    // 获取某一结点帖子列表.
    public void GetPosts () {
    }

    TopicService () {
    }
  }
}
