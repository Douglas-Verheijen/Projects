(function(define, undefined) {
	
	'use strict';
	define('core', [ ], function(){

        String.prototype.replaceAll = function(search, replacement) {
            var target = this;
            return target.replace(new RegExp(search, 'g'), replacement);
        };

        var core = {
            loadCss: function(url){
                var link = document.createElement("link");
                link.type = "text/css";
                link.rel = "stylesheet";
                link.href = url;
                document.getElementsByTagName("head")[0].appendChild(link);
            },
            
            loadScript: function(url){
                var link = document.createElement("script");
                link.type = "text/javascript";
                link.src = url;
                document.getElementsByTagName("head")[0].appendChild(link);
            }
        }
        
		return core;
	});
})(define);