using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2EX.Service {
  // 话题节点数据类型.
  // 用于定义话题节点.
  class TopicNode {
    string name = "";
    string label = "";

    TopicNode (string name = "", string label = "") {
      this.name = name;
      this.label = label;
    }
  }

  // 主题数据类型.
  // 用于定义话题帖预览列表.
  class Topic {
    
  }

  // 话题服务类.
  public class TopicService {
    // 话题节点.
    List<TopicNode> TopicNodes { get; set; }

    // 主题列表.
    List<Topic> TopicList { get; set; }

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
