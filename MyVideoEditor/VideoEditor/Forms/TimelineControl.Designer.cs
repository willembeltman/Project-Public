using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditor.Forms
{
    public partial class TimelineControl
    {
        private void InitializeComponent()
        {
            scrollBarControl = new HScrollBar();
            SuspendLayout();
            // 
            // scrollBarControl
            // 
            scrollBarControl.Location = new Point(0, 391);
            scrollBarControl.Name = "scrollBarControl";
            scrollBarControl.Size = new Size(665, 39);
            scrollBarControl.TabIndex = 2;
            scrollBarControl.Scroll += ScrollBarControl_Scroll;
            // 
            // TimelineControl
            // 
            AllowDrop = true;
            Controls.Add(scrollBarControl);
            Name = "TimelineControl";
            Size = new Size(665, 430);
            Load += TimelineControl_Load;
            DragDrop += TimelineControl_DragDrop;
            DragEnter += TimelineControl_DragEnter;
            DragOver += TimelineControl_DragOver;
            Paint += TimelineControl_Paint;
            Resize += TimelineControl_Resize;
            MouseWheel += TimelineControl_MouseWheel;
            ResumeLayout(false);
        }

        private HScrollBar scrollBarControl;
    }
}
