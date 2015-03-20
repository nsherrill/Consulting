<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
        Inherits="System.Web.Mvc.ViewPage<Common.TipshareResults>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tipshare
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tipshare</h2>

<%using (Html.BeginForm())
  { 
      bool useModel = Model!=null;
      %>
    <center>
        <table>
            <tr><td style="padding:5px 1px 5px 5px" align="right">Start Date: </td>
                <td style="padding:5px 5px 5px 1px"><%=Html.TextBox("startDate", 
                       (useModel ? Model.StartDate.ToShortDateString()
                             : Common.TipshareResults.GetLastThursday().ToShortDateString()))%></td>
                <td style="padding:5px 1px 5px 5px" rowspan="2">Store: </td>
                <td style="padding:5px 5px 5px 1px" rowspan="2"><%=Html.DropDownList(
                        "store", Common.TipshareResults.GetStoreList(
                             useModel?Common.TipshareResults.GetIndexOf(Model.Store):0)) %></td></tr>
            <tr><td style="padding:5px 1px 5px 5px" align="right">End Date: </td>
                <td style="padding:5px 5px 5px 1px"><%=Html.TextBox("endDate",
                       (useModel ? Model.EndDate.ToShortDateString()
                             : DateTime.Now.ToShortDateString())) %></td></tr>
            <tr><td colspan="4" align="center" style="padding:5px 5px 5px 5px"><input type="submit" value="Search"/></td></tr>
        </table>
        <label style="color:Red;"><%=useModel && !string.IsNullOrEmpty(Model.ErrorString)? Model.ErrorString: "" %></label>
        <br /><br />
        <label style="font-size:18px;" >Results</label>
        <table style="border-color:#69523b; border-width:2px; border-style:solid; width:300px;">
            <tr><td align="center" style="padding:5px 5px 5px 5px"><b>Store</b></td>
                <td align="center" style="padding:5px 5px 5px 5px"><b>Date Saved</b></td></tr>
            <%if (Model != null && Model.Results != null && Model.Results.Count > 0)
              {
                  foreach (Common.TSR result in Model.Results)
                  {%>          
                    <tr><td align="center" style="padding:5px 5px 5px 5px"><%=result.Storename%></td>
                        <td align="center" style="padding:5px 5px 5px 5px"><%=result.SaveDate.ToString("g")%></td></tr>
            <%  }
              }
              else
              { %>
                    <tr><td align="center" style="padding:5px 5px 5px 5px">None</td></tr>
              <%} %>
        </table>
    </center>
<%} %>
</asp:Content>
