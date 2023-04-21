using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace New_Overlay_GUI
{
    public partial class PosterOverlay : Form
    {
        private PictureBox pbResultImage;
        private ComboBox cboOverlayImage;
        private Image baseImage;
        private object overlayImagePath;
        private Button btnUp;
        private Button btnDown;

        public PosterOverlay()
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
            cboOverlayImage.Items.Add("4K with no HDR");
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
            pbResultImage.BorderStyle = BorderStyle.FixedSingle;
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
            btnReset.Location = new Point(190, 842);
            btnReset.BackColor = Color.LightGray;
            btnReset.ForeColor = Color.Black;
            btnReset.Font = new Font("Arial", 10, FontStyle.Regular);
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Click += new EventHandler(btnReset_Click);
            Controls.Add(btnReset);

            // Handle combo box selection changed event
            cboOverlayImage.SelectedIndexChanged += new EventHandler(cboOverlayImage_SelectedIndexChanged);

        }
        private void pbResultImage_Paint(object sender, PaintEventArgs e)
        {
            // Define border color and width
            Pen borderPen = new Pen(Color.White, 10);

            // Draw border
            e.Graphics.DrawRectangle(borderPen, new Rectangle(pbResultImage.Location, pbResultImage.Size));
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
                case 4:
                    overlayFilename = @"E:\Plex Posters\4k\Overlays\4K no HDR.png";
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
            // Check if both base image and overlay image have been selected
            if (baseImage == null || overlayImagePath == null)
            {
                MessageBox.Show("Please select a base image and overlay image.", "Error");
                return;
            }

            // Load overlay image
            Image overlayImage = Image.FromFile((string)overlayImagePath);

            // Calculate position of overlay image within result image
            int overlayX = (pbResultImage.Width - overlayImage.Width) / 2;
            int overlayY = (pbResultImage.Height - overlayImage.Height) / 2;

            // Create new bitmap to draw overlay and base image onto
            Bitmap resultBitmap = new Bitmap(pbResultImage.Width, pbResultImage.Height);

            // Draw overlay onto result bitmap
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.DrawImage(overlayImage, overlayX, overlayY);
            }

            // Calculate position of base image within result image
            int baseX = (pbResultImage.Width - baseImage.Width) / 2;
            int baseY = overlayY + ((overlayImage.Height - baseImage.Height) / 2);

            // Draw base image onto result bitmap
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                g.DrawImage((Image)baseImage, baseX, baseY);
            }

            // Set result bitmap as picture box image
            pbResultImage.Image = resultBitmap;

        }

        private void PosterOverlay_Load(object sender, EventArgs e)
        {

        }
    }
}