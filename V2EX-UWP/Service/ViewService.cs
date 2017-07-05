using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2EX.Service {
  // 导航项类型.
  public class View {
    public string icon { get; set; }
    public string label { get; set; }
    public Type page { get; set; }
    public bool isSelected { get; set; }

    public View (string icon = "", string label = "", Type page = null, bool isSelected = false) {
      this.icon = icon;
      this.label = label;
      this.page = page;
      this.isSelected = isSelected;
    }
  }

  // 视图配置服务.
  public class ViewConfig {
    public static List<View> views = new List<View> () {
      new View("\uE7E7", "话题", typeof(V2EX.Views.TopicPage), true),
      new View("\uECCB", "节点", typeof(V2EX.Views.NodesPage)),
      new View("\uE725", "通知", typeof(V2EX.Views.NotifyPage)),
      new View("\uE00A", "收藏", typeof(V2EX.Views.FavouritePage)),
      new View("\uE0A5", "关于", typeof(V2EX.Views.AboutPage))
    };
  }
}
