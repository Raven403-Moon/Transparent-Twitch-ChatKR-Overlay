using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TransparentTwitchChatWPF.Utils;

public static class ShellHelper
{
    /// <summary>
    /// Safely opens a URL in the user's default web browser.
    /// Shows a user-friendly MessageBox if the operation fails.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    public static void OpenUrl(string url)
    {
        // First, validate the input to prevent basic errors.
        if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            MessageBox.Show($"제공된 URL이 올바르지 않습니다: {url}",
                            "잘못된 URL", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }
        catch (Win32Exception winEx)
        {
            // Handle specific OS-level errors, like the user canceling a prompt.
            MessageBox.Show($"웹사이트를 열 수 없습니다. 시스템 오류: {winEx.Message}",
                            "작업 실패", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected errors.
            MessageBox.Show($"웹사이트를 여는 중 예기치 않은 오류가 발생했습니다. 기본 웹 브라우저가 설정되어 있는지 확인하세요.",
                            "오류", MessageBoxButton.OK, MessageBoxImage.Error);

            // Log the full error for debugging purposes.
            Debug.WriteLine($"Failed to open URL '{url}': {ex}");
        }
    }

    /// <summary>
    /// Safely opens a folder in the default file explorer.
    /// Shows a user-friendly MessageBox if the folder doesn't exist or if the operation fails.
    /// </summary>
    /// <param name="folderPath">The absolute path to the folder.</param>
    public static void OpenFolder(string folderPath)
    {
        // Validate the input path.
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            MessageBox.Show("제공된 폴더 경로가 비어 있습니다.",
                            "잘못된 경로", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Check if the directory exists before trying to open it.
        if (!Directory.Exists(folderPath))
        {
            MessageBox.Show($"지정된 경로에 폴더가 존재하지 않습니다:\n\n{folderPath}",
                            "폴더를 찾을 수 없음", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            // Use the Shell to execute the default action for a folder, which is to open it.
            Process.Start(new ProcessStartInfo(folderPath)
            {
                UseShellExecute = true
            });
        }
        catch (Win32Exception winEx)
        {
            MessageBox.Show($"폴더를 열 수 없습니다. 시스템 오류: {winEx.Message}",
                            "작업 실패", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            // Handle other potential errors, like permission denied.
            MessageBox.Show($"폴더를 여는 중 예기치 않은 오류가 발생했습니다:\n\n{ex.Message}",
                            "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            Debug.WriteLine($"Failed to open folder '{folderPath}': {ex}");
        }
    }
}