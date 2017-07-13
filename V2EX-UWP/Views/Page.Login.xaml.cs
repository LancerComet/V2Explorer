using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace V2EX.Views.Login {
  public class ViewModel: INotifyPropertyChanged {
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
    /// 错误登陆提示控制标识.
    /// </summary>
    private bool _wrongLoginHint;
    public bool wrongLoginHint {
      get {
        return this._wrongLoginHint;
      }
      
      set {
        this._wrongLoginHint = value;
        this.notify("wrongPassHint");
      }
    }

    /// <summary>
    /// 登陆错误提示文字.
    /// </summary>
    private string _wrongLoginHintText;
    public string wrongLoginHintText {
      get {
        return this._wrongLoginHintText;
      }

      set {
        this._wrongLoginHintText = value;
        this.notify("wrongLoginHintText");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void notify (string propertyName) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  /// <summary>
  /// 可用于自身或导航至 Frame 内部的空白页。
  /// </summary>
  public sealed partial class View : Page {
    private ViewModel vm { get; set; }

    /// <summary>
    /// 登陆函数.
    /// </summary>
    private void login (object sender, RoutedEventArgs e) {
      var username = this.vm.username;
      var password = this.vm.password;

      Service.Login.Service.login(username, password, (bool isLogin) => {

      }, () => {

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
