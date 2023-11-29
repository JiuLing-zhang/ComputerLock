using System;

namespace ComputerLock;
[Flags]
internal enum PasswordBoxActiveMethodEnum
{
    KeyboardDown = 1,
    MouseDown = 2
}