<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryListCriteriaCtrl.ascx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.CategoryListCriteriaCtrl" %>
<div id="search">
    
    <asp:PlaceHolder ID="phAPIKey" runat="server">
        <h3>API Key</h3>
        <fieldset>
            <ol>
                <li>
                    <label for="APIKey">API Key:</label>
                    <input type="text" class="inputText" name="APIKey" id="APIKey" value="" />
                    <input type="hidden" name="action" id="action" value="search"/>
                </li>
            </ol>
        </fieldset>
    </asp:PlaceHolder>

    <div class="button">
        <input type="submit" value="Submit" class="submit" />
    </div>
</div>
<div style="clear: both"></div>