Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmDT

    Private Sub FrmDT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TampilData()
    End Sub

    Private Sub TampilData()
        Try
            Call Koneksi()
            Dim sql As String = "SELECT id_detail FROM detail_transaksi"

            Da = New MySqlDataAdapter(sql, Conn)
            Ds = New DataSet
            Da.Fill(Ds, "detail_transaksi")
            DataGridView1.DataSource = Ds.Tables("detail_transaksi")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        End Try
        With DataGridView1
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .Columns("id_detail").HeaderText = "ID Detail"
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.Black
            .DefaultCellStyle.SelectionForeColor = Color.Cyan
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.Cyan
            DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        End With
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim idDetail As String = DataGridView1.Rows(e.RowIndex).Cells("id_detail").Value.ToString()

        'Kirim ke FrmPembayaran
        FrmPembayaran.txtTransaksi.Text = idDetail

        'Tutup form detail (opsional)
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.White, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = {
                Color.Cyan,
                Color.Yellow,
                Color.Lime,
                Color.Cyan,
                Color.Azure,
                Color.Lime,
                Color.Cyan,
                Color.Yellow
            }
            blend.Positions = {0.0F, 0.12F, 0.25F, 0.4F, 0.55F, 0.7F, 0.85F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub


End Class