<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderSearchCriteriaCtrl.ascx.cs" Inherits="IMS.NCS.CourseSearchService.TestHarness.ProviderSearchCriteriaCtrl" %>
<div id="search">
    
    <asp:PlaceHolder ID="phAPIKey" runat="server">
        <h3>API Key</h3>
        <fieldset>
            <ol>
                <li>
                    <label for="APIKey">API Key:</label>
                    <input type="text" class="inputText" name="APIKey" id="APIKey" value="" />
                </li>
            </ol>
        </fieldset>
    </asp:PlaceHolder>

    <h3>Provider</h3>
    <fieldset>
        <ol>
            <li>
                <label for="Provider">Provider:</label>
                <input type="text" class="inputText" name="Provider" id="Provider" value=""/>
                <input type="hidden" name="action" id="action" value="search"/>
            </li>
        </ol>
    </fieldset>

    <div class="button">
        <input type="submit" value="Submit" class="submit" />
    </div>
</div>