using Microsoft.Win32;
using System.IO;

namespace ComputerLock.Platforms;

internal class TaskManagerHook
{
    private static readonly string OriginalStateFilePath = Path.Combine(Path.GetTempPath(), $"{AppBase.FriendlyName}_OriginalTaskMgr");
    private const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
    private const string DisableTaskMgrValueName = "DisableTaskMgr";

    private readonly ILogger _logger;

    private enum TaskManagerStateEnum
    {
        Enabled = 0,
        Disabled = 1
    }

    public TaskManagerHook(ILogger logger)
    {
        _logger = logger;
    }

    public void Lock()
    {
        var currentState = GetCurrentState();
        SaveOriginalState(currentState);
        SetState(TaskManagerStateEnum.Disabled);
    }

    public void Unlock()
    {
        try
        {
            string content = File.ReadAllText(OriginalStateFilePath);
            if (Enum.TryParse<TaskManagerStateEnum>(content, out var originalState))
            {
                SetState(originalState);
            }

            File.Delete(OriginalStateFilePath);
        }
        catch (Exception ex)
        {
            _logger.Error("恢复任务管理器状态失败", ex);
        }
    }

    /// <summary>
    /// 崩溃恢复
    /// </summary>
    public void RecoverFromCrash()
    {
        if (File.Exists(OriginalStateFilePath))
        {
            Unlock();
        }
    }

    private TaskManagerStateEnum GetCurrentState()
    {
        using var registryKey = Registry.CurrentUser.OpenSubKey(RegKey);
        if (registryKey == null)
        {
            return TaskManagerStateEnum.Enabled;
        }

        var value = registryKey.GetValue(DisableTaskMgrValueName, 0);
        return (int)value == 1 ? TaskManagerStateEnum.Disabled : TaskManagerStateEnum.Enabled;
    }

    private void SaveOriginalState(TaskManagerStateEnum state)
    {
        try
        {
            File.WriteAllText(OriginalStateFilePath, ((int)state).ToString());
        }
        catch (Exception ex)
        {
            _logger.Error($"保存任务管理器原始状态失败", ex);
        }
    }

    private void SetState(TaskManagerStateEnum state)
    {
        using var registryKey = Registry.CurrentUser.CreateSubKey(RegKey);
        if (registryKey == null)
        {
            _logger.Error($"注册表键不存在或无法创建:{RegKey}");
            return;
        }
        registryKey.SetValue(DisableTaskMgrValueName, (int)state);
    }
}
