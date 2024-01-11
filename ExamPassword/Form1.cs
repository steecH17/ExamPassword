using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamPassword
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        
        List<PointF> coords = new List<PointF>();
        List<int> passwordKey = new List<int>() { 0, 1, 2, 5, 4, 3};
        List<PointF> userKey = new List<PointF>();
        bool keyChangeMode = false;
        public Form1()
        {
            InitializeComponent();
            panelMain.Invalidate();
            bmp = new Bitmap(panelMain.ClientSize.Width, panelMain.ClientSize.Height);
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, panelMain, new object[] { true });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keyChangeMode = true;
            userKey.Clear();
        }

        private void DrawPasswordCircle()
        {
            coords.Clear();
            int stepX = panelMain.ClientSize.Width / 4;
            int stepY = panelMain.ClientSize.Height / 4;
            Graphics graphics = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.Aqua);
            for(int i = 1; i< 4;i++)
            {
                graphics.FillEllipse(brush, 1 * stepX, i * stepY, 30, 30);
                coords.Add(new PointF(1 * stepX, i * stepY));
                graphics.FillEllipse(brush, 2 * stepX, i * stepY, 30, 30);
                coords.Add(new PointF(2 * stepX, i * stepY));
                graphics.FillEllipse(brush, 3 * stepX, i * stepY, 30, 30);
                coords.Add(new PointF(3 * stepX, i * stepY));
            }

        }

        private bool ClickOnCircle(float coordX, float coordY)
        {
            float maxX;
            float maxY;
            foreach(PointF point in coords) 
            {
                maxX = point.X+30;
                maxY = point.Y+30;
                if (point.X <= coordX && coordX <= maxX && point.Y <= coordY && coordY <= maxY)
                {
                    userKey.Add(point);
                    return true;
                }
            }
            return false;
        }

        private bool KeyIsCorrect()
        {
            if (userKey.Count == 6)
            {
                int ind = 0;
                foreach(PointF point in userKey) 
                {
                    if(point != coords[passwordKey[ind]]) return false;
                    ind++;
                }
                return true;
            }
            else return false;
        }
        private void ChangeKey()
        {
            passwordKey.Clear();
            foreach(PointF point in userKey)
            {
                for(int i = 0; i < coords.Count; i++) 
                {
                    if(point == coords[i])
                    {
                        passwordKey.Add(i);
                        break;
                    }
                }
            }
        }

        private void DrawKeyLine()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            //graphics.DrawLines(Pens.Aqua, userKey.ToArray());
            Pen pen = new Pen(Color.Aqua, 8);
            for(int i = 0; i < userKey.Count-1;i++)
            {
                graphics.DrawLine(pen, userKey[i].X + 15, userKey[i].Y + 15, userKey[i+1].X + 15, userKey[i+1].Y + 15);
            }
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            bmp = new Bitmap(panelMain.ClientSize.Width, panelMain.ClientSize.Height);
            DrawPasswordCircle();
            if(userKey.Count > 1) DrawKeyLine();
            e.Graphics.DrawImageUnscaled(bmp, Point.Empty);
        }

        

        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if(!keyChangeMode && ClickOnCircle(e.X, e.Y)) 
            {
                panelMain.Invalidate();
                if (userKey.Count == 6)
                {
                    if(KeyIsCorrect())
                    {
                        MessageBox.Show("Ключ верный");
                        userKey.Clear();
                        panelMain.Invalidate();
                    }
                    else
                    {
                        MessageBox.Show("Ключ не верный");
                        userKey.Clear();
                        panelMain.Invalidate();
                    }
                    
                }

            }
            else if(keyChangeMode && ClickOnCircle(e.X, e.Y))
            {
                panelMain.Invalidate();
                if (userKey.Count == 6)
                {
                    ChangeKey();
                    MessageBox.Show("Ключ заменен");
                    keyChangeMode = false;
                    userKey.Clear();
                    panelMain.Invalidate();
                }
            }
        }

        private void panelMain_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
