using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace V2EX.Views.Nodes {
  /// <summary>
  /// 节点页面.
  /// </summary>
  public sealed partial class View : Page {
    private ViewModel vm { get; set; }
    private Service.Node.Service nodeSrv = new Service.Node.Service();

    private void initVM () {
      var vm = new ViewModel();
      this.vm = vm;
      DataContext = vm;
    }

    private async void initRequest () {
      try {
        var nodesList = await this.nodeSrv.getAllNodes();
        this.vm.nodesList = nodesList;
      } catch (Exception error) {
        // TODO: Error Handler.
      }

      this.vm.loading = false;
    }

    public View() {
      NavigationCacheMode = NavigationCacheMode.Enabled;  // 开启页面缓存模式.
      this.InitializeComponent();
      this.initVM();
      this.initRequest();
    }
  }

  /// <summary>
  /// 视图对象.
  /// </summary>
  public class ViewModel : INotifyPropertyChanged {
    /// <summary>
    /// 节点列表.
    /// </summary>
    private List<Service.Node.ModelNode> _nodesList;
    public List<Service.Node.ModelNode> nodesList {
      get {
        return this._nodesList;
      }
      set {
        this._nodesList = value;
        this.notify("nodesList");
      }
    }

    /// <summary>
    /// 进度条控制标识.
    /// </summary>
    private bool _loading = true;
    public bool loading {
      get {
        return this._loading;
      }
      set {
        this._loading = value;
        this.notify("loading");
      }
    }

    public ViewModel () {}

    /// <summary>
    /// 实现 Notify 接口通知视图更新.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify(string propertyName) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
