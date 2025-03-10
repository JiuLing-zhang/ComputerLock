﻿namespace ComputerLock.Interfaces;
public interface IWindowTitleBar
{
    public bool IsMaximized { get; }
    public void Minimize();
    public void Maximize();
    public void Close();
    public void Restart();
}