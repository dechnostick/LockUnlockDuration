Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Form1

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function LockWorkStation() As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Public Shared Event SessionSwitch As SessionSwitchEventHandler
    Private locked As DateTime = Nothing
    Private unlocked As DateTime = Nothing

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Label1.Text = String.Empty
        Me.Label2.Text = String.Empty
        Me.Label3.Text = String.Empty

        Dim handler As SessionSwitchEventHandler = New SessionSwitchEventHandler(AddressOf SystemEvents_SessionSwitch)
        AddHandler SystemEvents.SessionSwitch, handler
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If Not LockWorkStation() Then
            Throw New Win32Exception()
        End If
    End Sub

    Private Sub SystemEvents_SessionSwitch(ByVal sender As System.Object, ByVal e As SessionSwitchEventArgs)

        Select Case e.Reason
            Case SessionSwitchReason.SessionLock

                locked = DateTime.Now
                Me.Label1.Text = locked.ToString("HH:mm:ss")

            Case SessionSwitchReason.SessionUnlock

                unlocked = DateTime.Now
                Me.Label2.Text = unlocked.ToString("HH:mm:ss")

                Dim duration As TimeSpan = unlocked.Subtract(locked)
                Me.Label3.Text = Math.Ceiling(duration.TotalMinutes) & " min."
        End Select
    End Sub
End Class
