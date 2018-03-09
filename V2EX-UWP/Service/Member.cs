using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2EX.Service.Member {
  /// <summary>
  /// 社区用户 Model 定义。
  /// </summary>
  public class ModelMember {
    public string username { get; set; }
    public string avatarURL { get; set; }
    public string url { get; set; }
  }

  /// <summary>
  /// 登陆账号数据类型定义.
  /// </summary>
  public class ModelLoginMember: ModelMember {
  }
}
