<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OnlineImageLibrary.Domain.Entities.Image>" %>

<script type="text/javascript">

    function onComplete(result) {          
        if (result && result.success) {
            if (selectedAsset().id() == 0) {
                var asset = new Asset(result.assetID,
                    $("#ChapterID").val(),
                    $("#Title").val(),
                    $("#Artist").val(),
                    $("#Medium").val(),
                    $("#Date").val(),
                    $("#Size").val(),
                    $("#Credits").val(),
                    $("#Description").val(),                       
                    $("#Theme").val(),
                    $("#Period").val(),
                    $("#Origin").val(),
                    $("#Collection").val(),
                    $("#IsNew").val(),
                    $("#Figure").val(),
                    $("#Keywords").val());
                asset.chapter(findChapter(asset.chapterID()));
                asset.chapter().assets.push(asset);
                assets.push(asset);
            }
            else {
                selectedAsset().chapterID($("#ChapterID").val());
                selectedAsset().chapter(findChapter($("#ChapterID").val()));
                selectedAsset().title($("#Title").val());
                selectedAsset().artist($("#Artist").val());
                selectedAsset().date($("#Date").val());
                selectedAsset().medium($("#Medium").val());
                selectedAsset().size($("#Size").val());
                selectedAsset().credits($("#Credits").val());
                selectedAsset().description($("#Description").val());
                selectedAsset().theme($("#Theme").val());
                selectedAsset().period($("#Period").val());
                selectedAsset().origin($("#Origin").val());
                selectedAsset().collection($("#Collection").val());
                selectedAsset().isnew($("#IsNew").val());
                selectedAsset().figure($("#Figure").val());
                selectedAsset().keywords($("#Keywords").val());
            }
            findAssets("chapter", $("#ChapterID").val());
            $("#edit-asset").dialog("close")
        }
        else {
            alert("There was a problem saving this asset.  Please try again, and notify an administrator if the problem persists.");
        }
    }

    function getAssetMetadata() {
        var o = {
            ImageID: $("#ImageID").val(),
            ChapterID: $("#ChapterID").val(),
            Title: $("#Title").val(),
            Artist: $("#Artist").val(),
            Date: $("#Date").val(),
            Medium: $("#Medium").val(),
            Size: $("#Size").val(),
            Credits: $("#Credits").val(),
            Description: $("#Description").val(),
            Theme: $("#Theme").val(),
            Period: $("#Period").val(),
            Origin: $("#Origin").val(),
            Collection: $("#Collection").val(),
            IsNew: $("#IsNew").val(),
            Figure: $("#Figure").val(),
            Keywords: $("#Keywords").val()
        };
        return o;
    }

</script>

<div id="image-detail" title=""></div>

<% using (Html.BeginForm("Save", "Asset")) 
    { 
        %>
        <%= Html.EditorForModel() %>  
        <div class="editor-label">
            <label>Image</label>
        </div>
        <object id="AssetUploader" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="100%" height="100" codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab">
            <param name="movie" value="/Media/Flash/AssetUploader.swf" />
            <param name="quality" value="high" />
            <param name="allowScriptAccess" value="always" />
            <param name="wmode" value="transparent" />
            <embed src="/Media/Flash/AssetUploader.swf" quality="high" width="100%" wmode="transparent" height="100" name="AssetUploader" align="middle" play="true" loop="false" quality="high" allowScriptAccess="always" type="application/x-shockwave-flash" pluginspage="http://www.adobe.com/go/getflashplayer"></embed>
        </object>        
        <%
    } 
%>


    
