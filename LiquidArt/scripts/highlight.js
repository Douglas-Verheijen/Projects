(function(define, undefined) {
	
	'use strict';
	define('highlight', [ ], function() {
		
        var highlight = {
            init: function(parent, child) {
                $(child).hide();
                $(parent).mouseover(function(e){
                    $(this).find(child).delay(250).fadeIn(200, null);
                    $(this).find(child).text($(this).attr('data-desc'));
                });  
                
                $(parent).mouseleave(function(){
                    $(this).find(child).delay(250).fadeOut(200, null);
                }); 
            }
        }
        
        return highlight;
	});
})(define);