// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BootStrapMenu.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Renders Html for a BootStrapMenu
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.UI.WebControls;
// ReSharper disable once CheckNamespace


namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The boot strap menu item visibility.
    /// </summary>
    public enum BootStrapMenuItemVisibility
    {
        /// <summary>
        /// The menu item is not shown
        /// </summary>
        None,

        /// <summary>
        /// Default setting, the menu item is visible or hidden based on the permissions assigned
        /// </summary>
        PermissionBased,

        /// <summary>
        /// Menu item is visible when the user is authenticated and hidden when not authenticated
        /// </summary>
        Authenticated,

        /// <summary>
        /// Menu item is hidden when authenticated and visible when not authenticated
        /// </summary>
        NotAuthenticated,

        /// <summary>
        /// Menu item is always shown regardless of permissions or authenticated status
        /// </summary>
        ShowAlways
    }

    /// <summary>
    /// The boot strap menu alignment.
    /// </summary>
    public enum BootStrapMenuAlignment
    {
        /// <summary>
        /// Menu is left aligned.
        /// </summary>
        Left,

        /// <summary>
        /// Menu is right aligned.
        /// </summary>
        Right
    }

    /// <summary>
    /// Renders Html for a BootStrapMenu
    /// </summary>
    public class BootStrapMenu
    {
        /// <summary>
        /// The _boot strap menu items.
        /// </summary>
        private readonly List<BootStrapMenuItem> bootStrapMenuItems = new List<BootStrapMenuItem>();
        
        /// <summary>
        /// The site path.
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootStrapMenu"/> class.
        /// </summary>
        public BootStrapMenu()
        {
            Alignment = BootStrapMenuAlignment.Left;
        }

        /// <summary>
        /// The menu alignment.
        /// </summary>
        public BootStrapMenuAlignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified path to the page.  This path is pre-pended to the page name to create the full site relative Url. If a path is supplied 
        /// for the menu this setting has no effect
        /// </summary>
        public string FullyQualifiedPathToLinks
        {
            get
            {
                return path;
            }

            set
            {
                this.path = value.TrimEnd('/');
            }
        }

        // Formatting strings
        private const string MenuLink = "<li class=\"{0}\"><a {4}href=\"{1}\" {3}>{2}</a></li>";
        private const string DropDownLink = "<a href=\"#\" class=\"dropdown-toggle {0}\" data-toggle=\"dropdown\">{1} <b class=\"caret\"></b></a>";
        private const string Divider = "<li class=\"divider\"></li>";
        private const string GroupLink = "<li class=\"nav-header\">{0}</li>";
        private const string SubMenu = "<li class=\"dropdown dropdown-submenu\"><a tabindex=\"0\" href=\"#\" class=\"dropdown-toggle {0}\" data-toggle=\"dropdown\">{1}</a><ul class=\"dropdown-menu\">{2}</ul></li>";
        
        /// <summary>
        /// Adds a boot strap menu item
        /// </summary>
        /// <param name="bootStrapMenuItem">
        /// The boot strap menu item.
        /// </param>
        /// <returns>
        /// Returns the index of the added item
        /// </returns>
        public int Add(BootStrapMenuItem bootStrapMenuItem)
        {
            this.bootStrapMenuItems.Add(bootStrapMenuItem);
            return this.bootStrapMenuItems.Count - 1;
        }
        
        /// <summary>
        /// Returns the rendered Bootstrap menu
        /// </summary>
        /// <param name="currentUrl">The current Url of the page being displayed</param>
        /// <returns>The Bootstrap menu Html</returns>
        public string RenderedHtml(string currentUrl)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine(string.Format("<ul class=\"nav navbar-nav {0}\">",
                Alignment == BootStrapMenuAlignment.Left ? "navbar-left" : "navbar-right"));
            currentUrl = currentUrl.Trim('/');
            if (currentUrl == "default.aspx")
            {
                currentUrl = "/";
            }

            var items = RenderedItems(this.bootStrapMenuItems, currentUrl, true/*isRoot*/);
            html.Append(items);

            html.AppendLine("</ul>");
            return html.ToString();
        }

        private StringBuilder RenderedItems(List<BootStrapMenuItem> menuItems, string currentUrl, bool isRoot)
        {
            var html = new StringBuilder();
            foreach (BootStrapMenuItem item in menuItems)
            {
                BootStrapParentItem parentItem = item as BootStrapParentItem;

                // Is this the active menu item
                // TODO Make selecting current menu item more reliable
                if (currentUrl.Equals(item.NavigateToUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    item.IsActive = true;
                }

                if (item.HasPermission || parentItem != null)
                {
                    if (parentItem != null)
                    {
                        StringBuilder menuChildItems = new StringBuilder();
                        bool isActive = false;
                        bool isChildItemsAdded = false;

                        // Now add all the children
                        for (int index = 0; index < parentItem.Children.Count; index++)
                        {
                            BootStrapMenuItem itemChild = parentItem.Children[index];
                            if (!itemChild.IsDivider &&
                                currentUrl.Equals(itemChild.NavigateToUrl, StringComparison.InvariantCultureIgnoreCase))
                            {
                                itemChild.IsActive = true;
                                isActive = true;
                            }
                            if (itemChild.HasPermission)
                            {
                                if (itemChild is BootStrapParentItem)
                                {
                                    var itemAsParent = ((BootStrapParentItem) itemChild);
                                    var subMenuItems = RenderedItems(itemAsParent.Children, currentUrl, false /*isRoot*/);
                                    if (subMenuItems.Length > 0)
                                    {
                                        menuChildItems.AppendFormat(SubMenu, itemAsParent.Class ?? String.Empty, itemAsParent.Text, subMenuItems);
                                        isChildItemsAdded = true;
                                    }
                                }
                                else if (itemChild.IsDivider)
                                {
                                    // Only add a divider if items above or below the divider are being shown                               
                                    if (index > 0 && (index - 1) < parentItem.Children.Count)
                                    {
                                        if (parentItem.Children[index - 1].HasPermission &&
                                            parentItem.Children[index + 1].HasPermission)
                                        {
                                            menuChildItems.AppendLine(Divider);
                                        }
                                    }
                                }
                                else if (string.IsNullOrEmpty(this.GetFullyQualifiedPath(itemChild.NavigateToUrl)))
                                {
                                    menuChildItems.AppendFormat(GroupLink, itemChild.Text);
                                    isChildItemsAdded = true;
                                }
                                else
                                {
                                    menuChildItems.AppendFormat(MenuLink, itemChild.IsActive ? "active" : string.Empty,
                                        this.GetFullyQualifiedPath(itemChild.NavigateToUrl), itemChild.Text,
                                        itemChild.Class != "" ? "class=\"" + itemChild.Class + "\"" : "",
                                        !String.IsNullOrWhiteSpace(itemChild.id) ? "id=\"" + itemChild.id + "\" " : "");
                                    isChildItemsAdded = true;
                                }
                            }
                        }

                        if (item.HasPermission && isChildItemsAdded)
                        {
                            if (isRoot)
                            {
                                html.AppendLine(isActive ? 
                                    "<li class=\"dropdown active\">"
                                    : "<li class=\"dropdown\">");
                                html.AppendFormat(DropDownLink, parentItem.Class ?? String.Empty, item.Text);
                                html.AppendLine("<ul class=\"dropdown-menu\">");
                                html.AppendLine(menuChildItems.ToString());
                                html.AppendLine("</ul></li>");
                            }
                            else
                            {
                                // TODO Do we need to inject the active class?
                                html.AppendFormat(SubMenu, parentItem.Class ?? String.Empty, item.Text, menuChildItems);
                            }
                        }
                    }
                    else
                    {
                        html.AppendFormat(MenuLink, item.IsDivider ? "divider" : item.IsActive ? "active" : string.Empty,
                            this.GetFullyQualifiedPath(item.NavigateToUrl), item.Text,
                            item.Class != "" ? "class=\"" + item.Class + "\"" : "",
                            !String.IsNullOrWhiteSpace(item.id) ? "id=\"" + item.id + "\" " : "");
                    }
                }
            }
            return html;
        }

        /// <summary>
        /// Gets the fully qualified path
        /// </summary>
        /// <param name="page">
        /// The page name
        /// </param>
        /// <returns>
        /// The fully qualified path
        /// </returns>
        private string GetFullyQualifiedPath(string page)
        {
            if (!string.IsNullOrWhiteSpace(this.FullyQualifiedPathToLinks) && !page.Contains('/'))
            {
                return string.Concat(this.FullyQualifiedPathToLinks, '/', page);
            }
            else
            {
                return page;
            }
        }
    }

    /// <summary>
    /// The boot strap parent item.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class BootStrapParentItem : BootStrapMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BootStrapParentItem"/> class.
        /// </summary>
        /// <param name="menuText">
        /// The menu text.
        /// </param>
        public BootStrapParentItem(string menuText)
            : base(menuText, string.Empty, BootStrapMenuItemVisibility.PermissionBased, null)
        {
            this.Children = new List<BootStrapMenuItem>();
        }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public System.Collections.Generic.List<BootStrapMenuItem> Children { get; set; }
    }

    /// <summary>
    /// The boot strap menu item.
    /// </summary>
    public class BootStrapMenuItem
    {
        /// <summary>
        /// The _permissions required.
        /// </summary>
        private readonly List<Permission.PermissionName> permissionsRequired = new List<Permission.PermissionName>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BootStrapMenuItem"/> class. 
        /// Creates a BootStrapMenuItem
        /// </summary>
        /// <param name="menuText">
        /// The text for the menu
        /// </param>
        /// <param name="navigateToUrl">
        /// The link the menu item navigates to when clicked
        /// </param>
        /// <param name="bootStrapMenuItemVisibility">
        /// The boot Strap Menu Item Visibility.
        /// </param>
        /// <param name="requiredPermissions">
        /// The permissions required for this item to be visible, if the user has one or more permissions the menu item is shown.  Set IsAllPermissionsRequired if the user must have
        /// all permissions to see this menu item.  If no requiredPermissions set the menu item is always shown.
        /// </param>
        public BootStrapMenuItem(string menuText, string navigateToUrl, BootStrapMenuItemVisibility bootStrapMenuItemVisibility, params Permission.PermissionName[] requiredPermissions)
        {
            this.Text = menuText;
            this.NavigateToUrl = navigateToUrl;
            if (requiredPermissions != null)
            {
                this.permissionsRequired.AddRange(requiredPermissions);
            }

            this.BootStrapMenuItemVisibility = bootStrapMenuItemVisibility;

            this.Class = Class;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootStrapMenuItem"/> class. 
        /// Creates a BootStrapMenuItem
        /// </summary>
        /// <param name="isDivider">
        /// When true the menu item is a divider, all other properties are ignored except for the permission check. If permission check fails the divider is not rendered
        /// </param>
        public BootStrapMenuItem(bool isDivider)
        {
            this.IsDivider = isDivider;
            this.NavigateToUrl = string.Empty;
            this.BootStrapMenuItemVisibility = BootStrapMenuItemVisibility.PermissionBased;
        }
        
        /// <summary>
        /// Gets or sets the URL this item navigates to
        /// </summary>
        public string NavigateToUrl { get; set; }

        /// <summary>
        /// Gets or sets the boot strap menu item visibility.
        /// </summary>
        public BootStrapMenuItemVisibility BootStrapMenuItemVisibility { get; set; }

        /// <summary>
        /// Gets the permissions required.
        /// </summary>
        public List<Permission.PermissionName> PermissionsRequired
        {
            get
            {
                return this.permissionsRequired;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the id for this menu item
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating to add a class to this menu item
        /// </summary>
        public String Class { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this menu is the active menu, when active the menu is shown highlighted, the default is false
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all permissions are required, when true HasPermission is only true if the user has all the permissions, this setting default is false
        /// </summary>
        public bool IsAllPermissionsRequired { get; set; }

        /// <summary>
        /// Gets the text of the menu item, used by the renderer.
        /// </summary>
        internal string Text { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this link is a divider, used by the renderer.
        /// </summary>
        internal bool IsDivider { get; private set; }
    
        /// <summary>
        /// Gets a value indicating whether the user has permission to see this menu item, false if does not have permission.  Typically this property is used during the render and so does not need to be used by code.
        /// </summary>
        internal bool HasPermission
        {
            get
            {
                switch (this.BootStrapMenuItemVisibility)
                {
                    case BootStrapMenuItemVisibility.Authenticated:
                        return System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                    
                    case BootStrapMenuItemVisibility.NotAuthenticated:
                        return !System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                    
                    case BootStrapMenuItemVisibility.PermissionBased:
                        return permissionsRequired.Count <= 0
                            || Permission.HasPermission(false, false, this.permissionsRequired.ToArray());

                    case BootStrapMenuItemVisibility.None:
                        return false;

                    case BootStrapMenuItemVisibility.ShowAlways:
                        return true;

                    default:
                        return false;
                }
            }
        }
    }
}