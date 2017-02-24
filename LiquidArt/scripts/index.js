(function(define, undefined) {
	
	'use strict';
	define('index', [ 
		'core',
		'highlight'
	], function(core, highlight) {
		
		highlight.init('.indexImageComponent', '.indexImageComponentHighlight');	
	});
})(define);