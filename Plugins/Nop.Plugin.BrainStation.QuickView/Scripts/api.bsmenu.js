


var quickViewApi = function() {
};


quickViewApi.prototype.viewProductDetails = function(options) {
    var config = $.extend({
        data: { },
        success: function() {
        },
        error: function() {
        }
    }, options);

    $.apiCall({
        type: 'POST',
        data: config.data,
        url: '/product_details',
        success: function(html) {

            $("#quick-view-modal").html(html);

            $(document).trigger("hide-ajax-loading");
            $("#quick-view-loading-modal").modal("hide");
            $("#quick-view-product-details-modal").modal("show");

        }
    });

};






var api = new quickViewApi();