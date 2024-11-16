var bannerMaxIndex = 0;
var bannerIndex = 1;
var playBanner = true;
var istodaysDeal = false;

function LoadScroll() {
    $('.hscroll').each(function() {
        var lnkprev = "#" + $(this).find(".prev").attr("id");
        var lnknext = "#" + $(this).find(".next").attr("id");
        var scroll = $(this).find(".scrolpanel");
        scroll.cycle({
            fx: 'scrollHorz',
            speed: 500,
            timeout: 0,
            prev: lnkprev,
            next: lnknext
        });
    });
}
$(document).ready(function() {
    //product zoom images
    InitProductZoomPop();
    ReviewHOver();
    LoadScroll();
    LoadFlashBanner();
    InitAllCategoryMenu();
    InitOfferZoneMenu();
    InitColorSize();
    InitProductColorSize();
    InitStaticDialog();
    showCartCount();
    InitProductSearch();
    InitCheckBoxDiv();
    InitRating();
    InitWishlist();
    InitReviewUseful();
    InitScrollTop();
    loadProductExtraImage();
    //category listing
    var _ismouseOnSubMenu = false;
    $(".catmenu").hover(function() {
        var left = $(this).parent().parent().position().left;
        var top = $(this).parent().position().top - 40;
        if ($(".category-list").css("position") == "absolute") {
            top = $(this).parent().position().top + 100;
        }
        var submenu = null;
        var menu = $(this).text();
        $(".subcatmenu").hide();
        $(".subcatmenu").each(function() {
            if ($(this).attr("target") == menu) {
                submenu = $(this);
            }
        });
        if (submenu == null) return;
        left += $(this).width() + 25;

        submenu.css("left", left);
        submenu.css("top", top);
        submenu.show();
        $(".catmenu").removeClass("catmenu-hover");
        $(this).addClass("catmenu-hover");
    }, function(e) {
        if (e.pageY < 10) $(".subcatmenu").hide();
    });
    $(".subcatmenu").hover(function() {
        _ismouseOnSubMenu = true;
    }, function(e) {
        if (e.pageY > 50) $(this).hide();
    });
    $(window).click(function() {
        $(".subcatmenu").hide();
    });

    $(".back-to-top").click(function() {
        $('html, body').animate({ scrollTop: 0 }, 1000);
    });
    $(document).click(function(e) {
        $(".hideonclick").each(function() {
            var left = $(this).position().left - 20;
            var w = $(this).width() + 20;
            var h = $(this).height() + 20;
            var top = $(this).position().top - 20;
            if (e.pageX > (left + w) || e.pageX < left) {
                $(this).hide();
            }
            if (e.pageY < top || e.pageY > top + h) {
                $(this).hide();
            }
        });
    });
    function InitProductZoomPop() {
        $(".imgextra").click(function() {
            loadProdZoomControl();
        });
        $(".prodpop-thumbimg").live("click", function() {
            var src = $(this).attr("src").replace("thumb", "large");
            $("#prodpop-img").attr("src", src);
        });
    }
    function loadProdZoomControl() {
        if ($('#prodpop-window').length == 0) {
            var productname = $(".productname").text();
            var extraImages = "";
            $(".imgextra").each(function() {
                var thumbSrc = $(this).attr("src");
                extraImages += "<div><img src='" + thumbSrc + "' width='100' class='hand prodpop-thumbimg'/></div>";
            });
            var largeSrc = $(".productimage").attr("src").replace("detail", "large");
            var html = "<div id='prodpop-window' title='" + productname + "'>" +
                   "<table cellspacing='5' width='100%'>" +
                   "<tr><td align='center'><div style='width:800px;overflow:scroll;'><img src='" + largeSrc + "' id='prodpop-img'/></div></td>" +
                        "<td class='valign'><div class='prodpop-thumbs'>" + extraImages + "</div></td>" +
                   "</tr></table></div>";
            $(html).appendTo('body');
        }
        $("#prodpop-window").dialog({
            modal: true,
            width: 950,
            height: $(window).height() * 0.98
        });
    }
    function ReviewHOver() {
        $(".review-row").hover(function() {
            $(this).find(".review-bar").show();
        },
            function() {
                $(this).find(".review-bar").hide();
            }
        );
    }

    function InitAllCategoryMenu() {
        $(".topmenu-allcategories").click(function() {
            $(".category-list").css("position", "absolute");
            $(".category-list").css("left", $(this).position().left);
            $(".category-list").css("top", $(this).position().top + 32);
            $(".category-list").show();
        });
    }
});
function rotateBanner() {
    var mainBanner = $(".mainbanner");
    if (mainBanner.length == 0) return;
    var bid = mainBanner.attr("bid");
    $(".imgmainbanner").append("<img src='upload/banner/large/" + bid + "-1.jpg' id='imgbanner-1' class='imgbanner'/>");
    var index = 1;
    mainBanner.find("td").each(function() {
        if (index > 1) {
            $(".imgmainbanner").append("<img src='upload/banner/large/" + bid + "-" + index + ".jpg' id='imgbanner-" + index + "' class='imgbanner hand'/>");
        }
        index++;
        bannerMaxIndex++;
    });
    $("#imgbanner-1").fadeIn(5000, function() {
        bannerSlide(bid, 1);
    });
    $(".imgbanner").click(function() {
        var index = $(this).attr("id").split('-')[1];
        var url = $(".banner" + index).find("a").attr("href");
        if (url != "#") window.location = url;
    });
    $(".banner").hover(function() {
        var bannerIndex = $(this).attr("index");
        $(".imgbanner").hide();
        $("#imgbanner-" + bannerIndex).show();
        $(".banner").removeClass("activebanner").addClass("bannertitle");
        $(".banner" + bannerIndex).removeClass("bannertitle").addClass("activebanner");
    });
}
function bannerSlide(bid) {
    if (!playBanner) return;
    if (parseInt(bannerIndex) > parseInt(bannerMaxIndex)) bannerIndex = 1;
    $(".imgbanner").hide();
    $(".banner").removeClass("activebanner").addClass("bannertitle");
    $(".banner"+bannerIndex).removeClass("bannertitle").addClass("activebanner");
    $("#imgbanner-" + bannerIndex).fadeIn(1000, function() {
        bannerIndex++;
        setTimeout("bannerSlide(" + bid + "," + bannerIndex + ")", 3000);
    });
}

function InitProductList() {
    $(window).scroll(function() {
        if ($(".nodata").length > 0) return;
        if (_productListIsLoadMore) return;
        if ($(window).scrollTop() == $(document).height() - $(window).height() && _productListLoading == false) {
            bindProductList(true, false);
        }
    });
    $(".loadmore").live("click", function() {
        $(this).remove();
        bindProductList(true, false);
    });
    openCloseSpec();
    initCompareProducts();
    loadCompareItems();
}
function InitAdvancedFilter() {
    $(".chk-advfilter").click(function() {
        bindProductList(false,true);
    });
}
function sortProductList() {
    bindProductList(false, true);
}
function bindProductList(isappend, isfade)
{
    if (_productListLoading) return;
    _productListLoading = true;
    if (!isappend) _currentPageNo = 1;
    var sb = $("#ddlsortby").val();
    if (isfade) {
        $(".tblproductlist").fadeTo("fast", 0.5);
        $(".tbladvfilter").fadeTo("fast", 0.5);
    }
    $(".tblproductlist").append("<tr class='loading'><td align='center' style='padding:20px;'><img src='../images/loader.gif'></td></tr>");
    var URL = "../product.ashx?m=product-list&";
    if ($(".tblproductlist").attr("search") == "keyword") {
        URL += "k=" + $(".tblproductlist").attr("k");
    }
    else {
        URL += "cid=" + $(".tblproductlist").attr("cid") + "&scid=" + $(".tblproductlist").attr("scid") + "&sscid=" + $(".tblproductlist").attr("sscid");
    }
    var specIds = "";
    var priceRange = "";
    var discountRange = "";
    var size = "";
    var color = "";
    var prevSpecId = 0;
    $(".chk-spec-advfilter").each(function() {
        if ($(this).is(":checked")) {
            var specId = $(this).attr("id").replace("ch_advfilter-spec-", "");
            var arr = specId.split('-');
            var sId = arr[0];
            var valId = arr[1];

            if (sId == prevSpecId) {
                specIds = specIds + "," + valId;
            }
            else {
                if (specIds == "") {
                    specIds = sId + "=" + valId;
                }
                else {
                    specIds += "~" + sId + "=" + valId;
                }
            }
            prevSpecId = sId;
        }
    });
    $(".chk-price-advfilter").each(function() {
        if ($(this).is(":checked")) {
            var prange = $(this).attr("id").replace("ch_advfilter-price-", "");
            if (priceRange == "") {
                priceRange = prange;
            }
            else {
                priceRange += "," + prange;
            }
        }
    });
    $(".chk-discount-advfilter").each(function() {
        if ($(this).is(":checked")) {
            var drange = $(this).attr("id").replace("ch_advfilter-discount-", "");
            if (discountRange == "") {
                discountRange = drange;
            }
            else {
                discountRange += "," + drange;
            }
        }
    });
    $(".chk-size-advfilter").each(function() {
        if ($(this).is(":checked")) {
            var s = $(this).attr("size");
            if (size == "") {
                size = s;
            }
            else {
                size += "," + s;
            }
        }
    });
    $(".chkdiv").each(function() {
        if ($(this).attr("sel") == "true") {
            var c = $(this).attr("c");
            if (color == "") {
                color = c;
            }
            else {
                color += "," + c;
            }
        }
    });
    URL += "&specids=" + specIds + "&pn=" + _currentPageNo + "&sb=" + sb + "&pr=" + priceRange + "&dr=" + discountRange + "&s=" + size + "&c=" + color;
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        success: function(html) {
            var nodataMsg = "<tr><td class='nodata'>No more data!</td></tr>";
            var index = html.indexOf('|');
            var recordCount = html.substring(0, index);
            html = html.substring(index + 1, html.length - index + 2);
            if (ConvertToInt(recordCount) == 0) {
                $(".recordcount").text("No data found!");
            }
            else {
                $(".recordcount").text("Total " + recordCount + " products found.");
            }
            if (isappend) {
                if (html != "") {
                    html = "<tr><td class='page-no'>Page : " + (_currentPageNo) + "</td></tr>" + html;
                }
                else {
                    html = nodataMsg;
                }
                $(".tblproductlist").append(html);
            }
            else {
                html = "<tr><td id='prodlist-page'" + _currentPageNo + ">" + html + "</td></tr>";
                $(".tblproductlist").html(html);
            }
            $(".tblproductlist").find(".loading").remove();
            _productListLoading = false;
            if (isfade) {
                $(".tblproductlist").fadeTo("fast", 1);
                $(".tbladvfilter").fadeTo("fast", 1);
            }
            if (_currentPageNo >= 4 || _productListIsLoadMore) {
                if (html != nodataMsg) {
                    $(".tblproductlist").append("<tr><td class='loadmore'>Load more...</td></tr>");
                    _productListIsLoadMore = true;
                }
            }
            _currentPageNo++;
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
            _productListLoading = false;
            if (isfade) {
                $(".tblproductlist").fadeTo("fast", 1);
                $(".tbladvfilter").fadeTo("fast", 1);
            }
        }
    });
}
function openCloseSpec() {
    $(".advfilter-title").click(function() {
        if ($(this).css("background-image").indexOf("minus") > 0) {
            $(this).css("background-image", $(this).css("background-image").replace("minus", "plus"));
            $(this).closest("tr").next().hide();
        }
        else {
            $(this).css("background-image", $(this).css("background-image").replace("plus", "minus"));
            $(this).closest("tr").next().show("slow");
        }
    });
    $(".specfilter").each(function() {
        if ($(this).height() > 150) {
            $(this).css("height", "150px");
        }
    });
}
function initCompareProducts() {
    $(".compare").live("click", function() {
        var compareItems = "";
        var tbl = $(this).closest(".product");
        var pn = tbl.find(".prod-list-name").text();
        var price = tbl.find(".price").text();
        var pid = $(this).attr("id").replace("com-", "");
        if (pn.length > 30) pn = pn + "...";
        if ($(this).is(":checked")) {
            addCompareItem(pid, pn, price, price);
        }
        else {
            removeCompare(pid);
        }
    });
    $(".compare-del").live("click", function() {
        var pid = $(this).attr("pid");
        removeCompare(pid);
    });
    $(".close-commpare").live("click", function() {
        $.cookie("show-come", false);
        $(".compare-panel").hide("slow");
    });
    $(".btncompare").live("click", function() {
        var compareItems = $.cookie("com");
        var arrcompareItems = compareItems.split('~');
        if (arrcompareItems.length < 2) {
            alert("You required atleast two products to compare!");
            return;
        }
        else {
            var pids = "";
            for (i = 0; i < arrcompareItems.length; i++) {
                var arr = arrcompareItems[i].split('`');
                var pid = arr[0];
                if (i == 0) {
                    pids = pid;
                }
                else {
                    pids += "," + pid;
                }
            }
            window.location = "../product/compare-products.aspx?pids=" + pids;
        }
    });
}
function removeCompare(productId) {
    var compareItems = $.cookie("com");
    var arrcompareItems = compareItems.split('~');
    var newCompareItems = "";
    for (i = 0; i < arrcompareItems.length; i++) {
        var arr = arrcompareItems[i].split('`');
        var pid = arr[0];
        if (pid != productId) {
            if (newCompareItems == "") {
                newCompareItems = arrcompareItems[i];
            }
            else {
                newCompareItems += "~" + arrcompareItems[i];
            }    
        }
    }
    $.cookie("com", newCompareItems);
    bindCompareItems(newCompareItems);
}
function addCompareItem(pid, pn, sprice, price) {
    var compareItems = $.cookie("com");
    var compareItem = pid + "`" + pn + "`" + sprice + "`" + price;
    
    if (ConvertToString(compareItems) != "") {
        var arrcom = compareItems.split('~');
        for (i = 0; i < arrcom.length; i++) {
            var arr = arrcom[i].split('`');
            var oldpid = arr[0];
            if (oldpid == pid) {
                alert("This product has already been added to your compare list");
                return;
            }
        }

    }
    if (ConvertToString(compareItems) == "" || compareItems.split('~').length < 5) {
        if (ConvertToString(compareItems) == "") {
            compareItems = compareItem;
        }
        else {
            compareItems += "~" + compareItem;
        }
    }
    else {
        var arr = compareItems.split('~');
        compareItems = "";
        for (i = 1; i < arr.length; i++) {
            if (i == 1) {
                compareItems = arr[i];
            }
            else {
                compareItems += "~" + arr[i];
            }
        }
        compareItems += "~" + compareItem;
    }
    $.cookie("com", compareItems);
    bindCompareItems(compareItems);
}
function bindCompareItems(compareItems) {
    if (compareItems == "") {
        $(".compare-panel").html("");
        return;
    }
    var arrcom = compareItems.split('~');
    var html = "";
    for (i = 0; i < arrcom.length; i++) {
        var arr = arrcom[i].split('`');
        var pid = arr[0];
        var pn = arr[1];
        var sprice = arr[2];
        var price = arr[3];
        html += "<div class='compare-item'><div class='left'><div><img src='../upload/product/thumb/" + pid + ".jpg' height='50'/></div>" +
                 "<div>" + pn + "</div><div>" + price + "</div></div><div class='compare-del' pid='"+pid+"'></div></div>";
    }
    html += "<div class='btncompare'>Compare</div>";
    html += "<div class='close-commpare' title='Close this compare panel'></div>";
    $(".compare-panel").html(html);
    $(".compare-panel").show();
    $.cookie("show-come", true);
}
function loadCompareItems() {
    if ($.cookie("show-come") == "true") {
        bindCompareItems(ConvertToString($.cookie("com")));
    }
}
function BindProductWidget(pid) {
    addHistory(pid);
    var h = $.cookie("h");
    var URL = "../product-widget.ashx?pid=" + pid + "&h=" + h;
    $.ajax({
        url: URL,
        type: 'GET',
        async: true,
        success: function(json) {
            var objJson = jQuery.parseJSON(json);
            bindPopulare(objJson[0].data);
            bindHistory(objJson[1].data);
            bindRecommended(objJson[2].data);
            LoadScroll();
        }
    });
}
function bindPopulare(data) {
    var html = "<div class='widget-right'><div><h2 class='widget-right-title'>POPULARE PRODUCTS</h2></div><div class='hscroll'>" +
                "<div class='scrolpanel homecat-scroll-div'>";
    for (i = 0; i < data.length; i++) {
        var productId = data[i].productid;
        var productName = data[i].productname;
        var mrp = data[i].mrp;
        var price = data[i].price;
        
        if (productName.length > 20) productName = productName.substring(0, 20) + "...";
        var url = data[i].url;
        if (i % 5 == 0) html += "<div>";
        
        html += "<div class='widget-right-panel'><div class='widget-right-img'><a href='"+url+"'><img src='../upload/product/thumb/" + productId + ".jpg' style='height:50px'/></a></div>";
        html += "<div class='widget-right-product'>";
        html += "<div><a href='" + url + "' class='brownlink'>" + productName + "</a></div>";
        if (mrp == price || mrp == 0) {
            html += "<div class='widget-pricing'><div class='rs'>Rs.</div><div class='widget-price'>" + price + "</div></div>";
        }
        else {
            html += "<div class='widget-pricing'><div class='rs'>Rs.</div><div class='widget-strike'><strike>" + mrp + "</strike></div><div class='widget-price'>" + price + "</div></div>";
        }
        html += "</div>";
        html += "</div>";
        if ((i + 1) % 5 == 0 || i == data.length-1) html += "</div>";
        
    }
    html += "</div>" +
            "<div style='clear:both;padding-left:75px;'><div class='scroll-left-side prev' id='scroll-populare-left'></div>" +
            "<div class='scroll-right-side next' id='scroll-populare-right'></div></div>" +
            "</div>";

    $(".widget-populare").html(html);
}
function bindHistory(data) {
    var html = "<div class='widget-right'><div><h2 class='widget-right-title'>YOU RECENTLY VIEWED</h2></div><div class='hscroll'>" +
                "<div class='scrolpanel homecat-scroll-div'>";
    for (i = 0; i < data.length; i++) {
        var productId = data[i].productid;
        var productName = data[i].productname;
        var mrp = data[i].mrp;
        var price = data[i].price;

        if (productName.length > 20) productName = productName.substring(0, 20) + "...";
        var url = data[i].url;
        if (i % 3 == 0) html += "<div>";

        html += "<div class='widget-right-panel'><div class='widget-right-img'><a href='" + url + "'><img src='../upload/product/thumb/" + productId + ".jpg' style='height:50px'/></a></div>";
        html += "<div class='widget-right-product'>";
        html += "<div><a href='" + url + "' class='brownlink'>" + productName + "</a></div>";
        if (mrp == price || mrp == 0) {
            html += "<div class='widget-pricing'><div class='rs'>Rs.</div><div class='widget-price'>" + price + "</div></div>";
        }
        else {
            html += "<div class='widget-pricing'><div class='rs'>Rs.</div><div class='widget-strike'><strike>" + mrp + "</strike></div><div class='widget-price'>" + price + "</div></div>";
        }
        html += "</div>";
        html += "</div>";
        if ((i + 1) % 3 == 0 || i == data.length - 1) html += "</div>";

    }
    html += "</div>" +
            "<div style='clear:both;padding-left:75px;'><div class='scroll-left-side prev' id='scroll-history-left'></div>" +
            "<div class='scroll-right-side next' id='scroll-history-right'></div></div>" +
            "</div>";

    $(".widget-history").html(html);
}
function bindRecommended(data) {
    var html = "<div class='widget-right'><div><h2 class='widget-right-title'>RECOMMENDATIONS BASED ON YOUR BROWSING HISTORY</h2></div><div class='hscroll'>" +
                "<div class='scrolpanel recommended-scroll-box'>";
    for (i = 0; i < data.length; i++) {
        var productId = data[i].productid;
        var productName = data[i].productname;
        var mrp = data[i].mrp;
        var price = data[i].price;

        if (productName.length > 20) productName = productName.substring(0, 20) + "...";
        var url = data[i].url;
        if (i % 4 == 0) html += "<div>";

        html += "<div class='widget-horizontal'><div><a href='" + url + "'><img src='../upload/product/thumb/" + productId + ".jpg' style='height:100px'/></a></div>";
        html += "<div style='padding-top:15px'>";
        html += "<div><a href='" + url + "' class='brownlink'>" + productName + "</a></div>";
        if (mrp == price || mrp == 0) {
            html += "<div class='widget-pricing'><div class='rs'>Rs.</div><div class='widget-price'>" + price + "</div></div>";
        }
        else {
            html += "<div class='widget-pricing'><div class='rs'>Rs.</div><div class='widget-strike'><strike>" + mrp + "</strike></div><div class='widget-price'>" + price + "</div></div>";
        }
        html += "</div>";
        html += "</div>";
        if ((i + 1) % 4 == 0 || i == data.length - 1) html += "</div>";

    }
    html += "</div>" +
            "<div style='clear:both;padding-left:350px;'><div class='scroll-left-side prev' id='scroll-recommended-left'></div>" +
            "<div class='scroll-right-side next' id='scroll-recommended-right'></div></div>" +
            "</div>";

    $(".widget-recommended").html(html);
}

function addHistory(pid) {
    var h = $.cookie("h");
    if (h == null) {
        h = pid;
    }
    else {
        var arr = h.split(',');
        for (i = 0; i < arr.length; i++) {
            if (arr[i] == pid) return;
        }
        if (arr.length > 20) {
            for (i = 1; i < arr.length; i++) {
                if (i == 1) {
                    h = arr[i];
                }
                else {
                    h += "," + arr[i];
                }
            }
        }
        h = h + "," + pid;
    }
    $.cookie("h", h);
}
/*Shopping Cart*/
var _showCartPopup = true;
function formatAmount(amt) {
    return amt;
}
$(document).ready(function() {
    $('.buy').live('click', function() {
        var size = "";
        if ($(".product-size").length > 0) {
            $(".product-size").find("div").each(function() {
                if ($(this).attr("sel") == "true") {
                    size = $(this).text();
                }
            });
            if (size == "") {
                alert("Please select size");
                return;
            }
        }
        var otherDetail = "";
        var isstitching = false;
        if ($(".blouse-options").length > 0) {
            if ($("#rbtnstitchedblouse").is(":checked")) {
                if ($("#ddlblousesize").val() == "0") {
                    alert("Please Select Blouse Size");
                    $("#ddlblousesize").focus();
                    return;
                }
                if ($("#ddlblouselength").val() == "0") {
                    alert("Please Select Blouse Length");
                    $("#ddlblouselength").focus();
                    return;
                }
                if ($("#ddlsleeveslength").val() == "0") {
                    alert("Please Select Sleeves Length");
                    $("#ddlsleeveslength").focus();
                    return;
                }
                var blouseSize = $("#ddlblousesize option:selected").text();
                var blouseLength = $("#ddlblouselength option:selected").text();
                var sleevesLength = $("#ddlsleeveslength option:selected").text();
                otherDetail += "Stitched Blouse with Size:" + blouseSize + ",Length=" + blouseLength + ",Sleeves Length=" + sleevesLength;
                isstitching = true;
            }
        }
        $this = $(this);
        var productId = parseInt($this.attr("id"));

        Cart(productId, 1, 1, size, otherDetail, isstitching);
        return false;
    });
    $('.remove-product').live('click', function() {
        if (confirm("Are you sure you want to remove this Product?")) {
            $this = $(this);
            arr = $this.attr("id").split('_');
            productId = arr[1];
            Cart(productId, 0, 3);
        }
    });
    $(".sclose").click(function() {
        ShowCart(false); return false;
    });
    $(".cart-items").click(function() {
        ViewCartDetails();
    });
    $('.cart-change').live('click', function() {
        var objqty = $(this).parent().parent().parent().find(".c-qty");
        UpdateCart(objqty, $(this));
        $(".Changequantity").val(1);
        return false;
    });

    $(".c-qty").live('keydown', function(event) {
        if (event.keyCode == 13) {
            var objchange = $(this).parent().parent().parent().find(".cart-change");
            UpdateCart($(this), objchange);
            $(".Changequantity").val(1);
            return false;
        }
        else {
            $(".Changequantity").val(1);
            return true;
        }

    });

});
function UpdateCart(objqty, objchange) {

    if (objchange.text() == "change") {
        objchange.text("update");
        objqty.show();
        objqty.focus();
    }
    else {
        if (!IsValidNumber(objqty.val())) {
            alert("Invalid quantity");
            return false;
        }
        Cart(objchange.attr("id").replace("cc-", ""), objqty.val(), 2);
        objqty.hide();
        objchange.text("change");
    }
}
function RequestData(URL) {
    var p;
    var isAsc = false;
    $.ajax({
        url: URL,
        type: 'GET',
        async: isAsc,
        success: function(jsonObj) {
            if ((jsonObj + "").indexOf("session expired") > 0) {
                window.location = "../account/userlogin.aspx";
                return;
            }
            else if ((jsonObj + "").indexOf("Error") >= 0) {
                alert("Error : \n\n" + jsonObj);
                jsonObj = "Error occurred!";
            }
            p = jsonObj;
        },
        complete: function() {
        },
        error: function(err, status, jqXHR) {
        }
    });
    return p;
}
function ViewCartDetails() {
    ShowCart(true);
    var d = RequestData("../cart.ashx?pid=0&qty=0&a=4");
    BuildCart(d);
    $(".loader").hide();
}

function Cart(pid, qty, action, size, otherDetail, isstitching) {
    isstitching = ConvertToBool(isstitching);
    $(".loader").show();
    ShowCart(true);
    var color = getSelectedProductColor();
    var productDetail = "";
    if (size != "") productDetail = "Size : " + size;
    if (color != "") productDetail += "Color : <div class='color' style='background:#" + color + "'></div>";
    var d = RequestData("../cart.ashx?pid=" + pid + "&qty=" + qty + "&a=" + action + "&c=" + color + "&s=" + size + "&detail=" + otherDetail + "&stitch=" + isstitching);
    BuildCart(d);
    $(".loader").hide();
}
function getSelectedProductSize() {
    var size = "";
    $(".product-size").find("div").each(function() {
        if ($(this).attr("sel") == "true") {
            size = $(this).text();
        }
    });
    return size;
}
function getSelectedProductColor() {
    var color = "";
    $(".product-color").find("div").each(function() {
        if ($(this).attr("sel") == "true") {
            color = $(this).attr("c");
        }
    });
    return color;
}
function ShowCartDialog(modal) {
    $("#overlay_s").show();
    $("#dialog_s").fadeIn(300);

    if (modal) {
        $("#overlay_s").unbind("click");
    }
    else {
        $("#overlay_s").click(function(e) {
            HideDialog();
        });
    }
}

function HideCartDialog() {
    $("#overlay_s").hide();
    $("#dialog_s").fadeOut(300);
}
function ShowCart(s) {
    if (!_showCartPopup) return;
    var div = $("#dialog_s");

    if (s) {
        ShowCartDialog();

        div.css("left", ($(window).width() - div.width()) / 2);
        div.css("top", ($(window).height() - div.height()) / 2);
    }
    else {
        HideCartDialog();
    }
}
function CurrencyCode() {
    return "Rs.";
}
function BuildCart(d) {
    var objCartDiv = $("#cart-detail");
    var html = '';
    var grossTotal = 0;
    var itemCount = d.length;
    var counts = ConvertToInt(d[0].counts);
    var style = "";

    if (itemCount > 0) {
        html = '<tr class="sc-head"><td class="sc-hf">Item</td><td class="sc-h">Description</td><td class="sc-h">Price</td><td class="sc-h" width="50">Quantity</td>'+
               '<td class="sc-h">Sub Total</td>' +
               '<td class="sc-hl">&nbsp;</td></tr>';
    }
    for (i = 0; i < d.length; i++) {
        var pid = d[i].ProductId;
        var pname = d[i].ProductName;
        var size = d[i].Size;
        var color = d[i].Color;
        var qty = d[i].Quantity;
        var aprice = d[i].ActualPrice;
        var price = d[i].Price;
        var discount = d[i].Discountprecentage;
        var discountAmt = d[i].DiscountAmt;
        var issitch = ConvertToBool(d[i].IsStitch);
        var stitchingCost = d[i].StitchingCost;
        var stotal = d[i].SubTotal;
        var otherDetail = d[i].OtherDetail;
        grossTotal += ConvertToDouble(stotal);
        var url = "productdetail.aspx?pid=" + pid;
        if (size != "") pname += "<br/>Size : " + size;
        if (color != "") pname += "<br/>Color : <div class='color' style='background-color:#" + color + "'></div>";
        if (otherDetail != "") pname += "<br/>" + otherDetail;
        if (issitch) {
            pname += "<br/>Stitching Cost : " + stitchingCost;
        }
        html += '<tr><td class="sc-d"><a href="' + url + '"><img src="../upload/product/thumb/' + pid + '.jpg" height="50"></a></td>' +
                        '<td class="sc-d">' + pname + '</td><td class="sc-d">' + CurrencyCode() + ' ' + formatAmount(price) + '</td>' +
                        '<td class="sc-d"><table align="center" width="100%"><tr><td class="center" width="50">' + qty + '</td></tr><tr><td><input type="text" class="c-qty" maxlength="4"/></td></tr><tr><td><a href="" class="cart-change" id="cc-' + pid + '">change</a></td></tr></table></td>' +
                        '<td class="sc-d">' + CurrencyCode() + ' ' + formatAmount(stotal) + '</td>' +
                        '<td class="sc-dl"><a href="#" id="imgremove_' + pid + '" class="remove-product"><img src="../images/sc-close.png" alt="Remove" title="Remove"/></a></td></tr>';

    }
    if (d.length == 0) {
        $("#gross-tot").html(CurrencyCode() + " 0.00");
        objCartDiv.html("<div class='error'>No items in your cart!<div>");
    }
    else {
        var totalamount = ConvertToDouble(grossTotal);
        $("#gross-tot").html(CurrencyCode() + " " + formatAmount(grossTotal));
        $("#tot-amt").html(CurrencyCode() + " " + formatAmount(totalamount));
        objCartDiv.html(html);
    }
    if (counts == 0) {
        $(".disc-detail").css("display", "none");
    }
    else {
        $(".disc-detail").css("display", "block");
    }
    $("#cart-count").text(itemCount);
    $.cookie("cartcount", itemCount, { expires: 365 });
    showCartCount();
}
function showCartCount() {
    var cartcount = $.cookie("_cartcount");
    if (cartcount == undefined || cartcount == null) {
        cartcount = "(0) Item"
    }
    else {
        if (cartcount == "1") {
            cartcount = "(1) Item"
        }
        else {
            cartcount = "(" + cartcount + ") Items";
        }
    }
    $(".cart-mstr-count").text(cartcount);
}
/*Shopping Cart*/
function InitOfferZoneMenu() {
    $(".offer-zone-menu").hover(function() {
        $(".offer-zone-cat").show();
    });
}
function InitColorSize() {
    $(".colors").find("div").live("click", function() {
        var css = ConvertToString($(this).attr("class"));
        if (css.indexOf("selected") >= 0) {
            $(this).removeClass("selected");
        }
        else {
            $(this).addClass("selected");
        }
    });
    $(".sizes").find("div").live("click", function() {
        var css = ConvertToString($(this).attr("class"));
        if (css.indexOf("selected") >= 0) {
            $(this).removeClass("selected");
        }
        else {
            $(this).addClass("selected");
        }
    });
}
function InitProductColorSize() {
    $(".product-size").find("div").click(function() {
        $(".product-size").find("div").css("background-color", "#fcfcfc");
        $(".product-size").find("div").css("color", "#444444");
        $(".product-size").find("div").attr("sel", "false");
        $(this).css("background-color", "#ff7101");
        $(this).css("color", "#ffffff");
        $(this).attr("sel", "true");
    });
    $(".product-color").find("div").click(function() {
        $(".product-color").find("div").css("border", "1px solid #a4a4a4");
        $(".product-color").find("div").attr("sel", "false");
        $(this).css("border", "2px solid");
        $(this).attr("sel", "true");
    });

}
function InitCheckBoxDiv() {
    $(".chkdiv").click(function() {
        var sel = $(this).attr("sel");
        if (sel == "true") {
            $(this).attr("sel", "false");
            $(this).css("border", "0px");
        }
        else {
            $(this).attr("sel", "true");
            $(this).css("border", "solid 1px #222222");
        }
    });
}
function InitStaticDialog() {
    $(".staticdialog").click(function() {
        var html = RequestData("../getpage.ashx?m=staticdialog&id=" + $(this).attr("pid"));
        var title = $(this).attr("title");
        var w = $(this).attr("w");
        var h = $(this).attr("h");
        if (w == null) w = 500;
        if (h == null) h = 600;
        var html = "<div id='staticdialog-pop' title='"+title+"'>"+html+"</div>";
        $(html).appendTo('body');
        $("#staticdialog-pop").dialog({
            modal: true,
            width: w,
            height: h
        });
    });
}
function getSearchUrl() {
    var txtsearch = $(".search-keyword");
    var val = txtsearch.val();
    var url = "../ac.ashx?m=search-product&q=" + val;
    return url;
}
function InitProductSearch() {
    var txtsearch = $(".search-keyword");
    txtsearch.autocomplete(getSearchUrl(), {
        width: 693,
        scrollHeight: 500,
        matchContains: true,
        issearch: true,
        selectFirst: false
    });
    $(".search-item-category").live("click", function() {
        window.location = "../ui/productlisting.aspx?cid=" + $(this).attr("cid");
    });
    $(".search-item-product").live("click", function() {
        window.location = "../ui/productdetail.aspx?pid=" + $(this).attr("pid");
    });
    $(".btnsearch").click(function() {
        if ($(".search-keyword").val() == $(".search-keyword").attr("wm") || $(".searchbox").val() == "") {
            alert("Please enter keyword to search");
            return false;
        }
        window.location = "../ui/productListing.aspx?keyword=" + $(".search-keyword").val();
    });
}
function InitRating() {
    $(".rate").click(function() {
        if ($(".rate").closest("td").attr("rated") == "true") {
            alert("You have already rated for this product!");
            return;
        }
        var r = $(this).attr("r");
        var pid = $(".productname").attr("pid");
        var URL = "../utilities.ashx?m=rate&pid=" + pid + "&r=" + r;
        $.ajax({
            url: URL,
            type: 'GET',
            async: true,
            success: function(html) {
                if (html == "-1") {
                    alert("You have already rated for this product!");
                }
                else {
                    $(".rate").closest("td").append("<div>Thanks for your rating!</div>");
                }
                $(".rate").closest("td").attr("rated", "true");
            }
        });
    });
    $(".ratethis").click(function() {
        var rate = ConvertToInt($(this).attr("r"));
        $(".ratethis").each(function() {
            var r = ConvertToInt($(this).attr("r"));
            if (r <= rate) {
                $(this).css("background-image", "url(../images/star.png)");
            }
        });
        $(this).closest("td").find("input").val(rate);
    });
}
function InitWishlist() {
    $(".wishlist").click(function() {
        var pid = $(".productname").attr("pid");
        var URL = "../utilities.ashx?m=wishlist&pid=" + pid;
        $.ajax({
            url: URL,
            type: 'GET',
            async: true,
            success: function(html) {
                if (html == "-1") {
                    window.location = "../account/userlogin.aspx?url=ui/productdetail.aspx?pid=" + pid;
                }
                else {
                    $(".wishlist").html("<a href='../account/wishlist.aspx'>View Wishlist</a>");
                    $(".wishlist").removeClass("wishlist").addClass("wishlist-selected");
                }
            }
        });
    });
}
function InitReviewUseful() {
    $(".useful-yesno").click(function() {
        var div = $(this).closest(".useful-review");
        var rid = $(this).closest(".review-row").attr("rid");
        var yesno = $(this).text();
        var URL = "../utilities.ashx?m=review-useful&rid=" + rid + "&yn=" + yesno;
        $.ajax({
            url: URL,
            type: 'GET',
            async: true,
            success: function(html) {
                div.html("Thanks for your report!");
            }
        });
    });
    $(".review-row").hover(function() {
        $(this).find(".useful-review").show();
    },
    function() {
        $(this).find(".useful-review").hide();
    }
    );
}
function LoadFlashBanner() {
    if ($(".flashbanner").length > 0) {
        $(".flashbanner").cycle({
            fx: 'fade',
            speed: 2000,
            timeout: 100
        });
    }
}
function InitScrollTop() {
    $(window).scroll(function() {
        if ($(window).scrollTop() > 200) {
            $("#scrolltop").show("slow");
        }
        else {
            $("#scrolltop").hide();
        }
    });
    $("#scrolltop").click(function() {
        $('html, body').animate({ scrollTop: 0 }, 'slow');
        $("#scrolltop").hide();
    });
}
function loadProductExtraImage() {
    $(".imgextra").hover(function() {
        var newsrc = $(this).attr("src").replace("thumb", "detail");
        $(".productimage").attr("src", newsrc);
        $(".cloud-zoom").attr("href", $(this).attr("src").replace("thumb", "large"));
        $('.cloud-zoom, .cloud-zoom-gallery').CloudZoom();
    });
}