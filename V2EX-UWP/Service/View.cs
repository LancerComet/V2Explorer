using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace V2EX.Service.View {
  // 导航项类型.
  public class View : INotifyPropertyChanged {
    public string icon { get; set; }
    public string label { get; set; }
    public Type page { get; set; }

    private bool _isSelected;
    public bool isSelected {
      get {
        return _isSelected; 
      }
      set {
        _isSelected = value;
        notify("isSelected");
      }
    }

    // 实现内部 Notify 接口.
    // 用于通知 XAML 进行数据更新.
    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify (string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public View (string icon = "", string label = "", Type page = null, bool isSelected = false) {
      this.icon = icon;
      this.label = label;
      this.page = page;
      this.isSelected = isSelected;
    }
  }

  // 视图配置服务.
  public class Config {
    public static List<View> views = new List<View> () {
      new View("\uE7E7", "话题", typeof(V2EX.Views.Topic.View), true),
      new View("\uECCB", "节点", typeof(V2EX.Views.Nodes.View)),
      new View("\uE725", "通知", typeof(V2EX.Views.Notify.View)),
      new View("\uE00A", "收藏", typeof(V2EX.Views.FavouritePage)),
      new View("\uE0A5", "关于", typeof(V2EX.Views.About.View))
    };
  }
}
