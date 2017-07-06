using Windows.UI.Xaml.Controls;
using System.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace V2EX.Views.Nodes {
  /// <summary>
  /// 节点页面.
  /// </summary>
  /// 
  public sealed partial class View : Page {
    public View() {
      this.InitializeComponent();
      DataContext = new NodesVM();
    }
  }

  /// <summary>
  /// 视图对象.
  /// </summary>
  /// 
  public class NodesVM : INotifyPropertyChanged {
    private Service.Node.NodesService _nodeSrv;
    public Service.Node.NodesService nodeSrv {
      get {
        return this._nodeSrv;
      }

      set {
        this._nodeSrv = value;
        this.notify("nodeSrv");
      }
    }

    public NodesVM () {
      this.nodeSrv = new Service.Node.NodesService();
      this.nodeSrv.getAllNodes();
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
