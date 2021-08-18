using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proje
{
    public partial class Form1 : Form
    {

        bool goLeft;
        bool goRight;
        bool isGameOver;
        int score;
        int ballx;
        int bally;
        int playerSpeed;
        PictureBox[] bricksArray;

        public Form1()
        {
            InitializeComponent();
            PlaceBlocks();
            SetupGame();
        }
        // Tuğla oluşturma
        public void PlaceBlocks()
        {
            bricksArray = new PictureBox[24];

            int a = 0;
            int top = 50;
            int left = 100;
            // her bir tuğlanın özellikleri(boy, en, tagname) ve sıralanması durumu
            for (int i = 0; i < bricksArray.Length; i++)
            {   
                bricksArray[i] = new PictureBox();
                bricksArray[i].Height = 30;
                bricksArray[i].Width = 100;
                bricksArray[i].Tag = "bricks";
                bricksArray[i].BackColor = Color.Brown;
                // ekran eni: 1000, her bir tuğlanın uzunluğu: 100, ilk boşluk: 100 aradaki boşluk sayısı: 5
                // tuğlalar 600+ ilk boşluk 100+ ara boşluklar 30*5:150+ son boşluk 150
                // satır atla 
                if (a == 6)
                {
                    // tuğla eni 30 arada kalacak boşluk 20
                    top = top + 50; // üstten satır atla 
                    left = 100;
                    a = 0;
                }
                // satıra diz
                if (a < 6)
                {
                    a++;
                    bricksArray[i].Left = left;
                    bricksArray[i].Top = top;
                    this.Controls.Add(bricksArray[i]); // tuğla oluştur
                    left = left + 130;  // tuğla boyu ve aradaki boşluk değeri

                }

            }

        }

        public void SetupGame()
        {
            isGameOver = false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 15;
            scorelabel.Text = "Score: " + score;
            //top başlangıç 
            ball.Left = 300;
            ball.Top = 300;
            // raket başlangıç yeri
            player.Left = 500;
            timer.Start();


        }
       

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            // oyuncunun ekran boyutunu aşmayacak şekilde sağa ve sola gitme hareketi 
            scorelabel.Text = "Score: " + score;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if (goRight == true && player.Left < 900)
            {
                player.Left += playerSpeed;
            }
            // top başlangıç hareketi
            ball.Left += ballx;
            ball.Top += bally;
            // topun ekran boyutunu aşmayacak şekilde sağa ve sola gitme hareketi 
            if (ball.Left < 0 || ball.Left > 950)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }
            // topun oyuncu ile çarpışma durumu
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {
                // topun oyuncuya çarpma durumundaki yön ve hızı
                bally = 8 * -1;

                if (ballx < 0)
                {
                    ballx = 8 * -1;
                }
                else
                {
                    ballx = 8;
                }
            }
            // topun tuğlaya çarpma durumu
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "bricks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds)) 
                    {
                        score += 1;

                        bally = -bally; 
                     //   ballx = -ballx;

                        this.Controls.Remove(x);
                    }
                }

            }


            if (score == 24)
            {
                PlaceBlocks();
                LevelLabel.Text = "Bölüm 2";
            }

            if (score == 48)
            {
                gameOver("Kazandınız!! Tekrar başlamak için Enter tuşuna basın.");
            }

            if (ball.Top > 580)
            {
                gameOver("Kaybettiniz... Tekrar başlamak için Enter tuşuna basın.");
            }



        }

        // baştan başlatma durumunda her tuğlanın silinmesi gerek
        private void RemoveBricks()
        {
            foreach (PictureBox x in bricksArray)
            {
                this.Controls.Remove(x);
            }
        }

        private void KeyDownE(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

        }


        private void KeyUpE(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RemoveBricks();
                PlaceBlocks();
                SetupGame();
                LevelLabel.Text = "Bölüm 1";
            }

        }

        public void gameOver(string message)
        {
            isGameOver = true;
            timer.Stop();
            scorelabel.Text = "Score: " + score + " " + message;
        }
    }
}
