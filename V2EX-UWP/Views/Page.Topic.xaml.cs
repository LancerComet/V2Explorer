using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace V2EX.Views.Topic {
  /// <summary>
  /// 节点数据类型.
  /// 此 Node 类为 Service.Node 中的 Node 类的简化版，并添加了 Topic 页面使用的属性字段.
  /// </summary>
  public class Node : INotifyPropertyChanged {
    public string name { get; set; }
    public string label { get; set; }

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
  public class ViewModel {
    /// <summary>
    /// 节点列表.
    /// </summary>
    public List<Node> nodes { get; set; }

    /// <summary>
    /// 页面标题.
    /// </summary>
    public string PAGE_LABEL {
      get {
        return Service.View.Config.views[0].label;
      }
    }

    public ViewModel (List<Node> nodes) {
      this.nodes = nodes;
    }
  }

  public sealed partial class View : Page {
    /// <summary>
    /// 视图对象.
    /// </summary>
    private ViewModel vm { get; set; }

    /// <summary>
    /// Topic 服务实例.
    /// </summary>
    private Service.Topic.Service topicSrv { get; set; }

    /// <summary>
    /// 获取特定节点下的话题.
    /// </summary>
    /// <param name="nodeName"></param>
    public void getTopicsOfNode (string nodeName) {
    }

    /// <summary>
    /// 获取最新话题.
    /// </summary>
    public void getLatestTopics () {
      this.topicSrv.getLatestTopics(topicList => {
        var allNodeItem = this.vm.nodes.Find(item => item.name == "all");
        allNodeItem.topicList = topicList;
      }, error => {
        // ...
      });
    }

    /// <summary>
    /// 获取最热话题.
    /// </summary>
    public void getHotTopics () {
      this.topicSrv.getHotTopics(topicList => {
        var hotNodeItem = this.vm.nodes.Find(item => item.name == "hot");
        hotNodeItem.topicList = topicList;
      }, error => {
        // ...
      });
    }

    /// <summary>
    /// 页面初始化逻辑.
    /// </summary>
    private void init() {
      // 创建预设节点数据.
      var presetNodes = new List<Node>() {
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

      // 初始化 VM.
      var vm = new ViewModel(presetNodes);
      this.DataContext = vm;
      this.vm = vm;

      // 创建 topicService.
      this.topicSrv = new Service.Topic.Service();

      // 获取初始数据.
      this.getLatestTopics();
    }

    public View() {
      NavigationCacheMode = NavigationCacheMode.Enabled;
      this.InitializeComponent();
      this.init();
    }
  }
}
