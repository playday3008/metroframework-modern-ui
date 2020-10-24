using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    public class MetroContextMenu : ContextMenuStrip, IMetroControl
    {
        #region Interface

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        [Category("Metro Appearance")]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == MetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == MetroColorStyle.Default)
                {
                    return MetroColorStyle.Blue;
                }

                return metroStyle;
            }
            set => metroStyle = value;
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        [Category("Metro Appearance")]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default)
                {
                    return MetroThemeStyle.Light;
                }

                return metroTheme;
            }
            set => metroTheme = value;
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get => metroStyleManager;
            set
            {
                metroStyleManager = value;
                SetTheme();
            }
        }

        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseCustomBackColor { get; set; } = false;
        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseCustomForeColor { get; set; } = false;
        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseStyleColors { get; set; } = false;

        [Browsable(false)]
        [Category("Metro Behaviour")]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get => GetStyle(ControlStyles.Selectable);
            set => SetStyle(ControlStyles.Selectable, value);
        }

        #endregion


        public MetroContextMenu(IContainer Container)
        {
            if (Container != null)
            {
                Container.Add(this);
            }
        }

        private void SetTheme()
        {
            BackColor = MetroPaint.BackColor.Form(Theme);
            ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
            Renderer = new MetroCTXRenderer(Theme, Style);
        }

        private class MetroCTXRenderer : ToolStripProfessionalRenderer
        {
            private readonly MetroThemeStyle _theme;
            public MetroCTXRenderer(MetroThemeStyle Theme, MetroColorStyle Style) : base(new ContextColors(Theme, Style))
            {
                _theme = Theme;
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = MetroPaint.ForeColor.Button.Normal(_theme);
                base.OnRenderItemText(e);
            }
        }

        private class ContextColors : ProfessionalColorTable
        {
            private readonly MetroThemeStyle _theme = MetroThemeStyle.Light;
            private readonly MetroColorStyle _style = MetroColorStyle.Blue;

            public ContextColors(MetroThemeStyle Theme, MetroColorStyle Style)
            {
                _theme = Theme;
                _style = Style;
            }

            public override Color MenuItemSelected => MetroPaint.GetStyleColor(_style);

            public override Color MenuBorder => MetroPaint.BackColor.Form(_theme);

            public override Color ToolStripBorder => MetroPaint.GetStyleColor(_style);

            public override Color MenuItemBorder => MetroPaint.GetStyleColor(_style);

            public override Color ToolStripDropDownBackground => MetroPaint.BackColor.Form(_theme);

            public override Color ImageMarginGradientBegin => MetroPaint.BackColor.Form(_theme);

            public override Color ImageMarginGradientMiddle => MetroPaint.BackColor.Form(_theme);

            public override Color ImageMarginGradientEnd => MetroPaint.BackColor.Form(_theme);
        }
    }
}
