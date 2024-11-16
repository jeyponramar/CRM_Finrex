var _onesignalAppId = "";
var _onesignalPlayerId = "";
var _onesignalNewPlayerId = "";
function initOneSignal(oneSignalAppId, onesignalPlayerId){
    _onesignalAppId = oneSignalAppId;
    _onesignalPlayerId = onesignalPlayerId;
    window.OneSignalDeferred = window.OneSignalDeferred || [];
    OneSignalDeferred.push(function(OneSignal) {
        OneSignal.init({
          appId: _onesignalAppId,
        });
        function pushSubscriptionChangeListener(event) {
          if (event.current.token) {
            //console.log("The push subscription has received a token!");
            updateOneSignalSubscription(event.current.id, event.current.optedIn);
          }
        }
        OneSignal.User.PushSubscription.addEventListener("change", pushSubscriptionChangeListener);
        getOneSignalPlayerId(0);
    });
}
function updateOneSignalSubscription(playerId, issubscribed)
{
    var url = "utilities.ashx?m=onesignal-suscribe&source=web&playerid="+playerId+"&issubscribed="+issubscribed;
    ajaxCall(url, function(response){
    console.log("playerId:"+playerId);
//        if(issubscribed)
//        {
//            alert("Thanks for subscribing push notification!");
//        }
//        else
//        {
//            alert("You have unsubscribed the push notification!");
//        }
    });
}    
function getOneSignalPlayerId(retry)
{
    if(OneSignal.User.PushSubscription._id != undefined)
    {
        _onesignalNewPlayerId = OneSignal.User.PushSubscription._id;
        if(_onesignalNewPlayerId != _onesignalPlayerId)
        {
            updateOneSignalSubscription(_onesignalNewPlayerId, OneSignal.User.PushSubscription._optedIn);
        }
        return;
    }
    if(retry >= 20) return;
    retry++;
    setTimeout(function(){
        getOneSignalPlayerId(retry);
    }, 1000)
}