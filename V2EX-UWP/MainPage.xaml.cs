/**
 * V2Explorer is a V2EX UWP Client.
 * By LancerComet at 20:35, 2017.07.04.
 * # Carry Your World #
 * ---
 * @author LancerComet
 * @copyright LancerComet
 * @license MIT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace V2EX {
  /// <summary>
  /// 主页面视图对象.
  /// </summary>
  public class MainVM {
    public List<Service.View.View> views {
      get {
        return Service.View.Config.views;
      }
    }

    public bool isLogin {
      get {
        return Service.Login.Service.isLogin;
      }
    }
  }


  public partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
      this.initBackButton();
      this.navigateToMainPage();
      DataContext = new MainVM();
    }

    /// <summary>
    /// 初始化 BackButton 相关事件.
    /// </summary>
    /// 
    void initBackButton () {
      // 注册导航回调.
      AppCanvas.Navigated += (object sender, NavigationEventArgs e) => {
        // 在每次导航时更新 backButton 可见性.
        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
          ((Frame)sender).CanGoBack
            ? AppViewBackButtonVisibility.Visible
            : AppViewBackButtonVisibility.Collapsed;
      };

      // 注册一个 backButton 后退的事件处理器.
      SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, BackRequestedEventArgs e) => {
        if (AppCanvas.CanGoBack) {
          e.Handled = true;
          AppCanvas.GoBack();
        }
      };

      // 设置首次状态.
      SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
        AppCanvas.CanGoBack
          ? AppViewBackButtonVisibility.Visible
          : AppViewBackButtonVisibility.Collapsed;

    }

    /// <summary>
    /// 导航到主页面. 
    /// </summary>
    /// 
    void navigateToMainPage () {
      Service.View.Config.views.Any(view => {
        if (view.isSelected) {
          AppCanvas.Navigate(view.page);
          return true;
        }
        return false;
      });
    }

    /// <summary>
    /// 切换侧栏显示状态.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    void toggleSplitMenu (object sender, RoutedEventArgs e) {
      AppSplitView.IsPaneOpen = !AppSplitView.IsPaneOpen;
    }

    /// <summary>
    /// 导航选择事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    private void navigateTo (object sender, RoutedEventArgs e) {
      var targetPage = ((Button)sender).Tag as Type;
      AppCanvas.Navigate(targetPage);
      Service.View.Config.views.ForEach(item => {
        item.isSelected = (item.page == targetPage);
      });
    }
  }
}
