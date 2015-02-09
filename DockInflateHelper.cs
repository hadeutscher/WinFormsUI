using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace WeifenLuo.WinFormsUI
{
    public class DockInflateHelper
    {
        private DockPanel dockPanel;
        private int pl = 0, pr = 0, pt = 0, pb = 0;

        public DockInflateHelper(DockPanel dockPanel)
        {
            this.dockPanel = dockPanel;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.OnDockChanged += recalculatePadding;
        }

        private void recalculatePadding(object sender, EventArgs e)
        {
            pl = 0;
            pr = 0;
            pt = 0;
            pb = 0;
            foreach (DockPane p in dockPanel.Panes)
            {
                switch (p.DockState)
                {
                    case DockState.DockLeft:
                        pl = p.Width + p.Margin.Horizontal;
                        break;
                    case DockState.DockRight:
                        pr = p.Width + p.Margin.Horizontal;
                        break;
                    case DockState.DockTop:
                        pt = p.Height + p.Margin.Vertical;
                        break;
                    case DockState.DockBottom:
                        pb = p.Height + p.Margin.Vertical;
                        break;
                    default:
                        break;
                }
            }
            System.Windows.Forms.ScrollableControl.DockPaddingEdges dpe = dockPanel.GetPadding();
            pl += dpe.Left;
            pr += dpe.Right;
            pt += dpe.Top;
            pb += dpe.Bottom;
            RedockControls();
        }

        private void RedockControls()
        {
            if (RedockRequired != null)
                RedockRequired.Invoke();
        }

        public void AddContent(DockContent dc, DockState ds)
        {
            dc.SizeChanged += recalculatePadding;
            dc.Show(dockPanel);
            dc.DockState = ds;
        }

        public Rectangle GetActiveRectangle()
        {
            int w = dockPanel.Width;
            int h = dockPanel.Height;

            return new Rectangle(pl, pt, w - pr - pl, h - pt - pb);
        }

        public delegate void RedockDelegate();
        public event RedockDelegate RedockRequired;
    }
}
