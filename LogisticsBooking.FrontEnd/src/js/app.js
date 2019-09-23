/* JAVASCRIPT PLUGINS GOES HERE */

// jquery
import jQuery from "jquery";
window.$ = window.jQuery = jQuery;

require("popper.js");
require("tooltip.js");
import  "popper.js";
import tooltip from "tooltip.js";


// Bootstrap
import "bootstrap";

import "moment-es6"
import "moment"

import "datatables.net"
import "datatables.net-dt"


$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});

$(function () {
    $('[data-toggle="popover"]').popover()
})
