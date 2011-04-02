<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Label1 = New System.Windows.Forms.Label
        Me.CustomerID = New System.Windows.Forms.TextBox
        Me.ConnectBtn = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.LimitBox = New System.Windows.Forms.TextBox
        Me.SetBtn = New System.Windows.Forms.Button
        Me.ViewLogBtn = New System.Windows.Forms.Button
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.CloseConBtn = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter Customer ID:"
        '
        'CustomerID
        '
        Me.CustomerID.Location = New System.Drawing.Point(140, 25)
        Me.CustomerID.Name = "CustomerID"
        Me.CustomerID.Size = New System.Drawing.Size(169, 20)
        Me.CustomerID.TabIndex = 1
        '
        'ConnectBtn
        '
        Me.ConnectBtn.Location = New System.Drawing.Point(360, 18)
        Me.ConnectBtn.Name = "ConnectBtn"
        Me.ConnectBtn.Size = New System.Drawing.Size(103, 32)
        Me.ConnectBtn.TabIndex = 2
        Me.ConnectBtn.Text = "Connect"
        Me.ConnectBtn.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Set Load Limit %"
        '
        'LimitBox
        '
        Me.LimitBox.Location = New System.Drawing.Point(140, 71)
        Me.LimitBox.Name = "LimitBox"
        Me.LimitBox.Size = New System.Drawing.Size(169, 20)
        Me.LimitBox.TabIndex = 4
        '
        'SetBtn
        '
        Me.SetBtn.Location = New System.Drawing.Point(360, 65)
        Me.SetBtn.Name = "SetBtn"
        Me.SetBtn.Size = New System.Drawing.Size(103, 30)
        Me.SetBtn.TabIndex = 5
        Me.SetBtn.Text = "Set Limit"
        Me.SetBtn.UseVisualStyleBackColor = True
        '
        'ViewLogBtn
        '
        Me.ViewLogBtn.Location = New System.Drawing.Point(35, 152)
        Me.ViewLogBtn.Name = "ViewLogBtn"
        Me.ViewLogBtn.Size = New System.Drawing.Size(166, 26)
        Me.ViewLogBtn.TabIndex = 6
        Me.ViewLogBtn.Text = "View Customer Log"
        Me.ViewLogBtn.UseVisualStyleBackColor = True
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 2400
        Me.SerialPort1.PortName = "COM59"
        Me.SerialPort1.ReadTimeout = 1000
        Me.SerialPort1.WriteTimeout = 2000
        '
        'CloseConBtn
        '
        Me.CloseConBtn.Location = New System.Drawing.Point(243, 154)
        Me.CloseConBtn.Name = "CloseConBtn"
        Me.CloseConBtn.Size = New System.Drawing.Size(179, 24)
        Me.CloseConBtn.TabIndex = 7
        Me.CloseConBtn.Text = "Close Connection"
        Me.CloseConBtn.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 5000
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(482, 223)
        Me.Controls.Add(Me.CloseConBtn)
        Me.Controls.Add(Me.ViewLogBtn)
        Me.Controls.Add(Me.SetBtn)
        Me.Controls.Add(Me.LimitBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ConnectBtn)
        Me.Controls.Add(Me.CustomerID)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Real Time Customer Side Load Regulation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CustomerID As System.Windows.Forms.TextBox
    Friend WithEvents ConnectBtn As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LimitBox As System.Windows.Forms.TextBox
    Friend WithEvents SetBtn As System.Windows.Forms.Button
    Friend WithEvents ViewLogBtn As System.Windows.Forms.Button
    Friend WithEvents CloseConBtn As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Private WithEvents SerialPort1 As System.IO.Ports.SerialPort

End Class
