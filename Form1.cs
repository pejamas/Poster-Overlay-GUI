using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace New_Overlay_GUI
{
    public partial class Form1 : Form
    {
        private PictureBox pbResultImage;
        private ComboBox cboOverlayImage;
        public Form1()
        {
            NewMethod();
        }

        private void NewMethod()
        {
            InitializeComponent();

            // Set background color
            this.BackColor = Color.DarkGray;

            // Create overlay image combo box
            cboOverlayImage = new ComboBox();
            cboOverlayImage.Items.Add("4K with DV and IMAX");
            cboOverlayImage.Items.Add("4K with DV");
            cboOverlayImage.Items.Add("4K with HDR");
            cboOverlayImage.Items.Add("4K with HDR and IMAX");
            cboOverlayImage.Size = new Size(160, 23);
            cboOverlayImage.Location = new Point(193, 17);
            cboOverlayImage.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOverlayImage.BackColor = Color.LightGray;
            cboOverlayImage.ForeColor = Color.Black;
            cboOverlayImage.Font = new Font("Arial", 10, FontStyle.Regular);
            cboOverlayImage.FlatStyle = FlatStyle.Flat;
            cboOverlayImage.Text = "Choose Overlay";
            cboOverlayImage.SelectedIndex = 0; // set the default selected index to the first item (4K with DV and IMAX)
            Controls.Add(cboOverlayImage);

            // Create base image button
            Button btnBaseImage = new Button();
            btnBaseImage.Text = "Select Poster";
            btnBaseImage.Size = cboOverlayImage.Size;
            btnBaseImage.Location = new Point(23, 17);
            btnBaseImage.BackColor = Color.LightGray;
            btnBaseImage.ForeColor = Color.Black;
            btnBaseImage.Font = new Font("Arial", 10, FontStyle.Regular);
            btnBaseImage.FlatStyle = FlatStyle.Flat;
            btnBaseImage.Click += new EventHandler(btnBaseImage_Click);
            Controls.Add(btnBaseImage);

            // Create result image picture box
            pbResultImage = new PictureBox();
            pbResultImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbResultImage.Size = new Size(502, 755);
            pbResultImage.Location = new Point(23, 60);
            Controls.Add(pbResultImage);

            // Create save button
            Button btnSaveImage = new Button();
            btnSaveImage.Text = "Save Poster";
            btnSaveImage.Size = cboOverlayImage.Size;
            btnSaveImage.Location = new Point(365, 17);
            btnSaveImage.BackColor = Color.LightGray;
            btnSaveImage.ForeColor = Color.Black;
            btnSaveImage.Font = new Font("Arial", 10, FontStyle.Regular);
            btnSaveImage.FlatStyle = FlatStyle.Flat;
            btnSaveImage.Click += new EventHandler(btnSaveImage_Click);
            Controls.Add(btnSaveImage);

            // Create reset button
            Button btnReset = new Button();
            btnReset.Text = "Reset";
            btnReset.Size = cboOverlayImage.Size;
            btnReset.Location = new Point(167, 842);
            btnReset.BackColor = Color.LightGray;
            btnReset.ForeColor = Color.Black;
            btnReset.Font = new Font("Arial", 10, FontStyle.Regular);
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Click += new EventHandler(btnReset_Click);
            Controls.Add(btnReset);

            // Handle combo box selection changed event
            cboOverlayImage.SelectedIndexChanged += new EventHandler(cboOverlayImage_SelectedIndexChanged);

        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            // Clear the selected overlay image in the combo box
            cboOverlayImage.SelectedIndex = -1;

            // Clear the result image in the picture box
            pbResultImage.Image = null;
        }
        private void btnBaseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = new Bitmap(openFileDialog.FileName);

                // Resize the image to the same dimensions as the overlay image
                Bitmap resizedBitmap = new Bitmap(1000, 1500);
                using (Graphics graphics = Graphics.FromImage(resizedBitmap))
                {
                    graphics.DrawImage(bitmap, 0, 0, 1000, 1500);
                }

                pbResultImage.Image = resizedBitmap;
            }
        }

        private void cboOverlayImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            string overlayFilename = "";

            switch (cbo.SelectedIndex)
            {
                case 0:
                    overlayFilename = @"E:\Plex Posters\4k\Overlays\4K with DV and Imax.png";
                    break;
                case 1:
                    overlayFilename = @"E:\Plex Posters\4k\Overlays\4K with DV.png";
                    break;
                case 2:
                    overlayFilename = @"E:\Plex Posters\4k\Overlays\4K with HDR.png";
                    break;
                case 3:
                    overlayFilename = @"E:\Plex Posters\4k\Overlays\4K-HDR with Imax.png";
                    break;
            }

            if (overlayFilename != "")
            {
                Bitmap baseBitmap = pbResultImage.Image != null ? new Bitmap(pbResultImage.Image) : new Bitmap(1000, 1500);

                Bitmap overlayBitmap = new Bitmap(overlayFilename);

                // Resize the overlay image to fit within the bounds of the base image
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

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();
                ImageFormat format = extension == ".jpg" ? ImageFormat.Jpeg : ImageFormat.Png;
                pbResultImage.Image.Save(saveFileDialog.FileName, format);
            }
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}