using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System.IO;

namespace ComputerLock.Platforms;

internal class AutostartHook(ILogger logger)
{
    private const string TaskName = "ComputerLockAutoStart";


    /// <summary>
    /// 迁移老版本中的注册表自启动项到计划任务
    /// </summary>
    public void MigrateRegistryToTaskIfNeeded()
    {
        try
        {
            const string regKey = @"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
            var registryKey = Registry.LocalMachine.OpenSubKey(regKey);
            if (registryKey?.GetValue(AppBase.FriendlyName) == null)
            {
                return;
            }
            CreateOrUpdateTask();
            using var registryKeyWrite = Registry.LocalMachine.CreateSubKey(regKey);
            registryKeyWrite.DeleteValue(AppBase.FriendlyName);
        }
        catch (Exception ex)
        {
            logger.Error("迁移自启动项失败", ex);
        }
    }

    public bool IsAutostart()
    {
        // 判断计划任务是否存在
        try
        {
            using var taskService = new TaskService();
            var task = taskService.GetTask(TaskName);
            return task != null;
        }
        catch (Exception ex)
        {
            logger.Error("任务计划获取失败", ex);
            return false;
        }
    }

    public void EnabledAutostart()
    {
        CreateOrUpdateTask();
    }

    public void DisabledAutostart()
    {
        DeleteTaskIfExists();
    }

    private void CreateOrUpdateTask()
    {
        try
        {
            using var ts = new TaskService();

            // 构建任务定义
            var td = ts.NewTask();
            td.RegistrationInfo.Description = "透明锁屏开机自动启动任务";
            td.Principal.LogonType = TaskLogonType.InteractiveToken;
            td.Principal.RunLevel = TaskRunLevel.Highest;
            td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
            td.Settings.DisallowStartIfOnBatteries = false;
            td.Settings.StopIfGoingOnBatteries = false;
            td.Settings.StartWhenAvailable = true;

            // 触发器：用户登录
            td.Triggers.Add(new LogonTrigger());

            // 操作：启动程序
            var execPath = AppBase.ExecutablePath;
            var workingDir = Path.GetDirectoryName(execPath);
            td.Actions.Add(new ExecAction(execPath, null, workingDir));

            // 注册（创建或更新）
            ts.RootFolder.RegisterTaskDefinition(TaskName, td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.InteractiveToken);

        }
        catch (Exception ex)
        {
            logger.Error("任务计划创建失败", ex);
        }
    }

    private void DeleteTaskIfExists()
    {
        try
        {
            using var taskService = new TaskService();
            var task = taskService.GetTask(TaskName);
            if (task != null)
            {
                taskService.RootFolder.DeleteTask(TaskName);
            }
        }
        catch (Exception ex)
        {
            logger.Error("任务计划删除失败", ex);
        }
    }
}