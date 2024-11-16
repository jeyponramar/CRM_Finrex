//user
var __ROLEID = 0;
//menu
var _selectedMenu = "Home";
//tab
var pageId = 1;
var prevPage = 1;
var curPage = 1;
var pfamount = 0;
var _currrentUrl = '';
var __SCREEN_HEIGHT = 0;
var _CHATTER_FOCUS = false;
var _CHATTER_BLUR_COUNT = 0;
var _iseditlink = true;
var _IsParentWindow = true;

//tax
var _iseditlink = true;
var _TAX_CRDR = "";
var _isAdjustTax = true;
var _selectedRow_QE = null;

//html editor
var _htmlEditor = null;
var _heditorSelectedRange = null;
var _heditorEnabled = false;
var _heditorForeColor = false;
var _heditorBgColor = false;
var _selectedColorPicker = null;
//html editor
//fileupload
var _selectedFileDiv = null;
var _enabledFileUpload = true;
var _fileUploadPrefix = "../";
//listing page
var _currentPageNo = 2;
var _productListLoading = false;
var _productListIsLoadMore = false;
//listing page
//common
//page detail
var _module = ""; var _moduleId = 0;
var _shareViewData = "";
var _isMobile = false;
var _outsideClickDiv = null;
var _clientId = 0;
var __LOGIN_USERID = "0"; 
var _txtselected_ac = null;
IsMobile();
function IsMobile() {
    if (window.location.href.toLowerCase().indexOf("/mobile/") > 0) {
        _isMobile = true;
        return true;
    }
    return false;
}