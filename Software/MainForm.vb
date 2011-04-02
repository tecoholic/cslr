Imports System.Text
Imports System.IO.Ports
Imports Excel = Microsoft.Office.Interop.Excel

Public Class MainForm
    Dim APP As New Excel.Application
    Dim worksheet As Excel.Worksheet
    Dim workbook As Excel.Workbook
    Dim rowNum As Integer = 3

    Private Sub MainForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            ' try and close the serial port if not done already
            SerialPort1.Close()
        Catch ex As Exception
            ' do nothig
        End Try
        Try
            workbook.Save()
            workbook.Close()
            APP.Quit()
        Catch ex As Runtime.InteropServices.COMException
            MsgBox("Data sheet has been closed!")
        End Try

    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'All the buttons are disabled by default
        SetBtn.Enabled = False
        ViewLogBtn.Enabled = False
        CloseConBtn.Enabled = False
        LimitBox.Enabled = False
        ConnectBtn.Enabled = False
        ' spefify the excel file and the sheet to load
        APP = New Excel.ApplicationClass
        workbook = APP.Workbooks.Add("G:\CustomerLog.xls")
        worksheet = workbook.Worksheets("sheet1")
        Try
            SerialPort1.Open()
        Catch ex As Exception
            MsgBox("The specified port cannot be opened", MsgBoxStyle.Critical, "Error!")
            Exit Sub
        End Try
    End Sub

    Private Sub ConnectBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectBtn.Click
        Dim response As String
        Try
            If Not (SerialPort1.IsOpen = True) Then
                SerialPort1.Open()
            End If
            SerialPort1.WriteLine("1")
            Threading.Thread.Sleep(20)
            response = SerialPort1.ReadExisting()
            ' MsgBox(response)
            If CustomerID.Text = response Then
                'Enable the buttons and text fields
                SetBtn.Enabled = True
                ViewLogBtn.Enabled = True
                CloseConBtn.Enabled = True
                LimitBox.Enabled = True
                'Enable timer here to start the logging
                Timer1.Enabled = True
                worksheet.Cells(1, 1).Value = "Cutomer ID: " + response
            End If
        Catch ex As InvalidOperationException
            MsgBox("Sorry! System cannot be connected since serial port is closed!")
            Exit Sub
        End Try
    End Sub

    Private Sub CloseConBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseConBtn.Click
        SerialPort1.Close()
        SetBtn.Enabled = False
        ViewLogBtn.Enabled = False
        CloseConBtn.Enabled = False
        Timer1.Enabled = False
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' put in logging code here
        SerialPort1.WriteLine("3")
        Dim response As String
        Threading.Thread.Sleep(20)
        response = SerialPort1.ReadExisting()
        ' MsgBox(response)
        ' write code to log data into excel sheets
        Dim log(1, 4) As String
        log(1, 1) = Format(Now, "Short Date")
        log(1, 2) = Format(Now, "Short Time")
        log(1, 3) = response
        log(1, 4) = "0000"
        worksheet.Cells(rowNum, 1).Value = log(1, 1)
        worksheet.Cells(rowNum, 2).Value = log(1, 2)
        worksheet.Cells(rowNum, 3).Value = log(1, 3)
        worksheet.Cells(rowNum, 4).Value = log(1, 4)
        rowNum += 1
    End Sub

    Private Sub CustomerID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerID.TextChanged
        ' the connect button is enabled only when Customer ID is given
        If CustomerID.Text.Length > 0 Then
            ConnectBtn.Enabled = True
        Else
            ConnectBtn.Enabled = False
        End If

    End Sub

    Private Sub SetBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetBtn.Click
        If LimitBox.Text.Length > 0 Then
            ' Send command 2 and then limit value
            SerialPort1.WriteLine("2")
            Dim response As String
            ' recieve Acknowledgement here
            Threading.Thread.Sleep(20)
            response = SerialPort1.ReadExisting()
            ' MsgBox(response)
            If response = "1" Then
                SerialPort1.WriteLine(LimitBox.Text)
            End If
        End If
    End Sub

    Private Sub ViewLogBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewLogBtn.Click
        APP.Visible = True
    End Sub

End Class
