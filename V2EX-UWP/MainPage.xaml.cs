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

using V2EX.Service.EventBus;

namespace V2EX {
  /// <summary>
  /// 主页面视图对象.
  /// </summary>
  public class MainVM {
    /// <summary>
    /// 视图列表.
    /// </summary>
    public List<Service.View.View> views {
      get {
        return Service.View.Config.views;
      }
    }

    /// <summary>
    /// 登陆状态标识.
    /// </summary>
    public bool isLogin {
      get {
        return Service.Login.Service.isLogin;
      }
    }
  }

  /// <summary>
  /// 主体视图类.
  /// </summary>
  public partial class MainPage : Page {
    /// <summary>
    /// 初始化 BackButton 相关事件.
    /// </summary>
    /// 
    private void initBackButton () {
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
    private void navigateToMainPage () {
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
    private void toggleSplitMenu (object sender, RoutedEventArgs e) {
      AppSplitView.IsPaneOpen = !AppSplitView.IsPaneOpen;
    }

    /// <summary>
    /// 导航选择事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    private void navigateToView (object sender, RoutedEventArgs e) {
      var targetPage = ((Button)sender).Tag as Type;
      AppCanvas.Navigate(targetPage);
    }

    /// <summary>
    /// 导航至
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void navigateToLogin (object sender, RoutedEventArgs e) {
      AppCanvas.Navigate(typeof(V2EX.Views.Login.View));
    }

    /// <summary>
    /// 导航完成后回调事件.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    private void onNavigated (object sender, NavigationEventArgs e) {
      var targetPage = ((Frame)sender).CurrentSourcePageType;
      Service.View.Config.views.ForEach(item => {
        item.isSelected = (item.page == targetPage);
      });
    }

    /// <summary>
    /// 后退方法.
    /// </summary>
    private void goBack (object value) {
      if (AppCanvas.CanGoBack) {
        AppCanvas.GoBack();
      }
    }

    /// <summary>
    /// 注册事件.
    /// </summary>
    private void registerEvents () {
      EventBus.on("AppCanvas:GoBack", this.goBack);
    }

    public MainPage() {
      this.InitializeComponent();
      this.initBackButton();
      this.navigateToMainPage();
      this.registerEvents();
      DataContext = new MainVM();
    }
  }
}
