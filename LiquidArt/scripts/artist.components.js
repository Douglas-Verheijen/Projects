(function(define, undefined){
    
    'use strict';
    define('artist.components', [ 'highlight' ], function(highlight) {
       
	   	highlight.init('.artistGalleryComponent', '.artistGalleryComponentHighlight');
    });
})(define);