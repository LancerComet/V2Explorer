using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace V2EX.Views.Topic {
  /// <summary>
  /// 节点数据类型.
  /// </summary>
  public class Node : Service.Node.NodeSimple, INotifyPropertyChanged {
    private List<Service.Topic.Topic> _topicList;
    public List<Service.Topic.Topic> topicList {
      get {
        return this._topicList;
      }

      set {
        this._topicList = value;
        this.notify("topicList");
      }
    }

    public Node (string name, string label) {
      this.name = name;
      this.label = label;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void notify (string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  /// <summary>
  /// 视图对象.
  /// </summary>
  public class ViewModel: INotifyPropertyChanged {
    /// <summary>
    /// 预设节点列表.
    /// </summary>
    public List<Node> nodes {
      get {
        return new List<Node>() {
          new Node("all", "全部"),
          new Node("hot", "最热"),
          new Node("tech", "技术"),
          new Node("creative", "创意"),
          new Node("play", "好玩"),
          new Node("windows", "Windows"),
          new Node("apple", "Apple"),
          new Node("jobs", "酷工作"),
          new Node("deals", "交易"),
          new Node("city", "城市"),
          new Node("qna", "问与答"),
          new Node("r2", "R2")
        };
      }
    }

    /// <summary>
    /// 载入状态控制标识.
    /// </summary>
    private bool _loading;
    public bool loading {
      get {
        return this._loading;
      }
      set {
        this._loading = value;
        this.notify("loading");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void notify(string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  /// <summary>
  /// 话题页面.
  /// </summary>
  public sealed partial class View : Page {
    /// <summary>
    /// 视图对象.
    /// </summary>
    private ViewModel vm {
      get {
        return new ViewModel();
      }
    }

    /// <summary>
    /// Topic 服务实例.
    /// </summary>
    private Service.Topic.Service topicSrv {
      get {
        return new Service.Topic.Service();
      }
    }

    /// <summary>
    /// 获取特定节点下的话题.
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="targetNode"></param>
    public void getTopics (string nodeName, Node targetNode) {
      this.vm.loading = true;
      this.topicSrv.getTopics(nodeName, topicList => {
        targetNode.topicList = topicList;
        this.vm.loading = false;
      }, error => {
        // ...
        this.vm.loading = false;
      });
    }

    public View() {
      NavigationCacheMode = NavigationCacheMode.Enabled;
      this.InitializeComponent();
      
      // 设置 DataContext.
      this.DataContext = this.vm;

      // 获取初始数据.
      this.getTopics("all", this.vm.nodes.Find(item => item.name == "all"));
    }

    /// <summary>
    /// 导航触发事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onSelectionChanged(object sender, SelectionChangedEventArgs e) {
      var selectedNode = (sender as Pivot).SelectedItem as Node;
      var nodeName = selectedNode.name;
      this.getTopics(nodeName, selectedNode);
    }

    /// <summary>
    /// 帖子选择事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onTopicTap(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
      var selectedItem = (sender as Microsoft.Toolkit.Uwp.UI.Controls.MasterDetailsView).SelectedItem as Service.Topic.Topic;
      var url = selectedItem.url;
      var id = selectedItem.id;
      Console.WriteLine(url);
    }
  }
}
