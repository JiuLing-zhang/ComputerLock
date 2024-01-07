## 透明锁屏 (Transparent Lock Screen)  
一个 `Windows` 下另类的锁屏工具。  
A screen lock tool for Windows.  

<img src="https://github.com/JiuLing-zhang/ComputerLock/raw/main/resources/app.png" width="40%">
<img src="https://github.com/JiuLing-zhang/ComputerLock/raw/main/resources/app_en.png" width="40%">

## 功能介绍 (Features)  
<table>
    <tr>
        <td>开机时自动启动</br>Automatically start at boot</td>
        <td>✔️</td>
    </tr>
    <tr>
        <td>透明锁屏</br>Transparent Lock</td>
        <td>✔️</td>
    </tr>
    <tr>
        <td>禁用 Windows 锁屏</br>Disable Windows lock screen</td>
        <td>✔️</td>
    </tr>
    <tr>
        <td>屏蔽任务管理器</br>Disable task manager</td>
        <td>✔️</td>
    </tr>
    <tr>
        <td>屏蔽系统按键（Ctrl、Win）</br>Disable system keys (Ctrl, Win)</td>
        <td>✔️</td>
    </tr>
    <tr>
        <td>一键锁屏</br>Shortcut key screen lock</td>
        <td>✔️</td>
    </tr>
    <tr>
        <td>无操作时自动锁屏</br>Automatically lock screen when no operation is performed</td>
        <td>✔️</td>
    </tr>
</table>

## 权限说明 (About Permission)  
* 程序在锁屏时，需要通过注册表屏蔽掉任务管理器，因此需要管理员权限运行。  
* 程序通过 user32 设置全局快捷键、禁用部分按键，因此部分杀毒软件会报病毒。  

* When the program is on the lock screen, the Task Manager needs to be disabled (via the registry), so it needs administrator privileges to run.  
* The program uses user32 to set global shortcut keys and disable some keys, so some anti-virus software will report viruses.  

## 忘记密码 (Lost Password)  
**如果忘记密码，可以通过删除配置文件来重置设置。**  
**If you forget your password, deleting the profile can reset the settings.**  
> `C:\Users\Username\AppData\Local\ComputerLock\config.json`  
