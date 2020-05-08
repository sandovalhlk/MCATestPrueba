namespace SistContabilidadMCA.Reportes
{
    partial class RepSituacionFinanciera
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepSituacionFinanciera));
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCierreAnual = new DevExpress.XtraReports.UI.XRLabel();
            this.lblProyecto = new DevExpress.XtraReports.UI.XRLabel();
            this.lblModuloMCA = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUcr = new DevExpress.XtraReports.UI.XRRichText();
            this.lblFechaFinal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblFechaInicial = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.moduloMCAId = new DevExpress.XtraReports.Parameters.Parameter();
            this.fechaInicial = new DevExpress.XtraReports.Parameters.Parameter();
            this.fechaFinal = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.totalSaldoBanco = new DevExpress.XtraReports.UI.CalculatedField();
            this.SumSaldoDebito = new DevExpress.XtraReports.UI.CalculatedField();
            this.sumSaldoCredito = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblUsuario = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.saldoAnteriorC = new DevExpress.XtraReports.Parameters.Parameter();
            this.formattingRule2 = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.formattingRule3 = new DevExpress.XtraReports.UI.FormattingRule();
            this.lblTesorero = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPresidente = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblContador = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.lblUcr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.FormattingRules.Add(this.formattingRule1);
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.Visible = false;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Condition = "[totalSaldoBanco] > 0";
            // 
            // 
            // 
            this.formattingRule1.Formatting.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.formattingRule1.Name = "formattingRule1";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.lblCierreAnual,
            this.lblProyecto,
            this.lblModuloMCA,
            this.lblUcr,
            this.lblFechaFinal,
            this.lblFechaInicial,
            this.xrLabel7,
            this.xrLabel16,
            this.xrLabel6,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLine5});
            this.TopMargin.HeightF = 202.0834F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.saldoAnteriorC, "Text", "{0: #,#0.00; (#,#.00) }")});
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(497.9167F, 168.0417F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(123.9583F, 23F);
            this.xrLabel12.StylePriority.UseBorders = false;
            // 
            // lblCierreAnual
            // 
            this.lblCierreAnual.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblCierreAnual.LocationFloat = new DevExpress.Utils.PointFloat(465.0834F, 91.00001F);
            this.lblCierreAnual.Name = "lblCierreAnual";
            this.lblCierreAnual.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCierreAnual.SizeF = new System.Drawing.SizeF(174.9166F, 23.00002F);
            this.lblCierreAnual.StylePriority.UseFont = false;
            this.lblCierreAnual.StylePriority.UseTextAlignment = false;
            this.lblCierreAnual.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblProyecto
            // 
            this.lblProyecto.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblProyecto.LocationFloat = new DevExpress.Utils.PointFloat(3.000005F, 45.00001F);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblProyecto.SizeF = new System.Drawing.SizeF(644.0001F, 23F);
            this.lblProyecto.StylePriority.UseFont = false;
            this.lblProyecto.StylePriority.UseTextAlignment = false;
            this.lblProyecto.Text = "lblProyecto";
            this.lblProyecto.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblModuloMCA
            // 
            this.lblModuloMCA.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblModuloMCA.LocationFloat = new DevExpress.Utils.PointFloat(3.000021F, 68.00001F);
            this.lblModuloMCA.Name = "lblModuloMCA";
            this.lblModuloMCA.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblModuloMCA.SizeF = new System.Drawing.SizeF(644.0001F, 23F);
            this.lblModuloMCA.StylePriority.UseFont = false;
            this.lblModuloMCA.StylePriority.UseTextAlignment = false;
            this.lblModuloMCA.Text = "lblModuloMCA";
            this.lblModuloMCA.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblUcr
            // 
            this.lblUcr.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblUcr.LocationFloat = new DevExpress.Utils.PointFloat(474.2502F, 21.02084F);
            this.lblUcr.Name = "lblUcr";
            this.lblUcr.SerializableRtfString = resources.GetString("lblUcr.SerializableRtfString");
            this.lblUcr.SizeF = new System.Drawing.SizeF(81.75024F, 23F);
            this.lblUcr.StylePriority.UseFont = false;
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.LocationFloat = new DevExpress.Utils.PointFloat(172.1666F, 173.0416F);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFechaFinal.SizeF = new System.Drawing.SizeF(106.2501F, 23.00002F);
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.LocationFloat = new DevExpress.Utils.PointFloat(57.58338F, 173.0416F);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFechaInicial.SizeF = new System.Drawing.SizeF(78.125F, 23.00001F);
            // 
            // xrLabel7
            // 
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(135.7085F, 173.0416F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(36.45834F, 23F);
            this.xrLabel7.Text = "Hasta";
            // 
            // xrLabel16
            // 
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(12.79171F, 173.0416F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(44.79166F, 23F);
            this.xrLabel16.Text = "Desde";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(138.7085F, 22.00001F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(335.5417F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "MODULO COMUNITARIO DE ADOQUINADO MTI - ";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(3.000021F, 91.00001F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(647F, 22.99999F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "ESTADO DE LA SITUACION FINANCIERA";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(329.1667F, 168.0417F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(168.75F, 23F);
            this.xrLabel3.Text = "SALDO MES ANTERIOR";
            // 
            // xrLine5
            // 
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 196.7083F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(647F, 4.333338F);
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 4.166667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "ContabilidadMCA";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "RepSituacionFinanciera";
            queryParameter1.Name = "@moduloMCAId";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.moduloMCAId]", typeof(int));
            queryParameter2.Name = "@fechaInicial";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("[Parameters.fechaInicial]", typeof(System.DateTime));
            queryParameter3.Name = "@fechaFinal";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("[Parameters.fechaFinal]", typeof(System.DateTime));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.StoredProcName = "RepSituacionFinanciera";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // moduloMCAId
            // 
            this.moduloMCAId.Name = "moduloMCAId";
            this.moduloMCAId.Type = typeof(int);
            this.moduloMCAId.ValueInfo = "0";
            // 
            // fechaInicial
            // 
            this.fechaInicial.Name = "fechaInicial";
            this.fechaInicial.Type = typeof(System.DateTime);
            // 
            // fechaFinal
            // 
            this.fechaFinal.Name = "fechaFinal";
            this.fechaFinal.Type = typeof(System.DateTime);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.GroupHeader1.FormattingRules.Add(this.formattingRule2);
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("descripcion", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 25F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(427.2916F, 25F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RepSituacionFinanciera.descripcion")});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Weight = 1.50626317671565D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RepSituacionFinanciera.DebitoCredito")});
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0: #,#0.00; (#,#.00) }";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell4.Summary = xrSummary1;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell4.Weight = 0.634655317710097D;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(3.000021F, 33.16669F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(647F, 4.333338F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RepSituacionFinanciera.MovimientosBancos")});
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(15.87499F, 10F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(140.625F, 22.29168F);
            this.xrLabel1.StylePriority.UseFont = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(275.4167F, 9.999974F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(53.75F, 23F);
            this.xrLabel10.Text = "TOTAL\r\n";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RepSituacionFinanciera.DebitoCredito")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(329.1667F, 9.999974F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(126.6666F, 23F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0: #,#0.00; (#,#.00) }";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel2.Summary = xrSummary2;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel5});
            this.ReportFooter.HeightF = 34.46592F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RepSituacionFinanciera.totalSaldoBanco", "{0: #,#0.00; (#,#.00) }")});
            this.xrLabel8.FormattingRules.Add(this.formattingRule2);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(535.25F, 8.041699F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel8.StylePriority.UseBorders = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(300.2083F, 8.041674F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(235.0417F, 23F);
            this.xrLabel9.Text = "SALDO ACTUAL EN CAJA Y BANCO ";
            // 
            // totalSaldoBanco
            // 
            this.totalSaldoBanco.DataMember = "RepSituacionFinanciera";
            this.totalSaldoBanco.Expression = "([Parameters.saldoAnteriorC] + [].Sum([Creditos]))-[].Sum([Debitos])";
            this.totalSaldoBanco.Name = "totalSaldoBanco";
            // 
            // SumSaldoDebito
            // 
            this.SumSaldoDebito.DataMember = "RepSituacionFinanciera";
            this.SumSaldoDebito.Expression = " Iif([MovimientosBancos] == \'COSTO\', [].Sum([Debitos]) , 0 )";
            this.SumSaldoDebito.Name = "SumSaldoDebito";
            // 
            // sumSaldoCredito
            // 
            this.sumSaldoCredito.DataMember = "RepSituacionFinanciera";
            this.sumSaldoCredito.Expression = " Iif([MovimientosBancos] == \'INGRESOS\', [].Sum([Creditos]) , 0 )";
            this.sumSaldoCredito.Name = "sumSaldoCredito";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "{0:dddd, dd\' de \'MMMM\' de \'yyyy hh:mm tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(8.87502F, 134.4583F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(257.2916F, 23.00001F);
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(303.625F, 134.4583F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(48.95831F, 23F);
            // 
            // lblUsuario
            // 
            this.lblUsuario.LocationFloat = new DevExpress.Utils.PointFloat(421.25F, 134.4583F);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUsuario.SizeF = new System.Drawing.SizeF(218.75F, 23F);
            this.lblUsuario.Text = "lblUsuario";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblTesorero,
            this.xrLabel11,
            this.lblPresidente,
            this.xrLabel18,
            this.xrLabel13,
            this.lblContador,
            this.xrLabel21,
            this.xrLabel22,
            this.xrLabel23,
            this.lblUsuario,
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.PageFooter.HeightF = 161.4583F;
            this.PageFooter.Name = "PageFooter";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLine1});
            this.GroupHeader2.FormattingRules.Add(this.formattingRule2);
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("MovimientosBancos", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)});
            this.GroupHeader2.HeightF = 40.625F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // GroupFooter2
            // 
            this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel10});
            this.GroupFooter2.FormattingRules.Add(this.formattingRule2);
            this.GroupFooter2.HeightF = 41.66667F;
            this.GroupFooter2.Level = 1;
            this.GroupFooter2.Name = "GroupFooter2";
            // 
            // saldoAnteriorC
            // 
            this.saldoAnteriorC.Description = "saldoAnterior x codigo";
            this.saldoAnteriorC.Name = "saldoAnteriorC";
            this.saldoAnteriorC.Type = typeof(float);
            this.saldoAnteriorC.ValueInfo = "0";
            // 
            // formattingRule2
            // 
            this.formattingRule2.Condition = "[DataSource.RowCount]==0";
            // 
            // 
            // 
            this.formattingRule2.Formatting.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.formattingRule2.Name = "formattingRule2";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.saldoAnteriorC, "Text", "{0: #,#0.00; (#,#.00) }")});
            this.xrLabel5.FormattingRules.Add(this.formattingRule3);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(535.2499F, 8.041699F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel5.StylePriority.UseBorders = false;
            // 
            // formattingRule3
            // 
            this.formattingRule3.Condition = "[totalSaldoBanco]>0";
            // 
            // 
            // 
            this.formattingRule3.Formatting.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.formattingRule3.Name = "formattingRule3";
            // 
            // lblTesorero
            // 
            this.lblTesorero.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblTesorero.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTesorero.LocationFloat = new DevExpress.Utils.PointFloat(229.9166F, 52.29166F);
            this.lblTesorero.Name = "lblTesorero";
            this.lblTesorero.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTesorero.SizeF = new System.Drawing.SizeF(210.8756F, 23F);
            this.lblTesorero.StylePriority.UseBorders = false;
            this.lblTesorero.StylePriority.UseFont = false;
            this.lblTesorero.StylePriority.UseTextAlignment = false;
            this.lblTesorero.Text = "lblTesorero";
            this.lblTesorero.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(480.8551F, 75.29161F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(157.2083F, 22.25378F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "PRESIDENTE MCA";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblPresidente
            // 
            this.lblPresidente.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPresidente.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPresidente.LocationFloat = new DevExpress.Utils.PointFloat(464.8548F, 52.29165F);
            this.lblPresidente.Name = "lblPresidente";
            this.lblPresidente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPresidente.SizeF = new System.Drawing.SizeF(182.1452F, 23.00002F);
            this.lblPresidente.StylePriority.UseBorders = false;
            this.lblPresidente.StylePriority.UseFont = false;
            this.lblPresidente.StylePriority.UseTextAlignment = false;
            this.lblPresidente.Text = "lblPresidente";
            this.lblPresidente.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(12.81312F, 38.83334F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(156.1666F, 13.45825F);
            this.xrLabel18.StylePriority.UseBorders = false;
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "ELABORADO POR ";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(12.81312F, 75.29161F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(156.1666F, 22.25378F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "CONTADOR MCA";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblContador
            // 
            this.lblContador.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblContador.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblContador.LocationFloat = new DevExpress.Utils.PointFloat(12.81312F, 52.29166F);
            this.lblContador.Name = "lblContador";
            this.lblContador.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblContador.SizeF = new System.Drawing.SizeF(194.2562F, 23F);
            this.lblContador.StylePriority.UseBorders = false;
            this.lblContador.StylePriority.UseFont = false;
            this.lblContador.StylePriority.UseTextAlignment = false;
            this.lblContador.Text = "lblContador";
            this.lblContador.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel21
            // 
            this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(256.9584F, 38.83334F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(156.1666F, 13.45825F);
            this.xrLabel21.StylePriority.UseBorders = false;
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "REVISADO POR ";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(255.9584F, 75.29161F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(156.1666F, 22.25378F);
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "TESORERO MCA";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(478.8548F, 37.83345F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(157.2083F, 13.45825F);
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "AUTORIZADO POR ";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // RepSituacionFinanciera
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.ReportFooter,
            this.PageFooter,
            this.GroupHeader2,
            this.GroupFooter2});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.totalSaldoBanco,
            this.SumSaldoDebito,
            this.sumSaldoCredito});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "RepSituacionFinanciera";
            this.DataSource = this.sqlDataSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1,
            this.formattingRule2,
            this.formattingRule3});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 202, 4);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.moduloMCAId,
            this.fechaInicial,
            this.fechaFinal,
            this.saldoAnteriorC});
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this.lblUcr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter moduloMCAId;
        private DevExpress.XtraReports.Parameters.Parameter fechaInicial;
        private DevExpress.XtraReports.Parameters.Parameter fechaFinal;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.CalculatedField totalSaldoBanco;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.CalculatedField SumSaldoDebito;
        private DevExpress.XtraReports.UI.CalculatedField sumSaldoCredito;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        public DevExpress.XtraReports.UI.XRLabel lblUsuario;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        public DevExpress.XtraReports.UI.XRLabel lblFechaFinal;
        public DevExpress.XtraReports.UI.XRLabel lblFechaInicial;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLine xrLine5;
        public DevExpress.XtraReports.UI.XRLabel lblProyecto;
        public DevExpress.XtraReports.UI.XRLabel lblModuloMCA;
        public DevExpress.XtraReports.UI.XRRichText lblUcr;
        public DevExpress.XtraReports.UI.XRLabel lblCierreAnual;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter2;
        private DevExpress.XtraReports.Parameters.Parameter saldoAnteriorC;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule3;
        public DevExpress.XtraReports.UI.XRLabel lblTesorero;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        public DevExpress.XtraReports.UI.XRLabel lblPresidente;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        public DevExpress.XtraReports.UI.XRLabel lblContador;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
    }
}
