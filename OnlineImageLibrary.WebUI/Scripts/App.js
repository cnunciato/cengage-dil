var user;
var books;
var chapters;
var assets;
var filteredAssets;
var artists;
var media;
var themes;
var periods;
var origins;
var collections;

var _booksLoaded = false;
var _chaptersLoaded = false;
var _assetsLoaded = false;
var _artistsLoaded = false;
var _mediaLoaded = false;
var _themesLoaded = false;
var _periodsLoaded = false;
var _originsLoaded = false;
var _collectionsLoaded = false;
var _loadTimer;

var thumbPath = "/asset/thumb";
var originalPath = "/media/images";
var selectedAsset = ko.observable(assetPlaceholder());
var selectionCount = 0;
var selectionsInfo = ko.observable("");

function Book(id, title) {
    this.id = id;
    this.title = title;
    this.chapters = [];
};

function Chapter(id, bookID, title) {
    this.id = id;
    this.bookID = bookID;
    this.book = null;
    this.assets = ko.observableArray([]);
    this.title = title;
};

function Asset(id, chapterID, title, artist, medium, date, size, credits, description, theme, period, origin, collection, isnew, figure, keywords) {
    this.id = ko.observable(id);
    this.chapterID = ko.observable(chapterID);
    this.chapter = ko.observable(null);
    this.title = ko.observable(title || "");
    this.artist = ko.observable(artist || "");
    this.medium = ko.observable(medium || "");
    this.date = ko.observable(date || "");
    this.size = ko.observable(size || "");
    this.credits = ko.observable(credits || "");
    this.description = ko.observable(description || "");
    this.selected = ko.observable(false);
    this.theme = ko.observable(theme || "");
    this.period = ko.observable(period || "");
    this.origin = ko.observable(origin || "");
    this.collection = ko.observable(collection || "");
    this.isnew = ko.observable(isnew || false);
    this.figure = ko.observable(figure || "");
    this.keywords = ko.observable(keywords || "");
};

function findBook(id) {
    for (var i = 0; i < books.length; i++) {
        if (books[i].id.toString() == id.toString())
            return books[i];
    }
}

function findChapter(id) {
    for (var i = 0; i < chapters().length; i++) {
        if (chapters()[i].id.toString() == id.toString())
            return chapters()[i];
    }
}

$(document).ready(function() {
    clearForms();
    setBehaviors();
    loadData();

    ko.applyBindings(books);
    ko.applyBindings(chapters);
    ko.applyBindings(assets);
    ko.applyBindings(filteredAssets);
    ko.applyBindings(artists);
    ko.applyBindings(media);
    ko.applyBindings(selectedAsset);
});

function clearForms() {
    $(":text").val("");
    $(":text, :checkbox").attr("autocomplete", "off");
    $("#ChapterID").replaceWith("<select id='ChapterID' />");
}

function loadData() {

    books = [];
    chapters = ko.observableArray([]);
    assets = ko.observableArray([]);
    filteredAssets = ko.observableArray([]);
    artists = ko.observableArray([]);
    media = ko.observableArray([]);
    themes = ko.observableArray([]);
    periods = ko.observableArray([]);
    origins = ko.observableArray([]);
    collections = ko.observableArray([]);

    loadBooks();
    loadArtists();
    loadChapters();
    loadImages();
    loadMedia();
    loadThemes();
    loadPeriods();
    loadOrigins();
    loadCollections();

    _loadTimer = setInterval(function () {
        if (_booksLoaded && _chaptersLoaded && _assetsLoaded && _artistsLoaded && _mediaLoaded && _themesLoaded && _periodsLoaded && _originsLoaded && _collectionsLoaded) {
            clearInterval(_loadTimer);
            consolidate();
            filteredAssets(chapters()[0].assets());
        }
    }, 100);
}

function loadBooks() {
    $.getJSON("/Data/Books", null, function(data) {
        $.each(data, function(i, el) {
            books.push(new Book(el.BookID, el.Title));
        });
        _booksLoaded = true;
    });
}

function loadChapters() {
    $.getJSON("/Data/Chapters", null, function(data) {
        $.each(data, function(i, el) {
        chapters.push(new Chapter(el.ChapterID, el.BookID, el.ChapterID.toString() + ": " + el.Title));
        });
        _chaptersLoaded = true;
    });
}

function loadImages() {
    $.getJSON("/Data/Images", null, function(data) {
        $.each(data, function(i, el) {
            assets.push(new Asset(el.ImageID, el.ChapterID, el.Title, el.Artist, el.Medium, el.Date, el.Size, el.Credits, el.Description, el.Theme, el.Period, el.Origin, el.Description, el.IsNew, el.Figure, el.Keywords));
        });
        _assetsLoaded = true;
    });
}

function loadArtists() {
    $.getJSON("/Data/Artists", null, function(data) {
        $.each(data, function(i, el) {
            artists.push(el || "");
        });
        _artistsLoaded = true;
    });
}

function loadMedia() {
    $.getJSON("/Data/Media", null, function(data) {
        $.each(data, function(i, el) {
            media.push(el);
        });
        _mediaLoaded = true;
    });
}

function loadThemes() {
    $.getJSON("/Data/Themes", null, function (data) {
        $.each(data, function (i, el) {
            themes.push(el || "");
        });
        _themesLoaded = true;
    });
}

function loadPeriods() {
    $.getJSON("/Data/Periods", null, function (data) {
        $.each(data, function (i, el) {
            periods.push(el || "");
        });
        _periodsLoaded = true;
    });
}

function loadOrigins() {
    $.getJSON("/Data/Origins", null, function (data) {
        $.each(data, function (i, el) {
            origins.push(el || "");
        });
        _originsLoaded = true;
    });
}

function loadCollections() {
    $.getJSON("/Data/Collections", null, function (data) {
        $.each(data, function (i, el) {
            collections.push(el || "");
        });
        _collectionsLoaded = true;
    });
}     

function setBehaviors() {

    // UI tweaks
    $($(".editor-label")[0]).find("label").text("Chapter");
    $("[for='IsNew']").insertAfter($("#IsNew").css({ width: 20 })).text("New to collection").css({ fontWeight: "normal" })

    var p = $("#Description").parent();
    $("#Description").remove();
    $("<textarea></textarea").attr("id", "Description").attr("name", "Description").appendTo(p);
    
    $("#login").click(function() {
        $("#auth").find(".feedback").hide();
        $("#auth").dialog({ modal: true, closeOnEscape: true, buttons: { "Cancel": function() { $(this).dialog("close"); }, "Submit": function() {
            $.post("/Authentication/Login", { password: $("#auth .password").val() }, function(data) {
                if (data && data.authenticated) {
                    token = data.token;
                    $("#auth").dialog("close");
                }
                else {
                    $("#auth").find(".feedback").show().find(".message").text("Sorry, try again.");
                }
            });
        } 
        }
        });
        return false;
    });

    $("#upload").click(function() {
        selectedAsset(assetPlaceholder());
        $("#edit-asset").find(".feedback").hide();
        $("#edit-asset input:text input:hidden").val("");
        $("#edit-asset").dialog({ modal: true, width: 885, height: 700, top: 0, closeOnEscape: true, resizable: false, draggable: false });
        $("#edit-asset form").scrollTop(0);
        return false;
    });

    $("#browse-types").change(function() {
        if ($("#browse-types").val() == "chapter") {
            $("#browse-options").attr("data-bind", "options: chapters, optionsText: 'title', optionsValue: 'id'");
            ko.applyBindings(chapters);
        }
        else if ($("#browse-types").val() == "artist") {
            $("#browse-options").attr("data-bind", "options: artists");
            ko.applyBindings(artists);
        }
        else if ($("#browse-types").val() == "medium") {
            $("#browse-options").attr("data-bind", "options: media");
            ko.applyBindings(media);
        }
        else if ($("#browse-types").val() == "theme") {
            $("#browse-options").attr("data-bind", "options: themes");
            ko.applyBindings(media);
        }
        else if ($("#browse-types").val() == "period") {
            $("#browse-options").attr("data-bind", "options: periods");
            ko.applyBindings(media);
        }
        else if ($("#browse-types").val() == "origin") {
            $("#browse-options").attr("data-bind", "options: origins");
            ko.applyBindings(media);
        }
        else if ($("#browse-types").val() == "collection") {
            $("#browse-options").attr("data-bind", "options: collections");
            ko.applyBindings(media);
        }
        findAssets($("#browse-types").val(), $("#browse-options").val());
    });
    
    $("#browse-options").change(function() {
        findAssets($("#browse-types").val(), $("#browse-options").val());
    });

    $("#export-ppt").click(function() {
        var ids = assetIDs(selectedAssets());
        
        if (ids.length > 0) {
            window.location.href = "/Asset/Pptx?ids=" + ids.join("&ids=");
        }
        else {
            alert("Please make a selection.");
        }
    });

    $("#export-zip").click(function() {
        var ids = assetIDs(selectedAssets());

        if (ids.length > 0) {
            window.location.href = "/Asset/Zip?ids=" + ids.join("&ids=");
        }
        else {
            alert("Please make a selection.");
        }
    });

    $(".asset").live("click", function () {
        var asset = $(this).data("asset");
        asset.selected(!asset.selected());

        selectionCount = 0;
        selectionsInfo("");

        for (var i = 0; i < assets().length; i++) {
            if (assets()[i].selected()) {
                selectionCount++;
                selectionsInfo(selectionCount.toString() + (selectionCount == 1 ? " item" : " items") + " selected");
            }
        }
    });

    $(".asset").live("dblclick", function() {
        var asset = $(this).data("asset");
        selectedAsset(asset);

        var zoom = $("#image-detail");
        var a = $("<a></a>").attr("href", "/Media/Images/" + asset.id() + ".jpg");
        var img = $("<img />").attr("src", "/Asset/Thumb?id=" + asset.id() + "&width=480").attr("width", "480");

        img.appendTo(a);
        zoom.children().remove();
        zoom.append(a);

        a.jqzoom({
            position: "left",
          
            title: false
        });

        $("#edit-asset").dialog({ modal: true, closeOnEscape: true, width: 885, height: 700, top: 0, resizable: false, draggable: false });
        $("#edit-asset form").scrollTop(0);
    });

    $(".asset").live("hover", function(event) { if (event.type == "mouseenter") { $(this).find(".delete-button").css({visibility: "visible"}); } else if (event.type == "mouseleave") { $(this).find(".delete-button").css({visibility: "hidden"}); } });

    $("#search-term").keyup(function(event) {
        findAssets("search", $(this).val());
    });

    $(".delete-button").live("click", function (event) { event.stopImmediatePropagation(); del($(this).parent().data("asset")); });

    $("#ImageID").attr("data-bind", "value: selectedAsset().id");
    $("#ChapterID").attr("data-bind", "options: chapters, optionsText: 'title', optionsValue: 'id'");
    $("#Title").attr("data-bind", "value: selectedAsset().title");
    $("#Artist").attr("data-bind", "value: selectedAsset().artist");
    $("#Date").attr("data-bind", "value: selectedAsset().date");
    $("#Medium").attr("data-bind", "value: selectedAsset().medium");
    $("#Size").attr("data-bind", "value: selectedAsset().size");
    $("#Credits").attr("data-bind", "value: selectedAsset().credits");
    $("#Description").attr("data-bind", "value: selectedAsset().description");
    $("#Theme").attr("data-bind", "value: selectedAsset().theme");
    $("#Period").attr("data-bind", "value: selectedAsset().period");
    $("#Origin").attr("data-bind", "value: selectedAsset().origin");
    $("#Collection").attr("data-bind", "value: selectedAsset().collection");
    $("#IsNew").attr("data-bind", "checked: selectedAsset().isnew");
    $("#Figure").attr("data-bind", "value: selectedAsset().figure");
    $("#Keywords").attr("data-bind", "value: selectedAsset().keywords");
    $("#export li").attr("data-bind", "css: {opacity: filteredAssets().length == 0 ? 0.5 : 1}");
    $("#browse-options").attr("data-bind", "options: chapters, optionsText: 'title', optionsValue: 'id'");
    $("#image-detail").attr("data-bind", "visible: selectedAsset().id() != 0");
    $("#by-artist").attr("data-bind", "visible: assets().length > 0");
    $("#by-medium").attr("data-bind", "visible: assets().length > 0");
}

function assets_afterAdd(el, i, o) {
    $(el).data('asset', o);
}

function assets_beforeRemove(el, i, o) {
    $(el).remove();
}

function del(a) {
    if (confirm("Are you sure you want to delete this image?")) {
        $.post("/Asset/Delete", assetPlaceholder(a), function (success) {
            if (success) {
                a.chapter().assets.remove(a);
                assets.remove(a);
                $("#assets li").each(function (i, el) {
                    var b = $(el).data("asset");
                    if (b && b == a) {
                        $(this).remove();
                    }
                });
            }
            else {
                alert("There was a problem deleting this image.  Please notify an administrator.");
            }
        });
    }   
}

function assetPlaceholder(a) {
    if (a) {
        return {
            ImageID: a.id(),
            ChapterID: a.chapterID(),
            Title: a.title(),
            Artist: a.artist(),
            Medium: a.medium(),
            Date: a.date(),
            Size: a.size(),
            Credits: a.credits(),
            Description: a.description(),
            Theme: a.theme(),
            Period: a.period(),
            Origin: a.origin(),
            Collection: a.collection(),
            IsNew: a.isnew(),
            Figure: a.figure(),
            Keywords: a.keywords()
        }
    }
    return new Asset(0, "", "", "", "", "", "", "", "", "", "", "", "", false, "", "");
}

function assetIDs(assets) {
    var ids = new Array(assets.length);

    for (var i = 0; i < ids.length; i++)
        ids[i] = assets[i].id();

    return ids;
}

function selectedAssets() {
    var results = [];
    $.each(assets(), function(i, el) {
        if (el.selected())
            results.push(el);
    });
    return results;
}

function consolidate() {
    var i;
    var j;

    for (i = 0; i < chapters().length; i++) {
        chapters()[i].book = findBook(chapters()[i].bookID);
    }

    for (i = 0; i < assets().length; i++) {
        assets()[i].chapter(findChapter(assets()[i].chapterID()));
        assets()[i].chapter().assets.push(assets()[i]);
    }

    for (i = 0; i < books.length; i++) {
        for (var j = 0; j < chapters().length; j++) {
            if (chapters()[j].book == books[i])
                books[i].chapters.push(chapters()[j]);
        }    
    }  
    
    for (i = 0; i < chapters().length; i++) {
        for (var j = 0; j < assets.length; j++) {
            if (assets()[j].chapter == chapters()[i])
                chapters()[i].assets.push(assets[j]);
        }
    }

    for (i = 0; i < assets().length; i++) {
        filteredAssets.push(assets()[i]);
    }
}

function findAssets(filterType, criteria) {
    filteredAssets([]);

    if (filterType == "chapter") {
        $.each(assets(), function(j, asset) {
            if (asset.chapter().id.toString() == criteria.toString()) {
                filteredAssets.push(asset);
            }
        });
    }
    else if (filterType == "artist") {
        $.each(assets(), function(j, asset) {
            if (asset.artist() == criteria) {
                filteredAssets.push(asset);
            }
        });
    }
    else if (filterType == "medium") {
        $.each(assets(), function(j, asset) {
            if (asset.medium() == criteria) {
                filteredAssets.push(asset);
            }
        });
    }
    else if (filterType == "search") {
        criteria = criteria.toLowerCase();

        $.each(assets(), function(j, asset) {
            out = true;

            if (asset.title().toLowerCase().indexOf(criteria) != -1) {
                out = false;
            }

            if (asset.artist().toLowerCase().indexOf(criteria) != -1) {
                out = false;
            }

            if (!out)
                filteredAssets.push(asset);
        });
    }
}

