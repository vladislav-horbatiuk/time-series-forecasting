using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


using System.Text;
using System.Windows.Forms;

using BS_Main.Properties;


namespace BSWork.BS_Main
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _AddComponentsImages();
            _ConfigureMainDrawingArea();
            arrowDrawingPen.EndCap = LineCap.ArrowAnchor;
            arrowDrawingPen.CustomEndCap = new AdjustableArrowCap(5, 7);
            BSManager.RegisterRunFinishedDelegate(new RunFinishedDelegate(SimulationFinished));
            BSManager.RegisterRunAbortedDelegate(new RunAbortedDelegate(SimulationAborted));
        }

        private void _ConfigureMainDrawingArea()
        {
            mainPictureBox.AllowDrop = true;
            mainPictureBox.Image = new Bitmap(mainPictureBox.Size.Width, mainPictureBox.Size.Height);
        }

        private void _AddComponentsImages()
        {
            componentsImageList.Images.Add(Resources.input);
            componentsImageList.Images.Add(Resources.preprocessor);
            componentsImageList.Images.Add(Resources.graph);
            componentsImageList.Images.Add(Resources.modelbuilder);
            componentsImageList.Images.Add(Resources.forecaster);
            componentsImageList.Images.Add(Resources.savedata);
            for (int i = 0; i < 6; ++i) componentsListView.Items[i].ImageIndex = i;
        }

        private void componentsListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem draggedItem = (e.Item as ListViewItem);
            (sender as Control).DoDragDrop(draggedItem.Tag, DragDropEffects.Copy);
        }

        private void mainPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            if (null != draggedImg)
            {
                Point mousePos = (sender as Control).PointToClient(new Point(e.X, e.Y));
                Rectangle componentRect = new Rectangle(mousePos.X - draggedImg.Width / 2, mousePos.Y - draggedImg.Height / 2, draggedImg.Width, draggedImg.Height);
                _DrawComponentOnPictureBoxInRectangle(e, draggedImg, (sender as PictureBox), componentRect);
                BSManager.AddComponentWithNameAndRectangle(e.Data.GetData(DataFormats.StringFormat) as string, componentRect);
                draggedImg = null;
            }
        }

        private void _DrawComponentOnPictureBoxInRectangle(DragEventArgs e, Image iImgToDraw, PictureBox iPictureBox, Rectangle iRect)
        {
            Bitmap pictureBoxImage = new Bitmap(iPictureBox.Image);
            using (Graphics gr = Graphics.FromImage(pictureBoxImage))
            {
                gr.DrawImage(iImgToDraw, iRect);
            }
            iPictureBox.Image = pictureBoxImage;
        }

        private void mainPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                try
                {
                    draggedImg = (Image)Resources.ResourceManager.GetObject((e.Data.GetData(DataFormats.StringFormat) as string));
                    e.Effect = DragDropEffects.Copy;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, exc.GetType().Name);
                    return;
                }
            }
        }

        private void componentsListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (null != draggedImg)
            {
                e.UseDefaultCursors = false;
                Cursor.Current = new Cursor(new Bitmap(draggedImg, 32, 32).GetHicon());
            }
        }

        private void mainPictureBox_DragLeave(object sender, EventArgs e)
        {
            draggedImg = null;
        }

        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            dragStartingPoint = e.Location;
            if (BSManager.ExistsSuitableInputComponentAtPoint(dragStartingPoint))
                imageBeforeDrag = new Bitmap((sender as PictureBox).Image);
        }

        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (imageBeforeDrag != null)
            {
                try
                {
                    BSManager.LinkComponentsAtPoints(dragStartingPoint, e.Location);
                }
                catch (Exception exc)
                {
                    if (exc is ComponentsCantBeLinkedException || exc is ArgumentException)
                        (sender as PictureBox).Image = imageBeforeDrag;
                    else
                        throw;
                }
            }
            imageBeforeDrag = null;
        }

        private void mainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (imageBeforeDrag != null)
            {
                DrawLineFromComponent(sender as PictureBox, e);
            }
        }

        private void DrawLineFromComponent(PictureBox sender, MouseEventArgs e)
        {
            Bitmap tmp = new Bitmap(imageBeforeDrag);
            using (Graphics gr = Graphics.FromImage(tmp))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.DrawLine(arrowDrawingPen, dragStartingPoint, e.Location);
                
            }
            sender.Image = tmp;
            sender.Update();
        }

        private Image draggedImg = null;
        private Bitmap imageBeforeDrag = null;
        private Point dragStartingPoint;
        private readonly Pen arrowDrawingPen = new Pen(Color.Black);

        private void mainPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BSManager.UserDoubleClickedOnPoint(e.Location);
        }

        private void runToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                transparentImage = ImgUtils.SetImgOpacity(mainPictureBox.Image, 0.3F);
                realImage = mainPictureBox.Image;
                SwitchSimulationVisualization(true);
                BSManager.Run();
            }
            catch (ComponentsArentReadyException exc)
            {
                MessageBox.Show(exc.Message, "Не усі компоненти корректно налаштовані!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SwitchSimulationVisualization(false);
            }
        }

        private void SimulationAborted(string iExceptionMessage)
        {
            this.Invoke(new MethodInvoker(delegate {
                SwitchSimulationVisualization(false);
                ticksCount = 0;
                MessageBox.Show("У ході виконання виникли наступні помилки:\n" + iExceptionMessage,
                    "Помилка у ході симуляції.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void SimulationFinished()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                SwitchSimulationVisualization(false);
                ticksCount = 0;
            }));
        }

        private void SwitchSimulationVisualization(bool on)
        {
            runToolStripButton.Enabled = !on;

            mainPictureBox.Enabled = !on;
            mainPictureBox.Image = (on) ? transparentImage : realImage;
            mainPictureBox.Update();

            progressBar.Visible = on;
            progressBar.Enabled = on;

            timer.Enabled = on;

            mainToolStrip.Items[mainToolStrip.Items.Count - 1].Visible = on;
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.ShowDialog();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F1:
                    helpToolStripButton_Click(sender, e);
                    break;
                case Keys.F5:
                    runToolStripButton_Click(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void mainPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Items.Clear();
                if (BSManager.ExistsComponentAtPoint(e.Location))
                {
                    contextMenuStrip.Items.Add("Налаштувати", null, new EventHandler((obj, arg) => BSManager.UserDoubleClickedOnPoint(e.Location)));
                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                }
                AddGeneralHelpItemToContextMenuStrip(contextMenuStrip, "Це головна робоча область. Перетягніть сюди усі потрібні елементи (із списку елементів " +
                    "зліва), з'єднайте їх потрібним чином і натисніть кнопку 'Запустити' на панелі згори (або просто клавішу F5). Це призведе до запуску побудованої " +
                    "схеми на виконання. Для налаштування будь-якого елемента двічі натисніть лівою кнопкою миші на ньому, або один раз правою кнопкою, і виберіть 'Налаштувати'.");
                contextMenuStrip.Show((sender as Control).PointToScreen(e.Location));
            }
        }

        private void AddGeneralHelpItemToContextMenuStrip(ContextMenuStrip iContextMenuStrip, string iMessage)
        {
            iContextMenuStrip.Items.Add("Що це таке?", null, 
                new EventHandler((obj, arg) => MessageBox.Show(iMessage, "Допомога", MessageBoxButtons.OK, MessageBoxIcon.Information)));
        }

        private void componentsListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip.Items.Clear();
                AddGeneralHelpItemToContextMenuStrip(contextMenuStrip, "Це список усіх елементів. Перетягніть потрібні елементи звідси на головгу робочу область (праворуч).");
                contextMenuStrip.Show((sender as Control).PointToScreen(e.Location));
            }
        }

        private void componentsListView_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            toolTip.Show(e.Item.ToolTipText, (sender as Control), e.Item.Position.X, e.Item.Position.Y, 2000);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ticksCount = (ticksCount + 1) % 4;
            mainToolStrip.Items[mainToolStrip.Items.Count - 1].Text = "Виконання" + new String('.', ticksCount);
        }

        private int ticksCount = 0;
        private Image transparentImage, realImage;
    }


}
