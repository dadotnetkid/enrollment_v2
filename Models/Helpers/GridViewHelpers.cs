using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting.Native;

namespace Helpers
{
    public class GridSettings
    {
        private List<Action<MVCxGridViewToolbarItem>> _menuItems;
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public List<Action<MVCxGridViewToolbarItem>> MenuItems
        {
            get
            {
                if (_menuItems == null)
                    _menuItems = new List<Action<MVCxGridViewToolbarItem>>();
                return _menuItems;
            }
            set => _menuItems = value;
        }
    }
    public static class GridViewHelpers
    {
        public static GridViewSettings AddAddEditDeleteToolbar(this GridViewSettings settings, bool canAdd = true, bool canEdit = true, bool canDelete = true)
        {
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CommandColumn.ShowEditButton = false;
            settings.CommandColumn.ShowNewButtonInHeader = false;
            settings.CommandColumn.ShowDeleteButton = false;
            settings.CommandColumn.Visible = false;
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;

                if (canAdd)
                    tb.Items.Add(GridViewToolbarCommand.New);
                if (canEdit)
                    tb.Items.Add(GridViewToolbarCommand.Edit);
                if (canDelete)
                    tb.Items.Add(GridViewToolbarCommand.Delete);
            });

            return settings;
        }
        public static GridViewSettings AddAddEditDeleteToolbar(this GridViewSettings gridViewSettings, Action<GridSettings> settings)
        {
            gridViewSettings.SettingsBehavior.AllowFocusedRow = true;
            gridViewSettings.CommandColumn.ShowEditButton = false;
            gridViewSettings.CommandColumn.ShowNewButtonInHeader = false;
            gridViewSettings.CommandColumn.ShowDeleteButton = false;
            gridViewSettings.CommandColumn.Visible = false;
            gridViewSettings.SettingsBehavior.AllowFocusedRow = true;
            gridViewSettings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;
                var gridSettings = new GridSettings();
                settings(gridSettings);
                if (gridSettings.CanAdd)
                    tb.Items.Add(GridViewToolbarCommand.New);
                if (gridSettings.CanAdd)
                    tb.Items.Add(GridViewToolbarCommand.Edit);
                if (gridSettings.CanDelete)
                    tb.Items.Add(GridViewToolbarCommand.Delete);
                foreach (var i in gridSettings.MenuItems)
                {
                    tb.Items.Add(i);
                }
            });

            return gridViewSettings;
        }
        public static GridViewSettings AddEditDeleteImportToolbar(this GridViewSettings settings, Action<MVCxGridViewToolbarItem> items)
        {
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CommandColumn.ShowEditButton = false;
            settings.CommandColumn.ShowNewButtonInHeader = false;
            settings.CommandColumn.ShowDeleteButton = false;
            settings.CommandColumn.Visible = false;
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;


                tb.Items.Add(GridViewToolbarCommand.New);
                tb.Items.Add(GridViewToolbarCommand.Edit);
                tb.Items.Add(GridViewToolbarCommand.Delete);
                tb.Items.Add(items);
            });

            return settings;
        }
        public static void AddAddEditDeleteToolbar(this GridViewSettings settings, MVCxGridViewToolbarItemCollection items)
        {
            settings.SettingsBehavior.AllowFocusedRow = true;
            settings.CommandColumn.ShowEditButton = false;
            settings.CommandColumn.ShowNewButtonInHeader = false;
            settings.CommandColumn.ShowDeleteButton = false;
            settings.Toolbars.Add(tb =>
            {
                tb.Enabled = true;
                tb.Position = GridToolbarPosition.Top;


                tb.Items.Add(GridViewToolbarCommand.New);
                tb.Items.Add(GridViewToolbarCommand.Edit);
                tb.Items.Add(GridViewToolbarCommand.Delete);


                tb.Items.AddRange(items);

            });
        }
    }
}
