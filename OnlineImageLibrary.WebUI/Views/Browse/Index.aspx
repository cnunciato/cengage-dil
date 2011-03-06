<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="filters">
        <div id="search">
            <h3>Search</h3>
            <input type="text" id="search-term" />
        </div>
        <div id="browse">
            <h3>Browse by</h3>
            <select id="browse-types">
                <option value="chapter" selected="selected">Chapter</option>
                <option id="by-theme" value="theme">Theme</option>
                <option id="by-artist" value="artist">Artist</option>
                <option id="by-medium" value="medium">Medium</option>
                <option id="by-period" value="period">Period</option>
                <option id="by-origin" value="origin">Origin</option>
                <option id="by-collection" value="collection">Collection</option>
                <!--<option id="by-new" value="artist">New</option>-->
            </select>
            <select id="browse-options"></select>
        </div>
        <div id="filter">
            <h3>Show only</h3>
            <div id="filter-types">
                <div class="item"><input type="checkbox" id="filter-images" checked="checked" disabled="disabled" /><label for="filter-images">Images</label></div>
                <div><input type="checkbox" id="filter-videos" disabled="disabled" /><label for="filter-videos">Videos</label></div>
                <div><input type="checkbox" id="filter-maps" disabled="disabled" /><label for="filter-maps">Maps</label></div>
                <div><input type="checkbox" id="filter-presentations" disabled="disabled" /><label for="filter-presentations">Presentations</label></div>
            </div>
        </div>
    </div>
    <div id="results">
        <div id="export">
            <div id="selections">
                <span id="item-count" data-bind="text: selectionsInfo"></span>
            </div>
            <ul>
                <li id="export-ppt">
                    <span class="label">Export Presentation</span>
                </li>
                <li id="export-zip">
                    <span class="label">Export ZIP File</span>
                </li>
            </ul>
        </div>
        <div id="feedback" class="info">Here is an interesting message.</div>
        <ol id="assets" data-bind="template: {name: 'assetTemplate', foreach: filteredAssets, beforeRemove: assets_beforeRemove, afterAdd: assets_afterAdd}"></ol>
    </div>

    <script id="assetTemplate" type="text/html">
        <li class="asset" data-bind="css: {selected: selected()}">
            <div class="delete-button" title="Delete this item"></div>
            <img src="/asset/thumb/?id=${id}&height=100&width=100" width="100" />
            <div class="title">${title}</div>
            <div class="artist">${artist}</div>
            <input type="checkbox" data-bind="checked: selected()" title="Select/deselect this item" />
        </li>
    </script>

</asp:Content>
