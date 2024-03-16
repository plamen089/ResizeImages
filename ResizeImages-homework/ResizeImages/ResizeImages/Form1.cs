using System;
using System.Drawing;
using System.Windows.Forms;

namespace ResizeImages
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Image Image;
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap newImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return newImage;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Image != null && comboBox1.SelectedItem != null)
            {
                string selectedValue = comboBox1.SelectedItem.ToString().Replace("%", "");
                float downscalingFactor = float.Parse(selectedValue) / 100f;

                int newWidth = (int)(Image.Width * downscalingFactor);
                int newHeight = (int)(Image.Height * downscalingFactor);

                Image newImage = ResizeImage(Image, newWidth, newHeight);

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "JPG(*.JPG)|*.jpg";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    newImage.Save(save.FileName);
                }
                MessageBox.Show("Готово е !!");
            }
        }
    }
}
