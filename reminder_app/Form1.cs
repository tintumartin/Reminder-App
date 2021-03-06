﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using System.Data.OleDb;
namespace reminder_app
{
    public partial class Form1 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
      
        public Form1()
        {
            InitializeComponent();
            
        }

     

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure about exiting this application. You won't get further reminders", "Exit Application", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("You cant find the app in system tray");
                notifyIcon1.Visible = true;
                Hide();
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con= new OleDbConnection();
            con.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=../../App Data/reminder.mdb";
            con.Open();
            bind();
            InitializeTimer();
            
        }

       
        private void bind()
        {
           cmd = new OleDbCommand("select * from reminder order by id desc", con);
            try
            {
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occured");
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label1.Text= dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            
        }

     

        private void InitializeTimer()
        {
            
            timer1.Interval = 1000*60*60;
            timer1.Enabled = true;
           
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cmd = new OleDbCommand("select * from reminder where date='" + DateTime.Now.ToShortDateString() + "'", con);
            try
            {
                OleDbDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    PopupNotifier pop = new PopupNotifier();
                    pop.ContentText = dr[2].ToString();
                    pop.Popup();


                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) || dateTimePicker1.Value.Date > DateTime.Now)
            {

            }
            else
            {
                MessageBox.Show("Be futuristic");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter reminder message");
            }
            else
            {

                cmd = new OleDbCommand("select max(id) from reminder", con);
                try
                {
                    int id = 0;
                    OleDbDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr[0].ToString() == "")
                        {
                            id = 0;

                        }
                        else
                        {
                            id = Convert.ToInt32(dr[0].ToString());
                        }
                    }

                    if (id > 0)
                    {
                        id += 1;
                        cmd = new OleDbCommand("insert into [reminder] values(" + id + ",'" + dateTimePicker1.Value.Date.ToShortDateString() + "','" + textBox1.Text + "')", con);
                        cmd.ExecuteNonQuery();
                        bind();
                    }
                    else
                    {
                        cmd = new OleDbCommand("insert into [reminder] values(1,'" + dateTimePicker1.Value.Date.ToShortDateString() + "','" + textBox1.Text + "')", con);
                        cmd.ExecuteNonQuery();
                        bind();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured" + ex);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text != "" && textBox1.Text != "")
            {
                cmd = new OleDbCommand("update reminder set msg='" + textBox1.Text + "' where id=" + Convert.ToInt32(label1.Text) + "", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    bind();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Some error occured");
                }
            }
            else
            {
                MessageBox.Show("Please select a record to update");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label1.Text != "")
            {
                cmd = new OleDbCommand("delete from reminder where id=" + Convert.ToInt32(label1.Text) + "", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    bind();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Some error occured");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure about exiting this application. You won't get further reminders", "Exit Application", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                con.Close();
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("You cant find the app in system tray");
                notifyIcon1.Visible = true;
                Hide();
            }
        }

    

       

    }
}
