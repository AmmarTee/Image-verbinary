using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace picture_save_and_retrive
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string connetion = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security = True";
        SqlConnection sql = new SqlConnection(connetion);
        string imgloc = "";
        SqlCommand cmd;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png files(*.png|)*.png|jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";
            if(dialog.ShowDialog()==DialogResult.OK){
                imgloc = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imgloc;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] images = null;
            FileStream stream = new FileStream(imgloc, FileMode.Open, FileAccess.Read);
            BinaryReader brs = new BinaryReader(stream);
            images = brs.ReadBytes((int)stream.Length);


            sql.Open();
            string sqlQuery = "insert into BioData(name,Image)values('"+textBox1.Text+"',@images) ";
            cmd = new SqlCommand(sqlQuery,sql);
            cmd.Parameters.Add(new SqlParameter("@images",images));
            int N = cmd.ExecuteNonQuery();
            sql.Close();
            MessageBox.Show(N.ToString() +"Picture is saved...!");

        
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            sql.Open();
            string sqlQuery = "select  name,Image from BioData where name='"+ textBox1.Text + "'";
      
            cmd = new SqlCommand(sqlQuery,sql);
            SqlDataReader DataRead = cmd.ExecuteReader();
            DataRead.Read();

            if(DataRead.HasRows)
            {
                textBox1.Text = DataRead[0].ToString();
                byte[] images = ((byte[])DataRead[1]);
            
            if(images == null)
            {
                pictureBox1.Image = null;
            }
            else 
            {
                MemoryStream mstream = new MemoryStream(images);
                pictureBox1.Image = Image.FromStream(mstream);
            }
            }
            else 
            {
                MessageBox.Show(" This image is not Available..!");
            }
            sql.Close();
        }
    }
}
