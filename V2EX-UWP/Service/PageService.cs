using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2EX.Service {
  // 导航项类型.
  public class View {
    public string icon;
    public string label;
    public Type page;

    public View (string icon = "", string label = "", Type page = null) {
      this.icon = icon;
      this.label = label;
      this.page = page;
    }
  }

  // 视图配置服务.
  public class ViewConfig {
    public static List<View> views = new List<View> () {
      new View("&#xED0C;", "话题", typeof(V2EX.Views.TopicPage)),
      new View("&#xECCB;", "节点", typeof(V2EX.Views.NodesPage)),
      new View("&#xE725;", "通知", typeof(V2EX.Views.NotifyPage)),
      new View("&#xE00A;", "收藏", typeof(V2EX.Views.FavouritePage)),
      new View("", "关于", typeof(V2EX.Views.AboutPage))
    };
  }
}
