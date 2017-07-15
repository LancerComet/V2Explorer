using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace V2EX.Views.Login {
  public class ViewModel: INotifyPropertyChanged {
    private bool _loading;
    public bool loading {
      get {
        return _loading;
      }

      set {
        _loading = value;
        this.notify("loading");
      }
    }

    public bool isLogin {
      get {
        return Service.Login.Service.isLogin;
      }
      set {
        this.notify("isLogin");
      }
    }

    /// <summary>
    /// 用户名.
    /// </summary>
    private string _username;
    public string username {
      get {
        return this._username;
      }
      
      set {
        this._username = value;
        this.notify("username");
      }
    }

    /// <summary>
    /// 密码.
    /// </summary>
    private string _password;
    public string password {
      get {
        return this._password;
      }

      set {
        this._password = value;
        this.notify("password");
      }
    }

    /// <summary>
    /// 错误登陆类型.
    /// </summary>
    private string _loginErrorType;
    public string loginErrorType {
      get {
        return this._loginErrorType;
      }
      
      set {
        this._loginErrorType = value;
        this.notify("wrongPassHint");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify (string propertyName) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  /// <summary>
  /// Page.
  /// </summary>
  public sealed partial class View : Page {
    private ViewModel vm { get; set; }

    /// <summary>
    /// 登陆函数.
    /// </summary>
    private void login (object sender, RoutedEventArgs e) {
      var username = this.vm.username;
      var password = this.vm.password;

      this.vm.loading = true;
      Service.Login.Service.login(username, password, (bool isLogin) => {
        this.vm.loginErrorType = null;
        this.vm.isLogin = isLogin;  // 只是通知数据更改，从 Login 服务中的静态成员重新获取数据.
        this.vm.loading = false;
      }, (string errorType) => {
        this.vm.loginErrorType = errorType;
        this.vm.loading = false;
      });
    }

    private void initVM () {
      var vm = new ViewModel();
      this.vm = vm;
      this.DataContext = vm;
    }

    public View() {
      this.InitializeComponent();
      this.initVM();
    }
  }
}
