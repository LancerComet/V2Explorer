using System;
using System.Collections.Generic;
using System.ComponentModel;
using V2EX.Service.Topic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace V2EX.Views.Topic {
  /// <summary>
  /// Topic 页面视图中的节点数据类型.
  /// </summary>
  public class NodeDataKeeper : Service.Node.ModelNodeSimple, INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    private void notify(string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private List<Service.Topic.ModelTopic> _topicList;
    public List<Service.Topic.ModelTopic> topicList {
      get {
        return this._topicList;
      }

      set {
        this._topicList = value;
        this.notify("topicList");
      }
    }

    public NodeDataKeeper (string name, string label) {
      this.name = name;
      this.label = label;
    }
  }

  /// <summary>
  /// 视图对象.
  /// </summary>
  public class ViewModel: INotifyPropertyChanged {
    /// <summary>
    /// 预设节点列表.
    /// </summary>
    public List<NodeDataKeeper> nodes {
      get {
        return new List<NodeDataKeeper>() {
          new NodeDataKeeper("all", "全部"),
          new NodeDataKeeper("hot", "最热"),
          new NodeDataKeeper("tech", "技术"),
          new NodeDataKeeper("creative", "创意"),
          new NodeDataKeeper("play", "好玩"),
          new NodeDataKeeper("windows", "Windows"),
          new NodeDataKeeper("apple", "Apple"),
          new NodeDataKeeper("jobs", "酷工作"),
          new NodeDataKeeper("deals", "交易"),
          new NodeDataKeeper("city", "城市"),
          new NodeDataKeeper("qna", "问与答"),
          new NodeDataKeeper("r2", "R2")
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

    /// <summary>
    /// Notify 实现.
    /// </summary>
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
    public async void getTopics (string nodeName, NodeDataKeeper targetNode) {
      this.vm.loading = true;

      try {
        var result = await this.topicSrv.getTopicsUnderNode(nodeName);
        targetNode.topicList = result;
      } catch (Exception error) {
        // TODO: Error handler.
      }

      this.vm.loading = false;
    }

    /// <summary>
    /// 导航触发事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onSelectionChanged(object sender, SelectionChangedEventArgs e) {
      var selectedNode = (sender as Pivot).SelectedItem as NodeDataKeeper;
      var nodeName = selectedNode.name;
      this.getTopics(nodeName, selectedNode);
    }

    /// <summary>
    /// 帖子选择事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void onTopicTap (object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) {
      var selectedItem = (sender as Microsoft.Toolkit.Uwp.UI.Controls.MasterDetailsView).SelectedItem as ModelTopic;
      var topicList = (sender as Microsoft.Toolkit.Uwp.UI.Controls.MasterDetailsView).ItemsSource as List<ModelTopic>;
      var id = selectedItem.id;
      var index = topicList.FindIndex(item => item.id == id);  // 获取点击帖子是列表中的第几项.
      this.loadCurrentShowingTopicData(id, topicList[index]);
    }

    /// <summary>
    /// 加载选中帖子主体数据.
    /// </summary>
    private async void loadCurrentShowingTopicData (string topicId, ModelTopic bindSource) {
      try {
        ModelTopic result = await this.topicSrv.getTopicMainData(topicId);

        // 将获取的帖子数据赋值至列表中的项.
        bindSource.content = result.content;
      } catch (Exception error) {
        throw error;
      }
    }

    public View() {
      NavigationCacheMode = NavigationCacheMode.Enabled;
      this.InitializeComponent();

      // 设置 DataContext.
      this.DataContext = this.vm;

      // 获取初始数据.
      this.getTopics("all", this.vm.nodes.Find(item => item.name == "all"));
    }
  }
}
