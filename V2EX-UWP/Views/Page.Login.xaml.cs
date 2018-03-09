using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using V2EX.Service.EventBus;
using V2EX.Service.Login;

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
    /// 验证码.
    /// </summary>
    private string _captcha;
    public string captcha {
      get {
        return this._captcha;
      }
      set {
        this._captcha = value;
        this.notify("captcha");
      }
    }

    /// <summary>
    /// 验证码图片 URL.
    /// </summary>
    private string _captchaUrl;
    public string captchaUrl {
      get {
        return this._captchaUrl;
      }
      set {
        this._captchaUrl = value;
        this.notify("captchaUrl");
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
    private Service.Login.V2EXLoginPeparingData preparingData { get; set; }

    /// <summary>
    /// 获取登陆预置数据.
    /// </summary>
    private async void getPreparingData () {
      try {
        var result = await Service.Login.Service.getLoginPresetData();
        this.preparingData = result;
      } catch (Exception error) {
        // TODO: Error Handler.
      }
    }

    /// <summary>
    /// 登陆函数.
    /// </summary>
    private async void login (object sender, RoutedEventArgs e) {
      var username = this.vm.username;
      var password = this.vm.password;
      var captcha = this.vm.captcha;
      var once = this.preparingData.once;
      var usernameKey = this.preparingData.usernameKey;
      var passwordKey = this.preparingData.passwordKey;
      var captchaKey = this.preparingData.captchaKey;

      this.vm.loading = true;

      try {
        await Service.Login.Service.login(
          username, password, once, captcha, usernameKey, passwordKey, captchaKey
        );

        // 登陆成功后后退.
        EventBus.emit("AppCanvas:GoBack", null);
      } catch (Exception error) {
        // TODO: Error handler.
      }

      this.vm.loading = false;
    }

    private void initVM () {
      var vm = new ViewModel();
      this.vm = vm;
      this.DataContext = vm;
    }

    public View() {
      this.InitializeComponent();
      this.initVM();
      this.getPreparingData();
    }
  }
}
