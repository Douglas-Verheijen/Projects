(function(define, undefined){
    
    'use strict';
    define('artist.header', [ 'highlight' ], function(highlight) {
       
        var name = "artist"
        var url = window.location.href;
        var regex = new RegExp("[?&]" + name.replace(/[\[\]]/g, "\\$&") + "(=([^&#]*)|&|#|$)", "i"), results = regex.exec(url);
        var parameter = decodeURIComponent(results[2].replace(/\+/g, " "));
        var postUrl = "artist.php?artist=" + parameter;
        var sort = "Asc";
        
        $(".artistHeaderInput").on("click", function(){

            var control = $(this);
            var propertyName = control.data("ref");
            $.ajax({
                url: postUrl,
                type: "post",
                data: 
                {  
                    action: "getArtData",
                    order: propertyName,
                    sort: sort 
                },
                success: function(data) {

                    var children = $(data).children();

                    $(".artistComponents").empty();
                    $(".artistComponents").append(children);
	   	            highlight.init('.artistGalleryComponent', '.artistGalleryComponentHighlight');
                    
                    if (sort == "Asc") {
                        sort = "Desc";
                        control.val(propertyName + " ▼");
                    }
                    else {
                        sort = "Asc";
                        control.val(propertyName + " ▲");
                    }  
                },
                error: function(data) {
                    alert(data);
                }
            });
        });
    });
})(define);