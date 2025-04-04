using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace PosterOverlay
{
    public partial class PosterOverlay : Form
    {
        private PictureBox pbResultImage;
        private ComboBox cboOverlayImage;
        private Image baseImage;
        private object overlayImagePath;
        private string currentFileandPath;

        public PosterOverlay()
        {
            InitializeComponent();
            ConfigureForm();
        }

        private void ConfigureForm()
        {
            SetFormAppearance();
            CreateOverlayImageComboBox();
            CreateBaseImageButton();
            CreateResultImagePictureBox();
            CreateSaveAsImageButton();
            CreateSaveImageButton();
            CreateResetButton();
        }

        private void SetFormAppearance()
        {
            this.BackColor = Color.DarkGray;
        }

        private void CreateOverlayImageComboBox()
        {
            cboOverlayImage = new ComboBox();
            cboOverlayImage.Items.AddRange(new object[] {
                "UHDBD with HDR and Atmos",
                "UHDBD with DV and Atmos",
                "UHDBD with HDR and DTS",
                "UHDBD with DV and DTS",
                "UHDBD with DV and IMAX",
                "UHDBD with DV",
                "UHDBD with HDR",
                "UHDBD with HDR and IMAX",
                "UHDBD with HDR10+",
                "UHDBD with HDR10+ and IMAX",
                "UHDBD with IMAX",
                "UHDBD",
                "BD with DV and IMAX",
                "BD with DV",
                "BD with HDR",
                "BD with HDR and IMAX",
                "BD with HDR10+",
                "BD with HDR10+ and IMAX",
                "BD with IMAX",
                "BD 3D",
                "BD 3D with IMAX",
                "BD",
                "DTheater"
            });
            cboOverlayImage.Size = new Size(160, 23);
            cboOverlayImage.Location = new Point(193, 17);
            cboOverlayImage.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOverlayImage.BackColor = Color.LightGray;
            cboOverlayImage.ForeColor = Color.Black;
            cboOverlayImage.Font = new Font("Arial", 10, FontStyle.Regular);
            cboOverlayImage.FlatStyle = FlatStyle.Flat;
            cboOverlayImage.Text = "Choose Overlay";
            cboOverlayImage.SelectedIndex = 0;
            Controls.Add(cboOverlayImage);
            cboOverlayImage.SelectedIndexChanged += new EventHandler(cboOverlayImage_SelectedIndexChanged);
        }

        private void CreateBaseImageButton()
        {
            Button btnBaseImage = new Button
            {
                Text = "Select Poster",
                Size = cboOverlayImage.Size,
                Location = new Point(23, 17),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            btnBaseImage.Click += new EventHandler(btnBaseImage_Click);
            Controls.Add(btnBaseImage);
        }

        private void CreateResultImagePictureBox()
        {
            pbResultImage = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(502, 755),
                Location = new Point(23, 60),
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pbResultImage);
        }

        private void CreateSaveAsImageButton()
        {
            Button btnSaveAsImage = new Button
            {
                Text = "Save as...",
                Size = cboOverlayImage.Size,
                Location = new Point(365, 842),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            btnSaveAsImage.Click += new EventHandler(btnSaveAsImage_Click);
            Controls.Add(btnSaveAsImage);
        }

        private void CreateSaveImageButton()
        {
            Button btnSaveImage = new Button
            {
                Text = "Save",
                Size = cboOverlayImage.Size,
                Location = new Point(365, 17),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            btnSaveImage.Click += new EventHandler(btnSaveImage_Click);
            Controls.Add(btnSaveImage);
        }

        private void CreateResetButton()
        {
            Button btnReset = new Button
            {
                Text = "Reset",
                Size = cboOverlayImage.Size,
                Location = new Point(190, 842),
                BackColor = Color.Red,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat
            };
            btnReset.Click += new EventHandler(btnReset_Click);
            Controls.Add(btnReset);
        }

        private void pbResultImage_Paint(object sender, PaintEventArgs e)
        {
            Pen borderPen = new Pen(Color.White, 10);
            e.Graphics.DrawRectangle(borderPen, new Rectangle(pbResultImage.Location, pbResultImage.Size));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cboOverlayImage.SelectedIndex = -1;
            pbResultImage.Image = null;

            //Clear the base image so overwriting it is possible
            baseImage.Dispose();
        }

        private void btnBaseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Load the base image using method
                LoadBaseImage(openFileDialog.FileName);

                //Ensure we save the current filename and path so we can do quick overwrites later
                currentFileandPath = openFileDialog.FileName;
            }
        }

        private void LoadBaseImage(string filenameAndPath)
        {
            baseImage = Image.FromFile(filenameAndPath);
            int maxWidth = 1000;
            int maxHeight = 1500;
            double ratioX = (double)maxWidth / baseImage.Width;
            double ratioY = (double)maxHeight / baseImage.Height;
            double ratio = Math.Min(ratioX, ratioY);
            int newWidth = (int)(baseImage.Width * ratio);
            int newHeight = (int)(baseImage.Height * ratio);
            Bitmap bitmap = new Bitmap(baseImage, newWidth, newHeight);
            pbResultImage.Image = bitmap;

            //Select whatever Overlay is currently selected in the dropdown list as the one to apply when base image is loaded
            SelectOverlay();
        }

        private void SelectOverlay()
        {
            string overlayFilename = "";

            switch (cboOverlayImage.SelectedIndex)
            {
                case 0:
                    overlayFilename = @"OverlayImages\UHDBD with HDR and Atmos.png";
                    break;
                case 1:
                    overlayFilename = @"OverlayImages\UHDBD with DV and Atmos.png";
                    break;
                case 2:
                    overlayFilename = @"OverlayImages\UHDBD with HDR and DTS.png";
                    break;
                case 3:
                    overlayFilename = @"OverlayImages\UHDBD with DV and DTS.png";
                    break;
                case 4:
                    overlayFilename = @"OverlayImages\UHDBD with DV and Imax.png";
                    break;
                case 5:
                    overlayFilename = @"OverlayImages\UHDBD with DV.png";
                    break;
                case 6:
                    overlayFilename = @"OverlayImages\UHDBD with HDR.png";
                    break;
                case 7:
                    overlayFilename = @"OverlayImages\UHDBD with HDR and Imax.png";
                    break;
                case 8:
                    overlayFilename = @"OverlayImages\UHDBD with HDR10+.png";
                    break;
                case 9:
                    overlayFilename = @"OverlayImages\UHDBD with HDR10+ and Imax.png";
                    break;
                case 10:
                    overlayFilename = @"OverlayImages\UHDBD with Imax.png";
                    break;
                case 11:
                    overlayFilename = @"OverlayImages\UHDBD.png";
                    break;
                case 12:
                    overlayFilename = @"OverlayImages\BD with DV and Imax.png";
                    break;
                case 13:
                    overlayFilename = @"OverlayImages\BD with DV.png";
                    break;
                case 14:
                    overlayFilename = @"OverlayImages\BD with HDR.png";
                    break;
                case 15:
                    overlayFilename = @"OverlayImages\BD with HDR and Imax.png";
                    break;
                case 16:
                    overlayFilename = @"OverlayImages\BD with HDR10+.png";
                    break;
                case 17:
                    overlayFilename = @"OverlayImages\BD with HDR10+ and Imax.png";
                    break;
                case 18:
                    overlayFilename = @"OverlayImages\BD with Imax.png";
                    break;
                case 19:
                    overlayFilename = @"OverlayImages\BD 3D.png";
                    break;
                case 20:
                    overlayFilename = @"OverlayImages\BD 3D with IMAX.png";
                    break;
                case 21:
                    overlayFilename = @"OverlayImages\BD.png";
                    break;
                case 22:
                    overlayFilename = @"OverlayImages\DTheater.png";
                    break;
            }
            ApplyOverlay(overlayFilename);
        }

        private void cboOverlayImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectOverlay();
        }

        private void ApplyOverlay(string overlayFilename)
        {
            if (overlayFilename != "")
            {
                overlayImagePath = overlayFilename;
                Bitmap baseBitmap = pbResultImage.Image != null ? new Bitmap(pbResultImage.Image) : new Bitmap(1000, 1500);
                if (baseImage != null)
                {
                    int maxWidth = 1000;
                    int maxHeight = 1500;
                    double ratioX = (double)maxWidth / baseImage.Width;
                    double ratioY = (double)maxHeight / baseImage.Height;
                    double ratio = Math.Min(ratioX, ratioY);
                    int resizedWidth = (int)(baseImage.Width * ratio);
                    int resizedHeight = (int)(baseImage.Height * ratio);
                    Bitmap resizedBitmap = new Bitmap(resizedWidth, resizedHeight);
                    using (Graphics graphics = Graphics.FromImage(resizedBitmap))
                    {
                        graphics.DrawImage(baseImage, 0, 0, resizedWidth, resizedHeight);
                    }
                    baseBitmap = resizedBitmap;
                }
                Bitmap overlayBitmap = new Bitmap(overlayFilename);
                float scaleX = (float)baseBitmap.Width / overlayBitmap.Width;
                float scaleY = (float)baseBitmap.Height / overlayBitmap.Height;
                float scale = Math.Min(scaleX, scaleY);
                int newWidth = (int)(overlayBitmap.Width * scale);
                int newHeight = (int)(overlayBitmap.Height * scale);
                Bitmap resizedOverlayBitmap = new Bitmap(newWidth, newHeight);
                using (Graphics graphics = Graphics.FromImage(resizedOverlayBitmap))
                {
                    graphics.DrawImage(overlayBitmap, 0, 0, newWidth, newHeight);
                }
                Bitmap resultBitmap = new Bitmap(baseBitmap.Width, baseBitmap.Height);
                using (Graphics graphics = Graphics.FromImage(resultBitmap))
                {
                    graphics.DrawImage(baseBitmap, 0, 0);
                    graphics.DrawImage(resizedOverlayBitmap, 0, 0);
                }
                pbResultImage.Image = resultBitmap;
            }
        }
        private void btnSaveAsImage_Click(object sender, EventArgs e)
        {
            if (baseImage == null || overlayImagePath == null)
            {
                MessageBox.Show("Please select a base image and overlay image.", "Error");
                return;
            }

            Image overlayImage = Image.FromFile((string)overlayImagePath);
            int targetWidth = 1000;
            int targetHeight = 1500;

            Bitmap resultBitmap = new Bitmap(targetWidth, targetHeight, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.DrawImage(baseImage, 0, 0, targetWidth, targetHeight);
                g.DrawImage(overlayImage, 0, 0, targetWidth, targetHeight);
            }

            pbResultImage.Image = resultBitmap;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png",
                DefaultExt = "jpg" // Default extension
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Clear the base image to allow overwriting
                baseImage.Dispose();

                // Save the new image in the desired size
                resultBitmap.Save(saveFileDialog.FileName, ImageFormat.Jpeg);

                // Reload the now updated base image to continue working with it if needed
                LoadBaseImage(saveFileDialog.FileName);
            }
        }


        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (baseImage == null || overlayImagePath == null)
            {
                MessageBox.Show("Please select a base image and overlay image.", "Error");
                return;
            }

            Image overlayImage = Image.FromFile((string)overlayImagePath);

            // Zielgröße für das Bild festlegen
            int targetWidth = 1000;
            int targetHeight = 1500;

            // Erstellen eines neuen Bitmaps mit der Zielgröße
            Bitmap resultBitmap = new Bitmap(targetWidth, targetHeight, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                // Basisbild und Overlay-Bild skalieren und zeichnen
                g.DrawImage(baseImage, 0, 0, targetWidth, targetHeight);
                g.DrawImage(overlayImage, 0, 0, targetWidth, targetHeight);
            }

            pbResultImage.Image = resultBitmap;

            // Basisbild freigeben, um das Überschreiben zu ermöglichen
            baseImage.Dispose();

            // Neues Bild im aktuellen Pfad und Ursprungsformat speichern
            string extension = Path.GetExtension(currentFileandPath).ToLower();
            ImageFormat imageFormat;

            // Format basierend auf Dateierweiterung bestimmen
            switch (extension)
            {
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                default:
                    MessageBox.Show("Unsupported file format.", "Error");
                    return;
            }

            resultBitmap.Save(currentFileandPath, imageFormat);

            // Aktualisiertes Basisbild neu laden, falls es weiterverwendet werden soll
            LoadBaseImage(currentFileandPath);
        }
        private void PosterOverlay_Load(object sender, EventArgs e)
        {

        }
    }
}
