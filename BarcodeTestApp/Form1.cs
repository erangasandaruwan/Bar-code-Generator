using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TECIT.TBarCode;

namespace BarcodeTestApp
{
    public partial class Form1 : Form
    {
        private TECIT.TBarCode.Windows.BarcodeControl brcodeController;
        public Form1()
        {
            InitializeComponent();

            saveFileDlg.AddExtension = true;
            saveFileDlg.Filter =
              "Bitmap (*.BMP)|*.bmp"
              + "|CompuServe GIF (*.GIF)|*.gif"
              + "|Enhanced Metafile (*.EMF)|*.emf"
              + "|JPEG (*.JPG)|*.jpg"
              + "|PCX (*.PCX)|*.pcx"
              + "|PNG (*.PNG)|*.png"
              + "|TIFF (*.TIF)|*.tif";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                brcodeController = new TECIT.TBarCode.Windows.BarcodeControl();
                brcodeController.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
                brcodeController.Barcode.BearerBarType = TECIT.TBarCode.BearerBarType.None;
                brcodeController.Barcode.CodablockF.RowSeparatorHeight = -1F;
                brcodeController.Barcode.Data = txtBarcodeText.Text;
                brcodeController.Barcode.ModuleWidth = -1F;
                brcodeController.Location = new Point(0, 3);
                brcodeController.Name = "barcodeControl1";
                brcodeController.Size = new Size(300, 100);
                brcodeController.TabIndex = 0;

                panelBarcode.Controls.Clear();
                panelBarcode.Controls.Add(brcodeController);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generate Barcodes with TBarCode",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (brcodeController == null)
                {
                    MessageBox.Show("Generate Barcodes first before print", "Generate Barcodes with TBarCode", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = saveFileDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string filename = saveFileDlg.FileName;
                    string extension = System.IO.Path.GetExtension(filename);
                    ImageType type;
                    switch (extension.ToUpper())
                    {
                        case ".BMP":
                            type = ImageType.Bmp;
                            break;
                        case ".GIF":
                            type = ImageType.Gif;
                            break;
                        case ".EMF":
                            type = ImageType.Emf;
                            break;
                        case ".JPG":
                            type = ImageType.Jpg;
                            break;
                        case ".PCX":
                            type = ImageType.Pcx;
                            break;
                        case ".PNG":
                            type = ImageType.Png;
                            break;
                        case ".TIF":
                            type = ImageType.Tif;
                            break;
                        default:
                            MessageBox.Show("The selected bitmap format is not supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    brcodeController.Barcode.Draw(filename, type);
                    MessageBox.Show("Barcode image saved successfully", "Generate Barcodes with TBarCode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generate Barcodes with TBarCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (brcodeController == null)
                {
                    MessageBox.Show("Generate Barcodes first before print", "Generate Barcodes with TBarCode", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                prntDlg.Document = new PrintDocument();
                DialogResult result = prntDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    prntDlg.Document.PrinterSettings = prntDlg.PrinterSettings;
                    prntDlg.Document.PrintPage += PrintPage;
                    prntDlg.Document.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generate Barcodes with TBarCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                float dpiX = ev.Graphics.DpiX;
                float dpiY = ev.Graphics.DpiY;
                brcodeController.Barcode.BoundingRectangle = new Rectangle((int)(10 / 25.4 * dpiX),
                                                           (int)(10 / 25.4 * dpiY),
                                                           (int)(50 / 25.4 * dpiX),
                                                           (int)(30 / 25.4 * dpiY));

                brcodeController.Barcode.Draw(ev.Graphics);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generate Barcodes with TBarCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
