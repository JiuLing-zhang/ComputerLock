using Microsoft.Win32;

namespace ComputerLock.Hooks;
internal class TaskManagerHook
{
    private const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
    private enum TaskManagerStateEnum
    {
        Enabled = 0,
        Disabled = 1
    }

    public void EnabledTaskManager()
    {
        SetState(TaskManagerStateEnum.Enabled);
    }

    public void DisabledTaskManager()
    {
        SetState(TaskManagerStateEnum.Disabled);
    }

    private void SetState(TaskManagerStateEnum state)
    {
        var registryKey = Registry.CurrentUser.CreateSubKey(RegKey);
        if (registryKey == null)
        {
            throw new Exception("注册表扫描失败");
        }
        registryKey.SetValue("DisableTaskMgr", (int)state);
        registryKey.Close();
    }
}
