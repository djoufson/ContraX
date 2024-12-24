using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ContraX
{
    public partial class Fenetre : Form
    {
        #region Init Variables

        public int TitleSoundIndex = 0;
        public int counter = 0;
        public bool isStarted;
        public bool isKeyPressed;
        public bool isKeyReleased;
        public bool isLeftKey;
        public bool isRightKey;
        public bool isUpKey;
        public bool isDownKey;
        public bool isSettingsPage;
        public bool isMenuPage;
        public bool isScorePage;
        public bool isGamePage;
        public bool isSystemBox;
        public bool isGameBox;
        public bool isAccountPanel;
        public bool isSoundsPanel;
        public bool isSFXAllowed;
        public bool isMusicAllowed;
        public bool isChangingKey;
        public bool isChangingRightKey;
        public bool isChangingLeftKey;
        public bool isChangingUpKey;
        public bool isChangingDownKey;
        public bool isChangingShootKey;
        public bool isChangingShootUpKey;
        public bool isChangingShootDownKey;
        public bool isBackgroundMusicEnabled;
        public bool isSFXMusicEnabled;
        public bool isRestarted;
        public bool reachedTheRightBorder;
        public bool reachedTheLeftBorder;
        public bool isFalling;
        public bool shootedOnce;
        public bool isJumping;
        public bool isGameOver;
        public bool isNotClosed;
        public bool isShowingWarning;
        public bool isTakingAnyImput;
        public bool[] stages;

        public Keys LeftKey;
        public Keys RightKey;
        public Keys UpKey;
        public Keys DownKey;
        public Keys ShootKey;
        public Keys ShootDownKey;
        public Keys ShootUpKey;
        public Keys PauseKey;

        public Account currentAccount;
        public List<Account> Accounts;

        GameMap map;
        public List<PictureBox> backgroundList = new List<PictureBox>();
        public List<Ennemy> ennemies;

        public Thread PlayerMotionThread { get; set; }
        public delegate void DelegatePlayerMotion(bool isLeftKey, bool isRightKey, bool isUpKey, bool isDownKey);
        public DelegatePlayerMotion delegatePlayerMotion;

        public Thread PlayerShootThread { get; set; }
        public delegate void DelegatePlayerShootThread();
        public DelegatePlayerShootThread delegatePlayerShootThread;

        public Thread JumpThread { get; set; }
        public delegate void DelegateJumpMotion();
        public DelegateJumpMotion delegateJump;
        public Thread BulletsThread { get; set; }
        public delegate void DelegateBulletsMotion();
        public DelegateBulletsMotion delegateBulletsMotion;
        public Thread SoundAllowingThread { get; set; }

        public Player player;
        public Ennemy ennemy;

        public Ground ground;
        public Ground otherGround;
        #endregion

        #region Form Constructor
        public Fenetre()
        {
            InitializeComponent();

            isFalling = true;
            isMenuPage = true;
            isNotClosed = true;
            isBackgroundMusicEnabled = true;
            isSFXAllowed = true;
            isGameOver = false;
            isShowingWarning = false;
            isTakingAnyImput = false;

            LeftKey = Keys.Left;
            RightKey = Keys.Right;
            UpKey = Keys.Up;
            DownKey = Keys.Down;
            ShootDownKey = Keys.S;
            ShootUpKey = Keys.W;
            ShootKey = Keys.Space;
            PauseKey = Keys.Escape;

            currentAccount = new Account();
            Accounts = new List<Account>();
            ennemies = new List<Ennemy>();
            stages = new bool[5];

            this.TitleSound.URL = "Sounds/01 Title.mp3";
            this.HoverSound.URL = "Sounds/button_hover.wav";
            this.HoverSound.Ctlcontrols.play();
            //this.StageClearSound.URL = "Sounds/04 Stage Clear.mp3";
            this.TitleSound.Ctlcontrols.play();

            this.GamePagesControl.Controls.Remove(GamePage);
            this.GamePagesControl.Controls.Remove(SettingsPage);
            this.GamePagesControl.Controls.Remove(ScoresPage);
            this.GamePagesControl.Controls.Remove(HelpPage);
            this.MoveRightCommandLabel.Text = Keys.Right.ToString();

            PlayerMotionThread = new Thread(delegatedMotion);
            delegatePlayerMotion = new DelegatePlayerMotion(MotionThread);

            JumpThread = new Thread(jumpThread);
            delegateJump = new DelegateJumpMotion(jumpdelegate);

            delegatePlayerShootThread = new DelegatePlayerShootThread(shootThread);
            PlayerShootThread = new Thread(delagateShoot);

            delegateBulletsMotion = new DelegateBulletsMotion(bulletsThread);
            BulletsThread = new Thread(delegateBullets);
        }
        #endregion


        /// <summary>
        /// Here we start writing those methods that will help us configure our game
        /// and make it work right
        /// </summary>



        #region Threads
        public void delegateBullets()
        {
            while (isNotClosed)
            {
                try
                {
                    map.Invoke(delegateBulletsMotion);
                    Thread.Sleep(60);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an exception: " + ex);
                }
            }
        }
        public void bulletsThread()
        {
            if (isStarted)
            {
                for (int m = 0; m < ennemies.Count; m++)
                {
                    if (ennemies[m].shootedOnce)
                    {
                        if (ennemies[m].bulletList.Count >= 1)
                        {
                            for (int i = 0; i < ennemies[m].bulletList.Count; i++)
                            {
                                if ((string)ennemies[m].bulletList[i].Tag == "right")
                                {

                                    if (ennemies[m].bulletList[i].up)
                                    {
                                        var x = ennemies[m].bulletList[i].Left + 30;
                                        var y = ennemies[m].bulletList[i].Top - 30;

                                        ennemies[m].bulletList[i].Location = new Point(x, y);
                                    }
                                    else if (ennemies[m].bulletList[i].down)
                                    {
                                        var x = ennemies[m].bulletList[i].Left + 30;
                                        var y = ennemies[m].bulletList[i].Top + 30;

                                        ennemies[m].bulletList[i].Location = new Point(x, y);
                                    }
                                    else
                                    {
                                        ennemies[m].bulletList[i].Left += 30;
                                    }
                                }
                                else
                                {

                                    if (ennemies[m].isShootingDown)
                                    {
                                        ennemies[m].bulletList[i].Top += 30;
                                        ennemies[m].bulletList[i].Left -= 30;
                                    }
                                    else if (ennemies[m].isShootingUp)
                                    {
                                        ennemies[m].bulletList[i].Top -= 30;
                                        ennemies[m].bulletList[i].Left -= 30;
                                    }
                                    else
                                    {
                                        ennemies[m].bulletList[i].Left -= 30;
                                    }
                                }
                                if (ennemies[m].bulletList[i].Left < 0 ||
                                    ennemies[m].bulletList[i].Left > Width ||
                                    ennemies[m].bulletList[i].Top < 0 ||
                                    ennemies[m].bulletList[i].Top > Height ||
                                    ennemies[m].bulletList[i].Bounds.IntersectsWith(player.Bounds))
                                {
                                    map.Controls.Remove(ennemies[m].bulletList[i]);
                                }
                                if (ennemies[m].bulletList[i].Bounds.IntersectsWith(player.Bounds))
                                {
                                    if(LifeBar.Value >= 2)
                                    {
                                        LifeBar.Value -= 2;
                                        if(LifeBar.Value == 0)
                                        {
                                            GameOver();
                                        }
                                    }
                                    ennemies[m].bulletList.RemoveAt(i);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < player.bulletList.Count; i++)
                    {
                        if(ennemies.Count >= 1)
                        {
                            if (player.bulletList[i].Bounds.IntersectsWith(ennemies[m].Bounds))
                            {
                                map.Controls.Remove(ennemies[m]);
                                ennemies[m].isOnTheMap = false;
                                ennemies.RemoveAt(m);
                                map.Controls.Remove(player.bulletList[i]);
                                player.bulletList.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        public void delagateShoot()
        {
            while(isNotClosed)
            {
                try
                {
                    map.Invoke(delegatePlayerShootThread);
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an exception: " + ex);
                }
            }
        }
        public void shootThread()
        {
            if(isStarted)
            {
                foreach(var opponent in ennemies)
                {
                    opponent.Shoot(player);
                }
                if (player.isShooting)
                {
                    if(player.isShootingUp)
                    {
                        player.Shoot();
                        player.bulletList[player.bulletList.Count - 1].up = true;
                        player.bulletList[player.bulletList.Count - 1].down = false;
                    }
                    else if(player.isShootingDown)
                    {
                        player.Shoot();
                        player.bulletList[player.bulletList.Count - 1].up = false;
                        player.bulletList[player.bulletList.Count - 1].down = true;
                    }
                    else
                    {
                        player.Shoot();
                        player.bulletList[player.bulletList.Count - 1].up = false;
                        player.bulletList[player.bulletList.Count - 1].down = false;
                    }
                }
            }
        }
        public void delegatedMotion()
        {
            while(isNotClosed)
            {
                if(isStarted)
                {
                    try
                    {
                        map.Invoke(delegatePlayerMotion, new object[] { isLeftKey, isRightKey, isUpKey, isDownKey });
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur survenue: " + ex);
                    }
                }
            }
            PlayerMotionThread.Abort();
        }
        public void MotionThread(bool isLeftKey, bool isRightKey, bool isUpKey, bool isDownKey)
        {
            player._Move(isLeftKey, isRightKey, isUpKey, isDownKey, isJumping);
            foreach(var opponent in ennemies)
            {
                opponent._Move(player, 0, 1300);
            }
        }
        public void jumpdelegate()
        {
            if (isUpKey)
            {
                player.Top -= 112;
                Thread.Sleep(40);
            }
        }
        public void jumpThread()
        {
            while (isNotClosed)
            {
                if (isStarted)
                {
                    try
                    {
                        map.Invoke(delegateJump);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception catched: " + ex);
                    }
                }
                Thread.Sleep(10);
            }
            JumpThread.Abort();
        }
        #endregion

        #region Motion Methods
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (isStarted)
            {
                reachedTheLeftBorder = false;
                reachedTheRightBorder = false;
                player.BringToFront();
                isKeyPressed = true;
                isKeyReleased = false;

                player.BringToFront();
                if (e.KeyCode == ShootKey)
                {
                    player.isShooting = true;
                    shootedOnce = true;
                }

                if (player.isShooting && e.KeyCode == ShootUpKey)
                {
                    player.isShootingUp = true;
                    player.isShootingDown = false;
                }
                if (player.isShooting && 
                    !player.isShootingUp && e.KeyCode == ShootDownKey)
                {
                    player.isShootingDown = true;
                    player.isShootingUp = false;
                }

                if (!isFalling)
                {
                    if (player.isMoving && e.KeyCode == UpKey)
                    {
                        player.isMoving = true;
                        isJumping = true;
                        isUpKey = true;
                        isDownKey = false;
                    }
                    if (e.KeyCode == UpKey)
                    {
                        isFalling = true;
                        player.isMoving = true;
                        isJumping = true;
                        isUpKey = true;
                        isDownKey = false;
                    }
                    if (e.KeyCode == DownKey)
                    {
                        isFalling = true;
                        player.isMoving = true;
                        isJumping = true;
                        isUpKey = false;
                        isDownKey = true;

                        player.Top += 51;
                    }
                    if (e.KeyCode == RightKey)
                    {
                        isJumping = false;
                        player.isMoving = true;
                        if (player.Left < 1000)
                        {
                            MotionLeft_Timer.Stop();
                            Motion_Timer.Start();
                            player.isMoving = true;
                            player.isLookingRight = true;
                            isLeftKey = false;
                            isRightKey = true;
                        }
                        else
                        {
                            player.Left = 1000;
                            player.isMoving = true;
                        }
                    }
                    if (e.KeyCode == LeftKey)
                    {
                        isJumping = false;
                        player.isMoving = true;
                        if (player.Left > 0)
                        {
                            Motion_Timer.Stop();
                            MotionLeft_Timer.Start();
                            player.isLookingRight = false;
                            player.isMoving = true;
                            isRightKey = false;
                            isLeftKey = true;
                            player.isMoving = true;
                        }
                        else
                        {
                            player.Left = 0;
                            player.isMoving = true;
                        }
                    }
                }
                if (e.KeyCode == PauseKey)
                {
                    player.isMoving = false;
                    Pause();
                }
            }
            if(isShowingWarning)
            {
                isTakingAnyImput = true;
            }
            if(isTakingAnyImput)
            {
                warningPanel.Visible = false;
                isTakingAnyImput = false;
                EnableChangingKey();
            }
            if (isChangingKey)
            {
                if( e.KeyCode == ShootKey ||
                    e.KeyCode == RightKey ||
                    e.KeyCode == LeftKey ||
                    e.KeyCode == UpKey ||
                    e.KeyCode == DownKey ||
                    e.KeyCode == ShootDownKey ||
                    e.KeyCode == ShootUpKey ||
                    e.KeyCode == PauseKey)
                {
                    warningPanel.Visible = true;
                    ChooseNewKewPanel.Visible = false;
                    isShowingWarning = true;
                    isChangingKey = false;
                }
                else
                {
                    if (isChangingShootKey) { ShootKey = e.KeyCode; }
                    if (isChangingRightKey) { RightKey = e.KeyCode; }
                    if (isChangingLeftKey) { LeftKey = e.KeyCode; }
                    if (isChangingDownKey) { DownKey = e.KeyCode; }
                    if (isChangingUpKey) { UpKey = e.KeyCode; }
                    if (isChangingShootDownKey) { ShootDownKey = e.KeyCode; }
                    if (isChangingShootUpKey) { ShootUpKey = e.KeyCode; }

                    isChangingKey = false;
                    ChooseNewKewPanel.Visible = false;
                    EnableChangingKey();
                }
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (isStarted)
            {
                player.BringToFront();
                isKeyPressed = false;
                isKeyReleased = true;

                if (e.KeyCode == ShootKey || 
                    e.KeyCode == ShootDownKey ||
                    e.KeyCode == ShootUpKey)
                {
                    player.isShootingUp = false;
                    player.isShootingDown = false;
                }
                if (e.KeyCode == ShootKey)
                {
                    player.isShooting = false;
                }

                if (e.KeyCode == LeftKey ||
                    e.KeyCode == DownKey ||
                    e.KeyCode == RightKey ||
                    e.KeyCode == UpKey
                    )
                {
                    player.isMoving = false;
                }
                if (e.KeyCode == UpKey)
                {
                    isUpKey = false;
                }
                if (e.KeyCode == DownKey)
                {
                    isDownKey = false;
                }
                player.BringToFront();
            }
        }
        #endregion

        #region Controls Methods
        #region Mouse
        private void MouseOnOption (object sender, EventArgs e)
        {
            if (((Label)sender).Tag == SoundsLabel.Tag) { isSystemBox = true; isGameBox = false; isAccountPanel = false; }
            if (((Label)sender).Tag == AccountLabel.Tag) { isSystemBox = false; isGameBox = true; isAccountPanel = false; }
            if (((Label)sender).Tag == AccountPanel.Tag) { isSystemBox = false; isAccountPanel = true; isSystemBox = false; }
        }
        private void _MouseEnter(object sender, EventArgs e)
        {
            if (((Label)sender).Tag == ((Label)MoveRightCommandLabel).Tag ||
                ((Label)sender).Text == "Add" ||
                ((Label)sender).Text == "Delete" ||
                ((Label)sender).Text == "Choose")
            {
                ((Label)sender).BackColor = Color.DarkGray;
            }
            else
            {
                MouseOnOption(sender, e);
                if (isMenuPage) { this.MenuPage.Controls.Add(this.CursorPicture); }
                if (isScorePage) { this.ScoresPage.Controls.Add(this.CursorPicture); }

                if (isSettingsPage && isSystemBox)
                {
                    this.AccountPanel.Controls.Remove(this.CursorPicture);
                    this.SystemBox.Controls.Add(this.CursorPicture);
                }
                if (isSettingsPage && isGameBox)
                {
                    this.AccountPanel.Controls.Remove(this.CursorPicture);
                    this.GameBox.Controls.Add(this.CursorPicture);
                }

                if (isAccountPanel) { this.AccountPanel.Controls.Add(this.CursorPicture); }

                this.CursorPicture.Location = ((Label)sender).Location;
                this.CursorPicture.Left -= 40;
                this.CursorPicture.Top += 4;
                this.CursorPicture.BringToFront();

                ((Label)sender).ForeColor = Color.Yellow;
            }
        }

        private void _MouseLeave(object sender, EventArgs e)
        {
            this.MenuPage.Controls.Remove(this.CursorPicture);

            if (((Label)sender).Tag == ((Label)MoveRightCommandLabel).Tag ||
                ((Label)sender).Text == "Add" ||
                ((Label)sender).Text == "Delete" ||
                ((Label)sender).Text == "Choose")
            {
                ((Label)sender).BackColor = Color.DimGray;
            }
            else
            {
                ((Label)sender).ForeColor = Color.White;
            }
        }
        private void _MouseHover(object sender, EventArgs e)
        {
            if (isSFXAllowed)
            {
                HoverSound.Ctlcontrols.play();
            }
        }
        #endregion
        private void OptionSelection(object sender, EventArgs e)
        {
            if(((Label)sender).Tag == PlayButton.Tag)
            {
                map = new GameMap();
                map.Controls.Remove(player);
                player = new Player(counter, map);
                ennemy = new Ennemy(30, 143, map);
                ennemies.Add(ennemy);
                ennemy.Left += 900;
                player.Name = " " + LastNameBox.Text;

                if (player.Name == "" || player.Name == " ")
                {
                    player.Name = "Player";
                }
                map.Controls.Add(player);
                player.Left = 30;
                Stage1.Controls.Add(map);
                this.StagesPagesControl.Controls.Remove(Stage2);
                isStarted = true;
                this.GamePagesControl.Controls.Remove(ScoresPage);
                this.GamePagesControl.Controls.Remove(SettingsPage);
                this.GamePagesControl.Controls.Remove(MenuPage);
                this.GamePagesControl.Controls.Remove(HelpPage);
                this.GamePagesControl.Controls.Add(GamePage);

                isSettingsPage = false;
                isMenuPage = false;
                isScorePage = false;
                isGamePage = true;

                TitleSound.Ctlcontrols.stop();
                MenuSound.Ctlcontrols.stop();

                if(isBackgroundMusicEnabled)
                {
                    PlayingSound.URL = "C:/Users/Laprovidence/Desktop/ContraX/Resources/03 Stage 1 - Jungle % Stage 7 - Hangar.mp3";
                    PlayingSound.settings.setMode("loop", true);
                    PlayingSound.Ctlcontrols.play();
                }
                ground = new Ground(map);
                otherGround = new Ground(map);

                buildStage(otherGround);
                //Map.SendToBack();
                ground.BringToFront();
                player.BringToFront();
                isStarted = true;
                if(!isRestarted)
                {
                    PlayerMotionThread.Start();
                    PlayerShootThread.Start();
                    JumpThread.Start();
                    BulletsThread.Start();
                }
            }
            if (((Label)sender).Tag == ExitButton.Tag)
            {
                this.Cursor = global::System.Windows.Forms.Cursors.WaitCursor;
                isNotClosed = false;
                Application.Exit();
            }
            if (((Label)sender).Tag == SettingsButton.Tag)
            {
                isSettingsPage = true;
                isMenuPage = false;
                isScorePage = false;
                isGamePage = false;

                this.GamePagesControl.Controls.Remove(MenuPage);
                this.GamePagesControl.Controls.Remove(GamePage);
                this.GamePagesControl.Controls.Remove(ScoresPage);
                this.GamePagesControl.Controls.Remove(HelpPage);
                this.Controls.Remove(player);
                this.GamePagesControl.Controls.Add(SettingsPage);
                isSettingsPage = true;
            }
            if (((Label)sender).Tag == ScoreButton.Tag)
            {
                isSettingsPage = false;
                isMenuPage = false;
                isScorePage = true;
                isGamePage = false;

                this.GamePagesControl.Controls.Remove(MenuPage);
                this.GamePagesControl.Controls.Remove(GamePage);
                this.GamePagesControl.Controls.Remove(SettingsPage);
                this.GamePagesControl.Controls.Remove(HelpPage);
                this.Controls.Remove(player);
                this.GamePagesControl.Controls.Add(ScoresPage);
            }
            if (((Label)sender).Tag == _HelpButton.Tag)
            {
                isSettingsPage = false;
                isMenuPage = false;
                isScorePage = true;
                isGamePage = false;

                this.GamePagesControl.Controls.Remove(MenuPage);
                this.GamePagesControl.Controls.Remove(GamePage);
                this.GamePagesControl.Controls.Remove(SettingsPage);
                this.GamePagesControl.Controls.Remove(ScoresPage);
                this.Controls.Remove(player);
                this.GamePagesControl.Controls.Add(HelpPage);
            }
        }
        #endregion

        #region Game Methods
        private void Pause()
        {
            
        }
        private void GameOver()
        {
            GameOverSound.URL = "Sounds/13 Game Over.mp3";
            GamePagesControl.Controls.Remove(SettingsPage);
            GamePagesControl.Controls.Remove(ScoresPage);
            GamePagesControl.Controls.Remove(MenuPage);
            PlayingSound.Ctlcontrols.stop();
            Label gameOverTitle = new Label();
            gameOverTitle.AutoSize = true;
            gameOverTitle.Font = new System.Drawing.Font("Courant", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gameOverTitle.ForeColor = System.Drawing.Color.Azure;
            gameOverTitle.Location = new System.Drawing.Point(408, 81);
            gameOverTitle.Size = new System.Drawing.Size(598, 66);
            gameOverTitle.Text = "GAME OVER !!";

            Stage1.Controls.Add(gameOverTitle);
            gameOverTitle.BringToFront();

            isStarted = false;
            isGamePage = true;
            isMenuPage = false;
            isRestarted = false;

            timer1.Stop();
            GravityTimer.Stop();
            MotionLeft_Timer.Stop();
            Motion_Timer.Stop();
            SoundTimer.Stop();
            TitleTimer.Stop();

            isGameOver = true;
            GameOverTimer.Start();
            MenuPage.Controls.Add(CursorPicture);
            if(isBackgroundMusicEnabled)
            {
                GameOverSound.Ctlcontrols.play();
            }
        }
        public void Button_Click(object sender, EventArgs e)
        {
            GamePagesControl.Controls.Remove(MenuPage);
            GamePagesControl.Controls.Remove(SettingsPage);
            GamePagesControl.Controls.Remove(ScoresPage);
            GamePagesControl.Controls.Remove(GamePage);
            GamePagesControl.Controls.Remove(HelpPage);
            GamePagesControl.Controls.Add(MenuPage);
            MenuPage.Controls.Add(CursorPicture);

            isMenuPage = true;
            isGamePage = false;
            isScorePage = false;
            isSettingsPage = false;
        }
        public void buildStage(Ground ground)
        {
            #region Build Ground
            ground.PlatformeList.Add(new Platform(29 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(30 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(31 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(35 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(36 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(37 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(50 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(51 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(52 * 118, 233 - 90, map));
            ground.PlatformeList.Add(new Platform(53 * 118, 233 - 90, map));

            ground.PlatformeList.Add(new Platform(23 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(24 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(25 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(26 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(33 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(34 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(35 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(60 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(61 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(62 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(75 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(76 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(77 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(120 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(121 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(122 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(130 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(131 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(160 * 118, 233 + 180, map));
            ground.PlatformeList.Add(new Platform(161 * 118, 233 + 180, map));

            ground.PlatformeList.Add(new Platform(16 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(17 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(18 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(19 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(20 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(30 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(31 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(32 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(33 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(45 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(46 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(47 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(48 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(59 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(60 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(61 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(62 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(63 * 118, 233 + 320, map));

            ground.PlatformeList.Add(new Platform(86 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(87 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(88 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(119 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(120 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(130 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(131 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(132 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(133 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(145 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(146 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(147 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(148 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(189 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(190 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(191 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(192 * 118, 233 + 320, map));
            ground.PlatformeList.Add(new Platform(193 * 118, 233 + 320, map));
            #endregion

            #region Place ennemies

            ennemies.Add(new Ennemy(1230, 143, map));
            ennemies.Add(new Ennemy(1550, 143, map));
            ennemies.Add(new Ennemy(1830, 143, map));
            ennemies.Add(new Ennemy(1930, 143, map));
            ennemies.Add(new Ennemy(2430, 143, map));
            ennemies.Add(new Ennemy(2530, 143, map));
            ennemies.Add(new Ennemy(2580, 143, map));
            #endregion
        }
        #endregion

        #region Settings Methods

        private void Account_Click(object sender, EventArgs e)
        {
            this.AccountPanel.Visible = true;
            this.SoundsPanel.Visible = false;
            this.CommandsPanel.Visible = false;

            DisableControls();
        }

        private void Validate_Click(object sender, EventArgs e)
        {
            bool AlreadyExists = false;
            if (FirstNameBox.Text != "" && LastNameBox.Text != "" &&
                FirstNameBox.Text != " " && LastNameBox.Text != " ")
            {
                Account account = new Account();
                account.FirstName = FirstNameBox.Text;
                account.LastName = LastNameBox.Text;
                PlayerName.Text = FirstNameBox.Text;

                if (FirstNameBox.Text == "" ||
                        accountName.Name == " ")
                {
                    PlayerName.Text = LastNameBox.Text;
                    if(LastNameBox.Text == "" ||
                        accountName.Name == " ")
                    {
                        PlayerName.Text = "Player";
                    }
                }
                foreach (var login in AccountList.Items)
                {
                    if ((account.FirstName + " " + account.LastName).Equals((string)login, StringComparison.InvariantCultureIgnoreCase))
                    {
                        AlreadyExists = true;
                    }
                }

                if(!AlreadyExists)
                {
                    AccountList.Items.Add(account.FirstName + " " + account.LastName);
                    Accounts.Add(account);
                    AccountList.Text = account.FirstName + " " + account.LastName;
                }
            }

            FirstNameBox.Text = String.Empty;
            LastNameBox.Text = String.Empty;

            this.SoundsPanel.Visible = false;
            this.CommandsPanel.Visible = false;
        }
        private void SelectAccount(object sender, EventArgs e)
        {
            foreach(var account in Accounts)
            {
                if((account.FirstName + " " + account.LastName).Equals((string)AccountList.SelectedItem, StringComparison.OrdinalIgnoreCase))
                {
                    accountName.Text = account.FirstName;
                    if (accountName.Text == "" ||
                        accountName.Name == " ")
                    {
                        accountName.Text = account.LastName;
                        if(accountName.Text == "" || 
                            accountName.Name == " ")
                        {
                            accountName.Text = "Player";
                        }
                    }
                }
            }
        }
        private void ChooseAccount(object sender, EventArgs e)
        {
            if(accountName.Text != "" &&
                accountName.Text != " ")
            {
                
                foreach (var account in Accounts)
                {
                    if(account.FirstName + " " + account.LastName == AccountList.SelectedText)
                    {
                        currentAccount = account;
                    }
                }
                
                PlayerName.Text = accountName.Text;
                AccountPanel.Visible = false;
                EnableControls();
            }
        }
        private void Delete_Account(object sender, EventArgs e)
        {
            for(int i = 0; i < Accounts.Count; i ++)
            {
                if (Accounts[i].FirstName + " " + Accounts[i].LastName == (string)AccountList.SelectedItem)
                {
                    Accounts.RemoveAt(i);
                    AccountList.Items.RemoveAt(AccountList.SelectedIndex);
                    accountName.Text = String.Empty;

                    AccountList.Text = String.Empty;
                }
            }
        }
        private void BackButton_Click (object sender, EventArgs e)
        {
            this.AccountPanel.Visible = false;
            this.SoundsPanel.Visible = false;
            this.CommandsPanel.Visible = false;

            EnableControls();
        }

        private void EnableChangingKey()
        {
            MoveRightCommandLabel.Enabled = true;
            MoveLeftCommandLabel.Enabled = true;
            MoveDownCommandLabel.Enabled = true;
            MoveUpCommandLabel.Enabled = true;
            ShootCommandLabel.Enabled = true;
            ShootUpCommandLabel.Enabled = true;
            ShootDownCommandLabel.Enabled = true;
        }
        private void DisableChangingKey()
        {
            MoveRightCommandLabel.Enabled = false;
            MoveLeftCommandLabel.Enabled = false;
            MoveDownCommandLabel.Enabled = false;
            MoveUpCommandLabel.Enabled = false;
            ShootCommandLabel.Enabled = false;
            ShootUpCommandLabel.Enabled = false;
            ShootDownCommandLabel.Enabled = false;
        }
        private void DisableControls()
        {
            AccountLabel.Enabled = false;
            SoundsLabel.Enabled = false;
            CommandsLabel.Enabled = false;
            BackButton.Enabled = false;
        }
        private void EnableControls()
        {
            AccountLabel.Enabled = true;
            SoundsLabel.Enabled = true;
            CommandsLabel.Enabled = true;
            BackButton.Enabled = true;
        }
        private void SoundsLabel_Click(object sender, EventArgs e)
        {
            this.SoundsPanel.Visible = true;
            this.AccountPanel.Visible = false;
            this.CommandsPanel.Visible = false;
            isMusicAllowed = CheckBackGroundMusic.Checked;
            isSFXAllowed = CheckSFX.Checked;

            DisableControls();
        }

        private void CommandsLabel_Click(object sender, EventArgs e)
        {
            this.SoundsPanel.Visible = false;
            this.AccountPanel.Visible = false;
            this.CommandsPanel.Visible = true;

            DisableControls();

        }

        private void ChangeCommandKey(object sender, EventArgs e)
        {
            isChangingKey = true;
            ChooseNewKewPanel.Visible = true;
            ChooseNewKewPanel.BringToFront();

            DisableChangingKey();


            if (((Label)sender).Name == MoveRightCommandLabel.Name)
            {
                isChangingRightKey = true;
                isChangingLeftKey = false;
                isChangingShootDownKey = false;
                isChangingShootUpKey = false;
                isChangingUpKey = false;
                isChangingDownKey = false;
                isChangingShootKey = false;
            }
            if (((Label)sender).Name == MoveLeftCommandLabel.Name)
            {
                isChangingRightKey = false;
                isChangingLeftKey = true;
                isChangingShootDownKey = false;
                isChangingShootUpKey = false;
                isChangingUpKey = false;
                isChangingDownKey = false;
                isChangingShootKey = false;
            }
            if (((Label)sender).Name == MoveUpCommandLabel.Name)
            {
                isChangingRightKey = false;
                isChangingLeftKey = false;
                isChangingShootDownKey = false;
                isChangingShootUpKey = false;
                isChangingUpKey = true;
                isChangingDownKey = false;
                isChangingShootKey = false;
            }
            if (((Label)sender).Name == MoveDownCommandLabel.Name)
            {
                isChangingRightKey = false;
                isChangingLeftKey = false;
                isChangingShootDownKey = false;
                isChangingShootUpKey = false;
                isChangingUpKey = false;
                isChangingDownKey = true;
                isChangingShootKey = false;
            }
            if (((Label)sender).Name == ShootUpCommandLabel.Name)
            {
                isChangingRightKey = false;
                isChangingLeftKey = false;
                isChangingShootDownKey = false;
                isChangingShootUpKey = true;
                isChangingUpKey = false;
                isChangingDownKey = false;
                isChangingShootKey = false;
            }
            if (((Label)sender).Name == ShootDownCommandLabel.Name)
            {
                isChangingRightKey = false;
                isChangingLeftKey = false;
                isChangingShootDownKey = true;
                isChangingShootUpKey = false;
                isChangingUpKey = false;
                isChangingDownKey = true;
                isChangingShootKey = false;
            }
            if (((Label)sender).Name == ShootCommandLabel.Name)
            {
                isChangingShootKey = true;
                isChangingRightKey = false;
                isChangingLeftKey = false;
                isChangingShootDownKey = false;
                isChangingShootUpKey = false;
                isChangingUpKey = false;
                isChangingDownKey = false;
            }

            ChooseNewKewPanel.BringToFront();
        }

        #endregion

        #region Timers

        private void GravityTimer_Tick(object sender, EventArgs e)
        {
            if (isStarted)
            {
                if(player.Top > 560)
                {
                    GameOver();
                }
                isFalling = true;
                if(ennemies.Count >= 1)
                {
                    for (int i = 0; i < ennemies.Count; i++)
                    {
                        ennemies[i].isFalling = true;
                        foreach (var plateform in ground.PlatformeList)
                        {
                            if (ennemies[i].Bottom >= plateform.Top &&
                                ennemies[i].Bottom < plateform.Top + 10 &&
                                ennemies[i].Left + ennemies[i].Width >= plateform.Left &&
                                ennemies[i].Left <= plateform.Left + 118)
                            {
                                ennemies[i].isFalling = false;
                                Console.WriteLine("The gravity is working well");
                            }
                        }
                        foreach (var plateform in otherGround.PlatformeList)
                        {
                            if (ennemies[i].Bottom >= plateform.Top &&
                                ennemies[i].Bottom < plateform.Top + 10 &&
                                ennemies[i].Left + ennemies[i].Width >= plateform.Left &&
                                ennemies[i].Left <= plateform.Left + 118)
                            {
                                ennemies[i].Top = plateform.Top - ennemies[i].Height;
                                ennemies[i].isFalling = false;
                            }
                        }
                        if (ennemies[i].isFalling)
                        {
                            ennemies[i].Top += 6;
                        }
                    }
                }
                foreach (var plateform in ground.PlatformeList)
                {
                    if (player.Bottom >= plateform.Top &&
                        player.Bottom < plateform.Top + 10 &&
                        player.Left + player.Width >= plateform.Left &&
                        player.Left <= plateform.Left + 118)
                    {
                        isFalling = false;
                    }
                }
                foreach (var plateform in otherGround.PlatformeList)
                {
                    if (player.Bottom >= plateform.Top && 
                        player.Bottom < plateform.Top + 10 &&
                        player.Left + player.Width >= plateform.Left &&
                        player.Left <= plateform.Left + 118)
                    {
                        player.Top = plateform.Top - player.Height;
                        isFalling = false;
                    }
                }
                if (isFalling)
                {
                    player.Top += 6;
                }
                if(shootedOnce)
                {
                    for(int i = 0; i < player.bulletList.Count; i++)
                    {
                        if ((string)player.bulletList[i].Tag == "right")
                        {

                            if(player.bulletList[i].up)
                            {
                                var x = player.bulletList[i].Left + 30;
                                var y = player.bulletList[i].Top - 30;

                                player.bulletList[i].Location = new Point(x, y);
                            }
                            else if (player.bulletList[i].down)
                            {
                                var x = player.bulletList[i].Left + 30;
                                var y = player.bulletList[i].Top + 30;

                                player.bulletList[i].Location = new Point(x, y);
                            }
                            else
                            {
                                player.bulletList[i].Left += 30;
                            }
                        }
                        else
                        {

                            if (player.isShootingDown)
                            {
                                player.bulletList[i].Top += 30;
                                player.bulletList[i].Left -= 30;
                            }
                            else if (player.isShootingUp)
                            {
                                player.bulletList[i].Top -= 30;
                                player.bulletList[i].Left -= 30;
                            }
                            else
                            {
                                player.bulletList[i].Left -= 30;
                            }
                        }

                        if (player.bulletList[i].Left < 0 ||
                            player.bulletList[i].Left > Width ||
                            player.bulletList[i].Top < 0 ||
                            player.bulletList[i].Top > Height)
                        {
                            map.Controls.Remove(player.bulletList[i]);
                            player.bulletList.Remove(player.bulletList[i]);
                        }
                    }
                }
            }
        }

        public int time = 1;
        private void Motion_Timer_Tick(object sender, EventArgs e)
        {
            if (isStarted)
            {
                player.BringToFront();
                if (isRightKey)
                {
                    if (player.isMoving)
                    {
                        if (player.Left >= 800)
                        {
                            if(ennemies.Count >= 1)
                            {
                                for(int i = 0; i < ennemies.Count; i++)
                                {
                                    ennemies[i].canBeDeplaced = false;
                                }
                            }
                            for (int i = 0; i < otherGround.PlatformeList.Count; i++)
                            {
                                if(otherGround.PlatformeList.Count != 0)
                                {
                                    otherGround.PlatformeList[i].Left -= 25;
                                    if (otherGround.PlatformeList[i].Left < -otherGround.PlatformeList[i].Width)
                                    {
                                        map.Controls.Remove(otherGround.PlatformeList[i]);
                                        otherGround.PlatformeList.RemoveAt(i);
                                    }
                                }
                            }
                            for (int i = 0; i < ground.PlatformeList.Count; i++)
                            {
                                ground.PlatformeList[i].Left -= 25;
                                ground.PlatformeList[i].BringToFront();
                            }
                            for (int i = 1; i < map.mapsList.Count - 1; i++)
                            {
                                if (map.mapsList[i].Left <= 0)
                                {
                                    map.mapsList[i - 1].Left = map.mapsList[map.mapsList.Count - 1].Left + 1350;
                                    map.mapsList.Add(map.mapsList[i - 1]);
                                    map.mapsList.RemoveAt(i - 1);

                                    map.Controls.Add(map.mapsList[map.mapsList.Count - 1]);
                                }
                            }
                            for (int i = 0; i < map.mapsList.Count; i++)
                            {
                                map.mapsList[i].Left -= 25;
                            }
                            for (int i = 1; i < ground.PlatformeList.Count - 1; i++)
                            {
                                if (ground.PlatformeList[i].Left <= 0)
                                {
                                    ground.PlatformeList[i - 1].Left = ground.PlatformeList[ground.PlatformeList.Count - 1].Left + 118;
                                    ground.PlatformeList.Add(ground.PlatformeList[i - 1]);
                                    ground.PlatformeList.RemoveAt(i - 1);

                                    map.Controls.Add(ground.PlatformeList[ground.PlatformeList.Count - 1]);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ennemies.Count; i++)
                            {
                                ennemies[i].canBeDeplaced = true;
                            }
                        }
                    }
                    time++;
                    if (time == 4)
                    {
                        time = 1;
                    }
                }
                player.BringToFront();
            }
        }

        public int timeLeft = 1;
        private void MotionLeft_Timer_Tick(object sender, EventArgs e)
        {
            if (isStarted)
            {
                player.BringToFront();
                player.BringToFront();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveRightCommandLabel.Text = RightKey.ToString();
            MoveLeftCommandLabel.Text = LeftKey.ToString();
            MoveUpCommandLabel.Text = UpKey.ToString();
            MoveDownCommandLabel.Text = DownKey.ToString();
            ShootCommandLabel.Text = ShootKey.ToString();
            ShootUpCommandLabel.Text = ShootUpKey.ToString();
            ShootDownCommandLabel.Text = ShootDownKey.ToString();



            if (CheckSFX.Checked == true)
            {
                SFXvolume.Enabled = true;
                isSFXAllowed = true;
                HoverSound.settings.volume = SFXvolume.Value * 10;
            }
            else
            {
                HoverSound.Enabled = false;
                SFXvolume.Enabled = false;
                isSFXAllowed = false;
            }

            if (CheckBackGroundMusic.Checked == true)
            {
                BackGroundVolume.Enabled = true;
                isBackgroundMusicEnabled = true;
                if (!isStarted)
                {
                    MenuSound.Ctlcontrols.play();
                }
                if(isStarted)
                {
                    PlayingSound.settings.volume = BackGroundVolume.Value * 10;
                }
                MenuSound.settings.volume = BackGroundVolume.Value * 10;
                GameOverSound.settings.volume = BackGroundVolume.Value * 10;
            }
            else
            {
                if (!isStarted)
                {
                    BackGroundVolume.Enabled = false;

                    isBackgroundMusicEnabled = false;
                    PlayingSound.Ctlcontrols.pause();
                    MenuSound.Ctlcontrols.pause();
                }
            }
        }
        
        private void SoundTimer_Tick(object sender, EventArgs e)
        {
            if(!isStarted)
            {
                if (TitleSoundIndex == 7)
                {
                    this.MenuSound.URL = "C:/Users/Laprovidence/Desktop/ContraX/Resources/02 Intro (J).mp3";
                    this.MenuSound.settings.setMode("loop", true);
                    this.MenuSound.Ctlcontrols.play();

                    SoundTimer.Stop();
                }
                TitleSoundIndex++;
            }
        }

        int titleCounter = 1;
        private void TitleTimer_Tick(object sender, EventArgs e)
        {
            if(isStarted)
            {
                if (titleCounter == 2)
                {
                    Stage1.Controls.Remove(Stage1Title);
                    TitleTimer.Stop();
                }
                titleCounter++;
            }
        }
        int gameOverCounter = 0;
        private void GameOverTimer_Tick(object sender, EventArgs e)
        {
            if(isGameOver)
            {
                if (gameOverCounter == 1)
                {
                    isStarted = false;
                    isGamePage = false;
                    isMenuPage = true;
                    isRestarted = false;

                    if (isBackgroundMusicEnabled)
                    {
                        MenuSound.Ctlcontrols.play();
                    }
                    GamePagesControl.Controls.Add(MenuPage);
                    GamePagesControl.Controls.Remove(GamePage);
                    MenuPage.Controls.Add(CursorPicture);
                    PlayButton.Text = "Restart";
                    PlayButton.Left -= 35;
                    GameOverTimer.Stop();
                    isRestarted = true;
                }
                gameOverCounter++;
            }
        }
        #endregion

        private void Fenetre_FormClosing(object sender, FormClosingEventArgs e)
        {
            isNotClosed = false;
            Environment.Exit(0);
        }
    }
}