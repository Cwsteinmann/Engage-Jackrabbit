﻿// <copyright file="ViewJackrabbit.ascx.cs" company="Engage Software">
// Engage: Jackrabbit
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Jackrabbit
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Web.UI;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Web.Client.ClientResourceManagement;
    using DotNetNuke.Web.Mvp;
    using DotNetNuke.Web.UI.WebControls;

    using Telerik.Web.UI;

    using WebFormsMvp;

    /// <summary>Includes the scripts</summary>
    [PresenterBinding(typeof(ViewJackrabbitPresenter))]
    public partial class ViewJackrabbit : ModuleView<ViewJackrabbitViewModel>, IViewJackrabbitView
    {
        /// <summary>Occurs when a script is added.</summary>
        public event EventHandler<AddScriptEventArgs> AddScript = (_, __) => { }; 

        /// <summary>Occurs when a script is updated.</summary>
        public event EventHandler<UpdateScriptEventArgs> UpdateScript = (_, __) => { }; 

        /// <summary>Raises the <see cref="Control.PreRender" /> event.</summary>
        /// <param name="e">The <see cref="EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                base.OnPreRender(e);
            
                this.AutoDataBind = false;
                this.DataBind();

                foreach (var script in this.Model.Scripts)
                {
                    ClientResourceManager.RegisterScript(this.Page, script.FullScriptPath, script.Priority, script.Provider);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>Handles the <see cref="RadGrid.NeedDataSource" /> event of the scripts grid.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridNeedDataSourceEventArgs"/> instance containing the event data.</param>
        protected void Grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                ((DnnGrid)sender).DataSource = this.Model.Scripts;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>Handles the <see cref="RadGrid.InsertCommand" /> event of the scripts grid.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        protected void Grid_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                var item = (GridEditableItem)e.Item;
                var values = new Hashtable();
                item.ExtractValues(values);
                this.AddScript(
                    this,
                    new AddScriptEventArgs(
                        (string)values["PathPrefixName"],
                        (string)values["ScriptPath"],
                        (string)values["Provider"],
                        int.Parse(values["Priority"].ToString(), CultureInfo.InvariantCulture)));
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>Handles the <see cref="RadGrid.UpdateCommand" /> event of the scripts grid.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridCommandEventArgs"/> instance containing the event data.</param>
        protected void Grid_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                var item = (GridEditableItem)e.Item;
                var values = new Hashtable();
                item.ExtractValues(values);
                this.UpdateScript(
                    this,
                    new UpdateScriptEventArgs(
                        (int)item.GetDataKeyValue("Id"),
                        (string)values["PathPrefixName"],
                        (string)values["ScriptPath"],
                        (string)values["Provider"],
                        int.Parse(values["Priority"].ToString(), CultureInfo.InvariantCulture)));
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}