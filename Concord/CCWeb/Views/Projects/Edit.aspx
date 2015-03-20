<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
        Inherits="System.Web.Mvc.ViewPage<Common.Projects>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Project
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <%using (Html.BeginForm())
          { %>
    <h2>Edit Project #<%=Model==null?"[new]":Model.Id.ToString() %></h2>
                        
        <table style="padding:15px 5px 5px 25px;">
            <tr><td style="padding:5px 5px 5px 5px;">Name:</td>
                <td style="padding:5px 5px 5px 5px;"><%=Html.TextBox
                      ("name", Model==null?"?":Model.Name, new {style="width:100px;"}) %></td></tr>            
            <tr><td style="padding:5px 5px 5px 5px;">Status:</td>
                <td style="padding:5px 5px 5px 5px;"><%=Html.DropDownList
                      ("status", Common.Projects.GetStatusesItemList((int)Model.Status))%>
                </td></tr>
        </table>
        <table style="border-color:#69523b; border-width:2px; border-style:solid;"">
            <tr style="font-weight:bold;">
                <td valign="top" style="padding:5px 5px 5px 5px;">Author</td>
                <td valign="top" style="padding:5px 5px 5px 5px;">Date</td>
                <td valign="top" style="padding:5px 5px 5px 5px;">Comment</td></tr>
            
            <%if (Model != null && Model.Comments != null && Model.Comments.Count > 0)
              { %>
               <%foreach (Common.ProjectComment projCom in Model.Comments)
                 { %> 
                    <tr><td style="padding:5px 5px 5px 5px;" valign="top"><%=projCom.Author.UserName%></td>
                        <td style="padding:5px 5px 5px 5px;" valign="top"><%=projCom.SaveDate.ToString("g")%></td>
                        <td style="padding:5px 5px 5px 5px;"><%=projCom.Comment%></td></tr>
               <%} %>
            <%} %>
            
            <tr>
                <td valign="top" style="padding:5px 5px 5px 5px;display:none;">
                    <%=Html.TextBox("id", Model.Id, new { style = "display:none;"})%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;">
                    <%=Html.TextBox("author", Page.User.Identity.Name, new { style = "width:75px;", ReadOnly = "true" })%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;" colspan="2">
                    <%=Html.TextArea("comment", "Comment...", new { cols = "100", rows = "5" })%></td></tr>
        </table>
        <br />
        <input type="submit" value="Save Changes" />
        <%} %>
</asp:Content>
