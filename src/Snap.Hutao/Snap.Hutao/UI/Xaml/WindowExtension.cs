// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Snap.Hutao.UI.Windowing;
using Snap.Hutao.Win32.Foundation;
using Snap.Hutao.Win32.UI.WindowsAndMessaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;
using static Snap.Hutao.Win32.Kernel32;
using static Snap.Hutao.Win32.Macros;
using static Snap.Hutao.Win32.User32;

namespace Snap.Hutao.UI.Xaml;

internal static class WindowExtension
{
    private static readonly ConditionalWeakTable<Window, XamlWindowController> WindowControllers = [];

    public static void InitializeController<TWindow>(this TWindow window, IServiceProvider serviceProvider)
        where TWindow : Window
    {
        XamlWindowController windowController = new(window, serviceProvider);
        WindowControllers.Add(window, windowController);
    }

    public static bool IsControllerInitialized(Window window)
    {
        return WindowControllers.TryGetValue(window, out _);
    }

    public static bool IsControllerInitialized<TWindow>()
        where TWindow : Window
    {
        foreach ((Window window, _) in WindowControllers)
        {
            if (window is TWindow)
            {
                return true;
            }
        }

        return false;
    }

    public static void UninitializeController<TWindow>(this TWindow window)
        where TWindow : Window
    {
        WindowControllers.Remove(window);
    }

    public static HWND GetWindowHandle(this Window? window)
    {
        return WindowNative.GetWindowHandle(window);
    }

    public static void SwitchTo(this Window window)
    {
        SwitchTo(window.GetWindowHandle());
    }

    public static void SwitchTo(HWND hwnd)
    {
        if (!IsWindowVisible(hwnd))
        {
            ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_SHOW);
        }

        if (IsIconic(hwnd))
        {
            ShowWindow(hwnd, SHOW_WINDOW_CMD.SW_RESTORE);
        }

        SetForegroundWindow(hwnd);
    }

    public static void AddExStyleLayered(this Window window)
    {
        HWND hwnd = WindowNative.GetWindowHandle(window);
        nint style = GetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        style |= (nint)WINDOW_EX_STYLE.WS_EX_LAYERED;
        nint result = SetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, style);
        if (result is 0)
        {
            Marshal.ThrowExceptionForHR(HRESULT_FROM_WIN32(GetLastError()));
        }

        SetWindowPos(hwnd, default, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    public static void RemoveExStyleLayered(this Window window)
    {
        HWND hwnd = WindowNative.GetWindowHandle(window);
        nint style = GetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        style &= ~(nint)WINDOW_EX_STYLE.WS_EX_LAYERED;
        nint result = SetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, style);
        if (result is 0)
        {
            Marshal.ThrowExceptionForHR(HRESULT_FROM_WIN32(GetLastError()));
        }

        SetWindowPos(hwnd, default, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    public static void AddExStyleToolWindow(this Window window)
    {
        HWND hwnd = WindowNative.GetWindowHandle(window);
        nint style = GetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        style |= (nint)WINDOW_EX_STYLE.WS_EX_TOOLWINDOW;
        nint result = SetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, style);
        if (result is 0)
        {
            Marshal.ThrowExceptionForHR(HRESULT_FROM_WIN32(GetLastError()));
        }

        SetWindowPos(hwnd, default, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    public static void RemoveExStyleClientEdge(this Window window)
    {
        HWND hwnd = WindowNative.GetWindowHandle(window);
        nint style = GetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        style &= ~(nint)WINDOW_EX_STYLE.WS_EX_CLIENTEDGE;
        nint result = SetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, style);
        if (result is 0)
        {
            Marshal.ThrowExceptionForHR(HRESULT_FROM_WIN32(GetLastError()));
        }

        SetWindowPos(hwnd, default, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    public static void RemoveStyleDialogFrame(this Window window)
    {
        HWND hwnd = WindowNative.GetWindowHandle(window);
        nint style = GetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);
        style &= ~(nint)WINDOW_STYLE.WS_DLGFRAME;
        nint result = SetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, style);
        if (result is 0)
        {
            Marshal.ThrowExceptionForHR(HRESULT_FROM_WIN32(GetLastError()));
        }

        SetWindowPos(hwnd, default, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    public static void RemoveStyleOverlappedWindow(this Window window)
    {
        HWND hwnd = WindowNative.GetWindowHandle(window);
        nint style = GetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);
        style &= ~(nint)WINDOW_STYLE.WS_OVERLAPPEDWINDOW;
        nint result = SetWindowLongPtrW(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, style);
        if (result is 0)
        {
            Marshal.ThrowExceptionForHR(HRESULT_FROM_WIN32(GetLastError()));
        }

        SetWindowPos(hwnd, default, 0, 0, 0, 0, SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE);
    }

    public static double GetRasterizationScale(this Window window)
    {
        if (window is { Content.XamlRoot: { } xamlRoot })
        {
            return xamlRoot.RasterizationScale;
        }

        uint dpi = GetDpiForWindow(window.GetWindowHandle());
        return Math.Round(dpi / 96D, 2, MidpointRounding.AwayFromZero);
    }
}