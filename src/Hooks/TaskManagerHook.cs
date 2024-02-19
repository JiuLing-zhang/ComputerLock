﻿using Microsoft.Win32;

namespace ComputerLock
{
    internal class TaskManagerHook
    {
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
            var key = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
            var registryKey = Registry.CurrentUser.CreateSubKey(key);
            if (registryKey == null)
            {
                throw new Exception("注册表扫描失败");
            }
            registryKey.SetValue("DisableTaskMgr", (int)state);
            registryKey.Close();
        }
    }
}
