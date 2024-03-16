using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ResizeImagesWhitTreads
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
        private Image newImage1 = null;
        private Image newImage2 = null;
        private void button2_Click(object sender, EventArgs e)
        {
            if (Image != null && comboBox1.SelectedItem != null)
            {
                string selectedValue = comboBox1.SelectedItem.ToString().Replace("%", "");
                float downscalingFactor = float.Parse(selectedValue) / 100f;

                int newWidth = (int)(Image.Width * downscalingFactor);
                int newHeight = (int)(Image.Height * downscalingFactor);

                Thread threadImage1 = new Thread(() =>
                {
                    newImage1 = ResizeImage(Image, newWidth, newHeight);
                });
                threadImage1.Start();
                threadImage1.Join();

                Thread threadImage2 = new Thread(() =>
                {
                    newImage2 = ResizeImage(Image, newWidth, newHeight);
                });
                threadImage2.Start();
                threadImage2.Join();

                this.Invoke((MethodInvoker)delegate
                {
                    Image combinedImage = ComboImages(newImage1, newImage2);
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "JPG(*.JPG)|*.jpg";
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        combinedImage.Save(save.FileName);
                    }
                    MessageBox.Show("Готово е !!");
                });
            }
        }
        private Image ComboImages(Image Image1, Image Image2)
        {
            int width = Math.Max(Image1.Width, Image2.Width);
            int height = Math.Max(Image1.Height, Image2.Height);
            Bitmap result = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(Image1, 0, 0);
                graphics.DrawImage(Image2, 0, Image1.Height);
            }
            return result;
        }
    }
}
