using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace V2EX.Views {
  public class Node {
    public string name { get; set; }
    public string label { get; set; }

    public Node (string name, string label) {
      this.name = name;
      this.label = label;
    }
  }

  public class ViewModel {
    public List<Node> nodes {
      get {
        return new List<Node>() {
          new Node("all", "全部"),
          new Node("hot", "最热"),
          new Node("tech", "技术"),
          new Node("creative", "创意")
        };
      }
    }
  }

  public sealed partial class TopicPage : Page {
    public TopicPage () {
      this.InitializeComponent();
      DataContext = new ViewModel();
    }

    public string pageLabel {
      get {
        return Service.ViewConfig.views[0].label;
      }
    }
  }
}
