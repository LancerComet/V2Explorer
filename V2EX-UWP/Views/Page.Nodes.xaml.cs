using Windows.UI.Xaml.Controls;
using System.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace V2EX.Views.Nodes {
  /// <summary>
  /// 节点页面.
  /// </summary>
  public sealed partial class View : Page {
    private void initVM () {
      DataContext = new NodesVM();
    }

    public View() {
      NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;  // 开启页面缓存模式.
      this.InitializeComponent();
      this.initVM();
    }
  }

  /// <summary>
  /// 视图对象.
  /// </summary>
  public class NodesVM : INotifyPropertyChanged {
    /// <summary>
    /// 节点服务实例.
    /// </summary>
    private Service.Node.Service _nodeSrv;
    public Service.Node.Service nodeSrv {
      get { 
        return this._nodeSrv;
      }

      set {
        this._nodeSrv = value;
        this.notify("nodeSrv");
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

    /// <summary>
    /// 获取数据执行成功回调函数.
    /// </summary>
    private void getAllNodesResolve () {
      this.loading = false;
    }

    public NodesVM () {
      var nodeSrv = new Service.Node.Service();
      this.nodeSrv = nodeSrv;

      nodeSrv.getAllNodes(this.getAllNodesResolve);
    }

    /// <summary>
    /// 实现 Notify 接口通知视图更新.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify(string propertyName) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
