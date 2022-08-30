'#Region "#reference"
Imports System.Windows.Forms
Imports System
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.XtraReports.UI

' ...
'#End Region  ' #reference
Namespace RuntimeBindingToMdbDatabase

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

'#Region "#code"
        Private Function BindToData() As SqlDataSource
            ' Create a data source with the specified connection parameters.  
            Dim connectionParameters As Access97ConnectionParameters = New Access97ConnectionParameters("../../nwind.mdb", "", "")
            Dim ds As SqlDataSource = New SqlDataSource(connectionParameters)
            ' Create an SQL query to access the Products table.
            Dim query As CustomSqlQuery = New CustomSqlQuery()
            query.Name = "customQuery"
            query.Sql = "SELECT * FROM Products"
            ' Add the query to the collection. 
            ds.Queries.Add(query)
            ' Update the data source schema.
            ds.RebuildResultSchema()
            Return ds
        End Function

        Private Function CreateReport() As XtraReport
            ' Create a new report instance.
            Dim report As XtraReport = New XtraReport()
            ' Assign the data source to the report.
            report.DataSource = BindToData()
            report.DataMember = "customQuery"
            ' Add a detail band to the report.
            Dim detailBand As DetailBand = New DetailBand()
            detailBand.Height = 50
            report.Bands.Add(detailBand)
            ' Create a new label.
            Dim label As XRLabel = New XRLabel With {.WidthF = 300}

            ' Bind the label to the data, dependent on the data binding mode.
            Dim DesignerOptions = DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions
            If DesignerOptions.DataBindingMode = DataBindingMode.Bindings Then
                label.DataBindings.Add("Text", Nothing, "customQuery.ProductName")
            Else
                label.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[ProductName]"))
            End If

            ' Add the label to the detail band.
            detailBand.Controls.Add(label)
            Return report
        End Function

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Invoke the report preview.
            Dim printTool As ReportPrintTool = New ReportPrintTool(CreateReport())
            printTool.ShowPreview()
        End Sub

        Private Sub button2_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Invoke the End-User Designer and load the report.
            Dim designTool As ReportDesignTool = New ReportDesignTool(CreateReport())
            designTool.ShowRibbonDesignerDialog()
        End Sub
'#End Region  ' #code
    End Class
End Namespace
