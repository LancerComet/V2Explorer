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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using V2EX.Service;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace V2EX {
  public partial class MainPage : Page {
    public MainPage() {
      this.InitializeComponent();
      this.initBackButton();
      this.navigateToMainPage();
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
      AppCanvas.Navigate(typeof(Views.TopicPage));
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
    void navigateToView (object sender, SelectionChangedEventArgs e) {
      var selectedIndex = ViewMenus.SelectedIndex;
      AppCanvas.Navigate(Service.ViewConfig.views[selectedIndex].page);
    }
  }
}
