<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
        Inherits="System.Web.Mvc.ViewPage<List<Common.Issues>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Issues
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%using (Html.BeginForm())
  { %>
    <h2>Issues</h2>
    <br />
    <%=Html.ActionLink("Create New Issue", "Edit", new { id = -1 })%>
    <br /><br />
    View only status: <%=Html.DropDownList("status", Common.Issues.GetStatusesItemList(0))%><input type="submit" value="Go" />
    <br /><br />
      <table style="border-color:#69523b; border-width:2px; border-style:solid;">
        <tr style="font-weight:bold;">
            <td valign="top" style="padding:5px 5px 5px 5px;display:none;">ID</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Name</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Date</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Status</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Author/Comment</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Remove?</td></tr>
<%if (Model != null && Model.Count > 0)
  {%>
  <%
    foreach (Common.Issues iss in Model)
    { %>
            <tr><td valign="top" style="padding:5px 5px 5px 5px;display:none;"><%=iss.Id%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;"><%=Html.ActionLink(iss.Name, "Edit", new { id = iss.Id })%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;"><%=iss.SubmitDate.ToString("g")%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;"><%=iss.Status.ToString()%></td><td>
            <%if (iss.Comments != null && iss.Comments.Count > 0)
              { %>
            <table>
               <%foreach (Common.IssueComment issCom in iss.Comments)
                 { %> 
                    <tr><td style="padding:5px 5px 5px 5px;display:none;" valign="top"><%=issCom.Id%></td>
                        <td style="padding:5px 5px 5px 5px;" valign="top"><%=issCom.Author.UserName%></td>
                        <td style="padding:5px 5px 5px 5px;"><%=issCom.Comment%></td></tr>
               <%} %>
            </table>
            <%} %>
          </td><td valign="top" style="padding:5px 5px 5px 5px;">
                    <%=Html.ActionLink("Remove", "Remove", new { id = Html.Encode(iss.Id) })%></td></tr>
<%    }
      %>
      <%
    }
  } %>
      </table>
</asp:Content>
