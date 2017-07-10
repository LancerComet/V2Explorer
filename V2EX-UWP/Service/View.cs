using System;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// 程序视图服务.
/// 提供视图相关的基础功能.
/// </summary>
namespace V2EX.Service.View {
  /// <summary>
  /// 视图类.
  /// 定义当个页面的视图数据类型.
  /// </summary>
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

  /// <summary>
  /// 视图配置静态访问类.
  /// 提供程序视图定义列表.
  /// </summary>
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
