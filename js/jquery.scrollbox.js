/**
 * jQuery Scrollbox Plugin Beta1
 *
 * @author Hunter Wu
 * @version Beta1 (last modified 6 Jan. 2010)
 */

(function(){

$.fn.scrollbox = function(config) {
  //default config
 	var defConfig = {
 	  linear: false,          //Scroll method
		startDelay: 2,          //Start delay (in seconds)
		delay: 3,               //Delay after each scroll event (in seconds)
		step: 5,                //Distance of each single step (in pixels)
		speed: 32,              //Delay after each single step (in milliseconds)
		switchItems: 1,         //Items to switch after each scroll event
		direction: 'vertical',
		distance: 'auto',
		autoPlay: true,
		onMouseOverPause: true,
		paused: false,
		queue: null
	};
	config = $.extend(defConfig, config);
	config.scrollOffset = config.direction == 'vertical' ? 'scrollTop' : 'scrollLeft';
	if (config.queue)
	  config.queue = $('#' + config.queue);

  return this.each(function() {
    var scrollingId = null;
    var nextScrollId = null;
    var paused = false;
    var container = $(this);

  	if (config.onMouseOverPause) {
  	  container.bind('mouseover', function() { paused = true; });
  	  container.bind('mouseout', function() { paused = false; });
  	}
  	var containerUL = container.children('ul:first-child');

  	var scrollForward = function() {
  	  if (paused) return;

  	  var curLi = containerUL.children('li:first-child');

  	  //一次捲動的總長度
      var scrollDistance = config.distance != 'auto' ? config.distance :
        config.direction == 'vertical' ? curLi.height() : curLi.width();

      //捲動
      if (!config.linear) {
        var theStep = Math.max(3, parseInt((scrollDistance - container[0][config.scrollOffset]) * 0.3, 10));
        var newScrollOffset = Math.min(container[0][config.scrollOffset] + theStep, scrollDistance);
      } else {
        var newScrollOffset = Math.min(container[0][config.scrollOffset] + config.step, scrollDistance);
      }
      container[0][config.scrollOffset] = newScrollOffset;

      if (newScrollOffset >= scrollDistance) {
        for (var i = 0; i < config.switchItems; i++) {
          if (config.queue && config.queue.hasChildNodes() && config.queue.getElementsByTagName('li').length > 0) {
            containerUL.append(config.queue.getElementsByTagName('li')[0]);
            containerUL.remove(containerUL.children('li:first-child'));
          } else {
            containerUL.append(containerUL.children('li:first-child'));
          }
        }
        container[0][config.scrollOffset] = 0;
        clearInterval(scrollingId);
        if (config.autoPlay) nextScrollId = setTimeout(forward, config.delay * 1000);
      }
    };

    //反方向捲動
    //1.若正在正向捲動，則反轉歸零
    //2.若停止中，則反向捲動一次
    var scrollBackward = function() {
      if (paused) return;

      //init
      if (container[0][config.scrollOffset] == 0) {
        var liLen = containerUL.children('li').length;
        for (var i = 0; i < config.switchItems; i++)
          containerUL.children('li:last-child').insertBefore(containerUL.children('li:first-child'));

        var curLi = container.children('li:first-child');
        var scrollDistance = config.distance != 'auto' ? config.distance :
          config.direction == 'vertical' ? curLi.height() : curLi.width();
        container[0][config.scrollOffset] = scrollDistance;
      }

      //反向捲動
      if (!config.linear) {
        var theStep = Math.max(3, parseInt(container[0][config.scrollOffset] * 0.3, 10));
        var newScrollOffset = Math.max(container[0][config.scrollOffset] - theStep, 0);
      } else {
        var newScrollOffset = Math.max(container[0][config.scrollOffset] - config.step, 0);
      }
      container[0][config.scrollOffset] = newScrollOffset;

      if (newScrollOffset == 0) {
        clearInterval(scrollingId);
        if (config.autoPlay) nextScrollId = setTimeout(forward, config.delay * 1000);
      }
    };

    var forward = function() {
      clearInterval(scrollingId);
      scrollingId = setInterval(scrollForward, config.speed);
    };

    var backward = function() {
      clearInterval(scrollingId);
      scrollingId = setInterval(scrollBackward, config.speed);
    };

    var resetClock = function(delay) {
      config.delay = delay || config.delay;
      clearTimeout(nextScrollId);
      if (config.autoPlay) nextScrollId = setTimeout(forward, config.delay * 1000);
    };

    if (config.autoPlay) nextScrollId = setTimeout(forward, config.startDelay * 1000);

    //bind events for container
    container.bind('resetClock', function(delay) { resetClock(delay); });
    container.bind('forward', function() { clearTimeout(nextScrollId); forward(); });
    container.bind('backward', function() { clearTimeout(nextScrollId); backward(); });
    container.bind('speedUp', function(speed) {
      if (typeof speed == 'undefined')
        speed = Math.max(1, parseInt(config.speed / 2, 10));
      config.speed = speed;
    });
    container.bind('speedDown', function(speed) {
      if (typeof speed == 'undefined')
        speed = config.speed * 2;
      config.speed = speed;
    });
  });
};

})();