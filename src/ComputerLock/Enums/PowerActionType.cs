using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerLock.Enums;

/// <summary>
/// 电源操作类型
/// </summary>
public enum PowerActionType
{
    /// <summary>
    /// 关机
    /// </summary>
    [Description("关机")]
    Shutdown = 1,

    /// <summary>
    /// 休眠
    /// </summary>
    [Description("休眠")]
    Hibernate = 2
}